using UnityEngine;
using System.Collections;

[System.Serializable]
public class LogEntry : Data_Type{

	public string name, content;
	public bool unlocked=false;


	public LogEntry(string xname, string xcontent)
	{
		name = xname;
		content = xcontent;
	}

	public void Unlock_Entry(){
		unlocked = true;
	}
}

public class Data_StoryLog : MonoBehaviour {


	// Use this for initialization
	public LogEntry[] entries;

	// Use this for initialization
	void Awake () {
		Data_Game loadedGame = SaveLoad.Load ("StoryLog");
		if (loadedGame==null) {
			Initialize_Entries ();
			Data_Game newSaveGame = new Data_Game (); 
			newSaveGame.savegameName = "StoryLog";
			newSaveGame.all_elements = entries;
			SaveLoad.Save (newSaveGame);
		} else {
			entries = loadedGame.all_elements as LogEntry[];
		}

	}

	void Initialize_Entries(){
		entries = new LogEntry[4];
		entries [0] = new LogEntry ("Blank Page", "No Content");
		entries [0].Unlock_Entry();

		entries [1] = new LogEntry ("A Step into the Unknown", "Norman: I met up with Bernard.");
		entries [1].Unlock_Entry();
		entries [2] = new LogEntry ("Pie", "Norman: Im gay");
		entries [2].Unlock_Entry();
		entries [3] = new LogEntry ("A", "Norman: jk im bi");
		entries [3].Unlock_Entry();
	}

	// Update is called once per frame
	void Update () {

	}
}
