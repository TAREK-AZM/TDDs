using System.Text;

namespace BankAccountKata_2_Tests.Domain
{
    public class BankAccount : IBankAccount
    {
        private decimal dailyWithdrawalSum;

        public decimal Balance { get; internal set; } = 0;
        public  decimal DailyWithdrawalLimit { get; set; } = 5000;
        public decimal DepositMinimumLimit { get; } = 50;
        public decimal CreditLimit { get; } = 100;
        public IDateProvider dateProvider { get; set; }
        public ICollection<Transaction> TransactionsHistory { get; internal set; } = [];
        
        public BankAccount(IDateProvider dateProvider)
        {
            this.dateProvider = dateProvider;
        }
        public void Deposit(decimal amount)
        {
            ValidateAmount(amount);
            IncreaseBalance(amount);

            SaveTransaction(CreateTransaction(amount, TransactionType.DEPOSIT));
        }
        private void ValidateAmount(decimal amount)
        {
            CheckAmountIsPositive(amount);
            ChekAmountGreatherThanLimit(amount);
        }
        private static void CheckAmountIsPositive(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("The amount expected to be Positive for this operation.");
        }
        private void ChekAmountGreatherThanLimit(decimal amount)
        {
            if (IsAmountGreaterThanMinimumLimit(amount))
                throw new ArgumentException($"The amount expected to be Positive for this operation.and greather than: {DepositMinimumLimit}");
        }
        private bool IsAmountGreaterThanMinimumLimit(decimal amount)
        {
            return amount < DepositMinimumLimit;
        }
        private void IncreaseBalance(decimal amount)
        {
            Balance += amount;
        }
        private void SaveTransaction(Transaction? transaction)
        {
            if (transaction is not null)
            {
                AppendTransactionToHistory(transaction);

            }
        }
        private void AppendTransactionToHistory(Transaction transaction)
        {
            TransactionsHistory.Add(transaction);
        }
        private Transaction CreateTransaction(decimal amount, TransactionType type)
        {
            var newAmount = Balance - amount;
            return new Transaction
                            (
                            dateProvider.GetCurrentDate(),
                            amount,
                            newAmount,
                            type
                            );
        }
        private  bool AmountNotCauseCredit(decimal amount)
        {
            // amount will not cause credit if
            // Balance-amount >=0 and 
            // amount will make credit if
            // -100 <Balance - amount <0 

            var creditAmount = Balance - amount;

            return creditAmount >= 0;
        }
        public void Withdraw(decimal amount)
        {
            CheckAmountIsPositive(amount);
            CheckBalanceIsSufficient(amount);

            CanWithdrawToday(amount);

            // here the transaction saved with balance before deduct the amount
            SaveTransaction(CreateTransaction(amount, TransactionType.WITHDRAW));
            SaveTransaction(CreateCreditTransactionIfPossible(amount));
            IncreaseDailyWithdrawalSum(amount);
            DecreaseBalance(amount);


        }

        private void CheckBalanceIsSufficient(decimal amount)
        {
            // balance - amount <= 100
            //  ex: 80 - 100 = 20 < 100 -> good
            //  ex : 80-200 = 120 >  100 -> fail

            if (!AmountIsLessThanBalance(amount) && !CreditAmountIsValide(amount))
                throw new BusinessException("The Balance is not sufficient to make the withdraw operation.");
        }
        private bool AmountIsLessThanBalance(decimal amount)
        {
            return amount <= Balance;
        }
        private bool CreditAmountIsValide(decimal amount)
        {
            var creditAmount = Balance - amount;
            return Math.Abs(creditAmount) <= CreditLimit;
        }
        private void CanWithdrawToday(decimal amount)
        {
            CheckDailyWithdrawalLimitNotExceeded();
            CheckAmountNotDepassedDailyWithdrawalLimit(amount);
        }
        private Transaction? CreateCreditTransactionIfPossible(decimal amount)
        {
            if (!AmountNotCauseCredit(amount))
            {
                var creditAmount = CalculateCreditAmount(amount);
                return CreateTransaction(creditAmount, TransactionType.CREDIT);

            }
            return null;
        }
        private decimal CalculateCreditAmount(decimal amount)
        {
            return Math.Abs(Balance - amount);
        }

        private void CheckDailyWithdrawalLimitNotExceeded()
        {
            if (HasReachedDailyWithdrawalLimit())
                throw new BusinessException("You depassed the your daily total amount that you can withdraw from your account!, Please make sur to contact client support");
        }
        private bool HasReachedDailyWithdrawalLimit()
        {
            return dailyWithdrawalSum == DailyWithdrawalLimit;
        }
        private void CheckAmountNotDepassedDailyWithdrawalLimit(decimal amount)
        {
            if (HasAmountExeceedDailyLimit(amount))
                throw new BusinessException("The amount Execeeded the Allowed withdrawal sum per day.");
        }
        private bool HasAmountExeceedDailyLimit(decimal amount)
        {
            if (dailyWithdrawalSum == 0)
            {
                return amount > DailyWithdrawalLimit;
            }
            decimal amountOfWithdraw = CalculateAllowedAmmountToWithdraw();

            return amount > amountOfWithdraw;

        }
        private decimal CalculateAllowedAmmountToWithdraw()
        {
            return (DailyWithdrawalLimit + CreditLimit) - dailyWithdrawalSum;
        }
        private void DecreaseBalance(decimal amount)
        {
            Balance -= amount;
        }
        private void IncreaseDailyWithdrawalSum(decimal amount)
        {
            dailyWithdrawalSum += amount;
        }
        public void PrintStatement()
        {
            var table = new StringBuilder();
            FormatTableHeader(table);
            FormatTableRows(table);

            Console.WriteLine(table);       
        }
        private static void FormatTableHeader(StringBuilder table)
        {
            var header = "Date       || Amount || Balance";
            table.AppendLine(header);
        }
        private void FormatTableRows(StringBuilder table)
        {
            AppendRowsIntoTable(table);
        }
        private void AppendRowsIntoTable(StringBuilder table)
        {
            var orderedTransactions = GetOrderedTransactions();

            foreach (var transaction in orderedTransactions)
            {
                AppendRowIntoTable(table, transaction);

            }
        }

        private IOrderedEnumerable<Transaction> GetOrderedTransactions()
        {
            return TransactionsHistory.OrderByDescending(t => t.Date);
        }

        private static void AppendRowIntoTable(StringBuilder table, Transaction transaction)
        {
            table.AppendLine(FormatTableRow(transaction));
        }
        private static string FormatTableRow(Transaction transaction)
        {
            var amount = transaction.Type.Equals(TransactionType.WITHDRAW) ? -transaction.Amount : transaction.Amount;
            var row = $"{transaction.Date.ToString("yyyy-MM-dd")} || {amount}   || {transaction.Balance}";
            return row ;
        }

        public Transaction? GetTransactionByType(TransactionType type)
        {
            var resutls = TransactionsHistory.Where(t => t.Type.Equals(type)).FirstOrDefault();
            return resutls;
        }
    }
}
