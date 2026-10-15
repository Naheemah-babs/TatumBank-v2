using TatumBank.Welcome;

List<Account> accounts = AccountData.GetAccounts();

BankApp app = new BankApp();
app.Run(accounts);