#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif
using VRBuilder.Core.Configuration;
using VRBuilder.Core.Utils;

/// <summary>
/// Default process audio player.
/// </summary>
public class DefaultAudioPlayer : IProcessAudioPlayer
{
#if UNITY_5_3_OR_NEWER
    private AudioSource audioSource;

    public DefaultAudioPlayer()
    {
        GameObject user = RuntimeConfigurator.Configuration.LocalUser.Head.gameObject;

        audioSource = user.GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = user.AddComponent<AudioSource>();
        }
    }

    public DefaultAudioPlayer(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

    /// <inheritdoc />
    public AudioSource FallbackAudioSource => audioSource;

    /// <inheritdoc />
    public bool IsPlaying => audioSource.isPlaying;

    /// <inheritdoc />
    public void PlayAudio(IAudioData audioData, float volume = 1, float pitch = 1)
    {
        audioSource.clip = audioData.AudioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();
    }

    /// <inheritdoc />
    public void Reset()
    {
        audioSource.clip = null;
    }

    /// <inheritdoc />
    public void Stop()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }
#elif GODOT
    private /*AudioSource*/ AudioStreamPlayer audioSource;

    public DefaultAudioPlayer()
    {
        Node user = RuntimeConfigurator.Configuration.LocalUser.Head;

        audioSource = user.GetComponent<AudioStreamPlayer>() ?? user.AddComponent<AudioStreamPlayer>();
    }

    public DefaultAudioPlayer(AudioStreamPlayer audioSource)
    {
        this.audioSource = audioSource;
    }

    /// <inheritdoc />
    public AudioStreamPlayer FallbackAudioSource => audioSource;

    /// <inheritdoc />
    public bool IsPlaying => audioSource.Playing;

    /// <inheritdoc />
    public void PlayAudio(AudioStream audioData, float volume = 1, float pitch = 1)
    {
        audioSource.Stream = audioData;
        audioSource.VolumeDb = volume;
        audioSource.PitchScale = pitch;
        audioSource.Play();
    }

    /// <inheritdoc />
    public void Reset()
    {
        audioSource.Stream = null;
    }

    /// <inheritdoc />
    public void Stop()
    {
        audioSource.Stop();
        audioSource.Stream = null;
    }
#endif
}
