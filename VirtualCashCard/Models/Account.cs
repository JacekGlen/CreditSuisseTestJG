using System;
using System.Collections.Generic;
using System.Text;


using System.Security.Cryptography;

namespace VirtualCashCard.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string Pin { get; set; }

        public bool IsPinValid(string pin)
        {
            return Pin.Equals(pin);
        }
    }
}
