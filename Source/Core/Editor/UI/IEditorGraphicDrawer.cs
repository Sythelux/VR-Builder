﻿// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

#if UNITY_6000_0_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif


namespace VRBuilder.Core.Editor.UI
{
    /// <summary>
    /// Allows to draws over the normal EditorGraphics.
    /// </summary>
    internal interface IEditorGraphicDrawer
    {
        /// <summary>
        /// Draw priority, lower numbers will be drawn first.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Your draw call.
        /// </summary>
#if UNITY_6000_0_OR_NEWER
        void Draw(Rect windowRect);
#elif GODOT
        void Draw(Control windowRect);
#endif

    }
}
