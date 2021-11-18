using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public abstract class BaseFunction : IFunction
    {
        public FunctionParameterList Parameters { get; }
        public IPrototype ReturnType { get; }

        protected BaseFunction(IEnumerable<FunctionParameter> parameters, IPrototype returnType)
        {
            Parameters = new FunctionParameterList(parameters);
            ReturnType = returnType;
        }

        public IInstance Call(IEnumerable<IInstance> arguments)
        {
            var argumentsList = arguments as IReadOnlyCollection<IInstance> ?? arguments.ToList();
            
            var argumentTypes = argumentsList.Select(argument => argument.Prototype);
            if (!Parameters.MatchesArgumentTypes(argumentTypes))
                throw new ArgumentException("The given argument types don't match the function parameters.", nameof(arguments));

            var result = DoCall(argumentsList);
            if (!result.Prototype.IsSubtypeOf(ReturnType))
                throw new InvalidCastException("The result returned by the function doesn't match declared return type.");

            return result;
        }

        protected abstract IInstance DoCall(IEnumerable<IInstance> arguments);
    }
}
