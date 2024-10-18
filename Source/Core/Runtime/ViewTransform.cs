using System.Runtime.Serialization;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif

namespace VRBuilder.Core
{
    /// <summary>
    /// Stores position and scale in a viewport.
    /// </summary>
    [DataContract(IsReference = true)]
    public class ViewTransform
    {
        [DataMember]
        public Vector3 Position { get; set; }

        [DataMember]
        public Vector3 Scale { get; set; }

        public ViewTransform(Vector3 position, Vector3 scale)
        {
            Position = position;
            Scale = scale;
        }
    }
}
