using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class UnboundMethod : IUnboundMethod
    {
        public FunctionParameterList Parameters { get; }
        public IPrototype ReturnType { get; }

        private InstanceMethodCallback Callback { get; }
        
        public UnboundMethod(
            IEnumerable<VariableDeclaration> parameters,
            IPrototype returnType,
            InstanceMethodCallback callback
        )
        {
            Parameters = new FunctionParameterList(parameters);
            ReturnType = returnType;
            Callback = callback;
        }
        
        public IFunction Bind(IInstance instance)
        {
            return new InstanceMethod(Parameters, ReturnType, instance, Callback);
        }
    }
}