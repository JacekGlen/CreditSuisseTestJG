using System;
using System.Collections.Generic;
using System.Text;
using VirtualCashCard.Models;

namespace VirtualCashCard.Repositories
{
    public interface IAccountRepository
    {
        Account GetByCardNumber(string cardNummber);
    }
}
