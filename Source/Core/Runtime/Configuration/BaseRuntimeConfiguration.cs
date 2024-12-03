// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
using FileAccess = Godot.FileAccess;
#endif
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.IO;
using VRBuilder.Core.Properties;
using VRBuilder.Core.RestrictiveEnvironment;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Serialization;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Configuration
{
    /// <summary>
    /// Base class for your runtime process configuration. Extend it to create your own.
    /// </summary>
#pragma warning disable 0618
#if UNITY_5_3_OR_NEWER
    public abstract class BaseRuntimeConfiguration
#elif GODOT
    [Tool]
    public abstract partial class BaseRuntimeConfiguration : Resource
#endif
    {
#pragma warning restore 0618
        /// <summary>
        /// Name of the manifest file that could be used to save process asset information.
        /// </summary>
        public static string ManifestFileName => "ProcessManifest";

        private ISceneObjectRegistry sceneObjectRegistry = new SceneObjectRegistryV2();
        private ISceneConfiguration sceneConfiguration;

        /// <inheritdoc />
        public virtual ISceneObjectRegistry SceneObjectRegistry
        {
            get
            {
                if (sceneObjectRegistry == null)
                {
                    sceneObjectRegistry = new SceneObjectRegistryV2();
                }

                return sceneObjectRegistry;
            }
        }

        /// <inheritdoc />
        public IProcessSerializer Serializer { get; set; } = new NewtonsoftJsonProcessSerializerV4();

        /// <summary>
        /// Default input action asset which is used when no customization of key bindings are done.
        /// Should be stored inside the VR Builder package.
        /// </summary>
        public virtual string DefaultInputActionAssetPath { get; } = "KeyBindings/BuilderDefaultKeyBindings";

        /// <summary>
        /// Custom InputActionAsset path which is used when key bindings are modified.
        /// Should be stored in project path.
        /// </summary>
        public virtual string CustomInputActionAssetPath { get; } = "KeyBindings/BuilderCustomKeyBindings";

#if ENABLE_INPUT_SYSTEM && INPUT_SYSTEM_PACKAGE
        private UnityEngine.InputSystem.InputActionAsset inputActionAsset;

        /// <summary>
        /// Current active InputActionAsset.
        /// </summary>
        public virtual UnityEngine.InputSystem.InputActionAsset CurrentInputActionAsset
        {
            get
            {
                if (inputActionAsset == null)
                {
                    inputActionAsset = Resources.Load<UnityEngine.InputSystem.InputActionAsset>(CustomInputActionAssetPath);
                    if (inputActionAsset == null)
                    {
                        inputActionAsset = Resources.Load<UnityEngine.InputSystem.InputActionAsset>(DefaultInputActionAssetPath);
                    }
                }

                return inputActionAsset;
            }

            set => inputActionAsset = value;
        }
#endif

        /// <inheritdoc />
        public IModeHandler Modes { get; protected set; }

        // /// <inheritdoc />
        // [Obsolete("This property is obsolete and no longer returns a valid value. Use LocalUser instead.", true)]
        // public abstract ProcessSceneObject User { get; }

        /// <inheritdoc />
        public abstract UserSceneObject LocalUser { get; }

        /// <inheritdoc />
#if UNITY_5_3_OR_NEWER
        public abstract AudioSource InstructionPlayer { get; }
#elif GODOT
        public abstract AudioStreamPlayer InstructionPlayer { get; }
#endif


        /// <summary>
        /// Determines the property locking strategy used for this runtime configuration.
        /// </summary>
        public StepLockHandlingStrategy StepLockHandling { get; set; }

        /// <inheritdoc />
        public abstract IEnumerable<UserSceneObject> Users { get; }

        /// <inheritdoc />
        public abstract IProcessAudioPlayer ProcessAudioPlayer { get; }

        /// <inheritdoc />
        public abstract ISceneObjectManager SceneObjectManager { get; }

        /// <inheritdoc />
        public virtual ISceneConfiguration SceneConfiguration
        {
            get
            {
                if (sceneConfiguration == null)
                {
#if UNITY_5_3_OR_NEWER
                    ISceneConfiguration configuration = RuntimeConfigurator.Instance.gameObject.GetComponent<ISceneConfiguration>();

                    if (configuration == null)
                    {
                        configuration = RuntimeConfigurator.Instance.gameObject.AddComponent<SceneConfiguration>();
                    }
#elif GODOT
                    ISceneConfiguration configuration = RuntimeConfigurator.Instance.GetComponent<ISceneConfiguration>();

                    if (configuration == null) configuration = RuntimeConfigurator.Instance.AddComponent<SceneConfiguration>();
#endif


                    sceneConfiguration = configuration;
                }

                return sceneConfiguration;
            }
        }

        protected BaseRuntimeConfiguration() : this(new DefaultStepLockHandling())
        {
        }

        protected BaseRuntimeConfiguration(StepLockHandlingStrategy lockHandling)
        {
            StepLockHandling = lockHandling;
        }

        /// <inheritdoc />
        public virtual async Task<IProcess> LoadProcess(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    throw new ArgumentException("Given path is null or empty!");
                }

                int index = path.LastIndexOf("/");
                string processFolder = path.Substring(0, index);
                string processName = GetProcessNameFromPath(path);
                string manifestPath = $"{processFolder}/{ManifestFileName}.{Serializer.FileFormat}";

                IProcessAssetManifest manifest = await FetchManifest(processName, manifestPath);
                IProcessAssetStrategy assetStrategy = ReflectionUtils.CreateInstanceOfType(ReflectionUtils.GetConcreteImplementationsOf<IProcessAssetStrategy>().FirstOrDefault(type => type.FullName == manifest.AssetStrategyTypeName)) as IProcessAssetStrategy;

                string processAssetPath = $"{processFolder}/{manifest.ProcessFileName}.{Serializer.FileFormat}";
#if UNITY_5_3_OR_NEWER
                byte[] processData = await FileManager.Read(processAssetPath);
#elif GODOT
                byte[] processData = FileAccess.GetFileAsBytes(processAssetPath);
#endif

                List<byte[]> additionalData = await GetAdditionalProcessData(processFolder, manifest);

                return assetStrategy.GetProcessFromSerializedData(processData, additionalData, Serializer);
            }
            catch (Exception exception)
            {
#if UNITY_5_3_OR_NEWER
                Debug.LogError($"Error when loading process. {exception.GetType().Name}, {exception.Message}\n{exception.StackTrace}", RuntimeConfigurator.Instance.gameObject);
#elif GODOT
                GD.PrintErr($"Error when loading process. {exception.GetType().Name}, {exception.Message}\n{exception.StackTrace}", RuntimeConfigurator.Instance);
#endif

            }

            return null;
        }

        private async Task<List<byte[]>> GetAdditionalProcessData(string processFolder, IProcessAssetManifest manifest)
        {
            List<byte[]> additionalData = new List<byte[]>();
            foreach (string fileName in manifest.AdditionalFileNames)
            {
                string filePath = $"{processFolder}/{fileName}.{Serializer.FileFormat}";

#if UNITY_5_3_OR_NEWER
                if (await FileManager.Exists(filePath))
                {
                    additionalData.Add(await FileManager.Read(filePath));
                }
                else
                {
                    Debug.Log($"Error loading process. File not found: {filePath}");
                }
#elif GODOT
                if (FileAccess.FileExists(filePath))
                    additionalData.Add(FileAccess.GetFileAsBytes(filePath));
                else
                    GD.PrintErr($"Error loading process. File not found: {filePath}");
#endif

            }

            return additionalData;
        }

        private async Task<IProcessAssetManifest> FetchManifest(string processName, string manifestPath)
        {
            IProcessAssetManifest manifest;

#if UNITY_5_3_OR_NEWER
            if (await FileManager.Exists(manifestPath))
            {
                byte[] manifestData = await FileManager.Read(manifestPath);
                manifest = Serializer.ManifestFromByteArray(manifestData);
            }
#elif GODOT
            if (FileAccess.FileExists(manifestPath))
            {
                byte[] manifestData = FileAccess.GetFileAsBytes(manifestPath);
                manifest = Serializer.ManifestFromByteArray(manifestData);
            }
#endif

            else
            {
                manifest = new ProcessAssetManifest()
                {
                    AssetStrategyTypeName = typeof(SingleFileProcessAssetStrategy).FullName,
                    ProcessFileName = processName,
                    AdditionalFileNames = new string[0],
                };
            }

            return manifest;
        }

        private static string GetProcessNameFromPath(string path)
        {
            int slashIndex = path.LastIndexOf('/');
            string fileName = path.Substring(slashIndex + 1);
            int pointIndex = fileName.LastIndexOf('.');
            fileName = fileName.Substring(0, pointIndex);

            return fileName;
        }
    }

    public class EmptyStepLockHandling : StepLockHandlingStrategy
    {
        public override void Unlock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked)
    {
    }

        public override void Lock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked)
    {
    }
    }
}
