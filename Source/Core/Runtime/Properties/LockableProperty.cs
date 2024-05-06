// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

#if UNITY_5_3_OR_NEWER
 using UnityEngine;
#elif GODOT
using Godot;
#endif
using System;
using System.Collections.Generic;
using VRBuilder.Core.Utils.Logging;
using System.Text;
using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Properties
{
    /// <summary>
    /// <see cref="ProcessSceneObjectProperty"/> which is lockable, to allow the restrictive environment to handle
    /// locking/unlocking your properties, extend this class.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    public abstract class LockableProperty : ProcessSceneObjectProperty, ILockable
#elif GODOT
    public abstract partial class LockableProperty : ProcessSceneObjectProperty, ILockable
#endif
    {
        ///  <inheritdoc/>
        public event EventHandler<LockStateChangedEventArgs> Locked;

        ///  <inheritdoc/>
        public event EventHandler<LockStateChangedEventArgs> Unlocked;

#if UNITY_5_3_OR_NEWER
        [SerializeField]
#elif GODOT
        [Export]
#endif
        private bool lockOnParentObjectLock = true;

        private List<IStepData> unlockers = new List<IStepData>();

        /// <summary>
        /// Decides if the property will be locked when the parent scene object is locked.
        /// </summary>
        public bool LockOnParentObjectLock
        {
            get => lockOnParentObjectLock;
            set => lockOnParentObjectLock = value;
        }

        /// <inheritdoc/>
        public bool IsLocked { get; private set; }

        /// <summary>
        /// On default the lockable property will use this value to determine if its locked at the end of a step.
        /// </summary>
        public virtual bool EndStepLocked { get; } = true;

#if UNITY_5_3_OR_NEWER
        protected override void OnEnable()
#elif GODOT
        public override void _EnterTree()
#endif
        {
            base.OnEnable();

#if UNITY_5_3_OR_NEWER
            SceneObject.Locked += HandleObjectLocked;
            SceneObject.Unlocked += HandleObjectUnlocked;
#elif GODOT
            if (SceneObject is ProcessSceneObject pso)
            {
                pso.Locked += HandleObjectLocked;
                pso.Unlocked += HandleObjectUnlocked;
            }
#endif
        }

#if UNITY_5_3_OR_NEWER
        protected virtual void OnDisable()
#elif GODOT
        public override void _ExitTree()
#endif
        {
#if UNITY_5_3_OR_NEWER
            SceneObject.Locked -= HandleObjectLocked;
            SceneObject.Unlocked -= HandleObjectUnlocked;
#elif GODOT
            if (SceneObject is ProcessSceneObject pso)
            {
                pso.Locked -= HandleObjectLocked;
                pso.Unlocked -= HandleObjectUnlocked;
            }
#endif

        }

        /// <inheritdoc/>
        public virtual void SetLocked(bool lockState)
        {
            if (IsLocked == lockState)
            {
                return;
            }

            IsLocked = lockState;

            InternalSetLocked(lockState);

            if (IsLocked)
            {
                if (Locked != null)
                {
                    Locked.Invoke(this, new LockStateChangedEventArgs(IsLocked));
                }
            }
            else
            {
                if (Unlocked != null)
                {
                    Unlocked.Invoke(this, new LockStateChangedEventArgs(IsLocked));
                }
            }
        }

        /// <inheritdoc/>
        public virtual void RequestLocked(bool lockState, IStepData stepData = null)
        {
            if (lockState == false && stepData != null && unlockers.Contains(stepData) == false)
            {
                unlockers.Add(stepData);
            }

            if (lockState && stepData != null && unlockers.Contains(stepData))
            {
                unlockers.Remove(stepData);
            }

            bool canLock = unlockers.Count == 0;

            if (LifeCycleLoggingConfig.Instance.LogLockState)
            {
                string lockType = lockState ? "lock" : "unlock";
                string requester = stepData == null ? "NULL" : stepData.Name;
                StringBuilder unlockerList = new StringBuilder();

                foreach (IStepData unlocker in unlockers)
                {
                    unlockerList.Append($"\n<i>{unlocker.Name}</i>");
                }

                string listUnlockers = unlockers.Count == 0 ? "" : $"\nSteps keeping this property unlocked:{unlockerList}";

#if UNITY_5_3_OR_NEWER
                Debug.Log($"<i>{this.GetType().Name}</i> on <i>{gameObject.name}</i> received a <b>{lockType}</b> request from <i>{requester}</i>." +
                    $"\nCurrent lock state: <b>{IsLocked}</b>. Future lock state: <b>{lockState && canLock}</b>{listUnlockers}");
#elif GODOT
                GD.Print($"<i>{this.GetType().Name}</i> on <i>{Name}</i> received a <b>{lockType}</b> request from <i>{requester}</i>." +
                         $"\nCurrent lock state: <b>{IsLocked}</b>. Future lock state: <b>{lockState && canLock}</b>{listUnlockers}");
#endif
            }

            SetLocked(lockState && canLock);
        }

        /// <inheritdoc/>
        public bool RemoveUnlocker(IStepData data)
        {
            return unlockers.Remove(data);
        }

#if UNITY_5_3_OR_NEWER
        private void HandleObjectUnlocked(object sender, LockStateChangedEventArgs e)
#elif GODOT
        private void HandleObjectUnlocked(LockStateChangedEventArgs e)
#endif
        {
            if (LockOnParentObjectLock && IsLocked)
            {
                SetLocked(false);
            }
        }

#if UNITY_5_3_OR_NEWER
        private void HandleObjectLocked(object sender, LockStateChangedEventArgs e)
#elif GODOT
        private void HandleObjectLocked(LockStateChangedEventArgs e)
#endif
        {
            if (LockOnParentObjectLock && IsLocked == false)
            {
                SetLocked(true);
            }
        }

        /// <summary>
        /// Handle your internal locking affairs here.
        /// </summary>
        protected abstract void InternalSetLocked(bool lockState);
    }
}
