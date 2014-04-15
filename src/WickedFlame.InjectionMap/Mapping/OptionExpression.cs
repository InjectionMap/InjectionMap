using System;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class OptionExpression : IOptionExpression
    {
        public IMappingOption MappingOption { get; internal set; }

        public IOptionExpression WithOptions(InjectionFlags option)
        {
            var resolveInstanceOnMapping = (option & InjectionFlags.ResolveInstanceOnMapping) == InjectionFlags.ResolveInstanceOnMapping;
            var keepInstance = (option & InjectionFlags.KeepInstance) == InjectionFlags.KeepInstance;

            if (resolveInstanceOnMapping && !keepInstance)
                keepInstance = true;

            return new OptionExpression
            {
                MappingOption = new MappingOption
                {
                    ResolveInstanceOnMapping = resolveInstanceOnMapping,
                    KeepInstance = keepInstance,
                    WithoutOverwrite = !((option & InjectionFlags.WithOverwrite) == InjectionFlags.WithOverwrite)
                }
            };
        }
    }
}
