namespace TatumBank.Welcome;

public class BankApp
{
    public void Run(List<Account> accounts)
    {
        string tatumLogo = @"
 _____    _    _____  _   _  __  __    ___    _    _  _  _  __
|_   _|  /_\  |_   _|| | | ||  \/  |  | _ )  /_\  | \| || |/ /
  | |   / _ \   | |  | |_| || |\/| |  | _ \ / _ \ | .` || ' < 
  |_|  /_/ \_\  |_|   \___/ |_|  |_|  |___//_/ \_\|_|\_||_|\_\
";

        Console.WriteLine(tatumLogo);
        Console.WriteLine("Welcome to Tatum Bank!");
        Console.WriteLine();

        Console.WriteLine("Whats your name");
        string userName = Console.ReadLine();

        Console.WriteLine("Whats your branch");
        string userBranch = Console.ReadLine();

        bool found = false;

        foreach (Account account in accounts)
        {
            if (account.Name.ToLower() == userName.ToLower() && account.Branch.ToLower() == userBranch.ToLower())
            {
                found = true;

                Console.WriteLine("Whats your pin");
                string userPin = Console.ReadLine();

                if (account.Pin == userPin)
                {
                    Console.WriteLine($"Welcome, {account.Name}!");
                    bool running = true;

                    while (running)
                    {
                        Console.WriteLine();
                        Console.WriteLine("===== TATUM BANK MENU =====");
                        Console.WriteLine("1. Check Balance");
                        Console.WriteLine("2. Transfer");
                        Console.WriteLine("3. Exit");
                        Console.Write("Choose an option: ");

                        string choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "1":
                                Console.WriteLine($"Your balance is: {account.Balance}");
                                break;

                            case "2":
                                {
                                    Console.Write("Enter recipient account number: ");
                                    string recipientNumber = Console.ReadLine().Trim();

                                    Account recipient = null;

                                    foreach (Account candidate in accounts)
                                    {
                                        if (candidate.AccountNumber == recipientNumber)
                                        {
                                            recipient = candidate;
                                            break;
                                        }
                                    }

                                    if (recipient == null)
                                    {
                                        Console.WriteLine("Recipient account not found.");
                                        break;
                                    }

                                    if (recipient.AccountNumber == account.AccountNumber)
                                    {
                                        Console.WriteLine("You cannot transfer to your own account.");
                                        break;
                                    }

                                    Console.Write("Enter amount: ");
                                    string amountText = Console.ReadLine();

                                    decimal amount;
                                    if (!decimal.TryParse(amountText, out amount) || amount <= 0)
                                    {
                                        Console.WriteLine("Invalid amount.");
                                        break;
                                    }

                                    if (amount > account.Balance)
                                    {
                                        Console.WriteLine("Insufficient funds.");
                                        break;
                                    }

                                    DateTime now = DateTime.Now;

                                    account.Balance -= amount;
                                    recipient.Balance += amount;

                                    account.Transactions.Add(new Transaction
                                    {
                                        Type = TransactionType.TransferOut,
                                        Amount = amount,
                                        Date = now,
                                        Description = $"Transfer out to Recipient {recipient.Name} with Account number {recipient.AccountNumber}"
                                    });

                                    recipient.Transactions.Add(new Transaction
                                    {
                                        Type = TransactionType.TransferIn,
                                        Amount = amount,
                                        Date = now,
                                        Description = $"Transfer in from Sender {account.Name} with Account Number {account.AccountNumber}"
                                    });

                                    Console.WriteLine($"Transfer successful. Your new balance is: {account.Balance}");
                                    break;
                                }
                            case "3":
                                Console.WriteLine(tatumLogo);
                                Console.WriteLine("Thank you for banking with Tatum Bank!");
                                running = false;
                                break;
                            default:
                                Console.WriteLine("Invalid option, please choose 1 to 5.");
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid details, please try again.");
                }

                break;
            }
        }

        if (found == false)
        {
            Console.WriteLine("No account found. Please open an account with Tatum Bank.");
        }
    }
}