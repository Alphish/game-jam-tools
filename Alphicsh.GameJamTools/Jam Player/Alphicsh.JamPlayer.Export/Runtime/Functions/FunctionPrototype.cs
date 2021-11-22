using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class FunctionPrototype : BasePrototype<FunctionInstance>
    {
        // --------
        // Creation
        // --------
        
        private IReadOnlyCollection<IPrototype> ParameterTypes { get; }
        private IPrototype ReturnType { get; }
        
        private FunctionPrototype(TypeName name, IEnumerable<IPrototype> parameterTypes, IPrototype returnType)
        {
            Name = name;
            ParameterTypes = parameterTypes.ToList();
            ReturnType = returnType;
        }

        public static FunctionPrototype MatchingFunction(IFunction function)
        {
            var parameterTypes = function.Parameters.Select(parameter => parameter.Prototype).ToList();
            var returnType = function.ReturnType;
            var typeArguments = parameterTypes.Concat(new[] { returnType })
                .Select(prototype => prototype.Name)
                .ToList();
            
            var typeName = TypeName.CreateGeneric(CodeName.From("Function"), typeArguments);
            if (!KnownPrototypes.ContainsKey(typeName))
                KnownPrototypes[typeName] = new FunctionPrototype(typeName, parameterTypes, returnType);

            return KnownPrototypes[typeName];
        }

        private static IDictionary<TypeName, FunctionPrototype> KnownPrototypes { get; }
            = new Dictionary<TypeName, FunctionPrototype>();

        // ---------
        // Overrides
        // ---------
        
        public override TypeName Name { get; }

        public override IPrototype? GetCallReturnType(IEnumerable<IPrototype> argumentTypes)
        {
            var argumentTypesList = argumentTypes.ToList();
            if (argumentTypesList.Count != ParameterTypes.Count)
                return null;
                
            foreach (var parameterArgumentPair in ParameterTypes.Zip(argumentTypesList))
            {
                var parameterType = parameterArgumentPair.First;
                var argumentType = parameterArgumentPair.Second;
                if (!argumentType.IsSubtypeOf(parameterType))
                    return null;
            }
            
            return ReturnType;
        }

        public override IInstance Call(FunctionInstance instance, IEnumerable<IInstance> arguments)
        {
            return instance.InnerFunction.Call(arguments);
        }
    }
}