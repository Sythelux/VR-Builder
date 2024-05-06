#if UNITY_5_3_OR_NEWER
using System;
using System.Runtime.Serialization;

namespace VRBuilder.Core.SceneObjects
{
    /// <summary>
    /// Step inspector reference to a <see cref="SceneObjectTagBase"/> requiring a specific property.
    /// </summary>    
    [DataContract(IsReference = true)]
    [Obsolete("Use ProcessSceneReference and its derived classes instead.")]
    public sealed class SceneObjectTag<T> : SceneObjectTagBase
    {
        public SceneObjectTag() : base()
        {
        }

        public SceneObjectTag(Guid guid) : base(guid)
        {
        }

        /// <inheritdoc />
        internal override Type GetReferenceType()
        {
            return typeof(T);
        }
    }
}

#elif GODOT
using Godot;
//TODO
#endif
