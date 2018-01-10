using UnityEngine;
using System.Collections;

public class Title_Flow : MonoBehaviour {
	public GameObject gamedata;
	public GameObject erasebutton;
	public GameObject erasetext;

	bool erasing=false;
	// Use this for initialization
	void Start () {
		erasetext.SetActive (false);
		Debug.Log ("a");
		if (!System.IO.File.Exists ("Assets/Data/Saved/General.dat")) {
			erasebutton.GetComponent<UnityEngine.UI.Button> ().interactable=false;
		}
		Debug.Log ("b");
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (1) && erasing) {
			erasing = false;
		} else if (Input.GetKey (KeyCode.Space) && Input.GetKey (KeyCode.Backspace)) {
			EraseData ();
		}
	}

	public void Command_StartGame(){
		UnityEngine.SceneManagement.SceneManager.LoadScene (gamedata.GetComponent<Data_GameManager> ().generalinfo [0].current_map);
	}

	public void Erase_File(){
		erasetext.SetActive (true);
		erasing = true;	
	}

	void EraseData(){
		erasetext.SetActive (false);
		erasebutton.GetComponent<UnityEngine.UI.Button> ().interactable=false;
		System.IO.Directory.Delete ("Assets/Data/Saved/");
		System.IO.Directory.Delete ("Assets/Data/Temp/");
		SaveLoad.Initialize_Directories ();
		gamedata.GetComponent<Data_GameManager> ().Initialize_General ();
	}

	public void Command_Quit(){
		Application.Quit ();
	}
}
