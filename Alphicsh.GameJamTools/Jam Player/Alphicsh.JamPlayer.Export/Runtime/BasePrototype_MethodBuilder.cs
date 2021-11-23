using System;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamPlayer.Export.Runtime.Functions;

namespace Alphicsh.JamPlayer.Export.Runtime
{
    public partial class BasePrototype<TInstance>
    {
        protected MethodBuilder DefineMethod(string methodName)
        {
            return new MethodBuilder(this, CodeName.From(methodName));
        }
        
        protected class MethodBuilder
        {
            private BasePrototype<TInstance> Prototype { get; }
            private CodeName MethodName { get; }

            private ICollection<VariableDeclaration> Parameters { get; } = new List<VariableDeclaration>();
            private IPrototype ReturnType { get; set; } = default!;
            private InstanceMethodCallback Callback { get; set; } = default!;
        
            public MethodBuilder(BasePrototype<TInstance> prototype, CodeName methodName)
            {
                Prototype = prototype;
                MethodName = methodName;
            }

            public MethodBuilder WithParameter(string name, IPrototype prototype)
            {
                Parameters.Add(VariableDeclaration.Create(name, prototype));
                return this;
            }

            public MethodBuilder WithNoParameters()
            {
                return this;
            }

            public MethodBuilder Returning(IPrototype returnType)
            {
                ReturnType = returnType;
                return this;
            }

            public MethodBuilder Executing(InstanceMethodCallback callback)
            {
                Callback = callback;
                return this;
            }

            public void Complete()
            {
                var method = new UnboundMethod(Parameters, ReturnType, Callback);
                Prototype.Methods.Add(MethodName, method);
            }
        }
    }
}