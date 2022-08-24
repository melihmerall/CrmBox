namespace CrmBox.WebUI.Resources;

using CrmBox.WebUI.Resources.Languages;
using System;
using System.Collections.Concurrent;
using System.Globalization;

/// <summary>
/// Allows the default error message translations to be managed.
/// </summary>
public class LanguageManager : ILanguageManager
{
    private readonly ConcurrentDictionary<string, string> _languages = new ConcurrentDictionary<string, string>();

    /// <summary>
    /// Language factory.
    /// </summary>
    /// <param name="culture">The culture code.</param>
    /// <param name="key">The key to load</param>
    /// <returns>The corresponding Language instance or null.</returns>
    private static string GetTranslation(string culture, string key)
    {
        return culture switch
        {
            EnglishLanguage.AmericanCulture => EnglishLanguage.GetTranslation(key),
            EnglishLanguage.BritishCulture => EnglishLanguage.GetTranslation(key),
            EnglishLanguage.Culture => EnglishLanguage.GetTranslation(key),
            
            TurkishLanguage.Culture => TurkishLanguage.GetTranslation(key),

            _ => null,
        };
    }


    public bool Enabled { get; set; } = true;


    public CultureInfo Culture { get; set; }


    public void Clear()
    {
        _languages.Clear();
    }


    public virtual string GetString(string key, CultureInfo culture = null)
    {
        string value;

        if (Enabled)
        {
            culture = culture ?? Culture ?? CultureInfo.CurrentUICulture;

            string currentCultureKey = culture.Name + ":" + key;
            value = _languages.GetOrAdd(currentCultureKey, k => GetTranslation(culture.Name, key));

       
            var currentCulture = culture;
            while (value == null && currentCulture.Parent != CultureInfo.InvariantCulture)
            {
                currentCulture = currentCulture.Parent;
                string parentCultureKey = currentCulture.Name + ":" + key;
                value = _languages.GetOrAdd(parentCultureKey, k => GetTranslation(currentCulture.Name, key));
            }

            if (value == null && culture.Name != EnglishLanguage.Culture)
            {
       
                if (!culture.IsNeutralCulture && culture.Parent.Name != EnglishLanguage.Culture)
                {
                    value = _languages.GetOrAdd(EnglishLanguage.Culture + ":" + key, k => EnglishLanguage.GetTranslation(key));
                }
            }
        }
        else
        {
            value = _languages.GetOrAdd(EnglishLanguage.Culture + ":" + key, k => EnglishLanguage.GetTranslation(key));
        }

        return value ?? string.Empty;
    }

    public void AddTranslation(string language, string key, string message)
    {
        if (string.IsNullOrEmpty(language)) throw new ArgumentNullException(nameof(language));
        if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
        if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message));

        _languages[language + ":" + key] = message;
    }
}
