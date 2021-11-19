using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public delegate IInstance InstanceMethodCallback(IInstance instance, IEnumerable<IInstance> arguments);
}