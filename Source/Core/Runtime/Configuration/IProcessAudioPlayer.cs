#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif
using VRBuilder.Core.Audio;

namespace VRBuilder.Core.Configuration
{
    /// <summary>
    /// Interface for the class playing sounds for the process, i.e. tts and play audio behaviors.
    /// </summary>
    public interface IProcessAudioPlayer
    {
        /// <summary>
        /// Gets a fallback audio source. Used only for backwards compatibility.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        AudioSource FallbackAudioSource { get; }
#elif GODOT
        AudioStreamPlayer FallbackAudioSource { get; } // was AudioSource
#endif

        /// <summary>
        /// True if currently playing audio.
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Play the specified audio immediately with the set parameters.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        void PlayAudio(IAudioData audioData, float volume = 1f, float pitch = 1f);
#elif GODOT
        void PlayAudio(IAudioData /* was IAudioData */ audioData, float volume = 1f, float pitch = 1f);
#endif

        /// <summary>
        /// Stops playing audio.
        /// </summary>
        void Stop();

        /// <summary>
        /// Resets the player to its default settings.
        /// </summary>
        void Reset();
    }
}
