// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

using System;
using System.Collections.Generic;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif

namespace VRBuilder.Core.Input
{
    /// <summary>
    /// Base class for InputActionListener.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    public abstract class InputActionListener : MonoBehaviour, IInputActionListener
#elif GODOT
    public abstract partial class InputActionListener : Node, IInputActionListener
#endif
    {
        /// <inheritdoc/>
        public virtual int Priority { get; } = 1000;

        /// <inheritdoc/>
        public virtual bool IgnoreFocus { get; } = false;

        /// <summary>
        /// Registers the given method as input event, the name of the method will be the event name.
        /// </summary>
        protected virtual void RegisterInputEvent(Action<InputController.InputEventArgs> action)
        {
#if UNITY_5_3_OR_NEWER
            InputController.Instance.RegisterEvent(this, action);
#elif GODOT
            actions.Add(action);
#endif
        }

        /// <summary>
        /// Unregisters the given method as input event, the name of the method will be the event name.
        /// </summary>
        protected virtual void UnregisterInputEvent(Action<InputController.InputEventArgs> action)
        {
#if UNITY_5_3_OR_NEWER
            InputController.Instance.UnregisterEvent(this, action);
#elif GODOT
            actions.Remove(action);
#endif
        }

#if GODOT
        private readonly List<Action<InputController.InputEventArgs>> actions = new List<Action<InputController.InputEventArgs>>();

        public override void _UnhandledInput(InputEvent @event)
        {
            foreach (var action in actions)
                action.Invoke(new InputController.InputEventArgs(@event));
        }
#endif
    }
}
