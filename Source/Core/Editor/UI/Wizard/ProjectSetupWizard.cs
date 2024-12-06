#if UNITY_6000_0_OR_NEWER
// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VRBuilder.Core.Configuration;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Editor.UI.Wizard
{
    /// <summary>
    /// Wizard which guides the user through setting up a new project,
    /// including a process, scene and XR hardware.
    /// </summary>
    [InitializeOnLoad]
    public static class ProjectSetupWizard
    {
        /// <summary>
        /// Will be called when the VR Builder Setup wizard is closed.
        /// </summary>
        public static event EventHandler<EventArgs> SetupFinished;

        static ProjectSetupWizard()
        {
            if (Application.isBatchMode == false)
            {
        }
        }

        [MenuItem("Tools/VR Builder/Project Setup Wizard...", false, 0)]
        internal static void Show()
        {
            WizardWindow wizard = EditorWindow.CreateInstance<WizardWindow>();
            List<WizardPage> pages = new List<WizardPage>()
            {
                new WelcomePage(),
                new InteractionSettingsPage(),
                new LocalizationSettingsPage(),
                new AllAboutPage()
            };

            int xrSetupIndex = 2;
            int interactionComponentSetupIndex = 1;
            bool isShowingInteractionComponentPage = ReflectionUtils.GetConcreteImplementationsOf<IInteractionComponentConfiguration>().Count() != 1;

            if (isShowingInteractionComponentPage)
            {
                pages.Insert(interactionComponentSetupIndex, new InteractionComponentPage());
            }

            wizard.WizardClosing += OnWizardClosing;

            wizard.Setup("VR Builder - Project Setup Wizard", pages);
            wizard.ShowUtility();
        }

        private static bool IsXRInteractionComponent()
        {
            Type interactionComponentType = ReflectionUtils.GetConcreteImplementationsOf<IInteractionComponentConfiguration>().First();
            IInteractionComponentConfiguration interactionComponentConfiguration = ReflectionUtils.CreateInstanceOfType(interactionComponentType) as IInteractionComponentConfiguration;
            return interactionComponentConfiguration.IsXRInteractionComponent;
        }

        private static void OnWizardClosing(object sender, EventArgs args)
        {
            ((WizardWindow)sender).WizardClosing -= OnWizardClosing;
            SetupFinished?.Invoke(sender, args);
        }
    }
}

#endif
