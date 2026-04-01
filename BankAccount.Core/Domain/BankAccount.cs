using System.Text;

namespace BankAccountKataTDD
{
    
    public  class BankAccount : IBankAccount
    {

        public static readonly string TransactionsTableHeader = "Date       || Amount || Balance";

        public  IClockProvider Clock { get; set; }

        public int Balance { get; internal set; }

        public ICollection<Transaction> _transactionsHistory = new List<Transaction>();

        public DateOnly CreatedAt;

        public BankAccount()
        {
            Balance = 0;
        }

        public BankAccount(IClockProvider clock)
        {
            Clock = clock;
            Balance = 0;
        }

        public void Deposit(int amount)
        {
            if (amount < 0)
                throw new ArgumentException();

            IncreaseBalance(amount);
            SaveTransaction(amount);

        }

        public void Withdraw(int amount)
        {
            if (amount < 0 || amount > Balance)
                throw new ArgumentException();

            DecreaseBalance(amount);
            SaveTransaction(-amount);
        }

        public void PrintStatement()
        {
            StringBuilder table = GetTransactionsTable();
            Console.WriteLine(table.ToString());
        }

        private StringBuilder GetTransactionsTable()
        {
            List<Transaction> orderedTransactions = OrderTransactions();
            StringBuilder table = FormatTransactionsHistoryTable(orderedTransactions);

            return table;
        }

        public static StringBuilder FormatTransactionsHistoryTable(List<Transaction> orderedTransactions)
        {
            var table = new StringBuilder();

            FormatTableHeader(table);
            FormatTableRows(orderedTransactions, table);

            return table;
        }

        private static void FormatTableRows(List<Transaction> orderedTransactions, StringBuilder table)
        {
            foreach (var transaction in orderedTransactions)
            {

                table.AppendLine(transaction.ToString());
            }
        }

        private static void FormatTableHeader(StringBuilder table)
        {
            table.AppendLine(TransactionsTableHeader);
        }

        private List<Transaction> OrderTransactions()
        {
            _transactionsHistory.Reverse();

            var orderedTransactions = _transactionsHistory.OrderBy(t => t.Date).ToList();
            return orderedTransactions;
        }

        private void DecreaseBalance(int amount)
        {
            Balance -= amount;
        }

        private void IncreaseBalance(int amount)
        {
            Balance += amount;
        }

        private void SaveTransaction(int amount)
        {
            _transactionsHistory.Add(CreateTransaction(amount));
        }

        private Transaction CreateTransaction(int amount)
        {
            var date =  GetCurrentDate(Clock.Now());
             return new Transaction(date, amount,Balance);
        }

        public static string GetCurrentDate(DateTime dateTime)
        {

            return dateTime.ToString("yyyy-MM-dd");
        }

        public IEnumerable<List<Transaction>> GetTransactions() { // to avoid any changes in the list
            return (IEnumerable < List < Transaction >> )_transactionsHistory;
        }

    }
}