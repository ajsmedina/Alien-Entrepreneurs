using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public enum RangeScope {Self,One_Ally,One_Enemy,All_Allies,All_Enemies};
public enum SkillTypes {Phys, Prim, Alm, Heal, Effect, Item};
public enum SecondaryEffects {None, Regen, DEFup, OwnDEFup, ATKup, Revive, Restore, Poison, Paral, DEFdown, OwnDEFdown, RemoveBuffs, Guard, FakeGuard, OwnGuard, Taunt, Revenge, Seal, Curse};


[System.Serializable]
public class Data_Type{
}

[System.Serializable]
public class Skill : Data_Type{

	public string name,description;
	public RangeScope range;
	public SkillTypes type;
	public SecondaryEffects effect;
	public float bp, activationrate;
	public int cost, duration;


	public Skill(string xname, string xdescription, SkillTypes xtype, RangeScope xrange, float xbp, int xcost, SecondaryEffects xeffect=SecondaryEffects.None, int xduration = 3, float xactivationrate=1.0f)
	{
		name = xname;
		description = xdescription;
		type = xtype;
		range = xrange;
		bp = xbp;
		cost = xcost;
		duration = xduration;
		effect = xeffect;
		activationrate = xactivationrate;

	}

}


public class Data_Skill : MonoBehaviour {

	// Use this for initialization
	public Skill[] skills;

	public float base_single, base_all, base_severe;
	public int cost_single, cost_all, cost_severe;

	// Use this for initialization
	void Awake () {
		Data_Game loadedGame = SaveLoad.Load ("Skills");
		if (loadedGame==null) {
			Initialize_Skills ();
			Data_Game newSaveGame = new Data_Game (); 
			newSaveGame.savegameName = "Skills";
			newSaveGame.all_elements = skills;
			SaveLoad.Save (newSaveGame);
		} else {
			skills = loadedGame.all_elements as Skill[];
		}

	}

