using UnityEngine;
using UnityEngine.UI;

public class UIDialogs : MonoBehaviour {
	public Text dialogText;

	public Transform responseArea;

	public GameObject buttonPrefab;

	public string initialDialog;

	Dialog curDialog;

	// Use this for initialization
	void Start () {
		curDialog = DialogEditor.GetDialog (initialDialog);
		SetNewDialog ();
	}

	void EndOfDialogTree()	{
		dialogText.text = "";
	}

	/// <summary>
	/// When a response button is pressed reset the dialog scene to the next dialog
	/// </summary>
	/// <param name="response">Response.</param>
	public void OnResponsePressed(string response)	{
		curDialog = curDialog.GetDialogFor (response);

		SetNewDialog ();
	}

	/// <summary>
	/// Arranges buttons for responses and changes the text showing the dialog.
	/// </summary>
	void SetNewDialog()	{

		for (int i = 0; i < responseArea.childCount; i++)
			Destroy (responseArea.GetChild (i).gameObject);

		if (curDialog == null) {
			EndOfDialogTree();
			return;
		}

		dialogText.text = Localization.GetDialog (curDialog.contentKey);

		for (int i = 0; i < curDialog.responses.Count; i++) {
			GameObject btnResponse = (GameObject)Instantiate(buttonPrefab);

			// Arrange buttons vertically
			btnResponse.transform.parent = responseArea;
			btnResponse.transform.position = 
				new Vector3(responseArea.position.x, 
			    responseArea.position.y - ((i) * btnResponse.GetComponent<RectTransform>().rect.height));

			btnResponse.transform.GetComponentInChildren<Text>().text = curDialog.GetResponse(i);

			AddResponseEventListener(btnResponse.GetComponent<Button>(), curDialog.responses[i]);
		}
	}

	void AddResponseEventListener(Button b, string value)
	{
		b.onClick.AddListener(() => OnResponsePressed(value));
	}
}
