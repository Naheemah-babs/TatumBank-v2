
namespace TatumBank.Welcome;

public static class AccountData
{
    public static List<Account> GetAccounts()
    {
        return new List<Account>
        {
            new Account
            {
                Name = "Ada Smith",
                Branch = "Lagos",
                AccountNumber = "1222334455",
                Pin = "1234",
                Balance = 200000m,
                Transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        Type = TransactionType.Deposit,
                        Amount = 800000m,
                        Date = new DateTime(2026, 2, 17, 9, 0, 0),
                        Description = "Deposit"
                    },
                    new Transaction
                    {
                        Type = TransactionType.TransferOut,
                        Amount = 600000m,
                        Date = new DateTime(2026, 2, 17, 9, 5, 0),
                        Description = "Transfer out to Recipient Ola Wale with Account number 1222334055"
                    }
                }
            },

            new Account
            {
                Name = "Ola Wale",
                Branch = "Kano",
                AccountNumber = "1222334055",
                Pin = "1111",
                Balance = 200000m,
                Transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        Type = TransactionType.TransferIn,
                        Amount = 600000m,
                        Date = new DateTime(2026, 2, 17, 9, 5, 0),
                        Description = "Transfer in from Sender Ada Smith with Account Number 1222334455"
                    },
                    new Transaction
                    {
                        Type = TransactionType.TransferOut,
                        Amount = 400000m,
                        Date = new DateTime(2026, 2, 17, 9, 10, 0),
                        Description = "Transfer out to Baba Jide with Account Number 1222334655"
                    }
                }
            },

            new Account
            {
                Name = "Baba Jide",
                Branch = "Abuja",
                AccountNumber = "1222334655",
                Pin = "1911",
                Balance = 402000m,
                Transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        Type = TransactionType.TransferIn,
                        Amount = 400000m,
                        Date = new DateTime(2026, 2, 17, 9, 10, 0),
                        Description = "Transfer in from Sender Ola Wale with Account Number 1222334055"
                    },
                    new Transaction
                    {
                        Type = TransactionType.Deposit,
                        Amount = 2000m,
                        Date = new DateTime(2026, 2, 17, 9, 15, 0),
                        Description = "Deposit"
                    }
                }
            },

            new Account
            {
                Name = "Baba Tunde",
                Branch = "Ibadan",
                AccountNumber = "1222334077",
                Pin = "2211",
                Balance = 589000m,
                Transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        Type = TransactionType.Deposit,
                        Amount = 600000m,
                        Date = new DateTime(2026, 2, 17, 9, 0, 0),
                        Description = "Deposit"
                    },
                    new Transaction
                    {
                        Type = TransactionType.AirtimePurchase,
                        Amount = 1000m,
                        Date = new DateTime(2026, 2, 17, 9, 0, 0),
                        Description = "AirtimePurchase"
                    },
                    new Transaction
                    {
                        Type = TransactionType.DataPurchase,
                        Amount = 10000m,
                        Date = new DateTime(2026, 2, 17, 9, 5, 0),
                        Description = "DataPurchase"
                    }
                }
            }
        };
    }
}
