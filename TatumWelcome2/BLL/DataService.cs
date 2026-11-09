
using TatumWelcome2.DTOs;
using TatumWelcome2.Models;

namespace TatumWelcome2.BLL;

public static class DataService
{
    public static TransactionResult Purchase(
        Account account,
        string network,
        string phone,
        DataPlan plan,
        BillPaymentService billPaymentService,
        out string reference)
    {
        reference = Guid.NewGuid().ToString("N");

        if (!DataPlans.Networks.Contains(network))
            return TransactionResult.InvalidNetwork;

        if (phone.Length != 11 || !phone.All(char.IsDigit))
            return TransactionResult.InvalidPhone;

        if (!DataPlans.DataPlanItems.Contains(plan))
            return TransactionResult.InvalidPlan;

        BillPaymentRequestDto request = new()
        {
            Reference = reference,
            BankId = "BankA",
            AccountNumber = account.AccountNumber,
            PaymentType = "Data",
            Network = network,
            PhoneNumber = phone,
            Amount = plan.Price,
            DataPlanName = plan.Name
        };

        return BillPurchaseService.Purchase(
            account,
            request,
            billPaymentService,
            $"Data {network} {plan.Name} {phone} Ref: {reference}");
    }
}
