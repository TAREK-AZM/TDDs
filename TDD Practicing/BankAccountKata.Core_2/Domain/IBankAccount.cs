using System;
using System.Collections.Generic;
using System.Text;

namespace BankAccountKata_2_Tests.Domain
{
    public interface IBankAccount
    {
        void Deposit(decimal amount);
        void Withdraw(decimal amount);
        void PrintStatement();
 }

}
