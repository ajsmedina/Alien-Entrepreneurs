using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EffectStates {KO, ATKup, ATKup2, ATKdown, DEFup, DEFup2, DEFdown, Poison, Paral, Regen, Taunt, Guard, FakeGuard, Seal, Curse, UsedItem, Life};
public enum Stances {Striker, Breaker, Defender, Augmenter, Enchanter};

/**
 * The Member_Base class is used for all classes that act as a character with hp, stats, and a name. This incldues players and enemies.
 */
[System.Serializable]
public class Member_Base : Data_Type{
	public int hp, mhp;
	public int str, mag, con, dex;
	public string name;
}

/**
 * A State is an effect that provides positive or negative bonuses. It lasts for a set amount of turns.
 */
[System.Serializable]
public class State{
	public EffectStates type;
	public int turns_left;

	public State (EffectStates xtype, int duration){
		type = xtype;
		turns_left = duration;
	}
}

/**
 * An actor is a player characer.
 */
[System.Serializable]
public class Actor : Member_Base{
	public List<Skill> knownSkills = new List<Skill>();
	public int lvl=1, exp=0;
	public EquipType weapontype, armortype;
	public int weapon, armor, acc=0;
	public int basestr, basemag, basecon, basedex;

	public Actor(string xname, EquipType xweapon, EquipType xarmor, int xmhp, int xstr, int xmag, int xcon, int xdex)
	{
		name = xname;

		weapontype = xweapon;
		armortype = xarmor;

		mhp = xmhp;
		hp = mhp;

		str = xstr;
		mag = xmag;
		con = xcon;
		dex = xdex;

		basestr = str;
		basemag = mag;
		basecon = con;
		basedex = dex;
	}

	public int[] Get_Stats(){
		int[] stats = { hp, mhp, str, mag, con, dex };
		return stats;
	}

	public void Learn_Skill(Skill newskill){
		knownSkills.Add (newskill);
	}

	public void Update_Exp(int addexp){
		exp += addexp;
		if(exp>Next_Level()){
			lvl++;
		}
	}

	public int Next_Level(){
		int nexp = 0;
		for(int i=0;i<lvl;i++){
			nexp += 200+123*(2^i);
		}
		return nexp;
	}

}

public class Data_Actor : MonoBehaviour {
	public Actor[] party;

	// Use this for initialization
	void Start () {
		Data_Game loadedGame = SaveLoad.Load ("Actors");
		if (loadedGame==null) {
			Initalize_Party ();
			Update_Actor_Data ();
		} else {
			party = loadedGame.all_elements as Actor[];
		}

	}

	public void Update_Actor_Data(){
		Data_Game newSaveGame = new Data_Game (); 
		newSaveGame.savegameName = "Actors";
		newSaveGame.all_elements = party;
		SaveLoad.Save (newSaveGame);
	}

	void Initalize_Party(){
		party = new Actor[4];
		party [0]= new Actor("Norman", EquipType.Sword, EquipType.Armor, 100, 15, 10, 20, 5);
		party [0].weapon = 2;
		party [0].armor = 7;
		party[0].Learn_Skill(GetComponent<Data_Skill>().skills[5]);
		party[0].Learn_Skill(GetComponent<Data_Skill>().skills[6]);
		party[0].Learn_Skill(GetComponent<Data_Skill>().skills[7]);
		party[0].Learn_Skill(GetComponent<Data_Skill>().skills[8]);

		party [1]= new Actor("Bernard", EquipType.Stick, EquipType.Cloak, 60, 5, 20, 5, 20);
		party [1].weapon = 4;
		party [1].armor = 1;
		party[1].Learn_Skill(GetComponent<Data_Skill>().skills[21]);
		party[1].Learn_Skill(GetComponent<Data_Skill>().skills[22]);
		party[1].Learn_Skill(GetComponent<Data_Skill>().skills[23]);
		party[1].Learn_Skill(GetComponent<Data_Skill>().skills[24]);
		party[1].Learn_Skill(GetComponent<Data_Skill>().skills[25]);
		party[1].Learn_Skill(GetComponent<Data_Skill>().skills[26]);
		party[1].Learn_Skill(GetComponent<Data_Skill>().skills[27]);
		party[1].Learn_Skill(GetComponent<Data_Skill>().skills[28]);

		party [2]= new Actor("Eliza", EquipType.Gauntlets, EquipType.Armor, 80, 10, 15, 15, 10);
		party [2].weapon = 3;
		party [2].armor = 7;
		party[2].Learn_Skill(GetComponent<Data_Skill>().skills[13]);
		party[2].Learn_Skill(GetComponent<Data_Skill>().skills[14]);
		party[2].Learn_Skill(GetComponent<Data_Skill>().skills[15]);
		party[2].Learn_Skill(GetComponent<Data_Skill>().skills[16]);
		party[2].Learn_Skill(GetComponent<Data_Skill>().skills[17]);
		party[2].Learn_Skill(GetComponent<Data_Skill>().skills[18]);
		party[2].Learn_Skill(GetComponent<Data_Skill>().skills[19]);
		party[2].Learn_Skill(GetComponent<Data_Skill>().skills[20]);

		party [3]= new Actor("Cordea", EquipType.Spear, EquipType.Cloak, 75, 20, 5, 10, 15);
		party [3].weapon = 6;
		party [3].armor = 5;
		party[3].Learn_Skill(GetComponent<Data_Skill>().skills[29]);
		party[3].Learn_Skill(GetComponent<Data_Skill>().skills[30]);
		party[3].Learn_Skill(GetComponent<Data_Skill>().skills[31]);
		party[3].Learn_Skill(GetComponent<Data_Skill>().skills[32]);

		UpdateStats ();
	}

	public void UpdateHP(int actorID, int newhp){
		party [actorID].hp = newhp;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void UpdateStats(){
		Equip[] equipment = GetComponent<Data_Equip>().equipment;
		for (int i = 0; i < party.Length; i++) {
			party[i].str = party[i].str+equipment [party[i].weapon].str + equipment [party[i].armor].str + equipment [party[i].acc].str;
			party[i].mag = party[i].mag+equipment [party[i].weapon].mag + equipment [party[i].armor].mag + equipment [party[i].acc].mag;
			party[i].con = party[i].con+equipment [party[i].weapon].vit + equipment [party[i].armor].vit + equipment [party[i].acc].vit;
			party[i].dex = party[i].dex+equipment [party[i].weapon].agi + equipment [party[i].armor].agi + equipment [party[i].acc].agi;
		}
	}
}
