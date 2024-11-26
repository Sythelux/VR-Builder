// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

using VRBuilder.Core.Runtime.Properties;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif

namespace VRBuilder.Core.Utils.Audio
{
    /// <summary>
    /// This class provides audio data in form of an AudioClip. Which also might not be loaded at the time needed,
    /// check first if there can be one provided.
    /// </summary>
    public interface IAudioData : ICanBeEmpty
    {
        /// <summary>
        /// Determs if the AudioSource has an AudioClip which can be played.
        /// </summary>
        bool HasAudioClip { get; }

        /// <summary>
        /// Data used to retrieve the audio clip.
        /// </summary>
        string ClipData { get; set; }

        /// <summary>
        /// The AudioClip of this source, can be null. Best check first with HasAudio.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        AudioClip AudioClip { get; }
#elif GODOT
        AudioStream AudioClip { get; }
#endif

        /// <summary>
        /// Initializes the audio clip from the given data.
        /// </summary>
        void InitializeAudioClip();
    }
}
