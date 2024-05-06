// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

using System;
using Godot;

namespace VRBuilder.Core.Configuration.Modes
{
    /// <summary>
    /// This is a <see cref="EventArgs"/> used for <see cref="IMode"/> changes.
    /// If you want so see more about EventArgs, please visit: https://docs.microsoft.com/en-us/dotnet/standard/events/
    /// </summary>
#if UNITY_5_3_OR_NEWER
    public class ModeChangedEventArgs : EventArgs
#elif GODOT
    public partial class ModeChangedEventArgs : Resource
#endif

    {
        /// <summary>
        /// The newly activated <see cref="IMode"/>.
        /// </summary>
        public IMode Mode { get; private set; }

        public ModeChangedEventArgs(IMode mode)
        {
            Mode = mode;
        }
    }
}
