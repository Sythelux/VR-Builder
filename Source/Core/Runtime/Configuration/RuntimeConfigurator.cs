// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

using System;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif

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
    [Tool, GlobalClass]
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

        /// <summary>
        /// Fully qualified name of the runtime configuration used.
        /// This field is magically filled by <see cref="RuntimeConfiguratorEditor"/>
        /// </summary>
        /// <remarks>
        /// This field is filled by <see cref="RuntimeConfiguratorEditor"/>
        /// </remarks>
#if UNITY_5_3_OR_NEWER
        [SerializeField]
#endif
        private string runtimeConfigurationName = typeof(DefaultRuntimeConfiguration).AssemblyQualifiedName;

        /// <summary>
        /// Process name which is selected.
        /// </summary>
        [SerializeField]
#endif
        private string selectedProcessStreamingAssetsPath = "";

        /// <summary>
        /// String localization table used by the current process.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        [SerializeField]
#endif
        private string processStringLocalizationTable = "";
        // private string ProcessStringLocalizationTable { get; set; } = "";

        private BaseRuntimeConfiguration runtimeConfiguration;

        private static RuntimeConfigurator instance;
        private static RuntimeConfigurator[] instances;

        /// <summary>
        /// Looks up all the RuntimeConfigurator game objects in the scene.
        /// </summary>
        /// <returns>An array of RuntimeConfigurator instances found in the scene.</returns>
        private static RuntimeConfigurator[] LookUpRuntimeConfiguratorGameObjects()
        {
#if UNITY_5_3_OR_NEWER
            RuntimeConfigurator[] instances = FindObjectsOfType<RuntimeConfigurator>();
#elif GODOT
            RuntimeConfigurator[] instances = NodeExtensions.FindObjectsOfType<RuntimeConfigurator>().Take(2).ToArray();
#endif
            return instances;
        }

        /// <summary>
        /// Gets the RuntimeConfigurator instance.
        /// </summary>
        /// <returns>The active RuntimeConfigurator instance.</returns>
        /// <remarks>
        /// If there are multiple instances of the RuntimeConfigurator in the scene, the first one found will be used and
        /// a warning will be logged. 
        /// </remarks>
        private static RuntimeConfigurator GetRuntimeConfigurator()
        {
            RuntimeConfigurator[] instances = LookUpRuntimeConfiguratorGameObjects();
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
            if (instances.Length > 1)
            {
                string errorLog = $"More than one process runtime configurator found in all open scenes. The active process will be {instances[0].GetSelectedProcess()} from Scene {instances[0].gameObject.scene.name}. Ignoring following processes: ";
                for (int i = 1; i < instances.Length; i++)
                {
                    errorLog += $"{instances[i].GetSelectedProcess()} from Scene {instances[i].gameObject.scene.name}";
                    if (i < instances.Length - 1)
                    {
                        errorLog += ", ";
                    }
                }
                Debug.LogError(errorLog);
            }

            return instances[0];
        }
        /// <summary>
        /// Checks if a process runtime configurator exists in the scene.
        /// </summary>
        /// <returns><c>true</c> if an instance of the runtime configurator exists; otherwise, <c>false</c>.</returns>
        public static bool Exists
        {
            get
            {
                return IsExisting();
            }
        }

        /// <summary>
        /// Checks if an instance of the runtime configurator exists.
        /// If <see cref="instance"/> is not set it tries to set it by calling <see cref="LookUpRuntimeConfiguratorGameObject"/>.
        /// </summary>
        /// <param name="forceNewLookup">If set to <c>true</c>, forces a new lookup for the instance.</param>
        /// <returns><c>true</c> if an instance of the runtime configurator exists; otherwise, <c>false</c>.</returns>
        public static bool IsExisting(bool forceNewLookup = false)
        {
            if (instance == null || instance.Equals(null) || forceNewLookup)
            {
                instance = GetRuntimeConfigurator();
            }

            return instance != null && instance.Equals(null) == false;
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

                Configuration = (BaseRuntimeConfiguration)ReflectionUtils.CreateInstanceOfType(type);
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
        /// Gets the current instance of the RuntimeConfigurator.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if there is no RuntimeConfigurator added to the scene.</exception>
        public static RuntimeConfigurator Instance
        {
            get
            {
                if (Exists == false)
                {
                    throw new NullReferenceException("Process runtime configurator is not set in the scene. Create an empty game object with the 'RuntimeConfigurator' script attached to it.");
                }

                return instance;
            }
        }

        /// <summary>
        /// Returns the assembly qualified name of the runtime configuration.
        /// </summary>
        /// <returns>The assembly qualified name of the runtime configuration.</returns>
        public string GetRuntimeConfigurationName()
        {
            return runtimeConfigurationName;
        }

        /// <summary>
        /// Sets the runtime configuration name, expects an assembly qualified name.
        /// </summary>
        /// <param name="configurationName">The assembly qualified name of the runtime configuration.</param>
        public void SetRuntimeConfigurationName(string configurationName)
        {
            runtimeConfigurationName = configurationName;
        }

        /// <summary>
        /// Returns the path to the selected process.
        /// </summary>
        /// <returns>The path to the selected process.</returns>
        public string GetSelectedProcess()
        {
#if UNITY_5_3_OR_NEWER
            return selectedProcessStreamingAssetsPath;
#elif GODOT
            return selectedProcessStreamingAssetsPath;
#endif
        }

        /// <summary>
        /// Sets the path to the selected process.
        /// </summary>
        /// <param name="path">The path to the selected process.</param>
        public void SetSelectedProcess(string path)
        {
#if UNITY_5_3_OR_NEWER
            selectedProcessStreamingAssetsPath = path;
#elif GODOT
            selectedProcessStreamingAssetsPath = path;
#endif
        }

        /// <summary>
        /// Returns the string localization table for the selected process.
        /// </summary>
        /// <returns>The string localization table for the selected process.</returns>
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
