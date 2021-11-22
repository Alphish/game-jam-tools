using System;
using System.Linq;

using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime.Common
{
    public class BaseInstanceTests
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

        [Fact]
        public void GetMemberType_ShouldReturnMethodReturnType()
        {
            var memberName = CodeName.From("getSelf");
            var memberType = TestDummyPrototype.Prototype.GetMemberType(memberName);

            memberType.Should().Be(TestDummyPrototype.Prototype);
        }
        
        [Fact]
        public void GetMemberType_ShouldThrowExceptionForUnknownName()
        {
            var memberName = CodeName.From("unknownMember");
            Action memberGetAttempt = () => TestDummyPrototype.Prototype.GetMemberType(memberName);

            memberGetAttempt.Should().ThrowExactly<ArgumentException>()
                .Which.ParamName.Should().Be("memberName");
        }
        
        [Fact]
        public void GetMember_ShouldReturnMethodBoundToInstance()
        {
            var instance = TestDummyPrototype.Prototype.CreateInstance();
            var memberName = CodeName.From("getSelf");
            var methodMember = instance.GetMember(memberName);
            var methodResult = methodMember.Call(Enumerable.Empty<IInstance>());

            methodResult.Should().BeSameAs(instance); // getSelf returns bound instance
        }
        
        [Fact]
        public void GetMember_ShouldReturnMethodAcceptingArguments()
        {
            var instance = TestDummyPrototype.Prototype.CreateInstance();
            var memberName = CodeName.From("getArgument");
            var methodMember = instance.GetMember(memberName);

            var argument = TestDummyPrototype.Prototype.CreateInstance();
            var methodResult = methodMember.Call(new [] { argument });

            methodResult.Should().BeSameAs(argument); // getArgument returns the argument
        }

        [Fact]
        public void GetMember_ShouldThrowExceptionForUnknownName()
        {
            var instance = TestDummyPrototype.Prototype.CreateInstance();
            var memberName = CodeName.From("unknownMember");
            Action memberGetAttempt = () => instance.GetMember(memberName);

            memberGetAttempt.Should().ThrowExactly<ArgumentException>()
                .Which.ParamName.Should().Be("memberName");
        }
        
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