using BankAccountKata_2_Tests.Domain;
using FluentAssertions;
using System.Text;

namespace BankAccountKata_2_Tests
{
    public class BankAccountKat_2_Tests
    {
        BankAccount sut;
        IDateProvider dateProvider;
        public BankAccountKat_2_Tests() {
            dateProvider = new FakeDateProvider(new DateTimeOffset(DateTime.Now));
            sut = new BankAccount(dateProvider);
        }


        [Fact]
        public void Create_new_bank_account_with_balance_equal_zero()
        {
            sut = new BankAccount(dateProvider);

            var CurrentBalance = sut.Balance;

            CurrentBalance.Should().Be(0);
        }

        [Fact]
        public void Balance_not_negative_when_new_account_is_created()
        {
            sut = new BankAccount(dateProvider);

            var currentBalance = sut.Balance;

            currentBalance.Should().Be(0);

        }

        // Deposit Operations 
        [Fact]
        public void Deposit_operation_succeed_when_amount_is_positive() 
        {
            sut.Deposit(50);
            var currentBalance = sut.Balance;

            currentBalance.Should().Be(50);
        }

        [Fact]
        public void Deposit_operation_fails_when_amount_is_negative() 
        {
            var action = ()=> sut.Deposit(-1);

            action.Should().ThrowExactly<ArgumentException>()
                .WithMessage("The amount expected to be Positive for this operation.");
        }

        [Fact]
        public void Deposit_operation_fails_when_amount_is_zero() 
        {
            var action = ()=> sut.Deposit(0);

            action.Should().ThrowExactly<ArgumentException>()
               .WithMessage("The amount expected to be Positive for this operation.");

        }
       
        [Fact]
        public void Diposit_operation_fails_when_amount_is_less_than_Minimum_limit()
        {
            var action = () => sut.Deposit(20);

            action.Should().ThrowExactly<ArgumentException>()
                .WithMessage("The amount expected to be Positive for this operation.and greather than: 50");

        }

        [Fact]
        public void Transaction_should_be_created_and_added_into_transactions_history_when_deposit_succeed()
        {
            sut.Deposit(100);
            var currenttLength = sut.TransactionsHistory.Count();

            currenttLength.Should().Be(1);

        }

        // Withdraw Operations
        [Fact]
        public void Withdraw_operation_succeed_when_amount_is_valid()
        {
            sut.Deposit(100);

            sut.Withdraw(50);
            var Currentbalance = sut.Balance;

            Currentbalance.Should().Be(50);

        }

        
        [Fact]
        public void Withdraw_operation_fails_when_amount_is_negative() 
        {
            sut.Deposit(100);

            var action = ()=> sut.Withdraw(-50);

            action.Should().ThrowExactly<ArgumentException>()
                .WithMessage("The amount expected to be Positive for this operation.");

        }


        [Fact]
        public void Withdraw_operation_fails_when_balance_is_not_sufficient()
        {
            sut.Deposit(100);

            var action = () => sut.Withdraw(201); // 100-201 =101 > 100

            action.Should().ThrowExactly<BusinessException>()
                .WithMessage("The Balance is not sufficient to make the withdraw operation.");
        }

 
        [Fact]
        public void Withdraw_operation_suceed_when_daily_withdrawal_sum_limit_has_not_Execeeded()
        {
            sut.Deposit(10000);

            sut.Withdraw(5000);
            var currentBalance = sut.Balance;

            currentBalance.Should().Be(5000);

        }

        [Fact]
        public void Withdraw_operation_fails_when_daily_withdrawal_sum_limit_has_reached_to_limit()
        {
            sut.Deposit(10000);
            sut.Withdraw(5000);

            var action = ()=>sut.Withdraw(1000);

            action.Should().ThrowExactly<BusinessException>()
                .WithMessage("You depassed the your daily total amount that you can withdraw from your account!, Please make sur to contact client support");
                
        }

        [Fact]
        public void Withdraw_operation_fails_when_amount_Execeeded_daily_withdrawal_limit()
        {
            sut.Deposit(10000);

            var action = ()=>sut.Withdraw(7000);
          
            action.Should().ThrowExactly<BusinessException>()
                .WithMessage("The amount Execeeded the Allowed withdrawal sum per day.");            

        }

        [Fact]
        public void Withdraw_operation_with_credit_suceeded_when_credit_not_execeed_100_DH()
        {
            sut.Deposit(80);
            
            sut.Withdraw(180);// 80 - 100 = 20 <= 100
            var currentBalance = sut.Balance;

            currentBalance.Should().Be(-100);

        }

        [Fact]
        public void Withdraw_operation_with_credit_fails_when_credit_execeed_100_DH()
        {
            sut.Deposit(80);
            
            var action = ()=> sut.Withdraw(181);// 80 - 181 = 101 > 100
            var expectedBalance = sut.Balance;

            action.Should().ThrowExactly<BusinessException>()
                .WithMessage("The Balance is not sufficient to make the withdraw operation.");

        }

        [Fact]
        public void Withdraw_operation_with_credit_create_credit_transaction()
        {
            sut.Deposit(50);
            sut.Withdraw(100); // credit of 50 DH

            var currentCreditTransaction = sut.GetTransactionByType(TransactionType.CREDIT);

            Assert.NotNull(currentCreditTransaction);
            currentCreditTransaction.Amount.Should().Be(50);
        }

        [Fact]
        public void Transaction_should_be_created_and_added_into_transactions_history_when_withdraw_succeed()
        {
            sut.Deposit(1000);
            sut.Withdraw(500);

            var lastTransaction = sut.TransactionsHistory.Last();
            var transactionsLength = sut.TransactionsHistory.Count();

            lastTransaction.Id.ToString().Should().NotBe(null);
            transactionsLength.Should().Be(2);
        }

        [Fact]
        public void Print_transaction_table_should_be_Ordered_by_date_ascending()
        {
            sut.dateProvider = CreateDateProvider(2026, 04, 08);
            sut.Deposit(10000);
            sut.dateProvider = CreateDateProvider(2026, 04, 09);
            sut.Withdraw(1000);
            sut.dateProvider = CreateDateProvider(2026, 04, 10);
            sut.Withdraw(1000);
            sut.dateProvider = CreateDateProvider(2026, 04, 11);
            sut.Withdraw(3000);

            var currentTble = GetCurrentTable();

            var expectedTable = GetExpectedDataTable();
            currentTble.Should().Be(expectedTable);
        }

        private string GetCurrentTable()
        {
            var currentTble = new StringWriter();
            Console.SetOut(currentTble);
            sut.PrintStatement();
            var formatedTable = currentTble.ToString().Trim();
            return formatedTable;
        }

        private static string GetExpectedDataTable()
        {
            var expectedTable = new StringBuilder();
            expectedTable.AppendLine("Date       || Amount || Balance");
            expectedTable.AppendLine("2026-04-11 || -3000   || 5000");
            expectedTable.AppendLine("2026-04-10 || -1000   || 8000");
            expectedTable.AppendLine("2026-04-09 || -1000   || 9000");
            expectedTable.AppendLine("2026-04-08 || 10000   || 0");
            return expectedTable.ToString().Trim();
        }

        private static FakeDateProvider CreateDateProvider(int year,int month,int day)
        {
            var dateTimeOffset = new DateTimeOffset(new DateTime(year, month, day));
            return new FakeDateProvider(dateTimeOffset);
        }

        
      
    }
}