﻿using System;
using Ncqrs.Domain;
using Ncqrs.Eventing.Sourcing;
using Xunit;
using FluentAssertions;

namespace Ncqrs.Tests.Domain
{
    
    public class TypeThresholdedActionBasedInternalEventHandlerSpecs
    {
        public class FooEvent
        {
        }

        public class BarEvent : FooEvent
        {
        }

        public  TypeThresholdedActionBasedInternalEventHandlerSpecs()
        {
            NcqrsEnvironment.Deconfigure();
        }
        
        [Fact]
        public void Threshold_should_hold_event_when_it_is_of_a_higher_type_when_exact_is_true()
        {
            Boolean handlerActionWasCalled = false;
            Action<object> handlerAction = (e) => handlerActionWasCalled = true;

            var handler = new TypeThresholdedActionBasedDomainEventHandler(handlerAction, typeof (FooEvent), "", true);
            var handeled = handler.HandleEvent(new BarEvent());

            handeled.Should().Be(false);
            handlerActionWasCalled.Should().Be(false);
        }

        [Fact]
        public void Threshold_should_not_hold_event_when_it_is_of_a_higher_type_when_exact_is_false()
        {
            Boolean handlerActionWasCalled = false;
            Action<object> handlerAction = (e) => handlerActionWasCalled = true;

            var handler = new TypeThresholdedActionBasedDomainEventHandler(handlerAction, typeof(FooEvent), "", false);
            var handeled = handler.HandleEvent(new BarEvent());

            handeled.Should().Be(true);
            handlerActionWasCalled.Should().Be(true);
        }

        [Fact]
        public void Threshold_should_hold_event_when_it_is_of_the_same_type_when_exact_is_true()
        {
            Boolean handlerActionWasCalled = false;
            Action<object> handlerAction = (e) => handlerActionWasCalled = true;

            var handler = new TypeThresholdedActionBasedDomainEventHandler(handlerAction, typeof(FooEvent), "", true);
            var handeled = handler.HandleEvent(new FooEvent());

            handeled.Should().Be(true);
            handlerActionWasCalled.Should().Be(true);
        }
        
        [Fact]
        public void Threshold_should_hold_event_when_it_is_of_a_lower_type_when_exact_is_true()
        {
            Boolean handlerActionWasCalled = false;
            Action<object> handlerAction = (e) => handlerActionWasCalled = true;

            var handler = new TypeThresholdedActionBasedDomainEventHandler(handlerAction, typeof(BarEvent), "", true);
            var handeled = handler.HandleEvent(new FooEvent());

            handeled.Should().Be(false);
            handlerActionWasCalled.Should().Be(false);
        }

        [Fact]
        public void Threshold_should_hold_event_when_it_is_of_a_lower_type_when_exact_is_false()
        {
            Boolean handlerActionWasCalled = false;
            Action<object> handlerAction = (e) => handlerActionWasCalled = true;

            var handler = new TypeThresholdedActionBasedDomainEventHandler(handlerAction, typeof(BarEvent), "", false);
            var handeled = handler.HandleEvent(new FooEvent());

            handeled.Should().Be(false);
            handlerActionWasCalled.Should().Be(false);
        }
    }
}
