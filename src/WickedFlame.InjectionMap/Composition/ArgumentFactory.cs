using System;
using System.Reflection;
using System.Linq;
using WickedFlame.InjectionMap.Mapping;

namespace WickedFlame.InjectionMap.Composition
{
    internal class ArgumentFactory : IDisposable
    {
        ParameterInfo _parameterInfo;

        public ArgumentFactory(ParameterInfo param)
        {
            _parameterInfo = param;
        }

        public void Dispose()
        {
            _parameterInfo = null;
        }

        public IArgument CreateArgument()
        {
            var composed = MappingManager.Get(_parameterInfo.ParameterType);

            return new ComposedArgument
            {
                Name = _parameterInfo.Name,
                Value = composed
            };
        }

        public IArgument CreateArgument(IMappingComponent component, ArgumentContainer ctx)
        {
            var argument = new ComposedArgument
            {
                Name = _parameterInfo.Name
            };

            if (component.Arguments.Any())
            {
                // compile arguments to list
                var arguments = component.Arguments.Select(
                    a => new
                    {
                        Name = a.Name,
                        Value = a.Value ?? (a.Callback != null ? a.Callback.Compile().Invoke() : null)
                    })
                    .Where(a => a.Value != null);

                // 1. check if argument is defined in arguments by name
                var arg = arguments.FirstOrDefault(a => a.Name == _parameterInfo.Name);
                if (arg != null)
                    argument.Value = arg.Value;

                // 2. check if an argument matches the type and is not used jet
                if (argument.Value == null)
                {
                    arg = arguments.FirstOrDefault(a => string.IsNullOrEmpty(a.Name) && !ctx.IsArgumentInUse(a.Value) && a.Value.GetType() == _parameterInfo.ParameterType);
                    if (arg != null)
                        argument.Value = arg.Value;
                }
            }

            // 3. try use injecitonresolver to resolve parameter
            if (argument.Value == null)
            {
                var composed = MappingManager.Get(_parameterInfo.ParameterType);
                if (composed != null)
                    argument.Value = composed;
            }

            return argument;
        }

        
    }
}
