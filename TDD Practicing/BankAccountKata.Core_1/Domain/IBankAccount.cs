using System;
using System.Collections.Generic;
using System.Text;

namespace BankAccountKataTDD
{
    public interface IBankAccount
    {
        void Deposit(int amount);
        void Withdraw(int amount);
        void PrintStatement();
 }

}
