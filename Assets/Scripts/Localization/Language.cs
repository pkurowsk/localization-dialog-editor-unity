using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Language : ScriptableObject{
	public string languageName;
	public List<string> dialogs = new List<string> ();

	public void Init(string n, List<string> d)	{
		languageName = n;

		for (int i = 0; i < d.Count; i++)
			dialogs.Add (d [i]);
	}
}

