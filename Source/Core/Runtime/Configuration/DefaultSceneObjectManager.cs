using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Configuration
{
    /// <summary>
    /// Default single-user implementation of <see cref="ISceneObjectManager"/>.
    /// </summary>
    public class DefaultSceneObjectManager : ISceneObjectManager
    {
        /// <inheritdoc/>
        public void SetSceneObjectActive(ISceneObject sceneObject, bool isActive)
        {
#if UNITY_5_3_OR_NEWER
            sceneObject.GameObject.SetActive(isActive);
#elif GODOT
            sceneObject.GameObject.SetProcess(isActive);
#endif

        }

        /// <inheritdoc/>
        public void SetComponentActive(ISceneObject sceneObject, string componentTypeName, bool isActive)
        {
#if UNITY_5_3_OR_NEWER
            IEnumerable<Component> components = sceneObject.GameObject.GetComponents<Component>().Where(c => c.GetType().Name == componentTypeName);
#elif GODOT
            IEnumerable<Node> components = sceneObject.GameObject.GetComponents<Node>()
                .Where(c => c.GetType().Name == componentTypeName);
#endif

            foreach (var component in components)
            {
                Type componentType = component.GetType();

                if (componentType.GetProperty("enabled") != null)
                {
                    componentType.GetProperty("enabled").SetValue(component, isActive, null);
                }
            }
        }

        /// <inheritdoc/>
#if UNITY_5_3_OR_NEWER
        public GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return GameObject.Instantiate(prefab, position, rotation);
        }
#elif GODOT
        public Node3D? InstantiatePrefab(PackedScene prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab.InstantiateOrNull<Node3D>() is Node3D instantiatePrefab)
            {
                instantiatePrefab.Position = position;
                instantiatePrefab.Quaternion = rotation;
                return instantiatePrefab;
            }
            return null;
        }
#endif

        /// <inheritdoc/>
        public void RequestAuthority(ISceneObject sceneObject)
        {
        }
    }
}
