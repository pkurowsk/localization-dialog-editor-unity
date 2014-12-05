using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISTexts : MonoBehaviour {
	public Text[] uiTexts;
	public string[] dialogKeys;

	// Use this for initialization
	void Start () {
		LanguageInit ();
		Localization.languageChanged += LanguageInit;
	}

	/// <summary>
	/// Set text on all UI Text objects
	/// </summary>
	private void LanguageInit()	{
		for (int i = 0; i < uiTexts.Length; i++)
			uiTexts [i].text = Localization.GetDialog (dialogKeys [i]);
	}
}
