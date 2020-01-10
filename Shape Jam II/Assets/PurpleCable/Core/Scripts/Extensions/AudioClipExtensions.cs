using UnityEngine;

/// <summary>
/// Extensions for AudioClips
/// </summary>
public static class AudioClipExtensions
{
    #region Play
    /// <summary>
    /// Plays the audio clip using the SoundPlayer
    /// </summary>
    /// <param name="audioClip">Audio clip (can be null)</param>
    public static void Play(this AudioClip audioClip)
        => PurpleCable.SoundPlayer.Play(audioClip);

    /// <summary>
    /// Plays the audio clip using the SoundPlayer
    /// </summary>
    /// <param name="audioClip">Audio clip (can be null)</param>
    /// <param name="pitch">The change in pitch (from 1 to ten)</param>
    public static void Play(this AudioClip audioClip, int pitch)
        => PurpleCable.SoundPlayer.Play(audioClip, pitch);
    #endregion
}
