#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif

namespace VRBuilder.BasicInteraction.Validation
{
    /// <summary>
    /// Base validator used to implement concrete validators.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    public abstract class Validator : MonoBehaviour
#elif GODOT
    public abstract partial class Validator : Node
#endif
    {
        /// <summary>
        /// When this returns true, the given object is allowed to be snapped.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        public abstract bool Validate(GameObject obj);
#elif GODOT
        public abstract bool Validate(Node obj);
#endif

        private void OnEnable()
        {
            // Has to be implemented to allow disabling this script.
        }

        private void OnDisable()
        {
            // Has to be implemented to allow disabling this script.
        }
    }
}