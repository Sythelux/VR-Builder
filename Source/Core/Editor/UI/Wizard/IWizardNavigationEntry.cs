// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

#if UNITY_6000_0_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
// TODO: Godot shouldn't Use the wizard?
#endif


namespace VRBuilder.Core.Editor.UI.Wizard
{
    internal interface IWizardNavigationEntry
    {
        bool Selected { get; set; }
#if UNITY_6000_0_OR_NEWER
        void Draw(Rect window);
#elif GODOT
        void Draw(Control window);
#endif

    }
}
