using UnityEngine;
using System.Collections;

public static class Localization {

	public delegate void LanguageChangedHandler();
	public static event LanguageChangedHandler languageChanged;

	static LocalizationSettings lSettings;

	public static string GetDialog(string dialogKey)	{
		lSettings = Resources.Load<LocalizationSettings> ("Languages/Settings");
		return lSettings.GetDialog(dialogKey);
	}

	/// <summary>
	/// Sets the language by string.
	/// 
	/// Return true if the language was found. False otherwise.
	/// </summary>
	/// <returns><c>true</c>, if language was set, <c>false</c> otherwise.</returns>
	/// <param name="language">Language.</param>
	public static bool SetLanguage(string language)	
	{
		if (lSettings == null)
			lSettings = Resources.Load<LocalizationSettings> ("Languages/Settings");

		for (int i = 0; i < lSettings.languages.Count; i++) {
			if (lSettings.languages [i].languageName.Equals (language)) {
				SetLanguage (i);
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Sets the language by index.
	/// 
	/// Return true if the index is a correct language index. False otherwise.
	/// </summary>
	/// <returns><c>true</c>, if language was set, <c>false</c> otherwise.</returns>
	/// <param name="language">Language.</param>
	public static bool SetLanguage(int i)	
	{
		if (lSettings == null)
			lSettings = Resources.Load<LocalizationSettings> ("Languages/Settings");

		if (i >= lSettings.languages.Count || i < 0)
			return false;

		lSettings.langIndex = i;

		// Fire changed event
		if (languageChanged != null)
			languageChanged();

		return true;
	}
}
