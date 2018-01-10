using UnityEngine;
using System.Collections;

public class Splash_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SaveLoad.Initialize_Directories ();
		UnityEngine.SceneManagement.SceneManager.LoadScene ("MainMenu");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
