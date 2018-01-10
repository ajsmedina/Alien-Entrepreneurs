using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item : Data_Type{

	public string name, description;
	public int skillid;
	public int quantity=0;


	public Item(string xname, string xdescription, int xskill)
	{
		name = xname;
		description = xdescription;
		skillid = xskill;
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

public class Data_Item : MonoBehaviour {


	// Use this for initialization
	public Item[] items;

	// Use this for initialization
	void Awake () {
		Data_Game loadedGame = SaveLoad.Load ("Items");
		if (loadedGame==null) {
			Initialize_Items ();
			Data_Game newSaveGame = new Data_Game (); 
			newSaveGame.savegameName = "Items";
			newSaveGame.all_elements = items;
			SaveLoad.Save (newSaveGame);
		} else {
			items = loadedGame.all_elements as Item[];
		}

	}

	void Initialize_Items(){
		items = new Item[5];
		items [0] = new Item ("Healing Potion", "A lesser healing potion. Recovers half of target's HP.", 37);
		items [0].Add (5);
		items [1] = new Item ("Greater Healing Potion", "A powerful potion. Fully recover one target's HP.", 38);
		items [1].Add (5);
		items [2] = new Item ("Life Stone", "A mystical stone. Revive a fallen ally with 30% HP.", 39);
		items [2].Add (5);
		items [3] = new Item ("Soothing Herbs", "A collection of healing herbs. Recover an ally's negative status affects.", 40);
		items [3].Add (5);
		items [4] = new Item ("Sleeping Bags", "Recover all HP. Usable only from the menu.", 41);
		items [4].Add (5);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
