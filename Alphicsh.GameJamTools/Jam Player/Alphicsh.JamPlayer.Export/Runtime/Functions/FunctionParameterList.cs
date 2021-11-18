using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class FunctionParameterList : IReadOnlyCollection<FunctionParameter>
    {
        private IReadOnlyCollection<FunctionParameter> Items { get; }

        public FunctionParameterList(IEnumerable<FunctionParameter> parameters)
        {
            Items = parameters.ToList();
            EnsureParametersAreValid(Items);
        }

        private void EnsureParametersAreValid(IReadOnlyCollection<FunctionParameter> parameters)
        {
            // checking for duplicate parameter names
            var parameterNames = parameters
                .GroupBy(parameter => parameter.Name)
                .Where(group => group.Skip(1).Any())
                .Select(group => group.Key)
                .ToList();

            if (parameterNames.Any())
            {
                throw new ArgumentException(
                    "Duplicate function parameter names: " + string.Join(", ", parameterNames),
                    nameof(parameters)
                );
            }
        }

        // ------
        // Checks
        // ------

        public bool MatchesArgumentTypes(IEnumerable<IPrototype> declaredTypes)
        {
            var remainingParameters = Items.ToList();
            foreach (var type in declaredTypes)
            {
                if (!remainingParameters.Any())
                    return false; // too many arguments given
                
                var matchingParameter = remainingParameters.First();
                remainingParameters.RemoveAt(0);
                if (!type.IsSubtypeOf(matchingParameter.Prototype))
                    return false; // invalid argument type
            }

            if (remainingParameters.Any())
                return false; // some expected arguments are missing

            return true;
        }

        // -------------------
        // IReadOnlyCollection
        // -------------------

        public int Count => Items.Count;

        public IEnumerator<FunctionParameter> GetEnumerator()
            => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
