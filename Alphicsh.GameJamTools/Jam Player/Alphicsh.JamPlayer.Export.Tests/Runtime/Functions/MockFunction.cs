using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class MockFunction : BaseFunction
    {
        private IInstance Result { get; }
            
        public MockFunction(IEnumerable<VariableDeclaration> parameterList, IPrototype returnType, IInstance result)
            : base(parameterList, returnType)
        {
            Result = result;
        }

        protected override IInstance DoCall(IEnumerable<IInstance> arguments)
        {
            return Result;
        }
    }
}