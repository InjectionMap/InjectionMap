using System.Collections.Generic;

namespace InjectionMap.Internal
{
    /// <summary>
    /// The componentcontainer containes the list of mappingcomponents that the mappingcontext uses to store the expression in. This class is used to keep a named reference to the components of a context.
    /// </summary>
    class ComponentContainer
    {
        public string Context { get; set; }

        public IList<IMappingComponent> Components { get; set; }
    }
}
