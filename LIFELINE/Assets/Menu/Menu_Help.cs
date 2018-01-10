using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu_Help : MonoBehaviour {

	public GameObject helpbar;
	public GameObject textobj;

	enum MenuLog {None, Objective, ItemSelect, PartySelect, EquipStatus, EquipSelect, Status, Options, Item, Equip, Story, Override, TargetAll, Save, Load, Difficulty, MainMenu};

	MenuLog selected;
	MenuLog hover;
	MenuLog oldstate;

	Data_Item items;

	Menu_Flow menu;

	public bool override_text = false;

	// Use this for initialization
	void Start () {

		selected = MenuLog.Objective;
		hover = MenuLog.Objective;
		oldstate = MenuLog.Objective;
		items = GetComponent<Menu_Flow> ().gamedata.GetComponent<Data_Item> ();
		Change_Text ("No Objective");
		menu = GetComponent<Menu_Flow> ();
	}

	// Update is called once per frame
	void Update () {
		if (hover != oldstate && hover != MenuLog.None && !override_text) {
			Update_Text (hover);
		}
		else if (selected != oldstate && hover == MenuLog.None && !override_text) {
			Update_Text (selected);
		}
	}

	void Update_Text(MenuLog state){
		oldstate = state;

		switch (state) {
		case MenuLog.Objective:
			Change_Text ("No Objective");
			break;

		case MenuLog.ItemSelect:
			Change_Text ("Choose an item to use.");
			break;

		case MenuLog.Item:
			Change_Text ("Use a consumable item.");
			break;

		case MenuLog.Equip:
			Change_Text ("Change equipped items.");
			break;

		case MenuLog.Status:
			Change_Text ("Hover over skills to view them.");
			break;

		case MenuLog.Save:
			Change_Text ("Save the game.");
			break;

		case MenuLog.Load:
			Change_Text ("Load the last save point.");
			break;

		case MenuLog.MainMenu:
			Change_Text ("Return to the main menu. Unsaved progress will be lost.");
			break;

		case MenuLog.Story:
			Change_Text ("Review important story events and items.");
			break;

		case MenuLog.Options:
			Change_Text ("Change game settings.");
			break;

		case MenuLog.PartySelect:
			Change_Text ("Choose a character.");
			break;

		case MenuLog.EquipStatus:
			Change_Text ("Click an equipped item to change it.");
			break;

		case MenuLog.EquipSelect:
			Change_Text ("Choose a new item to equip.");
			break;

		case MenuLog.TargetAll:
			Change_Text ("Left click to use Sleeping Bags.");
			break;
		}
	}

	public void Set_Hover_Item(int commandid){
		Set_Hover ("Override");
		int itemid = commandid == 3 ? 4 : commandid;
		Change_Text (items.items[itemid].description + " [" + items.items[itemid].quantity + "]");
	}

	public void Set_Hover_Skill(int commandid){
		Set_Hover ("Override");
		if (commandid < menu.party [menu.current_character].knownSkills.Count) {
			Skill skl = menu.party [menu.current_character].knownSkills [commandid];

			Change_Text (skl.description + " " + skl.cost + " CP");
		} 
	}

	public void Set_Hover_Inventory(int commandid){
		Set_Hover ("Override");
		Equip[] equips = menu.Find_Equipment_Of_Type (EquipType.Acc);
		if (menu.current_equiptype == 0) {
			equips = menu.Find_Equipment_Of_Type (menu.party [menu.current_character].weapontype);
		} else if (menu.current_equiptype == 1) {
			equips = menu.Find_Equipment_Of_Type (menu.party [menu.current_character].armortype);
		}

		if (commandid < equips.Length && menu.state==Menu_Flow.MenuState.EquipSelect) {
			if (equips [commandid] == menu.equipment [0]) {
				Change_Text (equips [commandid].description);
			} else {
				Change_Text (equips [commandid].description + " [" + equips [commandid].quantity.ToString () + "]");
			}
			menu.Show_Equip_Stat_Change (equips [commandid]);
		}
	}

	public void Set_Hover_Equipped(int commandid){
		Set_Hover ("Override");
		Equip equipped;
		if (commandid == 0) {
			equipped = menu.equipment [menu.party [menu.current_character].weapon];
		} else if (commandid == 1) {
			equipped = menu.equipment [menu.party [menu.current_character].armor];
		} else {
			equipped = menu.equipment [menu.party [menu.current_character].acc];
		}

		Change_Text (equipped.description);
	}

	public void Set_Hover_Portrait(int commandid){
		if (menu.state == Menu_Flow.MenuState.Base) {
			Set_Hover ("Override");
			Change_Text ("View " + menu.party [commandid].name + "'s stats and skills.");
		} else if (menu.state == Menu_Flow.MenuState.PartySelect && menu.futurestate==Menu_Flow.MenuState.EquipStatus) {
			Set_Hover ("Override");
			Change_Text ("Change " + menu.party [commandid].name + "'s equipment.");
		}
	}

	IEnumerator Flash_Text(string txt, float duration = 0.5f){
		helpbar.SetActive (true);
		Set_Hover ("Override");
		override_text = true;
		Change_Text (txt);

		yield return new WaitForSeconds (duration);
	}

	void Change_Text(string newtext){
		textobj.GetComponent<Text>().text = newtext;
	}

	public void Set_Hover(string state){
		hover = (MenuLog)System.Enum.Parse (typeof(MenuLog), state);
	}

	public void Set_Selected(string state){
		selected = (MenuLog)System.Enum.Parse (typeof(MenuLog), state);
		Exit_Hover ();
	}

	public void Exit_Hover(){
		hover = MenuLog.None; 
	}

	public void Exit_Inventory_Hover(){
		Exit_Hover ();
		menu.Reset_Equip_Stat_Change ();
	}
}
