
using TatumWelcome2.DTOs;
using TatumWelcome2.Models;

namespace TatumWelcome2.BLL;

public static class BillPurchaseService
{
    public static TransactionResult Purchase(
        Account account,
        BillPaymentRequestDto request,
        BillPaymentService billPaymentService,
        string description)
    {
        if (request.AccountNumber != account.AccountNumber)
            return TransactionResult.PaymentFailed;

        if (request.BankId != "BankA")
            return TransactionResult.PaymentFailed;

        return account.ExecuteBillPayment(
            request.Amount,
            request.Reference,
            description,
            () =>
            {
                BillPaymentResponseDto response =
                    billPaymentService.ProcessPayment(request);

                return response.Success;
            });
    }
}
