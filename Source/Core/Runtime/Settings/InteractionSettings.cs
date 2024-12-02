#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Settings
{
    /// <summary>
    /// Settings related to VR Builder's default interaction behaviour.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    [CreateAssetMenu(fileName = "InteractionSettings", menuName = "VR Builder/InteractionSettings", order = 1)]
    public class InteractionSettings : SettingsObject<InteractionSettings>
    {
        [SerializeField]
        [Tooltip("If enabled, objects with a Grabbable Property will not react to physics and therefore will not fall to the ground when released.")]
        public bool MakeGrabbablesKinematic = false;
    }
#elif GODOT
    public partial class InteractionSettings : SettingsObject<InteractionSettings>
    {
    }
#endif
}