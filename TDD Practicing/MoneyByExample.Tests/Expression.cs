namespace TDDByExample
{
    public interface IExpression
    {
        public Money Reduce(Bank bank,string to);
        public IExpression Plus(IExpression addend);
        IExpression Times(int multiplayer);
    }
}