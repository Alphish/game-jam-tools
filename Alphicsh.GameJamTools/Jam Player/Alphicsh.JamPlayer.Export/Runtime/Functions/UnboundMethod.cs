using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class UnboundMethod : IUnboundMethod
    {
        public IPrototype ReturnType { get; }

        private IReadOnlyCollection<FunctionParameter> Parameters { get; }
        private InstanceMethodCallback Callback { get; }
        
        public UnboundMethod(
            IEnumerable<FunctionParameter> parameters,
            IPrototype returnType,
            InstanceMethodCallback callback
        )
        {
            Parameters = parameters.ToList();
            ReturnType = returnType;
            Callback = callback;
        }
        
        public IFunction Bind(IInstance instance)
        {
            return new InstanceMethod(Parameters, ReturnType, instance, Callback);
        }
    }
}