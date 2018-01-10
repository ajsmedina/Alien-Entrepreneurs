using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Menu_Flow : MonoBehaviour {
	public Actor[] party;
	public GameObject gamedata;
	public GameObject[] HUD_Buttons;
	public GameObject[] nametext;
	public GameObject[] HP_Bars;
	public GameObject[] HP_Text;

	public GameObject[] skillbuttons;
	public GameObject[] itembuttons;

	public GameObject portraits;
	public GameObject status;
	public GameObject equip;
	public GameObject story;
	public GameObject buttons;
	public GameObject itemlist;
	public GameObject optionslist;
	public GameObject confirmationwindow;

	public enum MenuState {Base, ItemSelect, PartySelect, EquipStatus, EquipSelect, Status, Story, Options, TargetAll, Confirmation};
	public MenuState state;
	public MenuState laststate;

	public MenuState futurestate;

	public int current_character;
	public int current_equiptype=-1;

	public GameObject[] status_info;
	public GameObject statushp_text;
	public GameObject statushp_bar;
	public GameObject status_portrait;

	public GameObject[] equip_info;
	public GameObject equip_portrait;

	public GameObject[] inventorybuttons;

	public GameObject[] equipbuttons;
	public GameObject equip_type_text;

	public GameObject[] eventbuttons;
	public GameObject eventname;
	public GameObject eventcontent;

	public Menu_Help help;

	public GameObject myr_text;

	string confirmationqueue="Save";

	LogEntry[] unlocked_events;

	int itemqueue=-1;
	int scroll=0;


	public Equip[] equipment;
	public Item[] items;


	// Use this for initialization
	void Start () {
		current_character=0;
		gamedata = GameObject.Find ("GameManager");

		party = new Actor[gamedata.GetComponent<Data_GameManager> ().partymembers.Count];

		for (int i = 0; i < party.Length; i++) {
			party[i] = gamedata.GetComponent<Data_Actor> ().party[gamedata.GetComponent<Data_GameManager> ().partymembers[i]];
			HUD_Buttons [i].GetComponent<Image> ().sprite = Change_Sprite (party [i].name);

		}

		for(int i=party.Length; i<4;i++){
			HUD_Buttons [i].SetActive (false);
		}

		Update_HUD ();
		help = GetComponent<Menu_Help> ();
		equipment = gamedata.GetComponent<Data_Equip> ().equipment;
		items = gamedata.GetComponent<Data_Item> ().items;

		Change_State (MenuState.Base);
		state = MenuState.Base;
		laststate = MenuState.Base;
		gamedata.GetComponent<Data_GameManager> ().Fade_In ();
		myr_text.GetComponent<Text> ().text = "Myr:\n" + gamedata.GetComponent<Data_GameManager>().generalinfo[0].myr;

		int eventcount=0;
		LogEntry[] logentries = gamedata.GetComponent<Data_StoryLog> ().entries;
		for (int i = 0; i < logentries.Length; i++) {
			if (logentries [i].unlocked) {
				eventcount++;
			}
		}

		unlocked_events = new LogEntry[eventcount];

		int k = 0;
		for (int i = 0; i < logentries.Length; i++) {
			if (logentries [i].unlocked) {
				unlocked_events [k] = logentries [i];
				k++;
			}
		}


	}

	Sprite Change_Sprite(string name){
		Texture2D targetspriteimage;
		Rect spriterect;
		targetspriteimage = Resources.Load (name) as Texture2D;
		spriterect = new Rect (0, 0, targetspriteimage.width, targetspriteimage.height);
		return  Sprite.Create (targetspriteimage,spriterect, new Vector2(0,0));

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			gamedata.GetComponent<Data_GameManager> ().Close_Menu ();
		}
		if (Input.GetMouseButtonDown (0) && state==MenuState.TargetAll) {
			Use_Sleeping_Bags ();
		}

		if (Input.GetMouseButtonDown (1)) {
			help.Exit_Hover ();
			switch (state) {
			case MenuState.Status:
				Change_State (MenuState.Base);
				break;

			case MenuState.ItemSelect:
				Change_State (MenuState.Base);
				break;

			case MenuState.Options:
				Change_State (MenuState.Base);
				break;

			case MenuState.TargetAll:
				Change_State (MenuState.ItemSelect);
				break;

			case MenuState.PartySelect:
				if (itemqueue == -1) {
					Change_State (MenuState.Base);
				} else {
					Change_State (MenuState.ItemSelect);
				}
				break;

			case MenuState.EquipStatus:
				Change_State (MenuState.PartySelect);
				break;

			case MenuState.EquipSelect:
				Change_State (MenuState.EquipStatus);
				break;

			case MenuState.Story:
				Change_State (MenuState.Base);
				break;

			case MenuState.Confirmation:
				Change_State (MenuState.Options);
				break;
			}
		}
	}

	void Update_HUD(){
		for (int i = 0; i < party.Length; i++) {
			nametext [i].GetComponent<Text> ().text = party [i].name;
			HP_Text [i].GetComponent<Text> ().text = party [i].hp + "/ " + party [i].mhp;
			HP_Bars [i].transform.localScale = new Vector3 ((float)party [i].hp / (float)party [i].mhp, 1, 1);
		}
	}

	public void Command_Item(){
		Change_State (MenuState.ItemSelect);
	}

	public void Command_Options(){
		Change_State (MenuState.Options);
	}

	public void Command_Equip(){
		Change_State (MenuState.PartySelect);
		futurestate = MenuState.EquipStatus;
	}
	public void Command_Story(){
		Change_State (MenuState.Story);
	}

	public void Command_Equip_Change(int commandid){
		Change_State (MenuState.EquipSelect);
		Set_Inventory (commandid);
	}

	public void Command_Use_Item(int commandid){
		if (commandid == 3) {
			Set_Target_All ();
			Change_State (MenuState.TargetAll);
		}else{
			itemqueue = commandid;
			Change_State (MenuState.PartySelect);
		}
	}

	public void Command_Inventory(int commandid){
		if (state == MenuState.EquipSelect) {
			Equip[] equips = Find_Equipment_Of_Type (EquipType.Acc);

			if (current_equiptype == 0) {
				equips = Find_Equipment_Of_Type (party [current_character].weapontype);
			} else if (current_equiptype == 1) {
				equips = Find_Equipment_Of_Type (party [current_character].armortype);
			}

			Change_Equipment (equips [commandid]);
			Change_State (MenuState.EquipStatus);
		}
	}

	public void Command_Party(int partyid){
		if (state == MenuState.Base) {
			current_character = partyid;
			Change_State (MenuState.Status);
		} else if (state == MenuState.PartySelect && itemqueue > -1) {
			Use_Item (partyid);
		} else if (state == MenuState.PartySelect) {
			current_character = partyid;
			Change_State (futurestate);
		}
	}

	public void Command_Save(){
		confirmationqueue = "Save";
		help.Set_Selected ("Save");
		Change_State (MenuState.Confirmation);
	}
	public void Command_Load(){
		confirmationqueue = "Load";
		help.Set_Selected ("Load");
		Change_State (MenuState.Confirmation);
	}
	public void Command_MainMenu(){
		confirmationqueue = "MainMenu";
		help.Set_Selected ("MainMenu");
		Change_State (MenuState.Confirmation);
	}

	public void Command_ConfirmationYes(){
		if (confirmationqueue == "Save") {
			gamedata.GetComponent<Data_GameManager> ().Save_Game ();
		} else if (confirmationqueue == "Load") {
			gamedata.GetComponent<Data_GameManager> ().Load_Game ();
		} else if (confirmationqueue == "MainMenu") {
			gamedata.GetComponent<Data_GameManager> ().Return_MainMenu ();
		}
		Change_State (MenuState.Options);
	}
	public void Command_ConfirmationNo(){
		Change_State (MenuState.Options);
	}

	void Use_Item (int partyid){
		Actor actor = party [partyid];
		party[partyid].hp = Mathf.Min (actor.mhp, actor.hp+(int)((float)actor.mhp*gamedata.GetComponent<Data_Skill>().skills[items[itemqueue].skillid].bp));
		items[itemqueue].quantity--;
		Change_State (MenuState.ItemSelect);
		Update_HUD ();
	}

	void Use_Sleeping_Bags(){
		for (int i = 0; i < party.Length; i++) {
			party [i].hp = Mathf.Min (party [i].mhp, party [i].hp + (int)((float)party [i].mhp * gamedata.GetComponent<Data_Skill> ().skills [items [4].skillid].bp));
		}
		items[4].quantity--;
		Change_State (MenuState.ItemSelect);
		Update_HUD ();
	}

	void Change_State(MenuState newstate){
		laststate = state; 

		switch (newstate) {

		case MenuState.Base:
			portraits.SetActive (true);
			buttons.SetActive (true);
			status.SetActive (false);
			itemlist.SetActive (false);
			optionslist.SetActive (false);
			equip.SetActive (false);
			story.SetActive (false);
			confirmationwindow.SetActive (false);
			Set_Portrait_Interactable (true);
			help.Set_Selected ("Objective");
			futurestate = MenuState.Base;
			break;

		case MenuState.ItemSelect:
			buttons.SetActive (false);
			itemlist.SetActive (true);
			Set_Portrait_Interactable (false);
			Set_Items ();
			help.Set_Selected ("ItemSelect");
			Remove_Target_All ();
			itemqueue = -1;
			break;

		case MenuState.Options:
			buttons.SetActive (false);
			optionslist.SetActive (true);
			confirmationwindow.SetActive (false);
			Set_Portrait_Interactable (false);
			help.Set_Selected ("Options");
			Remove_Target_All ();
			break;

		case MenuState.PartySelect:
			portraits.SetActive (true);
			buttons.SetActive (false);
			status.SetActive (false);
			if (itemqueue != -1 && itemqueue != 2) {
				Set_Portrait_Dead_Interactable ();
			} else {
				Set_Portrait_Interactable (true);
			}
			equip.SetActive (false);
			help.Set_Selected ("PartySelect");
			break;

		case MenuState.Status:
			portraits.SetActive (false);
			buttons.SetActive (false);
			status.SetActive (true);
			Set_Skills (party[current_character]);
			Set_Status_Stats (party[current_character]);
			Set_Status_HP (party[current_character]);
			help.Set_Selected ("Status");
			break;

		case MenuState.EquipStatus:
			portraits.SetActive (false);
			buttons.SetActive (false);
			equip.SetActive (true);
			Set_Inventory (-1);
			Set_Equip_Stats (party[current_character]);
			Set_Equipment (party[current_character]);
			help.Set_Selected ("EquipStatus");
			break;

		case MenuState.Story:
			portraits.SetActive (false);
			buttons.SetActive (false);
			story.SetActive (true);
			Set_Events ();
			scroll = 0;
			Set_Selected_Event (0);
			help.Set_Selected ("Story");
			break;

		case MenuState.EquipSelect:
			help.Set_Selected ("EquipSelect");
			break;

		case MenuState.TargetAll:
			help.Set_Selected ("TargetAll");
			break;

		case MenuState.Confirmation:
			confirmationwindow.SetActive (true);
			break;
		}

		state = newstate;
	}

	public void Set_Target_All(){
		ColorBlock cb;

		for (int i = 0; i < party.Length; i++) {
			cb = HUD_Buttons [i].GetComponent<Button> ().colors;
			cb.highlightedColor = new Color (0.5f, 0.5f, 0.5f);
			cb.normalColor = new Color (0.5f, 0.5f, 0.5f);
			cb.disabledColor = new Color (0.5f, 0.5f, 0.5f);
			HUD_Buttons [i].GetComponent<Button> ().colors = cb;
		}
	}

	public void Remove_Target_All(){
		ColorBlock cb;

		for (int i = 0; i < party.Length; i++) {
			cb = HUD_Buttons [i].GetComponent<Button> ().colors;

			cb.normalColor = new Color (1.0f, 1.0f, 1.0f);

			cb.highlightedColor = new Color (0.5f, 0.5f, 0.5f);
			cb.disabledColor = new Color (1.0f, 1.0f, 1.0f);
			HUD_Buttons [i].GetComponent<Button> ().colors = cb;
		}
	}


	void Set_Skills(Actor actor){
		for (int i = 0; i <skillbuttons.Length; i++) {
			if (i < actor.knownSkills.Count) {
				skillbuttons [i].transform.Find ("Text").gameObject.GetComponent<Text>().text=actor.knownSkills [i].name;
			} else {
				skillbuttons [i].transform.Find ("Text").gameObject.GetComponent<Text> ().text = "";
			}
		}
	}

	void Set_Items(){
		int j;
		for (int i = 0; i <itembuttons.Length; i++) {
			j = i == 3 ? 4 : i;
			itembuttons [i].transform.Find ("Text").gameObject.GetComponent<Text> ().text = items [j].name;

			if (items [j].quantity > 0) {
				itembuttons [i].GetComponent<Button> ().interactable = true;
			} else {
				itembuttons [i].GetComponent<Button> ().interactable = false;
			}
				

		}
	}

	void Set_Status_Stats(Actor actor){
		status_info [0].GetComponent<Text> ().text = actor.name;
		status_info [1].GetComponent<Text> ().text = "Level: " + actor.lvl.ToString ();
		status_info [2].GetComponent<Text> ().text = actor.exp.ToString ();
		status_info [3].GetComponent<Text> ().text = (actor.Next_Level()-actor.exp).ToString();
		status_info [4].GetComponent<Text> ().text = actor.str.ToString ();
		status_info [5].GetComponent<Text> ().text = actor.mag.ToString ();
		status_info [6].GetComponent<Text> ().text = actor.con.ToString ();
		status_info [7].GetComponent<Text> ().text = actor.dex.ToString ();
		status_portrait.GetComponent<Image> ().sprite = Change_Sprite (actor.name);
	}

	void Set_Status_HP(Actor actor){
		statushp_text.GetComponent<Text> ().text = actor.hp + "/ " + actor.mhp;
		statushp_bar.transform.localScale = new Vector3 ((float)actor.hp / (float)actor.mhp, 1, 1);
	}

	void Set_Equip_Stats(Actor actor){
		equip_info [0].GetComponent<Text> ().text = actor.name;
		equip_info [1].GetComponent<Text> ().text = actor.str.ToString ()+" >";
		equip_info [2].GetComponent<Text> ().text = actor.str.ToString ();
		equip_info [3].GetComponent<Text> ().text = actor.mag.ToString ()+" >";
		equip_info [4].GetComponent<Text> ().text = actor.mag.ToString ();
		equip_info [5].GetComponent<Text> ().text = actor.con.ToString ()+" >";
		equip_info [6].GetComponent<Text> ().text = actor.con.ToString ();
		equip_info [7].GetComponent<Text> ().text = actor.dex.ToString ()+" >";
		equip_info [8].GetComponent<Text> ().text = actor.dex.ToString ();
		equip_portrait.GetComponent<Image> ().sprite = Change_Sprite (actor.name);
	}
		

	public void Show_Equip_Stat_Change(Equip newequip){
		Actor actor = party [current_character];
		Equip oldequip = equipment[actor.acc];
		if (current_equiptype == 0) {
			oldequip = equipment [actor.weapon];
		} else if (current_equiptype == 1) {
			oldequip = equipment [actor.armor];
		}
		equip_info [2].GetComponent<Text> ().text = (actor.str-oldequip.str+newequip.str).ToString ();
		equip_info [4].GetComponent<Text> ().text = (actor.mag-oldequip.mag+newequip.mag).ToString ();
		equip_info [6].GetComponent<Text> ().text = (actor.con-oldequip.vit+newequip.vit).ToString ();
		equip_info [8].GetComponent<Text> ().text = (actor.dex-oldequip.agi+newequip.agi).ToString ();
	}

	public void Reset_Equip_Stat_Change(){
		Actor actor = party [current_character];
		equip_info [2].GetComponent<Text> ().text = actor.str.ToString ();
		equip_info [4].GetComponent<Text> ().text = actor.mag.ToString ();
		equip_info [6].GetComponent<Text> ().text = actor.con.ToString ();
		equip_info [8].GetComponent<Text> ().text = actor.dex.ToString ();
	}

	public void Change_Equipment(Equip newequip){
		if (current_equiptype == 0) {
			equipment [party [current_character].weapon].quantity++;
			equipment [System.Array.IndexOf (equipment, newequip)].quantity--;
			Update_Stats (equipment [party [current_character].weapon], newequip);
			party [current_character].weapon = System.Array.IndexOf (equipment, newequip);
		}else if (current_equiptype == 1) {
			equipment [party [current_character].armor].quantity++;
			equipment [System.Array.IndexOf (equipment, newequip)].quantity--;
			Update_Stats (equipment [party [current_character].armor], newequip);
			party [current_character].armor = System.Array.IndexOf (equipment, newequip);
		} else if (current_equiptype == 2) {
			equipment [party [current_character].acc].quantity++;
			equipment [System.Array.IndexOf (equipment, newequip)].quantity--;
			Update_Stats (equipment [party [current_character].acc], newequip);
			party [current_character].acc = System.Array.IndexOf (equipment, newequip);
		}

		Set_Equip_Stats (party[current_character]);
		Set_Equipment (party[current_character]);
	}

	void Update_Stats(Equip oldequip, Equip newequip){
		Actor actor = party [current_character];
		actor.str = actor.str - oldequip.str + newequip.str;
		actor.mag = actor.mag - oldequip.mag + newequip.mag;
		actor.con = actor.con - oldequip.vit + newequip.vit;
		actor.dex = actor.dex - oldequip.agi + newequip.agi;
	}

	void Set_Inventory(int inventorytype){
		Equip[] equips = Find_Equipment_Of_Type (EquipType.Acc);
		Actor actor = party[current_character];
		current_equiptype = inventorytype;
		if (inventorytype == -1) {
			for (int i = 0; i < inventorybuttons.Length; i++) {
				inventorybuttons [i].transform.Find ("Text").gameObject.GetComponent<Text> ().text = " ";
				equip_type_text.GetComponent<Text> ().text = " ";
				inventorybuttons [i].GetComponent<Button> ().interactable = false;
			}
		} else {
			equip_type_text.GetComponent<Text> ().text = "Accessory";

			if (inventorytype == 0) {
				equips = Find_Equipment_Of_Type (actor.weapontype);
				equip_type_text.GetComponent<Text> ().text = actor.weapontype.ToString ();
			} else if (inventorytype == 1) {
				equips = Find_Equipment_Of_Type (actor.armortype);
				equip_type_text.GetComponent<Text> ().text = actor.armortype.ToString ();
			} 

			for (int i = 0; i <inventorybuttons.Length; i++) {
				if (i < equips.Length) {
					inventorybuttons [i].transform.Find ("Text").gameObject.GetComponent<Text>().text=equips[i].name;
					inventorybuttons [i].GetComponent<Button> ().interactable = true;
				} else {
					inventorybuttons [i].transform.Find ("Text").gameObject.GetComponent<Text> ().text = "";
					inventorybuttons [i].GetComponent<Button> ().interactable = false;
				}
			}
		}
	}

	void Set_Equipment(Actor actor){
		equipbuttons [0].transform.Find ("Text").gameObject.GetComponent<Text>().text=equipment[actor.weapon].name;
		equipbuttons [1].transform.Find ("Text").gameObject.GetComponent<Text>().text=equipment[actor.armor].name;
		equipbuttons [2].transform.Find ("Text").gameObject.GetComponent<Text>().text=equipment[actor.acc].name;
	}

	public Equip[] Find_Equipment_Of_Type(EquipType type){
		List<Equip> equips = new List<Equip> ();
		for (int i = 0; i < equipment.Length; i++) {
			if (equipment [i].type == type && equipment[i].quantity>0) {
				equips.Add (equipment [i]);
			}
		}

		return equips.ToArray ();
	}

	void Set_Portrait_Interactable(bool interactable){
		for (int i = 0; i < party.Length; i++) {
			HUD_Buttons [i].GetComponent<Button> ().interactable = interactable;
		}
	}

	void Set_Portrait_Dead_Interactable(){
		for (int i = 0; i < party.Length; i++) {
			if (party [i].hp > 0) {
				HUD_Buttons [i].GetComponent<Button> ().interactable = true;
			}
		}
	}

	void Set_Events(){
		for (int i = 0; i < eventbuttons.Length; i++) {
			if (i < unlocked_events.Length-1) {
				eventbuttons [i].transform.Find ("Text").gameObject.GetComponent<Text> ().text = unlocked_events [scroll+i+1].name;
			} else {
				eventbuttons [i].transform.Find ("Text").gameObject.GetComponent<Text> ().text = unlocked_events [0].name;
			}
		}
	}

	public void Set_Selected_Event(int buttonid){
		if (buttonid + 1 + scroll < unlocked_events.Length) {
			eventname.GetComponent<Text> ().text = unlocked_events [buttonid + 1 + scroll].name;
			eventcontent.GetComponent<Text> ().text = unlocked_events [buttonid + 1 + scroll].content;
		} else {
			eventname.GetComponent<Text> ().text = unlocked_events [0].name;
			eventcontent.GetComponent<Text> ().text = unlocked_events [0].content;
		}
	}

	public void Scroll_Up(){
		if (scroll > 0) {
			scroll--;
		}
		Set_Events ();
	}

	public void Scroll_Down(){
		if (scroll < (unlocked_events.Length-7)) {
			scroll++;
		}
		Set_Events ();
	}
}
