#if UNITY_6000_0_OR_NEWER
using UnityEngine.Events;
#elif GODOT
using Godot;
#endif


namespace VRBuilder.Core.Properties
{
    /// <summary>
    /// Interface for a property that stores a single value.
    /// </summary>
    /// <typeparam name="T">Type of the value to be stored.</typeparam>
    public interface IDataProperty<T> : IDataPropertyBase
    {
        /// <summary>
        /// Raised when the stored value changes.
        /// </summary>
#if UNITY_6000_0_OR_NEWER
        UnityEvent<T> OnValueChanged { get; }
#elif GODOT
        //TODO: Godot can't handle SignalHandler in Interfaces
#endif


        /// <summary>
        /// Returns the value.
        /// </summary>
        T GetValue();

        /// <summary>
        /// Sets the value.
        /// </summary>
        void SetValue(T value);      
    }
}
