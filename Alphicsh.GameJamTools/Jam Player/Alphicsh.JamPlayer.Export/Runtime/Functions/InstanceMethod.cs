using System;
using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class InstanceMethod : BaseFunction
    {
        private IInstance Instance { get; }
        private InstanceMethodCallback Callback { get; }
        
        public InstanceMethod(
            IEnumerable<VariableDeclaration> parameters,
            IPrototype returnType,
            IInstance instance,
            InstanceMethodCallback callback
        ) : base(parameters, returnType)
        {
            Instance = instance;
            Callback = callback;
        }

        protected override IInstance DoCall(IEnumerable<IInstance> arguments)
        {
            return Callback.Invoke(Instance, arguments);
        }
    }
}