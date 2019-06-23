using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCashCard
{
    public class WithdrawalCommandResponse
    {
        public bool IsSuccessful { get; set; }
        public string Reason { get; set; }

        public WithdrawalCommandResponse()
        {
        }

        public WithdrawalCommandResponse(bool isSuccessful, string reason = null)
        {
            IsSuccessful = isSuccessful;
            Reason = reason;
        }
    }
}
