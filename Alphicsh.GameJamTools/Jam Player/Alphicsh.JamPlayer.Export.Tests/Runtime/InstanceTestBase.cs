using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Export.Runtime
{
    public class InstanceTestBase<TInstance>
        where TInstance : IInstance
    {
        private TInstance? Instance { get; set; }
        
        protected void GivenInstance(TInstance instance)
            => Instance = instance;

        // -------------
        // Methods setup
        // -------------

        private IInstance? InstanceMethod { get; set; }
        private ICollection<IInstance> MethodArguments { get; } = new List<IInstance>();
        
        private IInstance? MethodResult { get; set; }
        
        // Givens

        protected void GivenMethodName(CodeName methodName)
            => InstanceMethod = Instance!.GetMember(methodName);
        protected void GivenMethodName(string methodName)
            => GivenMethodName(CodeName.From(methodName));

        protected void GivenNoMethodArguments() { }
        
        protected void GivenMethodArgument(IInstance argument)
            => MethodArguments.Add(argument);
        
        // Whens

        protected void WhenMethodExecuted()
            => MethodResult = InstanceMethod!.Call(MethodArguments);
        
        // Thens

        protected IInstance ThenMethodResult()
            => MethodResult!;
    }
}