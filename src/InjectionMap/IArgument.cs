
namespace InjectionMap
{
    /// <summary>
    /// Represents an Argument that can be passed to a parameter
    /// </summary>
    public interface IArgument
    {
        /// <summary>
        /// Name of the parameter
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Value to be passed to the parameter
        /// </summary>
        object Value { get; }
    }
}
