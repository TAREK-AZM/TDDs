using FluentAssertions;
using FluentAssertions.Common;


namespace BankAccountKataTDD
{
    public class BankAccountTests
    {

        IClockProvider _ClockProvider;

        public BankAccountTests()
        {
            _ClockProvider = new FakeDateProvider();
            _ClockProvider.SetDateTime(new DateTime(2026, 03, 31));
        }


        // Domain Tests:
        [Fact]
        public void Create_bank_account_with_initial_balance_zero()
        {
            // Arrange
            BankAccount account = new BankAccount();

            account.Balance.Should().Be(0);
        }


        [Fact]
        public void Deposit_should_success_when_amount_is_positive()
        {
            // Arrange
            BankAccount account = new BankAccount(_ClockProvider);

            // Action
            account.Deposit(200);

            // Assert
            account.Balance.Should().Be(200);
        }

        [Fact]
        public void Transaction_should_be_created_and_added_into_transactions_history_when_deposit_succeed()
        {
            // Arrange
            BankAccount account = new BankAccount(_ClockProvider);

            // Action
            account.Deposit(200);
            var transaction = account._transactionsHistory.First();

            // Assert
            
            transaction.Should().Be(new Transaction("2026-03-31", 200, 200));
        }


        [Fact]
        public void Deposit_should_fail_when_amount_is_negative()
        {
            // Arrange
            BankAccount account = new BankAccount(_ClockProvider);
            
            // Action
            var action = ()=> account.Deposit(-5);

            // Assert
            action.Should().ThrowExactly<ArgumentException>();

        }


        [Fact]
        public void Withdraw_should_success_when_amount_is_valid()
        {
            // Arrange
            BankAccount account = new BankAccount(_ClockProvider); 
            account.Deposit(200);


            // Action 
            account.Withdraw(100);

            // Assert
            account.Balance.Should().Be(100);

        }

        [Fact]
        public void Transaction_should_be_created_and_added_into_transactions_history_when_withdraw_succeed()
        {
            // Arrange
            BankAccount account = new BankAccount(_ClockProvider);
            account.Deposit(200);
            
            // Action
            account.Withdraw(200);
            var transaction = account._transactionsHistory.Last();

            // Assert

            transaction.Should().Be(new Transaction("2026-03-31", -200, 0));
        }


        [Fact]
        public void Withdraw_should_fail_when_amount_is_negative()
        {
            // Arrange
            BankAccount account = new BankAccount(_ClockProvider); 
            account.Deposit(200);

            // Action 
            var action = ()=>account.Withdraw(-100);

            // Assert
            action.Should().ThrowExactly<ArgumentException>();

        }


        [Fact]
        public void Withdraw_should_fail_when_amount_greater_than_balance()
        {
            // Arrange
            BankAccount account = new BankAccount(_ClockProvider); 
            account.Deposit(200);

            // Action 
            var action = ()=>account.Withdraw(500);

            // Assert
            action.Should().ThrowExactly<ArgumentException>();

        }


        [Fact]
        public void Print_transaction_table_should_be_Ordered_by_date_ascending()
        {
            // Arrange
            List<Transaction> transactions = new List<Transaction>();
            UpdateFakeClockDateProvider(_ClockProvider,new DateTime(2026,03,31));  
            BankAccount account = new BankAccount(_ClockProvider);
            account.Deposit(2500);
            transactions.Add(new Transaction(BankAccount.GetCurrentDate(_ClockProvider.Now()), 2500, 2500)); 

            UpdateFakeClockDateProvider(_ClockProvider, new DateTime(2026, 04, 01));
            account.Withdraw(500);
            transactions.Add(new Transaction(BankAccount.GetCurrentDate(_ClockProvider.Now()),-500, 2000)); 

            UpdateFakeClockDateProvider(_ClockProvider, new DateTime(2026, 04, 02));
            account.Deposit(500);
            transactions.Add(new Transaction(BankAccount.GetCurrentDate(_ClockProvider.Now()), 500, 2500)); 





            var table = BankAccount.FormatTransactionsHistoryTable(transactions).ToString();

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Action
            account.PrintStatement();


            // Assert
            var result = stringWriter.ToString().Trim();
            Assert.Equal(table.ToString().Trim(), result);
        }


        // Helper tests:
        [Fact]
        public void Create_transaction_date_should_return_valid_date_format()
        {

            // arrange
            _ClockProvider.SetDateTime(new DateTime(2026, 03, 31));

            // action
            var transactionDate = BankAccount.GetCurrentDate(_ClockProvider.Now());

            // Assert

            transactionDate.Should().Be(new DateOnly(2026,03,31).ToString("yyyy-MM-dd"));
        }




        // helper Mothods
        private static void UpdateFakeClockDateProvider(IClockProvider clock, DateTime dateTime)
        {
            clock.SetDateTime(dateTime);
        }

    }
}
