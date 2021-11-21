using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime.Common
{
    public class BasePrototypeTests
    {
        // ----
        // Name
        // ----
        
        [Fact]
        public void ToString_ShouldContainPrototypeName()
        {
            var prototype = TestDummyPrototype.Prototype;
            var name = prototype.Name.ToString();
            var prototypeString = prototype.ToString();
            
            prototypeString.Should().Contain(name);
        }
        
        // -------
        // Subtype
        // -------
        
        [Fact]
        public void IsSubtypeOf_ShouldReturnTrueForItself()
        {
            var prototype = TestDummyPrototype.Prototype;

            prototype.IsSubtypeOf(prototype).Should().BeTrue();
        }
        
        // ------------------
        // Instance prototype
        // ------------------

        [Fact]
        public void NonGenericPrototype_ShouldBeSameAsGenericPrototype()
        {
            var instance = TestDummyPrototype.Prototype.CreateInstance();

            ((IInstance)instance).Prototype.Should().BeSameAs(instance.Prototype);
        }
        
        // -------
        // Members
        // -------
        
        // to be implemented...
        
        // ---------------
        // Items and calls
        // ---------------

        [Fact]
        public void GetItemType_ShouldBeUnsupportedWhenNotOverriden()
        {
            var prototype = TestDummyPrototype.Prototype;
            Action attempt = () => prototype.GetItemType();

            attempt.Should().ThrowExactly<NotSupportedException>();
        }
        
        [Fact]
        public void GetItem_ShouldBeUnsupportedWhenNotOverriden()
        {
            var instance = TestDummyPrototype.Prototype.CreateInstance();
            Action attempt = () => instance.GetItem(0);

            attempt.Should().ThrowExactly<NotSupportedException>();
        }
        
        [Fact]
        public void UntypedGetItem_ShouldBeUnsupportedWhenNotOverriden()
        {
            var prototype = TestDummyPrototype.Prototype;
            Action attempt = () => ((IPrototype)prototype).GetItem(prototype.CreateInstance(), 0);

            attempt.Should().ThrowExactly<NotSupportedException>();
        }
        
        [Fact]
        public void GetCallReturnType_ShouldBeUnsupportedWhenNotOverriden()
        {
            var prototype = TestDummyPrototype.Prototype;
            Action attempt = () => prototype.GetCallReturnType(Enumerable.Empty<IPrototype>());

            attempt.Should().ThrowExactly<NotSupportedException>();
        }
        
        [Fact]
        public void Call_ShouldBeUnsupportedWhenNotOverriden()
        {
            var instance = TestDummyPrototype.Prototype.CreateInstance();
            Action attempt = () => instance.Call(Enumerable.Empty<IInstance>());

            attempt.Should().ThrowExactly<NotSupportedException>();
        }
        
        [Fact]
        public void UntypedCall_ShouldBeUnsupportedWhenNotOverriden()
        {
            var prototype = TestDummyPrototype.Prototype;
            Action attempt = () => ((IPrototype)prototype).Call(prototype.CreateInstance(), Enumerable.Empty<IInstance>());

            attempt.Should().ThrowExactly<NotSupportedException>();
        }
    }
}