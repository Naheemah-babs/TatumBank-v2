using TatumWelcome2.Models;

namespace TatumWelcome2.BLL;

public static class AirtimeService
{
    public static TransactionResult Purchase(
        Account account,
        string network,
        string phone,
        decimal amount)
    {
        if (!DataPlans.Networks.Contains(network))
            return TransactionResult.InvalidNetwork;

        if (phone.Length != 11 || !phone.All(char.IsDigit))
            return TransactionResult.InvalidPhone;

        if (amount < 50)
            return TransactionResult.InvalidAmount;

        return account.Withdraw(
            amount,
            $"Airtime {network} {phone}");
    }
}