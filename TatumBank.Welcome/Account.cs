using System;
using System.Collections.Generic;
using System.Text;

namespace TatumBank.Welcome
{
    public class Account
    {
        public string Name { get; set; } = string.Empty;
        public string Branch {  get; set; } = string.Empty;

        public string AccountNumber { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; } = new();

    }
}
