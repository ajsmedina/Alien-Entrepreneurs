using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * An Enemy is an opponent that can be fought in battle.
 */
[System.Serializable]
public class Enemy : Member_Base{
	public int mcp=100, msp=100;
	public int exp, myr;
	public List<EnemySkill> knownSkills = new List<EnemySkill>();
	public float physres=1.0f, primres=1.0f, almres=1.0f;

	public Enemy(string xname, int xmhp, int xstr, int xmag, int xcon, int xdex, int xexp, int xmyr, float xphys=1.0f, float xprim=1.0f, float xalm = 1.0f)
	{
		name = xname;
		mhp = xmhp;

		str = xstr;
		mag = xmag;
		con = xcon;
		dex = xdex;

		exp = xexp;
		myr = xmyr;

		physres = xphys;
		primres = xprim;
		almres = xalm;
	}

	public int[] Get_Stats(){
		int[] stats = { mhp, str, mag, con, dex };
		return stats;
	}

	public float[] Get_Res(){
		float[] stats = { physres,primres,almres };
		return stats;
	}

	public void Learn_Skill(EnemySkill newskill){
		knownSkills.Add (newskill);
	}

}

[System.Serializable]
public class EnemySkill{
	public enum Condition {None, DEFdown, Poison, Paral, HPb50, HPa50, HPb30, Buff, Debuff, Guard, Break};
	public enum Condition_Target {None, Self, Enemy, Ally};
	public enum Target {Random, All, HighHP, LowHP, LowSP, DEFdown, Poison, Paral, Buff};
	public int skillid, chance;
	public Condition use_condition;
	public Condition_Target target_condition;
	public Target target;

	public EnemySkill(int xid, int xchance, Condition xuse, Condition_Target xtc, Target xtarget = Target.Random)
	{
		skillid = xid;
		use_condition = xuse;
		target_condition = xtc;
		target = xtarget;
		chance = xchance;

	}
}

[System.Serializable]
public class Encounter : Data_Type{
	public int size;
	public int[] enemyid=new int[6];
	public int[] xpos=new int[6];
	public int[] ypos=new int[6];
	public bool canrun = true;

	public Encounter(int e0, int e1=-1, int e2=-1, int e3=-1, int e4=-1, int e5=-1)
	{
		enemyid [0] = e0;
		enemyid [1] = e1;
		enemyid [2] = e2;
		enemyid [3] = e3;
		enemyid [4] = e4;
		enemyid [5] = e5;

		size = 0;
		for (int i = 0; i < 5; i++) {
			if (enemyid [i] >= 0) {
				size++;
			}
		}
	}

	public void SetPosition(int xpos0, int ypos0,		int xpos1=0, int ypos1=0, int xpos2=0, int ypos2=0, 
							int xpos3=0, int ypos3=0,	int xpos4=0, int ypos4=0, int xpos5=0, int ypos5=0){

		xpos [0] = xpos0;
		xpos [1] = xpos1;
		xpos [2] = xpos2;
		xpos [3] = xpos3;
		xpos [4] = xpos4;
		xpos [5] = xpos5;

		ypos [0] = ypos0;
		ypos [1] = ypos1;
		ypos [2] = ypos2;
		ypos [3] = ypos3;
		ypos [4] = ypos4;
		ypos [5] = ypos5;
	}

}

public class Data_Enemy : MonoBehaviour {

	public Enemy[] enemies;
	public Encounter[] encounters;

	// Use this for initialization
	void Start () {

		Data_Game loadedGame = SaveLoad.Load ("Enemies");
		if (loadedGame==null) {
			Initialize_Enemies ();
			Data_Game newSaveGame = new Data_Game (); 
			newSaveGame.savegameName = "Enemies";
			newSaveGame.all_elements = enemies;
			SaveLoad.Save (newSaveGame);
		} else {
			enemies = loadedGame.all_elements as Enemy[];
		}

		Data_Game loadedGame2 = SaveLoad.Load ("Encounters");
		if (loadedGame2==null) {
			Initialize_Encounters ();
			Data_Game newSaveGame = new Data_Game (); 
			newSaveGame.savegameName = "Encounters";
			newSaveGame.all_elements = encounters;
			SaveLoad.Save (newSaveGame);
		} else {
			encounters = loadedGame2.all_elements as Encounter[];
		}


	}

	void Initialize_Enemies(){
		enemies = new Enemy[2];
		enemies [0]= new Enemy("Bat", 60, 20, 0, 10, 10, 200,50,1.3f,1.0f,1.0f);
		enemies [0].Learn_Skill (new EnemySkill(29,5,EnemySkill.Condition.None,EnemySkill.Condition_Target.None,EnemySkill.Target.Random));
		//enemies [0].Learn_Skill (new EnemySkill (0, 5, EnemySkill.Condition.None, EnemySkill.Condition_Target.None, EnemySkill.Target.Random));
		enemies [1]= new Enemy("Slime", 100, 20, 10, 10, 5, 200,50,0.75f,1.0f,1.0f);
		enemies [1].Learn_Skill (new EnemySkill(37,5,EnemySkill.Condition.None,EnemySkill.Condition_Target.None,EnemySkill.Target.Random));
		//enemies [1].Learn_Skill (new EnemySkill (0, 5, EnemySkill.Condition.None, EnemySkill.Condition_Target.None, EnemySkill.Target.Random));
		//enemies [2]= new Enemy("Bugbear", 60, 15, 0, 15, 5);
		//enemies [3]= new Enemy("Wolf", 50, 15, 0, 10, 15);
	}

	void Initialize_Encounters(){
		encounters = new Encounter[3];
		encounters [0] = new Encounter (0, 0);
		encounters [0].SetPosition(-100,-100, 50,50);
		encounters [1] = new Encounter (1);
		encounters [1].SetPosition(-100,0);
		encounters [2] = new Encounter (0,0,0);
		encounters [2].SetPosition(-100,0,50,50,200,-50);
		encounters [2].canrun = false;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
