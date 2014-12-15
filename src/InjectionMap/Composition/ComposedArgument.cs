
namespace InjectionMap.Composition
{
    /// <summary>
    /// Represents an Argument that can be passed to a parameter
    /// </summary>
    internal class ComposedArgument : IArgument
    {
        /// <summary>
        /// Name of the parameter
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Value to be passed to the parameter
        /// </summary>
        public object Value { get; internal set; }
    }
}
