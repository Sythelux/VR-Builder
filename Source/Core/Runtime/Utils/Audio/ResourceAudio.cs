// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Configuration;
using VRBuilder.Core.Localization;
using VRBuilder.Core.Settings;

namespace VRBuilder.Core.Utils.Audio
{
    /// <summary>
    /// Unity resource based audio data.
    /// </summary>
    [DataContract(IsReference = true)]
    [DisplayName("Play Audio File")]
    public class ResourceAudio : IAudioData, ILocalizedContent
    {
        private string path;

        /// <summary>
        /// File path relative to the Resources folder.
        /// </summary>
        [DataMember]
        [DisplayName("Resources path / Key")]
#if UNITY_5_3_OR_NEWER
        [DisplayTooltip("The audio clip needs to be in a folder called Resources or one of its subfolders. The path to enter here is the relative path to the Resources folder, without extension. So, if the path is 'Assets/Resources/Audio/MyFile.wav', you would need to enter 'Audio/MyFile'.")]
#endif
        public string ResourcesPath
        {
            get
            {
                return path;
            }
            set
            {
#if UNITY_5_3_OR_NEWER
                if (Application.isPlaying)
#endif
                {
                    InitializeAudioClip();
                }
            }
        }

        public ResourceAudio(string path)
        {
            ResourcesPath = path;
        }

        protected ResourceAudio()
        {
            path = "";
        }

        public bool HasAudioClip
        {
            get
            {
                return AudioClip != null;
            }
        }

        /// <inheritdoc/>
#if UNITY_5_3_OR_NEWER
        public AudioClip AudioClip { get; private set; }
#elif GODOT
        public AudioStream AudioClip { get; private set; }
#endif

        /// <inheritdoc/>
        public string ClipData
        {
            get
            {
                return ResourcesPath;
            }
            set
            {
                ResourcesPath = value;
            }
        }

        public void InitializeAudioClip()
        {
#if UNITY_5_3_OR_NEWER
            AudioClip = null;

            if (string.IsNullOrEmpty(ResourcesPath))
            {
                Debug.LogWarningFormat("Path to audio file is not defined.");
                return;
            }

            AudioClip = Resources.Load<AudioClip>(GetLocalizedContent());

            // Attempt to fallback to use the key as path.
            if (HasAudioClip == false)
            {
                AudioClip = Resources.Load<AudioClip>(ResourcesPath);
            }

            if (HasAudioClip == false)
            {
                Debug.LogWarningFormat("Given value '{0}' has returned no valid resource path for an audio clip, or it is not a valid resource path.", ResourcesPath);
            }
#elif GODOT
            AudioClip = null;

            if (string.IsNullOrEmpty(ResourcesPath))
            {
                GD.PushWarning("Path to audio file is not defined.");
                return;
            }

            AudioClip = ResourceLoader.Load<AudioStream>(GetLocalizedContent());

            // Attempt to fallback to use the key as path.
            if (HasAudioClip == false)
            {
                AudioClip = ResourceLoader.Load<AudioStream>(ResourcesPath);
            }

            if (HasAudioClip == false)
            {
                GD.PushWarning("Given value '{0}' has returned no valid resource path for an audio clip, or it is not a valid resource path.", ResourcesPath);
            }
#endif
        }

        /// <inheritdoc/>
        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(ResourcesPath);
        }

        public string GetLocalizedContent()
        {
#if UNITY_5_3_OR_NEWER
            return LanguageUtils.GetLocalizedString(ResourcesPath, RuntimeConfigurator.Instance.GetProcessStringLocalizationTable(), LanguageSettings.Instance.ActiveOrDefaultLocale);
#elif GODOT
            return TranslationServer.GetTranslationObject(LanguageSettings.Instance.ActiveOrDefaultLocale).GetMessage(ResourcesPath, RuntimeConfigurator.Instance.GetProcessStringLocalizationTable());
#endif
        }
    }
}
