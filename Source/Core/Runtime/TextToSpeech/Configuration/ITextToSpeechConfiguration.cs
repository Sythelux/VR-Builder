#if UNITY_6000_0_OR_NEWER
using UnityEngine.Localization;
#elif GODOT
using System.Globalization;
using Godot;
#endif


namespace VRBuilder.Core.TextToSpeech.Configuration
{
    /// <summary>
    /// Base interface to implement a new text to speech configuration
    /// </summary>
    /// <remarks>
    /// It always should implement with SettingsObject to prevent type cast errors
    /// </remarks>
    public interface ITextToSpeechConfiguration
    {
        /// <summary>
        /// Get GetUniqueIdentifier to identify the text relative to the locale and hash value
        /// </summary>
        /// <param name="text">Text to get the identifier</param>
        /// <param name="md5Hash">Hashed text value</param>
        /// <param name="locale">Used locale</param>
        /// <returns>A unique identifier of the text</returns>
#if UNITY_6000_0_OR_NEWER
        string GetUniqueIdentifier(string text, string md5Hash, Locale locale);
#elif GODOT
        string GetUniqueIdentifier(string text, string md5Hash, RegionInfo locale);
#endif


        /// <summary>
        /// Check if the localizedContent in the chosen locale is cached
        /// </summary>
        /// <param name="locale">Used locale</param>
        /// <param name="localizedContent">Content to be checked</param>
        /// <returns>True if the localizedContent in the chosen locale is cached</returns>
#if UNITY_6000_0_OR_NEWER
        bool IsCached(Locale locale, string localizedContent);
#elif GODOT
        bool IsCached(RegionInfo locale, string localizedContent);
#endif

    }
}
