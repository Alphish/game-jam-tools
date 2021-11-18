using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public interface IFunction
    {
        FunctionParameterList Parameters { get; }
        IPrototype ReturnType { get; }

        IInstance Call(IEnumerable<IInstance> arguments);
    }
}
