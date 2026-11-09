
using TatumWelcome2.DTOs;
using TatumWelcome2.Models;

namespace TatumWelcome2.BLL;

public static class AirtimeService
{
    public static TransactionResult Purchase(
        Account account,
        string network,
        string phone,
        decimal amount,
        BillPaymentService billPaymentService,
        out string reference)
    {
        reference = Guid.NewGuid().ToString("N");

        if (!DataPlans.Networks.Contains(network))
            return TransactionResult.InvalidNetwork;

        if (phone.Length != 11 || !phone.All(char.IsDigit))
            return TransactionResult.InvalidPhone;

        if (amount < 50)
            return TransactionResult.InvalidAmount;

        BillPaymentRequestDto request = new()
        {
            Reference = reference,
            BankId = "BankA",
            AccountNumber = account.AccountNumber,
            PaymentType = "Airtime",
            Network = network,
            PhoneNumber = phone,
            Amount = amount
        };

        return BillPurchaseService.Purchase(
            account,
            request,
            billPaymentService,
            $"Airtime {network} {phone} Ref: {reference}");
    }
}
