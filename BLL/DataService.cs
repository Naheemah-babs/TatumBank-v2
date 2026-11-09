using TatumWelcome2.Models;

namespace TatumWelcome2.BLL;

public static class DataService
{
    public static TransactionResult Purchase(
        Account account,
        string network,
        string phone,
        DataPlan plan)
    {
        if (!DataPlans.Networks.Contains(network))
            return TransactionResult.InvalidNetwork;

        if (phone.Length != 11 || !phone.All(char.IsDigit))
            return TransactionResult.InvalidPhone;

        if (!DataPlans.DataPlanItems.Contains(plan))
            return TransactionResult.InvalidPlan;

        return account.Withdraw(
            plan.Price,
            $"Data {network} {plan.Name} {phone}");
    }
}