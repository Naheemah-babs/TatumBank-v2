
using TatumWelcome2.BLL;
using TatumWelcome2.Models;

namespace TatumWelcome2.Presentation;

public static class DataMenu
{
    public static bool Run(
        Account current,
        BillPaymentService billPaymentService)
    {
        Console.WriteLine("(Enter 0 at any step to cancel)");

        string network;

        while (true)
        {
            Console.WriteLine(
                "Network: 1. MTN  2. Airtel  3. Glo  4. 9mobile");

            Console.Write("Choose network: ");
            string input = Console.ReadLine() ?? "";

            if (input == "0")
            {
                Console.WriteLine("Data purchase cancelled.");
                return false;
            }

            if (!int.TryParse(input, out int choice) ||
                choice < 1 ||
                choice > DataPlans.Networks.Length)
            {
                Console.WriteLine(
                    "Invalid network. Choose a number from the list.");
                continue;
            }

            network = DataPlans.Networks[choice - 1];
            break;
        }

        DataPlan selectedPlan;

        while (true)
        {
            Console.WriteLine("Plans:");

            for (int i = 0; i < DataPlans.DataPlanItems.Count; i++)
            {
                DataPlan plan = DataPlans.DataPlanItems[i];

                Console.WriteLine(
                    $"{i + 1}. {plan.Name} - {plan.Price:N2}");
            }

            Console.Write("Choose plan: ");
            string input = Console.ReadLine() ?? "";

            if (input == "0")
            {
                Console.WriteLine("Data purchase cancelled.");
                return false;
            }

            if (!int.TryParse(input, out int choice) ||
                choice < 1 ||
                choice > DataPlans.DataPlanItems.Count)
            {
                Console.WriteLine(
                    "Invalid plan. Choose a number from the list.");
                continue;
            }

            selectedPlan = DataPlans.DataPlanItems[choice - 1];
            break;
        }

        string phone;

        while (true)
        {
            Console.Write("Phone number, 11 digits: ");
            phone = Console.ReadLine() ?? "";

            if (phone == "0")
            {
                Console.WriteLine("Data purchase cancelled.");
                return false;
            }

            if (!PhoneNumberValidator.IsValid(phone))
            {
                Console.WriteLine(
                    "Enter a valid Nigerian mobile number.");
                continue;
            }

            break;
        }

        TransactionResult result =
            DataService.Purchase(
                current,
                network,
                phone,
                selectedPlan,
                billPaymentService,
                out string reference);

        if (result == TransactionResult.Success)
        {
            Console.WriteLine(
                $"{selectedPlan.Name} sent to {phone}. " +
                $"Balance: {current.Balance:N2}");

            Console.WriteLine($"Payment reference: {reference}");
            return true;
        }

        if (result == TransactionResult.InsufficientFunds)
        {
            Console.WriteLine(
                $"You don't have enough balance for this plan. " +
                $"Your balance is {current.Balance:N2}.");
        }
        else if (result == TransactionResult.PaymentFailed)
        {
            Console.WriteLine(
                "Data purchase failed. Your account was not debited.");

            Console.WriteLine($"Payment reference: {reference}");
        }
        else
        {
            Console.WriteLine($"Data purchase failed: {result}");
        }

        return false;
    }
}
