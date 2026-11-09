
using TatumWelcome2.DTOs;
using TatumWelcome2.Models;

namespace TatumWelcome2.BLL;

public class BillPaymentService
{
    private string _paymentMode = "Random";

    private readonly Dictionary<string, (
        BillPaymentRequestDto Request,
        BillPaymentResponseDto Response)> _payments = new();

    private readonly object _paymentLock = new();

    public string PaymentMode
    {
        get
        {
            lock (_paymentLock)
            {
                return _paymentMode;
            }
        }
    }

    public bool SetPaymentMode(string mode)
    {
        if (mode != "Random" &&
            mode != "Success" &&
            mode != "Failure")
        {
            return false;
        }

        lock (_paymentLock)
        {
            _paymentMode = mode;
        }

        return true;
    }

    public BillPaymentResponseDto ProcessPayment(
        BillPaymentRequestDto request)
    {
        lock (_paymentLock)
        {
            if (string.IsNullOrWhiteSpace(request.Reference))
            {
                return Failed(
                    request, "Payment reference is required.");
            }

            if (string.IsNullOrWhiteSpace(request.BankId))
            {
                return Failed(
                    request, "Bank identifier is required.");
            }

            if (string.IsNullOrWhiteSpace(request.AccountNumber))
            {
                return Failed(
                    request, "Account number is required.");
            }

            if (_payments.TryGetValue(request.Reference, out var previous))
            {
                bool sameRequest =
                    previous.Request.BankId == request.BankId &&
                    previous.Request.AccountNumber == request.AccountNumber &&
                    previous.Request.PaymentType == request.PaymentType &&
                    previous.Request.Network == request.Network &&
                    previous.Request.PhoneNumber == request.PhoneNumber &&
                    previous.Request.Amount == request.Amount &&
                    previous.Request.DataPlanName == request.DataPlanName;

                if (sameRequest)
                    return previous.Response;

                return Failed(
                    request,
                    "Payment reference already used for another request.");
            }

            if (request.PaymentType != "Airtime" &&
                request.PaymentType != "Data")
            {
                return Failed(request, "Invalid payment type.");
            }

            if (!DataPlans.Networks.Contains(request.Network))
            {
                return Failed(request, "Invalid network provider.");
            }

            if (request.PhoneNumber == null ||
                request.PhoneNumber.Length != 11 ||
                !request.PhoneNumber.All(char.IsDigit))
            {
                return Failed(request, "Invalid phone number.");
            }

            if (request.PaymentType == "Airtime" &&
                request.Amount < 50)
            {
                return Failed(
                    request, "Minimum airtime amount is 50.");
            }

            if (request.PaymentType == "Data")
            {
                bool validPlan = DataPlans.DataPlanItems.Any(
                    plan => plan.Name == request.DataPlanName &&
                            plan.Price == request.Amount);

                if (!validPlan)
                {
                    return Failed(request, "Invalid data plan.");
                }
            }

            bool success = _paymentMode switch
            {
                "Success" => true,
                "Failure" => false,
                _ => Random.Shared.Next(100) < 80
            };

            BillPaymentResponseDto response = new()
            {
                Success = success,
                Message = success
                    ? "Bill payment successful."
                    : "Bill payment failed. Please try again.",
                Reference = request.Reference
            };

            _payments.Add(request.Reference, (request, response));

            return response;
        }
    }

    private static BillPaymentResponseDto Failed(
        BillPaymentRequestDto request,
        string message)
    {
        return new BillPaymentResponseDto
        {
            Success = false,
            Message = message,
            Reference = request.Reference
        };
    }
}
