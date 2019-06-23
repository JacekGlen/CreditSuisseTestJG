using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCashCard
{
    public class TopupCommandRequest
    {
        public string CardNumber { get; set; }
        public string Pin { get; set; }
        public decimal Amount { get; set; }
    }
}
