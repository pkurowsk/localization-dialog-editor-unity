using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LocalizationSettings : ScriptableObject {
	public List<Language> languages = new List<Language>();
	public List<string> dialogKeys = new List<string>();

	public int langIndex;

	public string GetDialog(string dialogKey)	{
		int i = dialogKeys.IndexOf (dialogKey);
		if (i == -1)
			return "<TEXT NOT FOUND>";

		return languages[langIndex].dialogs[i];
	}
}
