using System;
using System.Collections.Generic;
using System.Text;

namespace TatumBank.Welcome
{
    public class Transaction
    {
        public TransactionType Type { get; set; }
        public decimal Amount {  get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;

    }
}
