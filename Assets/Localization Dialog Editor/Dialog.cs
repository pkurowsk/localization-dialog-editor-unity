using UnityEngine;
using System.Collections.Generic;

public class Dialog : ScriptableObject {
	public string dialogName;
	public string contentKey;
	public List<string> responses;
	public List<Dialog> responseNextDialogs;

	public void Init(string dName, string cK)	{
		dialogName = dName;
		contentKey = cK;

		responses = new List<string> ();
		responseNextDialogs = new List<Dialog> ();
	}

	public void AddNewBranch(string r, Dialog rND)	{
		responses.Add (r);
		responseNextDialogs.Add (rND);
	}

	public void DeleteBranch(string r)	{
		int i = responses.IndexOf (r);
		responses.RemoveAt (i);
		responseNextDialogs.RemoveAt (i);
	}

	public string GetContent()	{
		return Localization.GetDialog(contentKey);
	}

	public Dialog GetDialogFor(string response)	{
		return responseNextDialogs [responses.IndexOf (response)];
	}

	public string GetResponse(int i)	{
		return Localization.GetDialog(responses[i]);
	}
}
