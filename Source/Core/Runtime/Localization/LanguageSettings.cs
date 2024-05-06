// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

#if UNITY_5_3_OR_NEWER
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
#elif GODOT
using Godot;
#endif
using VRBuilder.Core.Runtime.Utils;

namespace VRBuilder.Core.Localization
{
    /// <summary>
    /// Language settings for VR Builder.
    /// </summary>
    public partial class LanguageSettings : SettingsObject<LanguageSettings>
    {
        /// <summary>
        /// Language which should be used if no localization settings are present.
        /// </summary>
        public string ApplicationLanguage = "En";

        /// <summary>
        /// Returns the active or default language.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        public string ActiveOrDefaultLanguage
        {
            get
            {
                return ActiveOrDefaultLocale.Identifier.Code;
            }
        }

        /// <summary>
        /// Returns the active or default locale.
        /// </summary>
        public Locale ActiveOrDefaultLocale
        {
            get
            {
                if (LocalizationSettings.HasSettings)
                {
                    if (LocalizationSettings.SelectedLocale != null)
                    {
                        return LocalizationSettings.SelectedLocale;
                    }

                    if (LocalizationSettings.ProjectLocale != null)
                    {
                        return LocalizationSettings.ProjectLocale;
                    }
                }

                Locale locale = GetLocaleFromString(ApplicationLanguage);

                if (locale.Identifier.CultureInfo != null)
                {
                    return locale;
                }
                else
                {
                    return Locale.CreateLocale(System.Globalization.CultureInfo.CurrentCulture);
                }
            }
        }

        /// <summary>
        /// Get Locale object from a language or language code string.
        /// </summary>
        /// <param name="languageOrCode">The language or language code string.</param>
        /// <returns>The Locale object corresponding to the language code string or NULL.</returns>
        public Locale GetLocaleFromString(string languageOrCode)
        {
            Locale locale = Locale.CreateLocale(languageOrCode);
            if (locale.Identifier.CultureInfo == null)
            {
                string convertedCode;
                if (LanguageUtils.TryConvertToTwoLetterIsoCode(languageOrCode, out convertedCode))
                {
                    locale = Locale.CreateLocale(convertedCode);
                }
            }

            return locale;
        }
#elif GODOT
        public string ActiveOrDefaultLanguage
        {
            get => TranslationServer.GetLocaleName(TranslationServer.GetLocale());
        }

        /// <summary>
        /// Returns the active or default locale.
        /// </summary>
        public string ActiveOrDefaultLocale => TranslationServer.GetLocale();
#endif
    }
}
