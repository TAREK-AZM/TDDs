
using System.Reflection.Metadata.Ecma335;

namespace BankAccountKataTDD
{
    internal class FakeDateProvider : IClockProvider
    {
        DateTime _DateTime = new DateTime();
       
        public DateTime Now()
        {
            return _DateTime;
        }

        public void SetDateTime(DateTime dateTime)
        {
            _DateTime = dateTime;
        }
    }
}