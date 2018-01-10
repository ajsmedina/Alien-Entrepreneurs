/*using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class FieldUnit : Data_Type{
	public float xpos,ypos;
	public float xscale,yscale;
	public float xvel,yvel;
}

[System.Serializable]
public class FieldPlayer : FieldUnit{
	public int jump_count;
}


[System.Serializable]
public class FieldDoor : FieldUnit{
	public bool open;
}

[System.Serializable]
public class FieldEnemy : FieldUnit{
	public int encounterid;
	public bool fought;
	public Field_Movement move;
}


[System.Serializable]
public class FieldConvo : FieldUnit{
	public string[] convos;
	public int convocount;
	public bool triggered;
	public bool oneshot;
	public bool invisible;
	public bool onetrigger;
	public bool disappears;
	public Field_Movement move;
}



public class Data_Field : MonoBehaviour {
	public FieldEnemy[] enemies;
	public FieldConvo[] npcs;
	public FieldConvo[] npcs2;
	public FieldUnit[] units;
	public FieldUnit[] platforms;
	public FieldDoor[] doors;
	public GameObject[] enemy_objects;
	public GameObject[] convo_objects;
	public GameObject[] convo_objects2;
	public GameObject[] platform_objects;
	public GameObject[] door_objects;
	public GameObject player;
	public GameObject cam;

	int unitcount;

	public GameObject popwindow;
	public GameObject poptext;
	public GameObject popname;
	public bool pop;
	public bool popup;
	public GameObject gamedata;

	// Use this for initialization
	void Start () {
		gamedata = GameObject.Find ("GameManager");

		Data_Game loadedGame = SaveLoad.Load ("Field_"+SceneManager.GetActiveScene().name);
		if (loadedGame == null) {
			Save_Field ();
		} else {
			Load_Field ();
		}
		gamedata.GetComponent<Data_GameManager> ().Fade_In ();

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space) && !pop) {
			Save_Field ();
			gamedata.GetComponent<Data_GameManager>().Open_Menu ();
		}

		if (Input.GetMouseButtonUp(0) && pop) {
			popup = true;
		}

		if (Input.GetMouseButtonDown(0) && popup) {
			Close_Pop ();
		}
	}

	void Save_Field(){
		Initialize_Field ();
		Data_Game newSaveGame = new Data_Game (); 
		newSaveGame.savegameName = "Field_"+SceneManager.GetActiveScene().name;
		newSaveGame.all_elements = units;
		SaveLoad.Save (newSaveGame);
	}

	void Load_Field(){
		Data_Game loadedGame = SaveLoad.Load ("Field_"+SceneManager.GetActiveScene().name);
		units = loadedGame.all_elements as FieldUnit[];

		player.transform.position = new Vector2 (units[0].xpos, units [0].ypos);
		player.GetComponent<Rigidbody2D>().velocity = new Vector2 (units[0].xvel, units [0].yvel);
		player.GetComponent<Player_Move> ().jump_count = (units [0] as FieldPlayer).jump_count;
		cam.transform.position = new Vector2 (units[1].xpos, units [1].ypos);

		enemy_objects = GameObject.FindGameObjectsWithTag ("Enemy");
		enemies = new FieldEnemy[enemy_objects.Length];

		platform_objects = GameObject.FindGameObjectsWithTag ("Floor_Move");
		platforms = new FieldUnit[platform_objects.Length];

		convo_objects = GameObject.FindGameObjectsWithTag ("Convo_Collide");
		npcs = new FieldConvo[convo_objects.Length];

		convo_objects2 = GameObject.FindGameObjectsWithTag ("Convo_Click");
		npcs2 = new FieldConvo[convo_objects2.Length];

		door_objects = GameObject.FindGameObjectsWithTag ("Door");
		doors = new FieldDoor[door_objects.Length];

		unitcount = 2;
		for (int i = 0; i < enemy_objects.Length; i++) {
			enemies [i] = units [i+unitcount] as FieldEnemy;
		}

		unitcount += enemy_objects.Length;

		for (int i = 0; i < platform_objects.Length; i++) {
			platforms [i] = units [i+unitcount] as FieldUnit;
		}

		unitcount += platform_objects.Length;

		for (int i = 0; i < convo_objects.Length; i++) {
			npcs [i] = units [i+unitcount] as FieldConvo;
		}
		unitcount += convo_objects.Length;

		for (int i = 0; i < convo_objects2.Length; i++) {
			npcs2 [i] = units [i+unitcount] as FieldConvo;
		}
		unitcount += convo_objects2.Length;

		for (int i = 0; i < door_objects.Length; i++) {
			doors [i] = units [i+unitcount] as FieldDoor;
		}
		unitcount += door_objects.Length;

		for (int i = 0; i < enemy_objects.Length; i++) {
			enemy_objects [i].transform.position = new Vector2 (enemies [i].xpos, enemies [i].ypos);
			enemy_objects [i].transform.localScale = new Vector2 (enemies [i].xscale, enemies [i].yscale);
			enemy_objects [i].GetComponent<Rigidbody2D>().velocity = new Vector2 (enemies [i].xvel, enemies [i].yvel);
			enemy_objects [i].GetComponent<Field_Enemy> ().encounterid = enemies [i].encounterid;
			enemy_objects [i].GetComponent<Field_Enemy> ().fought = enemies [i].fought;
			enemy_objects [i].GetComponent<Field_Enemy> ().move = enemies [i].move;
			enemy_objects [i].GetComponent<Field_Enemy> ().Set_Fought ();
		}

		for (int i = 0; i < platform_objects.Length; i++) {
			platform_objects [i].transform.position = new Vector2 (platforms [i].xpos, platforms [i].ypos);
			platform_objects [i].transform.localScale = new Vector2 (platforms [i].xscale, platforms [i].yscale);
			platform_objects [i].GetComponent<Rigidbody2D> ().velocity = new Vector2 (platforms [i].xvel, platforms [i].yvel);
		}

		for (int i = 0; i < convo_objects.Length; i++) {
			convo_objects [i].transform.position = new Vector2 (npcs [i].xpos, npcs [i].ypos);
			convo_objects [i].transform.localScale = new Vector2 (npcs [i].xscale, npcs [i].yscale);
			convo_objects [i].GetComponent<Field_Convo> ().convos = npcs [i].convos;
			convo_objects [i].GetComponent<Field_Convo> ().convocount = npcs [i].convocount;
			convo_objects [i].GetComponent<Field_Convo> ().triggered = npcs [i].triggered;
			convo_objects [i].GetComponent<Field_Convo> ().invisible = npcs [i].invisible;
			convo_objects [i].GetComponent<Field_Convo> ().move = npcs [i].move;
			convo_objects [i].GetComponent<Field_Convo> ().Set_Triggered ();
			convo_objects [i].GetComponent<Field_Convo> ().Set_Visibile ();
		}

		for (int i = 0; i < convo_objects2.Length; i++) {
			convo_objects2 [i].transform.position = new Vector2 (npcs2 [i].xpos, npcs2 [i].ypos);
			convo_objects2 [i].transform.localScale = new Vector2 (npcs2 [i].xscale, npcs2 [i].yscale);
			convo_objects2 [i].GetComponent<Field_Convo> ().convos = npcs2 [i].convos;
			convo_objects2 [i].GetComponent<Field_Convo> ().convocount = npcs2 [i].convocount;
			convo_objects2 [i].GetComponent<Field_Convo> ().triggered = npcs2 [i].triggered;
			convo_objects2 [i].GetComponent<Field_ConvoClick> ().oneshot = npcs2 [i].oneshot;
			convo_objects2 [i].GetComponent<Field_ConvoClick> ().onetrigger = npcs2 [i].onetrigger;
			convo_objects2 [i].GetComponent<Field_ConvoClick> ().disappears = npcs2 [i].disappears;
			convo_objects2 [i].GetComponent<Field_Convo> ().invisible = npcs2 [i].invisible;
			convo_objects2 [i].GetComponent<Field_Convo> ().move = npcs2 [i].move;
			convo_objects2 [i].GetComponent<Field_Convo> ().Set_Triggered ();
			convo_objects2 [i].GetComponent<Field_Convo> ().Set_Visibile ();
		}

		for (int i = 0; i < door_objects.Length; i++) {
			door_objects [i].transform.position = new Vector2 (doors [i].xpos, doors [i].ypos);
			door_objects [i].transform.localScale = new Vector2 (doors [i].xscale, doors [i].yscale);
			door_objects [i].GetComponent<Field_Door> ().open = doors [i].open;
			door_objects [i].GetComponent<Field_Door> ().Set_Open ();
		}
	}

	void Initialize_Field(){

		enemy_objects = GameObject.FindGameObjectsWithTag ("Enemy");
		platform_objects = GameObject.FindGameObjectsWithTag ("Floor_Move");
		convo_objects = GameObject.FindGameObjectsWithTag ("Convo_Collide");
		convo_objects2 = GameObject.FindGameObjectsWithTag ("Convo_Click");
		door_objects = GameObject.FindGameObjectsWithTag ("Door");

		enemies = new FieldEnemy[enemy_objects.Length];
		platforms = new FieldUnit[platform_objects.Length];
		npcs = new FieldConvo[convo_objects.Length];
		npcs2 = new FieldConvo[convo_objects2.Length];
		doors = new FieldDoor[door_objects.Length];

		FieldPlayer playerunit = new FieldPlayer();

		units = new FieldUnit[enemy_objects.Length+2+platform_objects.Length+convo_objects.Length+convo_objects2.Length+door_objects.Length];

		for (int i = 0; i < enemy_objects.Length; i++) {
			enemies [i] = new FieldEnemy();
			enemies [i].xpos = (enemy_objects [i].transform.position.x);
			enemies [i].ypos = (enemy_objects [i].transform.position.y);
			enemies [i].xscale = (enemy_objects [i].transform.localScale.x);
			enemies [i].yscale = (enemy_objects [i].transform.localScale.y);
			enemies [i].xvel = (enemy_objects [i].GetComponent<Rigidbody2D>().velocity.x);
			enemies [i].yvel = (enemy_objects [i].GetComponent<Rigidbody2D>().velocity.y);
			enemies [i].encounterid = enemy_objects [i].GetComponent<Field_Enemy> ().encounterid;
			enemies [i].fought = enemy_objects [i].GetComponent<Field_Enemy> ().fought;
			enemies [i].move = enemy_objects [i].GetComponent<Field_Enemy> ().move;
		}


		for (int i = 0; i < platform_objects.Length; i++) {
			platforms [i] = new FieldUnit ();
			platforms [i].xpos = (platform_objects [i].transform.position.x);
			platforms [i].ypos = (platform_objects [i].transform.position.y);
			platforms [i].xscale = (platform_objects [i].transform.localScale.x);
			platforms [i].yscale = (platform_objects [i].transform.localScale.y);
			platforms [i].xvel = (platform_objects [i].GetComponent<Rigidbody2D>().velocity.x);
			platforms [i].yvel = (platform_objects [i].GetComponent<Rigidbody2D>().velocity.y);
		}

		for (int i = 0; i < convo_objects.Length; i++) {
			npcs [i] = new FieldConvo();
			npcs [i].xpos = (convo_objects [i].transform.position.x);
			npcs [i].ypos = (convo_objects [i].transform.position.y);
			npcs [i].xscale = (convo_objects [i].transform.localScale.x);
			npcs [i].yscale = (convo_objects [i].transform.localScale.y);
			npcs [i].convos = convo_objects [i].GetComponent<Field_Convo> ().convos;
			npcs [i].convocount = convo_objects [i].GetComponent<Field_Convo> ().convocount;
			npcs [i].triggered = convo_objects [i].GetComponent<Field_Convo> ().triggered;
			npcs [i].invisible = convo_objects [i].GetComponent<Field_Convo> ().invisible;
			npcs [i].move = convo_objects [i].GetComponent<Field_Convo> ().move;
		}

		for (int i = 0; i < convo_objects2.Length; i++) {
			npcs2 [i] = new FieldConvo();
			npcs2 [i].xpos = (convo_objects2 [i].transform.position.x);
			npcs2 [i].ypos = (convo_objects2 [i].transform.position.y);
			npcs2 [i].xscale = (convo_objects2 [i].transform.localScale.x);
			npcs2 [i].yscale = (convo_objects2 [i].transform.localScale.y);
			npcs2 [i].convos = convo_objects2 [i].GetComponent<Field_Convo> ().convos;
			npcs2 [i].convocount = convo_objects2 [i].GetComponent<Field_Convo> ().convocount;
			npcs2 [i].triggered = convo_objects2 [i].GetComponent<Field_Convo> ().triggered;
			npcs2 [i].oneshot = convo_objects2 [i].GetComponent<Field_ConvoClick> ().oneshot;
			npcs2 [i].onetrigger = convo_objects2 [i].GetComponent<Field_ConvoClick> ().onetrigger;
			npcs2 [i].disappears = convo_objects2 [i].GetComponent<Field_ConvoClick> ().disappears;
			npcs2 [i].invisible = convo_objects2 [i].GetComponent<Field_Convo> ().invisible;
			npcs2 [i].move = convo_objects2 [i].GetComponent<Field_Convo> ().move;
		}

		for (int i = 0; i < door_objects.Length; i++) {
			doors [i] = new FieldDoor ();
			doors [i].xpos = (door_objects [i].transform.position.x);
			doors [i].ypos = (door_objects [i].transform.position.y);
			doors [i].xscale = (door_objects [i].transform.localScale.x);
			doors [i].yscale = (door_objects [i].transform.localScale.y);
			doors [i].open = door_objects [i].GetComponent<Field_Door> ().open;
		}

		playerunit.xpos = (player.transform.position.x);
		playerunit.ypos = (player.transform.position.y);
		playerunit.xvel = (player.GetComponent<Rigidbody2D>().velocity.x);
		playerunit.yvel = (player.GetComponent<Rigidbody2D>().velocity.y);
		playerunit.jump_count = player.GetComponent<Player_Move> ().jump_count;
		units [0] = playerunit;

		units [1] = new FieldUnit ();
		units [1].xpos = (cam.transform.position.x);
		units [1].ypos = (cam.transform.position.y);

		unitcount = 2;

		for (int i = unitcount; i < unitcount+enemy_objects.Length; i++) {
			units [i] = enemies [i-unitcount];
		}
		unitcount += enemy_objects.Length;

		for (int i = unitcount; i < unitcount+platform_objects.Length; i++) {
			units [i] = platforms [i-unitcount];
		}
		unitcount += platform_objects.Length;

		for (int i = unitcount; i < unitcount+convo_objects.Length; i++) {
			units [i] = npcs [i-unitcount];
		}
		unitcount += convo_objects.Length;

		for (int i = unitcount; i < unitcount+convo_objects2.Length; i++) {
			units [i] = npcs2 [i-unitcount];
		}
		unitcount += convo_objects2.Length;

		for (int i = unitcount; i < unitcount+door_objects.Length; i++) {
			units [i] = doors [i-unitcount];
		}
		unitcount += convo_objects2.Length;

	}

	public void Start_Battle(int encounterid){
		Save_Field ();
		gamedata.GetComponent<Data_GameManager>().Start_Battle (encounterid);
	}

	public void Start_Convo(string convoname){
		if (convoname [0] == '>') {
			Show_Pop (convoname);
		} else {
			Save_Field ();
			gamedata.GetComponent<Data_GameManager> ().Show_Convo (convoname);
		}
	}

	public void Show_Pop(string text){
		string[] popinfo = text.Split (';');
		int itemid;
		int itemamt;

		popwindow.SetActive (true);

		if (popinfo.Length > 2) {
			for (int i = 2; i < popinfo.Length; i++) {
				itemid = int.Parse(popinfo [i].Substring (1));
				itemamt = int.Parse (popinfo [i + 1]);
				if (popinfo [i] [0] == 'i') {
					gamedata.GetComponent<Data_Item> ().items [itemid].Add (itemamt); 
					popinfo[1]+="\n [Obtained "+gamedata.GetComponent<Data_Item> ().items[itemid].name + " x"+itemamt.ToString()+"]";
				} else {
					gamedata.GetComponent<Data_Equip> ().equipment [itemid].Add (itemamt);
					popinfo[1]+="\n [Obtained "+gamedata.GetComponent<Data_Equip> ().equipment[itemid].name + " x"+itemamt.ToString()+"]";
				}
				i++;
			}
		}
		popname.GetComponent<Text> ().text = popinfo[0].Substring(1,popinfo[0].Length-1);
		poptext.GetComponent<Text> ().text = popinfo[1];

		pop = true;
		popup = false;
		Time.timeScale = 0.0f;
	}

	public void Close_Pop(){
		popwindow.SetActive (false);
		popname.GetComponent<Text> ().text = "";
		poptext.GetComponent<Text> ().text = "";
		pop = false;
		popup = false;
		Time.timeScale = 1.0f;
	}
}
*/