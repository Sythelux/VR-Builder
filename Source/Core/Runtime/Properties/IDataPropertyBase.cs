#if UNITY_6000_0_OR_NEWER
using UnityEngine.Events;
#elif GODOT
// TODO: implement Godot Version
#endif


namespace VRBuilder.Core.Properties
{
    /// <summary>
    /// Base interface for a property that stores a single value.
    /// </summary>
    public interface IDataPropertyBase : ISceneObjectProperty
    {
        /// <summary>
        /// Raised when the stored value is reset to the default.
        /// </summary>
#if UNITY_6000_0_OR_NEWER
        UnityEvent OnValueReset { get; }
#elif GODOT
        // TODO: Godot can't handle Signal handler in Interfaces
#endif


        /// <summary>
        /// Resets the value to its default.
        /// </summary>
        void ResetValue();
    }
}

