// C# example:
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LocalizationWindow : EditorWindow {
	static LocalizationSettings lSettings;

	string newLang = "<new language>";
	string newKey = "";
	string newDialog = "";
	
	// Add menu named "My Window" to the Window menu
	[MenuItem ("Localization/Languages")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		LocalizationWindow window = (LocalizationWindow)EditorWindow.GetWindow (typeof (LocalizationWindow));
		Rect newRect = window.position;
		newRect.center = new Vector2 (Screen.GetResolution [0].width / 2, 
		                              Screen.GetResolution [0].height / 2);
		window.position = newRect;

		lSettings = (LocalizationSettings)AssetDatabase.LoadAssetAtPath ("Assets/Resources/Languages/Settings.asset", typeof(LocalizationSettings));

		// Create new localization settings if none exist
		if (lSettings == null) {
			lSettings = ScriptableObject.CreateInstance<LocalizationSettings>();
			AssetDatabase.CreateAsset(lSettings,
			                          "Assets/Resources/Languages/Settings.asset");
			AssetDatabase.SaveAssets();
		}
	}
	
	void OnGUI () {
		GUILayout.Label ("Localization Settings", EditorStyles.boldLabel);
		
		// Add a new language
		EditorGUILayout.BeginHorizontal();
		newLang = EditorGUILayout.TextField (newLang);
		if (GUILayout.Button ("Add Language")) {
			if (newLang != "" && newLang != "<new language>")	{
				AddLang(newLang);
				newLang = "<new language>";
			}
		}
		EditorGUILayout.EndHorizontal();

		// Select language
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label ("Selected Language:");
		lSettings.langIndex = EditorGUILayout.Popup(lSettings.langIndex, GetLangArray());

		if (GUILayout.Button ("-")) {
			AssetDatabase.DeleteAsset("Assets/Resources/Languages/" + lSettings.languages[lSettings.langIndex].languageName + ".asset");
			lSettings.languages.RemoveAt(lSettings.langIndex);
			lSettings.langIndex--;
		}
		EditorGUILayout.EndHorizontal();

		// Add a new dialog text
		if (lSettings.langIndex >= 0 && lSettings.languages.Count > 0) {
			for (int j = 0; j < lSettings.languages[lSettings.langIndex].dialogs.Count; j++) {
				// Show dialogs and keys
				EditorGUILayout.BeginHorizontal();
				lSettings.dialogKeys[j] = EditorGUILayout.TextField(lSettings.dialogKeys[j]);
				lSettings.languages[lSettings.langIndex].dialogs [j] = EditorGUILayout.TextField (lSettings.languages[lSettings.langIndex].dialogs [j]);

				if (GUILayout.Button ("-")) {
					RemoveDialog(j);
				}
				EditorGUILayout.EndHorizontal();
			}

			// Show new text
			GUILayout.Label ("New Text:");
			EditorGUILayout.BeginHorizontal();
			newKey = EditorGUILayout.TextField(newKey);
			newDialog = EditorGUILayout.TextField(newDialog);
			if (GUILayout.Button ("+")) {
				if (newKey != "" && newDialog != "" && !lSettings.dialogKeys.Contains(newKey))	{
					AddDialog(newKey, newDialog);

					newKey = "";
					newDialog = "";
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		// Update data
		if (GUI.changed) {
			Localization.SetLanguage(lSettings.langIndex);

			EditorUtility.SetDirty(lSettings);
			foreach (Language lang in lSettings.languages)
				EditorUtility.SetDirty(lang);
		}
	}

	/// <summary>
	/// Adds a new language
	/// </summary>
	/// <param name="newLang">New lang.</param>
	private void AddLang(string newLang)	{
		Language lang = ScriptableObject.CreateInstance<Language> ();
			
		if (lSettings.languages.Count == 0)
				lang.Init (newLang, new List<string> ());
			else
			lang.Init(newLang, lSettings.languages[0].dialogs);
		lSettings.languages.Add (lang);
			
		lSettings.langIndex = lSettings.languages.Count - 1;
		AssetDatabase.CreateAsset(lang, "Assets/Resources/Languages/" + newLang + ".asset");
		AssetDatabase.SaveAssets();
	}

	/// <summary>
	/// Returns a string array containing the names of all the languages.
	/// </summary>
	/// <returns>The language name array.</returns>
	private string[] GetLangArray()	{
		string[] a = new string[lSettings.languages.Count];

		if (lSettings.languages.Count == 0)
			return a;

		for (int i = 0; i < lSettings.languages.Count; i++)
			a [i] = lSettings.languages [i].languageName;

		return a;
	}

	/// <summary>
	/// Adds a new dialog to the languages
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="dialog">Dialog.</param>
	public void AddDialog(string key, string dialog)	{
		lSettings.dialogKeys.Add (newKey);

		foreach (Language lang in lSettings.languages)	{
			lang.dialogs.Add(dialog);
		}
	}

	/// <summary>
	/// Removes the dialog at j.
	/// </summary>
	/// <param name="j">J.</param>
	public void RemoveDialog(int j)	{
		for (int i = 0; i < lSettings.languages.Count; i++)
			lSettings.languages[i].dialogs.RemoveAt(j);

		lSettings.dialogKeys.RemoveAt (j);
	}
}