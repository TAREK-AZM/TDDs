namespace BankAccountKataTDD
{
    public class Transaction
    {
        public string Date { get; }
        public int Amount { get; }
        public int Balance { get; }


        public Transaction(string date,int amount,int balance)
        {
            Date = date;

            Amount = amount;

            Balance = balance;
        }

        public override string ToString()
        {
            return $"{Date} || {Amount}   || {Balance}";
        }

        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            if (obj is Transaction transaction) { 
               return Date.Equals(transaction.Date) 
                    && Amount.Equals(transaction.Amount) 
                    && Balance.Equals(transaction.Balance);
            }
            return false;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
     
    }
}