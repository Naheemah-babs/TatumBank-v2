
using TatumWelcome2.BLL;
using TatumWelcome2.Models;

namespace TatumWelcome2.Presentation;

public static class AdminMenu
{
    public static void Run(
        AdminService adminService,
        BillPaymentService billPaymentService)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== ADMIN MENU =====");
            Console.WriteLine("1. Freeze account");
            Console.WriteLine("2. Unfreeze account");
            Console.WriteLine("3. Check account status");
            Console.WriteLine("4. Bill payment testing mode");
            Console.WriteLine("0. Logout");
            Console.Write("Choose an option: ");

            string choice = (Console.ReadLine() ?? "").Trim();

            switch (choice)
            {
                case "1":
                    ChangeAccountStatus(adminService, true);
                    break;

                case "2":
                    ChangeAccountStatus(adminService, false);
                    break;

                case "3":
                    CheckAccountStatus(adminService);
                    break;

                case "4":
                    ChangePaymentMode(billPaymentService);
                    break;

                case "0":
                    Console.WriteLine("Admin logged out.");
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void ChangePaymentMode(
        BillPaymentService billPaymentService)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== BILL PAYMENT MODE =====");
            Console.WriteLine(
                $"Current mode: {billPaymentService.PaymentMode}");

            Console.WriteLine("1. Random (80% success, 20% failure)");
            Console.WriteLine("2. Force success");
            Console.WriteLine("3. Force failure");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");

            string choice = (Console.ReadLine() ?? "").Trim();

            if (choice == "0")
                return;

            string mode = choice switch
            {
                "1" => "Random",
                "2" => "Success",
                "3" => "Failure",
                _ => ""
            };

            if (mode == "")
            {
                Console.WriteLine("Invalid option. Please try again.");
                continue;
            }

            bool updated = billPaymentService.SetPaymentMode(mode);

            if (updated)
            {
                Console.WriteLine(
                    $"Bill payment mode changed to {mode}.");
                return;
            }

            Console.WriteLine("Unable to change payment mode.");
        }
    }

    private static void ChangeAccountStatus(
        AdminService adminService,
        bool freeze)
    {
        Console.Write("Enter customer account number (0 to cancel): ");
        string accountNumber = (Console.ReadLine() ?? "").Trim();

        if (accountNumber == "0")
        {
            Console.WriteLine("Operation cancelled.");
            return;
        }

        Account? account = adminService.FindAccount(accountNumber);

        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        Console.WriteLine($"Account holder: {account.Name}");
        Console.WriteLine($"Account number: {account.AccountNumber}");
        Console.WriteLine(
            $"Current status: {(account.IsFrozen ? "Frozen" : "Active")}");

        if (freeze && account.IsFrozen)
        {
            Console.WriteLine("This account is already frozen.");
            return;
        }

        if (!freeze && !account.IsFrozen)
        {
            Console.WriteLine("This account is already active.");
            return;
        }

        string action = freeze ? "freeze" : "unfreeze";

        Console.Write(
            $"Are you sure you want to {action} this account? (yes/no): ");

        string confirmation =
            (Console.ReadLine() ?? "").Trim().ToLower();

        if (confirmation != "yes")
        {
            Console.WriteLine("Operation cancelled.");
            return;
        }

        bool success = freeze
            ? adminService.FreezeAccount(accountNumber)
            : adminService.UnfreezeAccount(accountNumber);

        if (success)
        {
            Console.WriteLine(
                freeze
                    ? "Account frozen successfully."
                    : "Account unfrozen successfully.");
        }
        else
        {
            Console.WriteLine("Unable to update account status.");
        }
    }

    private static void CheckAccountStatus(AdminService adminService)
    {
        Console.Write("Enter customer account number (0 to cancel): ");
        string accountNumber = (Console.ReadLine() ?? "").Trim();

        if (accountNumber == "0")
        {
            Console.WriteLine("Operation cancelled.");
            return;
        }

        Account? account = adminService.FindAccount(accountNumber);

        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Account holder: {account.Name}");
        Console.WriteLine($"Account number: {account.AccountNumber}");
        Console.WriteLine(
            $"Account status: {(account.IsFrozen ? "Frozen" : "Active")}");
    }
}
