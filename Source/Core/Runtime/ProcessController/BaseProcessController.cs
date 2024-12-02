using System;
using System.Collections.Generic;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif

namespace VRBuilder.ProcessController
{
    /// <summary>
    /// Base process controller which instantiates a defined prefab.
    /// </summary>
    public abstract class BaseProcessController : IProcessController
    {
        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public abstract int Priority { get; }

        /// <summary>
        /// Name of the process controller prefab.
        /// </summary>
        protected abstract string PrefabName { get; }

        /// <inheritdoc />
#if UNITY_5_3_OR_NEWER
        public virtual GameObject GetProcessControllerPrefab()
        {
            if (PrefabName == null)
            {
                Debug.LogError($"Could not find process controller prefab named {PrefabName}.");
                return null;
            }

            return Resources.Load<GameObject>($"Prefabs/{PrefabName}");
        }
#elif GODOT
        public virtual Node GetProcessControllerPrefab()
        {
            if (PrefabName == null)
            {
                GD.PrintErr($"Could not find process controller prefab named {PrefabName}.");
                return null;
            }

            return ResourceLoader.Load<Node>($"Prefabs/{PrefabName}");
        }
#endif

        /// <inheritdoc />
        public virtual List<Type> GetRequiredSetupComponents()
        {
            return new List<Type>();
        }

        /// <inheritdoc />
#if UNITY_5_3_OR_NEWER
        public virtual void HandlePostSetup(GameObject processControllerObject)
        {
            // do nothing
        }
#elif GODOT
        public virtual void HandlePostSetup(Node processControllerObject)
        {
            // do nothing
        }
#endif
    }
}
