namespace BankAccountKata_2_Tests.Domain
{
    public interface IDateProvider
    {
        DateOnly GetCurrentDate();
        DateTimeOffset GetDateTimeOffset();

    }
}