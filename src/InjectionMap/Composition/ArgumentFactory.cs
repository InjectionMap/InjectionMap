using System;
using System.Linq;
using System.Reflection;
using InjectionMap.Mapping;

namespace InjectionMap.Composition
{
    internal class ArgumentFactory : IDisposable
    {
        ArgumentContainer _argumentContainer;
        IMappingComponent _component;

        public ArgumentFactory(IMappingComponent component, ArgumentContainer ctx)
        {
            _argumentContainer = ctx;
            _component = component;
        }

        public void Dispose()
        {
            _argumentContainer = null;
            _component = null;
        }

        public IArgument CreateArgument(ParameterInfo param)
        {
            var argument = new ComposedArgument
            {
                Name = param.Name
            };

            if (_component.Arguments.Any())
            {
                // compile arguments to list
                var arguments = _component.Arguments.Select(
                    a => new
                    {
                        Name = a.Name,
                        Value = a.Callback != null ? a.Callback.Compile().Invoke() : a.Value
                    });

                // 1. check if argument is defined in arguments by name
                var arg = arguments.FirstOrDefault(a => a.Name == param.Name);
                if (arg != null)
                    argument.Value = arg.Value;

                // 2. check if an argument matches the type and is not used jet
                if (argument.Value == null)
                {
                    arg = arguments.FirstOrDefault(a => string.IsNullOrEmpty(a.Name) && !_argumentContainer.IsArgumentInUse(a.Value) && a.Value.GetType() == param.ParameterType);
                    if (arg != null)
                        argument.Value = arg.Value;
                }
            }

            // 3. try use injecitonresolver to resolve parameter
            if (argument.Value == null)
            {
                using (var resolver = new ComponentResolver())
                {
                    var composed = resolver.Get(param.ParameterType);
                    if (composed != null)
                        argument.Value = composed;
                }
            }

            // if value is null and no argument with the same name as the parameter return null
            // if a argument has the same name as the parameter but the value is null, it may be meant that way
            if (argument.Value == null && !_component.Arguments.Any(a => a.Name == argument.Name))
                return null;

            return argument;
        }

        
    }
}
