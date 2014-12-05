using UnityEngine;
using System.Collections;

public static class DialogEditor {
	public static Dialog GetDialog(string dialogName)	{
		Dialog dialog = Resources.Load<Dialog>("Dialogs/" + dialogName);
		return dialog;
	}

}
