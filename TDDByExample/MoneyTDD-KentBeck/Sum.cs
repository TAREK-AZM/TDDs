

namespace TDDByExample
{
    public class Sum : IExpression
    {
        public IExpression? Augend { get;  set; }
        public IExpression? Addend { get;  set; }
        public Sum(IExpression augend,IExpression addend) { 
            Augend = augend;
            Addend = addend;
        }
        public Money Reduce(Bank bank, string to) 
        {
            var changedAugend = Augend.Reduce(bank,to); // if Augend is Sum : calculated recursively
            var changedAddend = Addend.Reduce(bank,to);

            var total = changedAugend.Amount + changedAddend.Amount;
            return new Money (total,to);
        }
        public IExpression Plus(IExpression addend)
        {
            return new Sum(this,addend);
        }
        public  IExpression Times(int multiplayer)
        {
           return new Sum(Augend.Times(multiplayer),Addend.Times(multiplayer));
        }
    }
}