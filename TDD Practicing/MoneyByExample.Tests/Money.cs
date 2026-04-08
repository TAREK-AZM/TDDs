using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TDDByExample
{
    public  class Money : IExpression
    {
        public int Amount { get; set; }
        protected string _currency;

        public Money(int Amount, string currency)
        {
            this.Amount = Amount;
            _currency = currency;

        }

        public  string Currency()
        {
            return _currency;
        }

        public override bool Equals(object? obj)
        {
            Money money = (Money) obj;

            return Amount.Equals(money?.Amount) && _currency.Equals(money._currency)

                //&& GetType() == money.GetType();
                ;

        }



        //public virtual Money Times(int multiplier) {
        //    return null;
        //}

        public  IExpression Times(int multiplier)
        {
            return new Money(Amount * multiplier, _currency);
        }

        public static Money doller(int amount)
        {
            return new Money(amount,"USD");
        }

        public static Money franc(int amount)
        {
            return new Money(amount,"CHF");
        }

        public override string ToString()
        {
            return Amount + " " + _currency;
        }

        public IExpression Plus(IExpression addend)
        {
            return new Sum(this,addend);
        }

        public Money Reduce(Bank bank,string to)
        {
            if (to.Equals(_currency)) 
                return this;

            var rateChange = bank.GetRateChangeFromTo(_currency, to);

            return new Money(Amount / rateChange, to);
        }
    }
}
