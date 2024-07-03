// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

using System.Linq;
using VRBuilder.Core.Properties;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
using VRBuilder.Unity;
#elif GODOT
using VRBuilder.Core.Godot;
using VRBuilder.Core.Godot.Attributes;
using Godot;
#endif

namespace VRBuilder.Core.Utils
{
    /// <summary>
    /// Handles locking of all process objects in the scene and makes them non-interactable before the process is started.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    public class LockObjectsOnSceneStart : MonoBehaviour
#elif GODOT
    public partial class LockObjectsOnSceneStart : Node
#endif
    {
#if UNITY_5_3_OR_NEWER
        [SerializeField]
#elif GODOT
        [Export]
#endif
        [Tooltip("Lock all process objects in the scene and makes them non-interactable before the process is started.")]
        private bool lockSceneObjectsOnSceneStart = true;

        // Start is called before the first frame update
#if UNITY_5_3_OR_NEWER
        void Start()
#elif GODOT
        public override void _Ready()
#endif
        {
            foreach(LockableProperty lockable in SceneUtils.GetActiveAndInactiveComponents<LockableProperty>())
            {
                if(lockable.LockOnParentObjectLock)
                {
                    lockable.SetLocked(lockSceneObjectsOnSceneStart);
                }
            }
        }
    }
}
