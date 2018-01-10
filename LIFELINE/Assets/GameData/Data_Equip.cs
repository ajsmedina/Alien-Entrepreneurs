using UnityEngine;
using System.Collections;

public enum EquipType {Armor, Cloak, Sword, Gauntlets, Stick, Spear, Acc};

[System.Serializable]
public class Equip : Data_Type{

	public string name, description;
	public int quantity=1, str=0,mag=0,vit=0,agi=0;
	public EquipType type;

	public Equip(string xname, string xdescription, EquipType xtype, int xstr, int xmag, int xvit, int xagi)
	{
		name = xname;
		description = xdescription;
		type = xtype;
		str = xstr;
		mag = xmag;
		vit = xvit;
		agi = xagi;
	}

	public void Add(int amt){
		quantity += amt;
	}

	public int Check_Amt(){
		return quantity;
	}

	public void Remove(int amt=1){
		quantity -= amt;
	}

}

public class Data_Equip : MonoBehaviour {

	// Use this for initialization
	public Equip[] equipment;

	// Use this for initialization
	void Awake () {
		Data_Game loadedGame = SaveLoad.Load ("Equipment");
		if (loadedGame==null) {
			Initialize_Equipment ();
			Data_Game newSaveGame = new Data_Game (); 
			newSaveGame.savegameName = "Equipment";
			newSaveGame.all_elements = equipment;
			SaveLoad.Save (newSaveGame);
		} else {
			equipment = loadedGame.all_elements as Equip[];
		}
	
	}

	void Initialize_Equipment(){
		equipment = new Equip[13];

		equipment [0] = new Equip ("- - -", "No accessory.", 														EquipType.Acc, 	0, 0, 0, 0);
		equipment [0].Add (4);
		equipment [1] = new Equip ("Formal Cloak", "A cloak worn to look fancy. Not very protective.", 				EquipType.Cloak, 	0, 0, 2, 3);
		equipment [2] = new Equip ("Knight's Sword", "A standard longsword wielded by a knight.",					EquipType.Sword, 	5, 0, 0, 0);
		equipment [3] = new Equip ("Knight's Gauntlets", "Protective gauntlets used by knights. Packs a punch.", 	EquipType.Gauntlets,3, 1, 1, 0);
		equipment [4] = new Equip ("Wooden Stick", "A random stick. Not very strong.", 									EquipType.Stick, 		2, 100, 0, 2);
		equipment [5] = new Equip ("Len Redan Cowl", "A cowl handcrafted in Len Reda.", 							EquipType.Cloak, 	0, 0, 3, 3);
		equipment [6] = new Equip ("Len Redan Spear", "A spear handcrafted in Len Reda.", 							EquipType.Spear, 	5, 0, 0, 2);
		equipment [7] = new Equip ("Knight's Armor", "Standard armor worn by a knight.", 							EquipType.Armor, 	0, 0, 5, 0);
		equipment [8] = new Equip ("Ugly Sword", "An ugly sword.",													EquipType.Sword, 	0, 0, 0, 0);
		equipment [9] = new Equip ("Super Sword", "A stupid sword.",												EquipType.Sword, 	25, 0, 0, 0);
		equipment [10] = new Equip ("Potato Costume", "A weird costume?",											EquipType.Armor, 	0, 0, 50, 50);
		equipment [11] = new Equip ("Eggplant Necklace", "An eggplant of power.",									EquipType.Acc, 	10, 0, 0, 0);
		equipment [12] = new Equip ("Eggplant Bracelet", "An eggplant of intelligence.",									EquipType.Acc, 	0, 10, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
