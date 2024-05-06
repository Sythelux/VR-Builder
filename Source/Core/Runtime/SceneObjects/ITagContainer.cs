using System;
using System.Collections.Generic;

namespace VRBuilder.Core.SceneObjects
{
    /// <summary>
    /// Event args for taggable objects events.
    /// </summary>
    [Obsolete("Will be removed with the next major version.")]
    public class TaggableObjectEventArgs : EventArgs
    {
        public readonly Guid Tag;

        public TaggableObjectEventArgs(Guid tag)
        {
            Tag = tag;
        }
    }

    /// <summary>
    /// A container for a list of guids that are associated to an object as tags.
    /// </summary>
    [Obsolete("Use IGuidContainer instead.")]
    public interface ITagContainer
    {
        /// <summary>
        /// Raised when a tag is added.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        event EventHandler<GuidContainerEventArgs> TagAdded;
#elif GODOT
        public delegate void TagAddedEventHandler(string tag);
#endif

        /// <summary>
        /// Raised when a tag is removed.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        event EventHandler<GuidContainerEventArgs> TagRemoved;
#elif GODOT
        public delegate void TagRemovedEventHandler(string tag);
#endif

        /// <summary>
        /// All tags on the object.
        /// </summary>
        IEnumerable<Guid> Tags { get; }

        /// <summary>
        /// True if the object has the specified tag.
        /// </summary>
        bool HasTag(Guid tag);

        /// <summary>
        /// Add the specified tag.
        /// </summary>        
        void AddTag(Guid tag);

        /// <summary>
        /// Remove the specified tag.
        /// </summary>
        bool RemoveTag(Guid tag);
    }
}
