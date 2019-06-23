using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCashCard
{
    public class TopupCommandResponse
    {
        public bool IsSuccessful { get; set; }
        public string Reason { get; set; }

        public TopupCommandResponse()
        {
        }

        public TopupCommandResponse(bool isSuccessful, string reason = null)
        {
            IsSuccessful = isSuccessful;
            Reason = reason;
        }
    }
}
