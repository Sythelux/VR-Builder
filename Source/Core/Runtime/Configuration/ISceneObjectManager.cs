#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif
using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Configuration
{
    /// <summary>
    /// Object that handles scene object operations, e.g. enabling/disabling them.
    /// </summary>
    public interface ISceneObjectManager
    {
        /// <summary>
        /// Set the specified scene object enabled or disabled.
        /// </summary>
        void SetSceneObjectActive(ISceneObject sceneObject, bool isActive);

        /// <summary>
        /// Sets all components of a given type on a specified scene object enabled or disabled.
        /// </summary>
        void SetComponentActive(ISceneObject sceneObject, string componentTypeName, bool isActive);

        /// <summary>
        /// Instantiates the specified prefab.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation);
#elif GODOT
        Node3D? /*GameObject*/ InstantiatePrefab(PackedScene /*GameObject*/ prefab, Vector3 position, Quaternion rotation);
#endif

        /// <summary>
        /// Requests authority on the specified scene object.
        /// </summary>        
        void RequestAuthority(ISceneObject sceneObject);
    }
}
