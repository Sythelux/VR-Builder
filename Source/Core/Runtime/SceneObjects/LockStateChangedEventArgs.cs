// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

using System;
#if GODOT
using Godot;
#endif

namespace VRBuilder.Core.SceneObjects
{
#if UNITY_5_3_OR_NEWER
    public class LockStateChangedEventArgs : EventArgs
#elif GODOT
    public partial class LockStateChangedEventArgs : Resource
#endif

    {
        public readonly bool IsLocked;

        public LockStateChangedEventArgs(bool isLocked)
        {
            IsLocked = isLocked;
        }
    }
}
