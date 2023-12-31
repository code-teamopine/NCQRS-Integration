﻿using System;
using FluentAssertions;
using Xunit;

namespace Ncqrs.Tests
{
    
    public class DateTimeBasedClockSpecs
    {
        [Fact]
        public void When_getting_the_current_time_it_should_be_a_utc_kind()
        {
            var clock = new DateTimeBasedClock();
            var currentTime = clock.UtcNow();

            currentTime.Kind.Should().Be(DateTimeKind.Utc);
        }

        [Fact]
        public void When_getting_the_current_time_it_should_be_the_same_as_the_result_from_the_DateTime_class()
        {
            var clock = new DateTimeBasedClock();
            
            var currentClockTime = clock.UtcNow();
            var currentDateTimeTime = DateTime.UtcNow;

            currentClockTime.Should().Be(currentDateTimeTime);
        }
    }
}
