using FluentAssertions;



namespace TDDByExample
{
    // Here suposed that : Class Franc , Doller are deleted only one class Wich is Money that exist

    public class MoneyTestMonoClass
    {

        [Fact]
        public void Test_mulrtiplication()
        {
            Money five = Money.doller(5);

            Assert.Equal(Money.doller(10), five.Times(2));
            Assert.Equal(Money.doller(15), five.Times(3));
        }

        [Fact]
        public void Test_equality()
        {
            Assert.True(Money.doller(5).Equals(Money.doller(5)));
            Assert.False(Money.doller(5).Equals(Money.doller(6)));
            Assert.False(Money.franc(5).Equals(Money.doller(5)));
        }

        [Fact]
        public void Test_curency()
        {
            Assert.True("USD".Equals(Money.doller(1).Currency()));
        }

        [Fact]
        public void Test_Plus_return_sum()
        {
            Money five = Money.doller(5);
            IExpression result = five.Plus(five);

            Sum sum = (Sum) result;

            Assert.Equal(five, sum.Augend);
            Assert.Equal(five, sum.Addend);

        }
        [Fact]
        public void Test_simple_addition()
        {
            Money five = Money.doller(5);
            IExpression result = five.Plus(five);
            Sum sum = (Sum)result;
            Bank bank = new Bank();

            Assert.Equal(five, sum.Augend);
            Assert.Equal(Money.doller(10), sum.Reduce(bank, "USD"));
        }

        [Fact]
        public void Test_reduced_Money()
        {
            Bank bank = new Bank();

            Money result = bank.Reduce(Money.doller(1), "USD");

            Assert.Equal(Money.doller(1), result);
        }

        [Fact]
        public void Test_reduce_Money_with_different_currency()
        {
            IExpression fiveDollers = Money.doller(5);
            IExpression twoFrancs = Money.franc(10);
            Bank bank = new Bank();
            var isAddedFrancToDoller = bank.AddRate("CHF", "USD", 2);


            IExpression result = fiveDollers.Plus(twoFrancs);
            var sum = result.Reduce(bank,"USD");

            Assert.True(isAddedFrancToDoller);
            Assert.Equal(Money.doller(10), sum);

        }

        [Fact]
        public void Test_identity_rate()
        {
            Assert.Equal(1, new Bank().GetRateChangeFromTo("USD", "USD"));
            Assert.Equal(1, new Bank().GetRateChangeFromTo("CHF", "CHF"));
        }

        [Fact]
        public void Test_diffrent_addition_throw_rate_change_not_suported_exception_when_no_conversion_pair_suported_by_bank()
        {
            Money fiveDollers = Money.doller(10);
            Money twoFrancs = Money.franc(10);
            Bank bank = new Bank();
            //var isAddedFrancToDoller = bank.AddRate("CHF", "USD", 2);


            IExpression result = fiveDollers.Plus(twoFrancs);
            var action = () => result.Reduce(bank, "CHF");
            action.Should().ThrowExactly<RatePairChangeNotFoundException>()
                .WithMessage($"The pair USD -> CHF or CHF -> USD not suported by bank.");
            
        }

        [Fact]
        public void Test_sum_plus_Money()
        {
            IExpression fiveDollers = Money.doller(5);
            IExpression twoFrancs = Money.franc(10);

            Bank bank = new Bank();
            var isAddedFrancToDoller = bank.AddRate("CHF", "USD", 2); // 2 chf = 1$

            IExpression sum = new Sum(fiveDollers,twoFrancs).Plus(fiveDollers);
            Money result = bank.Reduce(sum, "USD");

            Assert.Equal(Money.doller(15), result);

        }

        [Fact]
        public void Test_sum_times()
        {
            IExpression fiveDollers = Money.doller(5);
            IExpression twoFrancs = Money.franc(10);

            Bank bank = new Bank();
            var isAddedFrancToDoller = bank.AddRate("CHF", "USD", 2); // 2 chf = 1$

            IExpression sum = new Sum(fiveDollers, twoFrancs).Times(2);// 5*2 = 10 , 10*2 =20 => converting to USD : 10 + 20/2 = 20
            Money result = bank.Reduce(sum, "USD");

            Assert.Equal(Money.doller(20), result);
        }

    }
}
