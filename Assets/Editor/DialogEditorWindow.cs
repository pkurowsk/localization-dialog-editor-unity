using UnityEngine;
using UnityEditor;
using System.Collections;

public class DialogEditorWindow : EditorWindow {
	string newDialogName = "<DIALOG NAME>";
	string newDialogContentKey = "<DIALOG CONTENT KEY>";
	string newResponse = "<RESPONSE KEY>";
	Dialog newDialog;

	string changeKey = "";

	Dialog[] allDialogs;

	int dialogIndex = 0;

	// Add menu named "My Window" to the Window menu
	[MenuItem ("Dialogs/Editor")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		DialogEditorWindow window = (DialogEditorWindow)EditorWindow.GetWindow (typeof (DialogEditorWindow));

	}
	
	void OnGUI () {
		GUILayout.Label ("Dialog Editor", EditorStyles.boldLabel);

		// Add a new dialog
		EditorGUILayout.BeginHorizontal();
		newDialogName = EditorGUILayout.TextField (newDialogName);
		newDialogContentKey = EditorGUILayout.TextField (newDialogContentKey);
		if (GUILayout.Button ("Add Dialog")) {
			if (newDialogName != "" && newDialogName != "<DIALOG NAME>")	{
				AddDialog(newDialogName, newDialogContentKey);
				newDialogName = "<DIALOG NAME>";
				GetDialogNamesArray();
			}
		}
		EditorGUILayout.EndHorizontal();

		// Select / delete dialog
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label ("Selected Dialog:");
		dialogIndex = EditorGUILayout.Popup(dialogIndex, GetDialogNamesArray());
		
		if (GUILayout.Button ("-")) {
			AssetDatabase.DeleteAsset("Assets/Resources/Dialogs/" + allDialogs[dialogIndex].dialogName + ".asset");
			dialogIndex = dialogIndex == 0 ? 0 : dialogIndex - 1;
			GetDialogNamesArray();
		}
		EditorGUILayout.EndHorizontal();

		// Add a new dialog text
		if (dialogIndex >= 0 && allDialogs.Length > 0) {
			Dialog dialog = allDialogs[dialogIndex];
			// Show content and content key
			GUILayout.Label("Content: " + dialog.contentKey, EditorStyles.boldLabel);
			GUILayout.Label(Localization.GetDialog(dialog.contentKey));

			// Edit content key
			GUILayout.BeginHorizontal();
			GUILayout.Label("Change key: ");
			changeKey = GUILayout.TextField(changeKey);
			if (GUILayout.Button ("Change") && !changeKey.Equals(""))
				dialog.contentKey = changeKey;
			GUILayout.EndHorizontal();

			// Show all response branches
			GUILayout.Label("Response Branches", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			for (int i = 0; i < allDialogs[dialogIndex].responses.Count; i++) {
				// Show dialogs and keys
				EditorGUILayout.BeginVertical();
				dialog.responses[i] = EditorGUILayout.TextField(dialog.responses[i]);
				dialog.responseNextDialogs[i] = (Dialog)EditorGUILayout.ObjectField (dialog.responseNextDialogs[i], typeof(Dialog));
				
				if (GUILayout.Button ("-")) {
					dialog.DeleteBranch(dialog.responses[i]);
				}
				EditorGUILayout.EndVertical();

				// Add space between branches
				EditorGUILayout.BeginVertical();
				EditorGUILayout.Space();
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();

			if (dialog.responses.Count == 0)
				GUILayout.Label("No response branches");
			
			// Add new branch
			GUILayout.Label ("New Branch:", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			newResponse = EditorGUILayout.TextField(newResponse);
			newDialog = (Dialog)EditorGUILayout.ObjectField(newDialog, typeof(Dialog));
			if (GUILayout.Button ("+")) {
				if (newResponse != "" && !dialog.responses.Contains(newResponse))	{
					dialog.AddNewBranch(newResponse, newDialog);
					
					newResponse = "<RESPONSE KEY>";
					newDialog = null;
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		// Update data
		if (GUI.changed) {
			for (int i = 0; i < allDialogs.Length; i++)
				EditorUtility.SetDirty(allDialogs[i]);
		}
	}

	void AddDialog(string newDialogName, string contentKey)	{
		Dialog d = ScriptableObject.CreateInstance<Dialog> ();
		d.Init (newDialogName, contentKey);

		AssetDatabase.CreateAsset(d, "Assets/Resources/Dialogs/" + d.dialogName + ".asset");
		AssetDatabase.SaveAssets();
	}

	string[] GetDialogNamesArray()	{
		Object[] dialogResources = Resources.LoadAll("Dialogs");
		allDialogs = new Dialog[dialogResources.Length];
		string[] dialogNames = new string[dialogResources.Length];

		for (int i = 0; i < dialogResources.Length; i++) {
			allDialogs [i] = (Dialog)dialogResources [i];
			dialogNames[i] = allDialogs[i].dialogName;
		}

		return dialogNames;
	}
}
