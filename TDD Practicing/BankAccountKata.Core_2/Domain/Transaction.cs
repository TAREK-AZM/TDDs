namespace BankAccountKata_2_Tests.Domain
{
    public class Transaction
    {
        public Guid Id { get; private set; }

        public DateOnly Date {  get; private set; }

        public decimal Amount { get; private set; }

        public decimal Balance { get; private set; }

        public TransactionType Type { get; private set; }


        public Transaction(DateOnly date, decimal amount, decimal balance, TransactionType type)
        {
            Id = Guid.NewGuid();
            Date = date;
            Amount = amount;
            Balance = balance;
            Type = type;
        }


        public override bool Equals(object? obj)
        {
            if(obj == null || !(obj is Transaction))
                return false;

            var transaction = (Transaction)obj;

            if(this.Id.Equals(transaction.Id))
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
