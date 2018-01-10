using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

[System.Serializable]
public class Data_Game {
	public string savegameName = "New Save Game";
	public Data_Type[] all_elements;

}

public static class SaveLoad {
	public static void Initialize_Directories(){
		if (!Directory.Exists ("Assets/Data/Temp/")) {
			Directory.CreateDirectory ("Assets/Data/Temp/");
		}
		if (!Directory.Exists ("Assets/Data/Saved/")) {
			Directory.CreateDirectory ("Assets/Data/Saved/");
		}
	}

	//it's static so we can call it from anywhere
	public static void Save(Data_Game saveGame) {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file;
		file = File.Create ("Assets/Data/Temp/" + saveGame.savegameName + ".dat");

		bf.Serialize(file, saveGame);
		file.Close();

	}    

	public static Data_Game Load(string gameToLoad) {
		if(File.Exists("Assets/Data/Temp/"+gameToLoad + ".dat")) {
			BinaryFormatter bf = new BinaryFormatter();

			FileStream file;

			file = File.Open ("Assets/Data/Temp/" + gameToLoad + ".dat", FileMode.Open);

			Data_Game loadedGame = (Data_Game)bf.Deserialize(file);
			file.Close();
			return loadedGame;
		}
		else {
			return null;
		}
	}

}