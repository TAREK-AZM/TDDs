using FluentAssertions;

namespace TDDByExample
{
    public class MoneyTest
    {


        [Fact]
        public void Doller_mulrtiplication()
        {
            Money five = Money.doller(5);

            Assert.Equal(Money.doller(10), five.Times(2));
            Assert.Equal(Money.doller(15), five.Times(3));
        }

        [Fact]
        public void Franc_multiplication()
        {
            Money five = Money.franc(5);

            Assert.Equal(Money.franc(10), five.Times(2));
            Assert.Equal(Money.franc(15), five.Times(3));
        }

        [Fact]
        public void Test_equality()
        {
            Assert.True(Money.doller(5).Equals(Money.doller(5)));
            Assert.False(Money.doller(5).Equals(Money.doller(6)));
            Assert.False(Money.franc(5).Equals(Money.doller(5)));
        }

        [Fact]
        public void Test_different_class_equality()
        {
            Assert.False(new Money(10, "USA").Equals(new Franc(10, "CHF")));
        }

        [Fact]
        public void Test_curency()
        {
            Assert.True("USD".Equals(Money.doller(1).Currency()));
        }


    }
}
