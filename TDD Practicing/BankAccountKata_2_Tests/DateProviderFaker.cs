using BankAccountKata_2_Tests.Domain;
using FluentAssertions.Extensions;
using System;

namespace BankAccountKata_2_Tests
{
    public class FakeDateProvider : IDateProvider
    {
        public DateTimeOffset dateTimeOffset;

        public FakeDateProvider(DateTimeOffset dateTimeOffset) { 
            this.dateTimeOffset = dateTimeOffset;
        }

        public DateOnly GetCurrentDate()
        {
           return DateOnly.FromDateTime(dateTimeOffset.DateTime);
        }

        public DateTimeOffset GetDateTimeOffset()
        {
            return dateTimeOffset;
        }

       
    }
}