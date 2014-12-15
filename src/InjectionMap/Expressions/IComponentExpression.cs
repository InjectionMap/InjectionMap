
namespace InjectionMap
{
    /// <summary>
    /// Represents the base of a container class that containes the Context and the component for the expression
    /// </summary>
    public interface IComponentExpression
    {
        /// <summary>
        /// The context that the map is stored in
        /// </summary>
        IComponentCollection Context { get; }

        /// <summary>
        /// The component containing the map
        /// </summary>
        IMappingComponent Component { get; }
    }
}
