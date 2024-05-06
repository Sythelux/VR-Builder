using System;
using System.Collections.Generic;
#if GODOT
using Godot;
#endif


namespace VRBuilder.Core.SceneObjects
{
    /// <summary>
    /// Event args for guid container events.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    public class GuidContainerEventArgs : EventArgs
#elif GODOT
    public partial class GuidContainerEventArgs : Resource
#endif

    {
        public readonly Guid Guid;

        public GuidContainerEventArgs(Guid guid)
        {
            Guid = guid;
        }
    }

    /// <summary>
    /// A container for a list of guids that are associated to an object.
    /// </summary>
    public interface IGuidContainer
    {
        /// <summary>
        /// Raised when a guid is added.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        event EventHandler<GuidContainerEventArgs> GuidAdded;
#elif GODOT
        [Signal]
        delegate void GuidAddedEventHandler(GuidContainerEventArgs eventArgs);
#endif


        /// <summary>
        /// Raised when a guid is removed.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        event EventHandler<GuidContainerEventArgs> GuidRemoved;
#elif GODOT
        [Signal]
        delegate void GuidRemovedEventHandler(GuidContainerEventArgs eventArgs);
#endif


        /// <summary>
        /// All guids on the object.
        /// </summary>
        IEnumerable<Guid> Guids { get; }

        /// <summary>
        /// True if the object has the specified guid.
        /// </summary>
        bool HasGuid(Guid guid);

        /// <summary>
        /// Add the specified guid.
        /// </summary>
        void AddGuid(Guid guid);

        /// <summary>
        /// Remove the specified guid.
        /// </summary>
        bool RemoveGuid(Guid guid);
    }
}