	void Initialize_Skills(){
		skills = new Skill[44];

		//General Skills
		skills [0] = new Skill ("Attack", "A Basic Attack on one target.", 
			SkillTypes.Phys, RangeScope.One_Enemy, 1.0f, 0); 
		skills [1] = new Skill ("Guard", "Defend against attacks and restore Capacity.", 
			SkillTypes.Effect, RangeScope.Self, 0.0f, 0, SecondaryEffects.Guard, 1);
		skills [2] = new Skill ("Pass", "Defend against attacks and restore Capacity.", 
			SkillTypes.Effect, RangeScope.All_Allies, 0.0f, 0, SecondaryEffects.Guard);
		skills [3] = new Skill ("End", "End turn.", 
			SkillTypes.Effect, RangeScope.Self, 0.0f, 0);
		skills [4] = new Skill ("Flee", "End turn.", 
			SkillTypes.Effect, RangeScope.All_Allies, 0.0f, 0);

		//Norman Default
		skills [5] = new Skill ("Regen", "Heal 30% HP and recover more overtime.", 
			SkillTypes.Heal, RangeScope.One_Ally, 0.30f, 30, SecondaryEffects.Regen, 5); 
		skills [6] = new Skill ("Shield Bash", "Physical damage to one target.", 
			SkillTypes.Phys, RangeScope.One_Enemy, 0.1f, cost_single, SecondaryEffects.DEFdown); 
		skills [7] = new Skill ("Light Blast", "Almighty damage to one target.", 
			SkillTypes.Alm, RangeScope.One_Enemy, base_single, cost_single); 
		skills [8] = new Skill ("Challenge", "Attract single-target enemy attacks.", 
			SkillTypes.Effect, RangeScope.Self, 0.0f, 50, SecondaryEffects.Taunt); 

		//Norman Level Up
		skills [9] = new Skill ("Warrior's Charge", "Physical damage to all enemies.", 
			SkillTypes.Phys, RangeScope.All_Enemies, base_all, cost_all); 
		skills [10] = new Skill ("Healing Field", "Give an HP-regenerating effect to all party members.", 
			SkillTypes.Effect, RangeScope.All_Allies, 0.0f, 60, SecondaryEffects.Regen, 5); 
		skills [11] = new Skill ("Wrathful Shine", "Almighty damage to all enemies.", 
			SkillTypes.Alm, RangeScope.All_Enemies, base_all, cost_all); 
		skills [12] = new Skill ("Bastion", "Order an ally to guard, even if they can't.", 
			SkillTypes.Effect, RangeScope.One_Ally, 0.0f, 50, SecondaryEffects.FakeGuard, 1);

		//Eliza Default
		skills [13] = new Skill ("Recover", "Heal 65% HP to one target.", 
			SkillTypes.Heal, RangeScope.One_Ally, 0.65f, 30); 
		skills [14] = new Skill ("Life Surge", "Heal 45% HP to all allies.", 
			SkillTypes.Heal, RangeScope.All_Allies, 0.45f, 60); 
		skills [15] = new Skill ("Fireball", "Primal damage to one target.", 
			SkillTypes.Prim, RangeScope.One_Enemy, base_single, cost_single); 
		skills [16] = new Skill ("Sonic Punch", "Physical damage to one target.", 
			SkillTypes.Phys, RangeScope.One_Enemy, base_single, cost_single); 

		//Eliza Level Up
		skills [17] = new Skill ("Fire Dance", "Primal damage to all enemies.", 
			SkillTypes.Prim, RangeScope.All_Enemies, base_all, cost_all); 
		skills [18] = new Skill ("Remedy", "Heal 30% HP to one ally and heal status effects.", 
			SkillTypes.Heal, RangeScope.One_Ally, 0.3f, 30, SecondaryEffects.Restore); 
		skills [19] = new Skill ("Revive", "Revive a fallen ally with 50% HP.", 
			SkillTypes.Heal, RangeScope.One_Ally, 0.5f, 60, SecondaryEffects.Revive); 
		skills [20] = new Skill ("Hammer Fist", "Physical damage to all enemies.", 
			SkillTypes.Phys, RangeScope.One_Enemy, base_all, cost_all); 

		//Bernard Default
		skills [21] = new Skill ("Lightning Bolt", "Primal damage to one target.", 
			SkillTypes.Prim, RangeScope.One_Enemy, base_single, cost_single); 
		skills [22] = new Skill ("Thunderstorm", "Primal damage to all targets.", 
			SkillTypes.Prim, RangeScope.All_Enemies, base_all, cost_all); 
		skills [23] = new Skill ("Magic Arrow", "Almighty damage to one target.", 
			SkillTypes.Alm, RangeScope.One_Enemy, base_single, cost_single); 
		skills [24] = new Skill ("Sword Rain", "Almighty damage to all enemies.", 
			SkillTypes.Alm, RangeScope.All_Enemies, base_all, cost_all); 

		//Bernard Level Up
		skills [25] = new Skill ("Phoenix Blessing", "Revive a fallen ally with 20% HP.", 
			SkillTypes.Heal, RangeScope.One_Ally, 0.2f, 50, SecondaryEffects.Revive); 
		skills [26] = new Skill ("Dispel", "Remove buffs from one enemy.", 
			SkillTypes.Effect, RangeScope.All_Enemies, 0.0f, 20, SecondaryEffects.RemoveBuffs); 
		skills [27] = new Skill ("Primal Force", "Severe primal damage to one target.", 
			SkillTypes.Prim, RangeScope.One_Enemy, base_severe, cost_severe); 
		skills [28] = new Skill ("Justice Hammer", "Sever almight damage to one target.", 
			SkillTypes.Alm, RangeScope.One_Enemy, base_severe, cost_severe); 

		//Dea Default
		skills [29] = new Skill ("Storm Piercer", "Physical damage to one target.", 
			SkillTypes.Phys, RangeScope.One_Enemy, base_single, cost_single); 
		skills [30] = new Skill ("Tornado Blitz", "Physical damage to all enemies.", 
			SkillTypes.Phys, RangeScope.All_Enemies, base_all, cost_all); 
		skills [31] = new Skill ("Sure Shot", "Mild physical damage to one target. Guaranteed hit. Increase focus.", 
			SkillTypes.Phys, RangeScope.One_Enemy, base_all, cost_single); 
		skills [32] = new Skill ("Critical Blade", "Severe physical damage to one enemy.", 
			SkillTypes.Phys, RangeScope.One_Enemy, base_severe, cost_severe); 

		//Dea Level Up
		skills [33] = new Skill ("Bullet Hell", "Physical damage to all enemies.", 
			SkillTypes.Phys, RangeScope.All_Enemies, 1.3f, 40);
		skills [34] = new Skill ("Covering Fire", "Mild Physical damage to one target and guard self.", 
			SkillTypes.Phys, RangeScope.One_Enemy, 1.0f, 60, SecondaryEffects.OwnGuard, 1); 
		skills [35] = new Skill ("Reckless Charge", "Heavy Physical damage to all targets, but lower own defenses.", 
			SkillTypes.Phys, RangeScope.All_Enemies, 2.5f, 50, SecondaryEffects.OwnDEFdown); 
		skills [36] = new Skill ("Desperation", "Physical damage to one target. Deals more damage the less HP you have.", 
			SkillTypes.Phys, RangeScope.One_Enemy, 1.0f, 50, SecondaryEffects.Revenge); 

		//Items
		skills [37] = new Skill ("Healing Potion", "A lesser healing potion. Recovers half of target's HP.", 
			SkillTypes.Item, RangeScope.One_Ally, 0.5f, 0); 
		skills [38] = new Skill ("Greater Healing Potion", "A powerful potion. Fully recover one target's HP.", 
			SkillTypes.Item, RangeScope.One_Ally, 1.0f, 0); 
		skills [39] = new Skill ("Life Stone", "A mystical stone. Revive a fallen ally with 30% HP.", 
			SkillTypes.Item, RangeScope.One_Ally, 0.3f, 0, SecondaryEffects.Revive); 
		skills [40] = new Skill ("Soothing Herbs", "A collection of healing herbs. Recover an ally's negative status affects.", 
			SkillTypes.Item, RangeScope.One_Ally, 0.0f, 0, SecondaryEffects.Restore); 
		skills [41] = new Skill ("Sleeping Bags", "A set of sleeping bags. Recover all HP. Usable only from the menu.", 
			SkillTypes.Item, RangeScope.All_Allies, 1.0f, 0, SecondaryEffects.Revive); 


		skills [42] = new Skill ("Seal", "Heavy Physical damage to all targets, but lower own defenses.", 
			SkillTypes.Effect, RangeScope.One_Enemy, 0.0f, 50, SecondaryEffects.Seal); 
		skills [43] = new Skill ("Shock", "Physical damage to one target. Deals more damage the less HP you have.", 
			SkillTypes.Effect, RangeScope.One_Enemy, 0.0f, 50, SecondaryEffects.Paral); 
		/**
		skills = new Skill[44];
		skills [0] = new Skill ("Attack", "A Basic Attack on one target.", 
			SkillTypes.Phys, RangeScope.One_Enemy, 1.0f, 0); 
		skills [1] = new Skill ("Guard", "Defend against attacks and restore Capacity.", 
			SkillTypes.Effect, RangeScope.Self, 0.0f, 0, SecondaryEffects.Guard, 1);
		skills [2] = new Skill ("Pass", "Defend against attacks and restore Capacity.", 
			SkillTypes.Effect, RangeScope.All_Allies, 0.0f, 0, SecondaryEffects.Guard);
		skills [3] = new Skill ("End", "End turn.", 
			SkillTypes.Effect, RangeScope.Self, 0.0f, 0);
		skills [4] = new Skill ("Flee", "End turn.", 
			SkillTypes.Effect, RangeScope.All_Allies, 0.0f, 0);

		//Norman Default
		skills [5] = new Skill ("Regen", "Heal 30% HP and recover more overtime.", 
			SkillTypes.Heal, RangeScope.One_Ally, 0.30f, 30, SecondaryEffects.Regen, 5); 
		skills [6] = new Skill ("Shield Bash", "Physical damage to one target and raise own defense.", 
			SkillTypes.Phys, RangeScope.One_Enemy, 1.5f, 50, SecondaryEffects.OwnDEFup); 
		skills [7] = new Skill ("Light Blast", "Almighty damage to one target.", 
			SkillTypes.Alm, RangeScope.One_Enemy, base_single, cost_single); 
		skills [8] = new Skill ("Challenge", "Attract single-target enemy attacks.", 
			SkillTypes.Effect, RangeScope.Self, 0.0f, 50, SecondaryEffects.Taunt); 

		//Norman Level Up
		skills [9] = new Skill ("Dragon's Charge", "Physical damage to all enemies.", 
			SkillTypes.Phys, RangeScope.All_Enemies, base_all, cost_all); 
		skills [10] = new Skill ("Holy Field", "Give an HP-regenerating effect to all party members.", 
			SkillTypes.Effect, RangeScope.All_Allies, 0.0f, 60, SecondaryEffects.Regen, 5); 
		skills [11] = new Skill ("War Cry", "Raise all allies' attack.", 
			SkillTypes.Effect, RangeScope.All_Allies, 0.0f, 40, SecondaryEffects.ATKup); 
		skills [12] = new Skill ("Bastion", "Order an ally to guard, even if they can't.", 
			SkillTypes.Effect, RangeScope.One_Ally, 0.0f, 50, SecondaryEffects.FakeGuard, 1);

		//Eliza Default
		skills [13] = new Skill ("Healing Force", "Heal 65% HP to one target.", 
			SkillTypes.Heal, RangeScope.One_Ally, 0.65f, 30); 
		skills [14] = new Skill ("Life Surge", "Heal 40% HP to all allies.", 
			SkillTypes.Heal, RangeScope.All_Allies, 0.4f, 60); 
		skills [15] = new Skill ("Fireball", "Primal damage to one target.", 
			SkillTypes.Prim, RangeScope.One_Enemy, base_single, cost_single); 
		skills [16] = new Skill ("Sanctuary", "Raise all allies' defense.", 
			SkillTypes.Effect, RangeScope.All_Allies, 0.0f, 40, SecondaryEffects.DEFup); 

		//Eliza Level Up
		skills [17] = new Skill ("Fire Dance", "Primal damage to all enemies.", 
			SkillTypes.Prim, RangeScope.All_Enemies, base_all, cost_all); 
		skills [18] = new Skill ("Remedy", "Heal 30% HP to one ally and heal status effects.", 
			SkillTypes.Heal, RangeScope.One_Ally, 0.3f, 30, SecondaryEffects.Restore); 
		skills [19] = new Skill ("Revive", "Revive a fallen ally with 50% HP.", 
			SkillTypes.Heal, RangeScope.One_Ally, 0.5f, 60, SecondaryEffects.Revive); 
		skills [20] = new Skill ("Final Punch", "Physical damage to one target.", 
			SkillTypes.Phys, RangeScope.One_Enemy, base_single, cost_single); 

		//Vagnr Default
		skills [21] = new Skill ("Lightning Bolt", "Primal damage to one target.", 
			SkillTypes.Prim, RangeScope.One_Enemy, base_single, cost_single); 
		skills [22] = new Skill ("Thunderstorm", "Primal damage to all targets.", 
			SkillTypes.Prim, RangeScope.All_Enemies, base_all, cost_all); 
		skills [23] = new Skill ("Toxic Cloud", "Poison all enemies.", 
			SkillTypes.Effect, RangeScope.All_Enemies, 0.0f, 50, SecondaryEffects.Poison); 
		skills [24] = new Skill ("Magic Arrow", "Almighty damage to one target.", 
			SkillTypes.Alm, RangeScope.One_Enemy, base_single, cost_single); 

		//Vagnr Level Up
		skills [25] = new Skill ("Fear", "Chance to paralyze all enemies.", 
			SkillTypes.Effect, RangeScope.All_Enemies, 0.0f, 30, SecondaryEffects.Paral, 1, 0.8f); 
		skills [26] = new Skill ("Dispel", "Remove buffs from all enemies.", 
			SkillTypes.Effect, RangeScope.All_Enemies, 0.0f, 40, SecondaryEffects.RemoveBuffs); 
		skills [27] = new Skill ("Primal Force", "Severe primal damage to one target.", 
			SkillTypes.Prim, RangeScope.One_Enemy, base_severe, cost_severe); 
		skills [28] = new Skill ("Sword Rain", "Almighty damage to all enemies.", 
			SkillTypes.Alm, RangeScope.All_Enemies, base_all, cost_all); 

		//Deus Default
		skills [29] = new Skill ("Storm Piercer", "Physical damage to one target.", 
			SkillTypes.Phys, RangeScope.One_Enemy, base_single, cost_single); 
		skills [30] = new Skill ("Tornado Blitz", "Physical damage to all enemies.", 
			SkillTypes.Phys, RangeScope.All_Enemies, base_all, cost_all); 
		skills [31] = new Skill ("Head Shot", "Physical damage to one target and lower target's defenses.", 
			SkillTypes.Phys, RangeScope.One_Enemy, 1.3f, 40, SecondaryEffects.DEFdown); 
		skills [32] = new Skill ("Critical Blade", "Severe physical damage to one enemy.", 
			SkillTypes.Phys, RangeScope.One_Enemy, base_severe, cost_severe); 

		//Deus Level Up
		skills [33] = new Skill ("Leg Shot", "Physical damage to one target and paralyze target.", 
			SkillTypes.Phys, RangeScope.One_Enemy, 1.3f, 40, SecondaryEffects.Paral, 1, 0.8f); 
		skills [34] = new Skill ("Covering Fire", "Mild Physical damage to one target and guard self.", 
			SkillTypes.Phys, RangeScope.One_Enemy, 1.0f, 60, SecondaryEffects.OwnGuard, 1); 
		skills [35] = new Skill ("Reckless Charge", "Heavy Physical damage to all targets, but lower own defenses.", 
			SkillTypes.Phys, RangeScope.All_Enemies, 2.5f, 50, SecondaryEffects.OwnDEFdown); 
		skills [36] = new Skill ("Desperation", "Physical damage to one target. Deals more damage the less HP you have.", 
			SkillTypes.Phys, RangeScope.One_Enemy, 1.0f, 50, SecondaryEffects.Revenge); 

		//Items
		skills [37] = new Skill ("Healing Potion", "A lesser healing potion. Recovers half of target's HP.", 
			SkillTypes.Item, RangeScope.One_Ally, 0.5f, 0); 
		skills [38] = new Skill ("Greater Healing Potion", "A powerful potion. Fully recover one target's HP.", 
			SkillTypes.Item, RangeScope.One_Ally, 1.0f, 0); 
		skills [39] = new Skill ("Life Stone", "A mystical stone. Revive a fallen ally with 30% HP.", 
			SkillTypes.Item, RangeScope.One_Ally, 0.3f, 0, SecondaryEffects.Revive); 
		skills [40] = new Skill ("Soothing Herbs", "A collection of healing herbs. Recover an ally's negative status affects.", 
			SkillTypes.Item, RangeScope.One_Ally, 0.0f, 0, SecondaryEffects.Restore); 
		skills [41] = new Skill ("Sleeping Bags", "A set of sleeping bags. Recover all HP. Usable only from the menu.", 
			SkillTypes.Item, RangeScope.All_Allies, 1.0f, 0, SecondaryEffects.Revive); 

		skills [42] = new Skill ("Seal", "Heavy Physical damage to all targets, but lower own defenses.", 
			SkillTypes.Effect, RangeScope.One_Enemy, 0.0f, 50, SecondaryEffects.Seal); 
		skills [43] = new Skill ("Shock", "Physical damage to one target. Deals more damage the less HP you have.", 
			SkillTypes.Effect, RangeScope.One_Enemy, 0.0f, 50, SecondaryEffects.Paral); */
	}

	// Update is called once per frame
	void Update () {

	}
}