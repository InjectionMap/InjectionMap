using System;
using System.Collections.Generic;

namespace InjectionMap.Internal
{
    /// <summary>
    /// Represents a baseclass for components that contain mappingexpression
    /// </summary>
    internal abstract class Component : IComponent
    {
        internal Component(Guid id)
        {
            ID = id;
            MappingConfiguration = new MappingConfiguration();
        }

        #region Properties

        /// <summary>
        /// The identification of the mapping
        /// </summary>
        public Guid ID { get; private set; }

        /// <summary>
        /// The key that the mapping is registered to
        /// </summary>
        public Type KeyType { get; internal set; }

        /// <summary>
        /// The type that will be resilved from the mapping
        /// </summary>
        Type IComponent.ValueType
        {
            get
            {
                return GetValueType();
            }
        }

        /// <summary>
        /// The configuration
        /// </summary>
        public IMappingConfiguration MappingConfiguration { get; internal set; }

        IList<IBindingArgument> _arguments;

        /// <summary>
        /// A list of arguments that will be passed to the constructor when resolving
        /// </summary>
        public IList<IBindingArgument> Arguments
        {
            get
            {
                if (_arguments == null)
                    _arguments = new List<IBindingArgument>();
                return _arguments;
            }
        }

        private IList<PropertyDefinition> _properies;

        /// <summary>
        /// Gets a list of properties that will be injected when resolving the type
        /// </summary>
        public IList<PropertyDefinition> Properies
        {
            get
            {
                if (_properies == null)
                    _properies = new List<PropertyDefinition>();
                return _properies;
            }
        }

        /// <summary>
        /// Defines if the component is a substitue for all other mappings of the same key
        /// </summary>
        public bool IsSubstitute { get; internal set; }

        /// <summary>
        /// Gets the consturctor that will be injected into
        /// </summary>
        public IConstructorDefinition ConstructorDefinition { get; internal set; }

        #endregion

        #region Implementation

        protected abstract Type GetValueType();

        #endregion

        #region IDisposeable Implementation

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        internal bool IsDisposed { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases resources held by the object.
        /// </summary>
        public virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !IsDisposed)
                {
                    IsDisposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// Releases resources before the object is reclaimed by garbage collection.
        /// </summary>
        ~Component()
        {
            Dispose(false);
        }

        #endregion
    }
}
