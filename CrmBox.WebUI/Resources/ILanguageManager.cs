namespace CrmBox.WebUI.Resources;

using System.Globalization;

public interface ILanguageManager
{
    bool Enabled { get; set; }
    CultureInfo Culture { get; set; }
    string GetString(string key, CultureInfo culture = null);
}