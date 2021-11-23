using System.Linq;
using Alphicsh.JamPlayer.Export.Runtime.Functions;

namespace Alphicsh.JamPlayer.Export.Runtime
{
    public partial class BasePrototype<TInstance>
    {
        protected GetterBuilder DefineGetter(string methodName)
        {
            return new GetterBuilder(this, CodeName.From(methodName));
        }
        
        protected class GetterBuilder
        {
            private BasePrototype<TInstance> Prototype { get; }
            private CodeName MethodName { get; }

            private IPrototype ReturnType { get; set; } = default!;
            private InstanceGetterCallback Callback { get; set; } = default!;
        
            public GetterBuilder(BasePrototype<TInstance> prototype, CodeName methodName)
            {
                Prototype = prototype;
                MethodName = methodName;
            }

            public GetterBuilder Returning(IPrototype returnType)
            {
                ReturnType = returnType;
                return this;
            }

            public GetterBuilder Executing(InstanceGetterCallback callback)
            {
                Callback = callback;
                return this;
            }

            public void Complete()
            {
                InstanceMethodCallback methodCallback = (instance, parameters) => Callback(instance);
                var method = new UnboundMethod(Enumerable.Empty<VariableDeclaration>(), ReturnType, methodCallback);
                Prototype.Getters.Add(MethodName, method);
            }
        }
    }
}