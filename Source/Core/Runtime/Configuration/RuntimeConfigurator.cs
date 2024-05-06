// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif

using System;
using System.Linq;
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Configuration
{
    /// <summary>
    /// Configurator to set the process runtime configuration which is used by a process during its execution.
    /// There has to be one and only one process runtime configurator game object per scene.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    public sealed class RuntimeConfigurator : MonoBehaviour
#elif GODOT
    [Tool]
    public partial class RuntimeConfigurator : Node
#endif
    {
        /// <summary>
        /// The event that fires when a process mode or runtime configuration changes.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        public static event EventHandler<ModeChangedEventArgs> ModeChanged;
#elif GODOT
        [Signal]
        public delegate void ModeChangedEventHandler(ModeChangedEventArgs eventArgs);
        // public static event EventHandler<ModeChangedEventArgs> ModeChanged;
#endif

        /// <summary>
        /// The event that fires when a process runtime configuration changes.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        public static event EventHandler<EventArgs> RuntimeConfigurationChanged;
#elif GODOT
        [Signal]
        public delegate void RuntimeConfigurationChangedEventHandler();
#endif

        /// <summary>
        /// Fully qualified name of the runtime configuration used.
        /// This field is magically filled by <see cref="RuntimeConfiguratorEditor"/>
        /// </summary>
#if UNITY_5_3_OR_NEWER
        [SerializeField]
#elif GODOT
        [Export]
#endif
        private string runtimeConfigurationName = typeof(DefaultRuntimeConfiguration).AssemblyQualifiedName;

        /// <summary>
        /// Process name which is selected.
        /// This field is magically filled by <see cref="RuntimeConfiguratorEditor"/>
        /// </summary>
#if UNITY_5_3_OR_NEWER
        [SerializeField]
        private string selectedProcessStreamingAssetsPath = "";
#elif GODOT
        [Export]
        private string SelectedProcess { get; set; } = "";
#endif

        /// <summary>
        /// String localization table used by the current process.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        [SerializeField]
#elif GODOT
        [Export]
#endif
        private string processStringLocalizationTable = "";
        // private string ProcessStringLocalizationTable { get; set; } = "";

        private BaseRuntimeConfiguration runtimeConfiguration;

        private static RuntimeConfigurator instance;

        private static RuntimeConfigurator LookUpForGameObject()
        {
#if UNITY_5_3_OR_NEWER
            RuntimeConfigurator[] instances = FindObjectsOfType<RuntimeConfigurator>();
#elif GODOT
            RuntimeConfigurator[] instances = NodeExtensions.FindObjectsOfType<RuntimeConfigurator>().Take(2).ToArray();
#endif

            if (instances.Length > 1)
            {
#if UNITY_5_3_OR_NEWER
                Debug.LogError("More than one process runtime configurator is found in the scene. Taking the first one. This may lead to unexpected behaviour.");
#elif GODOT
                GD.Print("More than one process runtime configurator is found in the scene. Taking the first one. This may lead to unexpected behaviour.");
#endif
            }

            if (instances.Length == 0)
            {
                return null;
            }

            return instances[0];
        }

        /// <summary>
        /// Checks if a process runtime configurator instance exists in scene.
        /// </summary>
        public static bool Exists
        {
            get
            {
                if (instance == null || instance.Equals(null))
                {
                    instance = LookUpForGameObject();
                }

                return (instance != null && instance.Equals(null) == false);
            }
        }

        /// <summary>
        /// Shortcut to get the <see cref="IRuntimeConfiguration"/> of the instance.
        /// </summary>
        public static BaseRuntimeConfiguration Configuration
        {
            get
            {
                if (Instance.runtimeConfiguration != null)
                {
                    return Instance.runtimeConfiguration;
                }

#if UNITY_5_3_OR_NEWER
                Type type = ReflectionUtils.GetTypeFromAssemblyQualifiedName(Instance.runtimeConfigurationName);
#elif GODOT
                string instanceRuntimeConfigurationName =
                    Instance.runtimeConfigurationName
                        .Replace(";", ","); //,->; is a temporary fix as Godot, will interpret "," as separator for field names... now we have to switch it back.
                Type? type = ReflectionUtils.GetTypeFromAssemblyQualifiedName(instanceRuntimeConfigurationName);
#endif
                if (type == null)
                {
#if UNITY_5_3_OR_NEWER
                    Debug.LogErrorFormat("IRuntimeConfiguration type '{0}' cannot be found. Using '{1}' instead.", Instance.runtimeConfigurationName, typeof(DefaultRuntimeConfiguration).AssemblyQualifiedName);
#elif GODOT
                    GD.PrintErr($"IRuntimeConfiguration type '{instanceRuntimeConfigurationName}' cannot be found. returning for now");
#endif

                    type = typeof(DefaultRuntimeConfiguration);
                }

                var config = (IRuntimeConfiguration)ReflectionUtils.CreateInstanceOfType(type);
                if (config is BaseRuntimeConfiguration configuration)
                {
                    Configuration = configuration;
                }
                else
                {
                    //TODO: Debug.LogWarning("Your runtime configuration only extends the interface IRuntimeConfiguration, please consider moving to BaseRuntimeConfiguration as base class.");
                    // Configuration = new RuntimeConfigWrapper(config);
                }

                return Instance.runtimeConfiguration;
            }
            set
            {
                if (value == null)
                {
#if UNITY_5_3_OR_NEWER
                    Debug.LogError("Process runtime configuration cannot be null.");
#elif GODOT
                    GD.PrintErr("Process runtime configuration cannot be null.");
#endif
                    return;
                }

                if (Instance.runtimeConfiguration == value)
                {
                    return;
                }

                if (Instance.runtimeConfiguration != null)
                {
                    Instance.runtimeConfiguration.Modes.ModeChanged -= RuntimeConfigurationModeChanged;
                }

                value.Modes.ModeChanged += RuntimeConfigurationModeChanged;

                Instance.runtimeConfigurationName = value.GetType().AssemblyQualifiedName;
                Instance.runtimeConfiguration = value;

                EmitRuntimeConfigurationChanged();
            }
        }

        /// <summary>
        /// Current instance of the RuntimeConfigurator.
        /// </summary>
        /// <exception cref="NullReferenceException">Will throw a NPE if there is no RuntimeConfigurator added to the scene.</exception>
        public static RuntimeConfigurator Instance
        {
            get
            {
                if (Exists == false)
                {
                    throw new NullReferenceException(
                        "Process runtime configurator is not set in the scene. Create an empty game object with the 'RuntimeConfigurator' script attached to it.");
                }

                return instance;
            }
        }

        /// <summary>
        /// Returns the assembly qualified name of the runtime configuration.
        /// </summary>
        public string GetRuntimeConfigurationName()
        {
            return runtimeConfigurationName;
        }

        /// <summary>
        /// Sets the runtime configuration name, expects an assembly qualified name.
        /// </summary>
        public void SetRuntimeConfigurationName(string configurationName)
        {
            runtimeConfigurationName = configurationName;
        }

        /// <summary>
        /// Returns the path to the selected process.
        /// </summary>
        public string GetSelectedProcess()
        {
#if UNITY_5_3_OR_NEWER
            return selectedProcessStreamingAssetsPath;
#elif GODOT
            return SelectedProcess;
#endif
        }

        /// <summary>
        /// Sets the path to the selected process.
        /// </summary>
        public void SetSelectedProcess(string path)
        {
#if UNITY_5_3_OR_NEWER
            selectedProcessStreamingAssetsPath = path;
#elif GODOT
            SelectedProcess = path;
#endif
        }

        /// <summary>
        /// Returns the string localization table for the selected process.
        /// </summary>
        public string GetProcessStringLocalizationTable()
        {
            return processStringLocalizationTable;
        }

#if UNITY_5_3_OR_NEWER
        private void Awake()
        {
            Configuration.SceneObjectRegistry.RegisterAll();
            RuntimeConfigurationChanged += HandleRuntimeConfigurationChanged;
        }
#elif GODOT
        public override void _Ready()
        {
            // TODO: Configuration.SceneObjectRegistry.RegisterAll();
            RuntimeConfigurationChanged += EmitModeChanged;
        }
#endif


#if UNITY_5_3_OR_NEWER
        private void OnDestroy()
        {
            ModeChanged = null;
            RuntimeConfigurationChanged = null;
        }
#elif GODOT
        public override void _ExitTree()
        {
            RuntimeConfigurationChanged -= EmitModeChanged;
        }
#endif

        private static void EmitModeChanged()
        {
#if UNITY_5_3_OR_NEWER
            ModeChanged?.Invoke(Instance, new ModeChangedEventArgs(Instance.runtimeConfiguration.Modes.CurrentMode));
#elif GODOT
            Instance.EmitSignal(SignalName.ModeChanged, new ModeChangedEventArgs(Instance.runtimeConfiguration.Modes.CurrentMode));
#endif
        }

        private static void EmitRuntimeConfigurationChanged()
        {
#if UNITY_5_3_OR_NEWER
            RuntimeConfigurationChanged?.Invoke(Instance, EventArgs.Empty);
#elif GODOT
            Instance.EmitSignal(SignalName.RuntimeConfigurationChanged, Array.Empty<Variant>());
#endif
        }

        private void HandleRuntimeConfigurationChanged(object sender, EventArgs args)
        {
            EmitModeChanged();
        }

        private static void RuntimeConfigurationModeChanged(object sender, ModeChangedEventArgs modeChangedEventArgs)
        {
            EmitModeChanged();
        }
    }
}
