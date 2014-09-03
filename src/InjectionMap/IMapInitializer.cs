using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InjectionMap
{
    /// <summary>
    /// Represents a class that is used to register all mappings
    /// </summary>
    public interface IMapInitializer
    {
        /// <summary>
        /// Gets called to register all mappings
        /// </summary>
        /// <param name="container">The <see cref="IMappingProvider"/> that is used to register the mappings</param>
        void InitializeMap(IMappingProvider container);
    }

    /// <summary>
    /// Represents a class that is used to register all mappings
    /// </summary>
    [Obsolete("IInjectionMapping was renamed to IMapInitialier", true)]
    public interface IInjectionMapping
    {
        /// <summary>
        /// Gets called to register all mappings
        /// </summary>
        /// <param name="container">The <see cref="IMappingProvider"/> that is used to register the mappings</param>
        void InitializeMap(IMappingProvider container);
    }
}
