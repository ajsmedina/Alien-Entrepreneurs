using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class General : Data_Type{
	public string current_map;
	public int myr;
	public List<int> partymembers = new List<int>();
	public int difficulty;


	public General(){
		Initialize ();
	}
		
	void Initialize(){
		current_map = "Introduction";
		myr = 0;
		partymembers.Add (0);
		partymembers.Add (1);
		partymembers.Add (2);
		difficulty = 1;
	}
}
public class Data_GameManager : MonoBehaviour {
	public string fieldscene;
	public string convoname;
	public string postbattleconvo="";
	public int encounter;
	public List<int> partymembers = new List<int>();
	public int xvel;
	public int yvel;
	public int myr=0;
	public General[] generalinfo;

	GameObject scenefade;

	Data_Actor party;
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(this);

		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
		Initialize_General ();
	}

	void Start(){
		partymembers = generalinfo [0].partymembers;
		encounter = -1;

		party = GetComponent<Data_Actor> ();
	}



	public void Initialize_General(){
		Data_Game loadedGame = SaveLoad.Load ("General");
		if (loadedGame==null) {
			Initialize_GeneralInfo ();
			Data_Game newSaveGame = new Data_Game (); 
			newSaveGame.savegameName = "General";
			newSaveGame.all_elements = generalinfo;
			SaveLoad.Save (newSaveGame);
		} else {
			generalinfo = loadedGame.all_elements as General[];
		}
	}

	void Initialize_GeneralInfo(){

		generalinfo = new General[1];
		generalinfo [0] = new General ();
	}


	IEnumerator Fade_To_Scene(string scenename){
		//if (GameObject.FindWithTag ("Player") != null) {
		//	GameObject.FindWithTag ("Player").GetComponent<Player_Move> ().Freeze ();
		//}
		//Time.timeScale=0.0f;
		//scenefade.SetActive (true);
		//scenefade.GetComponent<UnityEngine.UI.Image>().CrossFadeAlpha(1.0f,0.3f,false);
		//yield return new WaitForSeconds (0.3f);
		yield return true;
		SceneManager.LoadScene (scenename);
	}

	public void Fade_In(){
		StartCoroutine (Fade_In_Scene ());
	}

	IEnumerator Fade_In_Scene(){
		scenefade = GameObject.FindWithTag ("Fade");
		scenefade.GetComponent<UnityEngine.UI.Image> ().canvasRenderer.SetAlpha (1.0f);
		scenefade.GetComponent<UnityEngine.UI.Image>().CrossFadeAlpha(0.0f,0.3f,false);
		yield return new WaitForSeconds (0.3f);
		scenefade.SetActive (false);
	}

	public void Fade_InOut(){
		StartCoroutine (Fade_InOut_Co ());
	}

	IEnumerator Fade_InOut_Co(){
		scenefade.SetActive (true);
		scenefade.GetComponent<UnityEngine.UI.Image> ().canvasRenderer.SetAlpha (0.0f);
		scenefade.GetComponent<UnityEngine.UI.Image>().CrossFadeAlpha(1.0f,0.2f,false);
		yield return new WaitForSeconds (0.5f);
		scenefade.GetComponent<UnityEngine.UI.Image>().CrossFadeAlpha(0.0f,0.2f,false);
		yield return new WaitForSeconds (0.3f);
		scenefade.SetActive (false);
	}

	public void Open_Menu(){
		fieldscene = SceneManager.GetActiveScene().name;
		generalinfo [0].current_map = fieldscene;
		StartCoroutine (Fade_To_Scene ("menutest"));
	}
		
	public void Show_Convo(string name){
		convoname = name;
		fieldscene = SceneManager.GetActiveScene().name;
		generalinfo [0].current_map = fieldscene;
		StartCoroutine (Fade_To_Scene ("convotest"));
	}

	public void Close_Menu(){
		SceneManager.LoadScene (fieldscene);
	}

	public void End_Dialog(){
		SceneManager.LoadScene (fieldscene);
	}

	// Update is called once per frame
	void Update () {
	}

	public void Save_Game(){
		Save_Game_Data ("Actors", GetComponent<Data_Actor> ().party);
		Save_Game_Data ("Equipment", GetComponent<Data_Equip> ().equipment);
		Save_Game_Data ("Items", GetComponent<Data_Item> ().items);
		Save_Game_Data ("StoryLog", GetComponent<Data_StoryLog> ().entries);
		Save_Game_Data ("General", generalinfo);


		foreach(string file in System.IO.Directory.GetFiles("Assets/Data/Temp/")){
			System.IO.File.Copy (file, file.Replace("Assets/Data/Temp/","Assets/Data/Saved/"), true);
		}
	}

	void Save_Game_Data(string name, Data_Type[] array){
		Data_Game newSaveGame = new Data_Game (); 
		newSaveGame.savegameName = name;
		newSaveGame.all_elements = array;
		SaveLoad.Save (newSaveGame);
	}

	public void Load_Game(){

		foreach(string file in System.IO.Directory.GetFiles("Assets/Data/Saved/")){
			System.IO.File.Copy (file, file.Replace("Assets/Data/Saved/","Assets/Data/Temp/"), true);
		}
		SceneManager.LoadScene (generalinfo[0].current_map);
	}

	public void Return_MainMenu(){
		SceneManager.LoadScene ("MainMenu");
	}
}
