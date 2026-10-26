using TatumWelcome2.BLL;
using TatumWelcome2.Models;

namespace TatumWelcome2.Presentation;

public static class AirtimeMenu
{
    public static bool Run(Account current)
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
                Console.WriteLine("Airtime purchase cancelled.");
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

        string phone;

        while (true)
        {
            Console.Write("Phone number, 11 digits: ");
            phone = Console.ReadLine() ?? "";

            if (phone == "0")
            {
                Console.WriteLine("Airtime purchase cancelled.");
                return false;
            }

            if (phone.Length != 11 || !phone.All(char.IsDigit))
            {
                Console.WriteLine(
                    "Enter a valid 11-digit phone number.");
                continue;
            }

            break;
        }

        while (true)
        {
            Console.Write("Airtime amount, minimum 50: ");
            string input = Console.ReadLine() ?? "";

            if (input == "0")
            {
                Console.WriteLine("Airtime purchase cancelled.");
                return false;
            }

            if (!decimal.TryParse(input, out decimal amount) ||
                amount < 50)
            {
                Console.WriteLine(
                    "Enter a valid amount of at least 50.");
                continue;
            }

            TransactionResult result =
                AirtimeService.Purchase(
                    current, network, phone, amount);

            if (result == TransactionResult.Success)
            {
                Console.WriteLine(
                    $"{amount:N2} airtime sent to {phone}. " +
                    $"Balance: {current.Balance:N2}");

                return true;
            }

            if (result == TransactionResult.InsufficientFunds)
            {
                Console.WriteLine(
                    $"You don't have enough balance. " +
                    $"Your balance is {current.Balance:N2}.");
                continue;
            }

            Console.WriteLine($"Airtime purchase failed: {result}");
            return false;
        }
    }
}