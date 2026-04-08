using System;
using System.Collections.Generic;
using System.Text;

namespace BankAccountKataTDD
{
    public interface IClockProvider
    {
        DateTime Now();
        void SetDateTime(DateTime dateTime);
    }
}
