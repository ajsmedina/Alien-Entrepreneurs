using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

[System.Serializable]
public class SpritesArray
{
	public Sprite[] SpriteArray;
}

public class Character
{
	public bool inuse=false;
	public string charname="";
	public GameObject portrait;

	public Character(){
	}
		
	public void SetVisible(bool visibility){
		if (visibility) {
			portrait.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
			inuse = true;
		}else{
			portrait.GetComponent<SpriteRenderer> ().color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
			Direction (false);
			inuse = false;
		}
	}

	public void Direction(bool right){
		portrait.GetComponent<SpriteRenderer> ().flipX = right;
	}
}

public class Dialogue_Flow : MonoBehaviour {

	public GameObject[] portraits;
	public SpritesArray[] portraitSprites;
	public Sprite[] bgSprites;
	public AudioClip[] sounds;
	public AudioClip[] music;
	public float autospeed = 0.07f;


	AudioSource[] sources;
	public GameObject nameobj;
	public GameObject textobj;
	public GameObject indicator;
 	public GameObject bg;
	public GameObject effect;
	new UnityEngine.UI.Text name;
	UnityEngine.UI.Text text;
	UnityEngine.UI.Image ef;
	AudioSource textsound;
	AudioSource effectsound;
	AudioSource bgmsound;
	bool messagedone = true;
	bool messageskip = false;
	bool canClick=true;
	bool messagechange;
	bool auto=false;
	bool autowait=false;
	bool donemessgae=false;
	bool fast=false;
	int ifstack=0;
	int linenum=0;
	GameObject gamedata;

	List<string> convo = new List<string>();

	Character[] characters;

	void Awake (){
		gamedata = GameObject.Find ("GameManager");
		name = nameobj.GetComponent<UnityEngine.UI.Text> ();
		text = textobj.GetComponent<UnityEngine.UI.Text> ();
		ef = effect.GetComponent<UnityEngine.UI.Image> ();
		sources = this.gameObject.GetComponents<AudioSource> ();
		textsound = sources [0];
	}

	// Use this for initialization
	void Start () {
		gamedata.GetComponent<Data_GameManager> ().Fade_In ();
		characters = new Character[4];
		characters [0] = new Character ();
		characters [1] = new Character ();
		characters [2] = new Character ();
		characters [3] = new Character ();
		for (int i = 0; i < characters.Length; i++) {
			characters [i].portrait = portraits [i];
			characters [i].SetVisible (false);
		}

		ef.color = new Color (0, 0, 0, 1.0f);
		LoadDialogue ("Assets/Dialogue/"+gamedata.GetComponent<Data_GameManager>().convoname+".txt");
		StartCoroutine(Execute_Convo ());

	}

	// Update is called once per frame
	void Update () {
		if ((Input.GetMouseButtonDown (0) && canClick) || auto || (fast && canClick)) {
			NextMessage ();
		} else if (canClick && messagedone == true && !indicator.activeSelf) {
			IndicatorToggle ();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			fast = true;
		} else if (Input.GetKeyUp (KeyCode.Space)) {
			fast = false;
		} 
	}

	void LoadDialogue(string filename) {
		string line;
		StreamReader r = new StreamReader (filename);

		using (r) {
			do {
				line = r.ReadLine();
				if (line != null && line!="") {
					convo.Add(line);
				}
			}
			while (line != null);
			r.Close();
		}
	}

	IEnumerator Execute_Convo(){
		string[] lineData;
		int portid=-1;
		//Debug.Log (linenum.ToString ());//s + "/" + convo.Count.ToString ());
		if (linenum > convo.Count-1) {
			donemessgae = true;
			gamedata.GetComponent<Data_GameManager> ().End_Dialog ();
		} else {
			lineData = convo [linenum].Split (';');
			for (int i = 0; i < lineData.Length; i++) {
				lineData [i] = lineData [i].Trim ();
			}
			auto = true;

			if (lineData [0] == "Char") {

				for (int i = 0; i < characters.Length; i++) {
					if (characters [i].inuse == false) {
						portid = i;
						break;
					}
				}

				characters [portid].portrait.GetComponent<SpriteRenderer> ().sprite = portraitSprites [int.Parse (lineData [2])].SpriteArray [int.Parse (lineData [3])];
				characters [portid].SetVisible (true);
				characters [portid].charname = lineData [1];
				Snap (portid, int.Parse (lineData [4]));

				if (lineData.Length > 5 && lineData[5] == "Right") {
					characters [portid].Direction (true);
				}  
			} else if (lineData [0] == "Cmd") {
				if (Check_Char_Exists (lineData [1]) != -1) {
					portid = Check_Char_Exists (lineData [1]);
					if (lineData [2] == "Erase") {
						characters [portid].SetVisible (false);
					} else if (lineData [2] == "ChangeName") {
						characters [portid].charname = lineData [3];
					}  else if (lineData [2] == "Right") {
						characters [portid].Direction (true);
					}  else if (lineData [2] == "Left") {
						characters [portid].Direction (false);
					}  else if (lineData [2] == "Move") {
						Move (portid, int.Parse (lineData [3]));
					} else if (lineData [2] == "MoveWait") {
						IndicatorToggle ();
						auto = false;
						yield return StartCoroutine (MovePortrait (portid, int.Parse (lineData [3]), 10.0f));
						auto = true;
					} else if (lineData [2] == "Change") {
						characters [portid].portrait.GetComponent<SpriteRenderer> ().sprite = portraitSprites [int.Parse (lineData [3])].SpriteArray [int.Parse (lineData [4])];
					}
				} else {
					if (lineData [1] == "AddItem") {
						gamedata.GetComponent<Data_Item> ().items [int.Parse (lineData [2])].Add (int.Parse (lineData [3]));
					} else if (lineData [1] == "AddEquip") {
						gamedata.GetComponent<Data_Equip> ().equipment [int.Parse (lineData [2])].Add (int.Parse (lineData [3]));
					} else if (lineData [1] == "ChangeScene") {
						gamedata.GetComponent<Data_GameManager> ().fieldscene = lineData [2];
					} else if (lineData [1] == "Wait") {
						auto = false;
						yield return new WaitForSeconds (float.Parse(lineData [2]));
						auto = true;
					}
				}
			} else if (lineData [0] == "if") {
				if (lineData [1] == "InParty") {
					if (Convo_Check_Party (int.Parse (lineData [2]))) {
					} else {
						Iterate_If ();
					}
				}
			} else if (lineData [0] == "else") {
				linenum++;
				Iterate_If ();
			} else if (lineData [0] == "end") {
				ifstack--;
			} else if (lineData [0] == "System") {
				int itemid;
				int itemamt;
				string itemlist="";

				ShadeAll ();

				if (lineData [1] == "AddItems") {
					for (int i = 2; i < lineData.Length; i++) {
						itemid = int.Parse (lineData [i].Substring (1));
						itemamt = int.Parse (lineData [i + 1]);
						if (lineData [i] [0] == 'i') {
							gamedata.GetComponent<Data_Item> ().items [itemid].Add (itemamt);
							itemlist += "[Obtained " + gamedata.GetComponent<Data_Item> ().items [itemid].name + " x" + itemamt.ToString () + "]\n";
						} else {
							gamedata.GetComponent<Data_Equip> ().equipment [itemid].Add (itemamt);
							itemlist += "[Obtained " + gamedata.GetComponent<Data_Equip> ().equipment [itemid].name + " x" + itemamt.ToString () + "]\n";
						}
						i++;
					}
					Say (lineData [0], itemlist);
				} else {
					Say (lineData [0], lineData [1]);
				}

				auto = false;
				
			} else if (Check_Char_Exists (lineData [0]) != -1) {
				portid = Check_Char_Exists (lineData [0]);


				if (lineData.Length > 2) {
					if (lineData [2] == "Right") {
						characters [portid].Direction (true);
					}  else if (lineData [2] == "Left") {
						characters [portid].Direction (false);
					} 
				}

				if (portid != -1) {
					Speaker (portid);
				} else {
					ShadeAll ();
				}

				Say (lineData [0], lineData [1]);

				auto = false;
			}

			linenum++;
		}
	}

	void Iterate_If(){
		string lineData;
		int startstack=ifstack+1;
		for (int i = linenum; i < convo.Count; i++) {
			//Debug.Log (ifstack.ToString ()+"/"+i.ToString());
			lineData = convo [i].Split (';')[0].Trim();

			if ((lineData == "end" || lineData == "else")&&ifstack==startstack) {
				linenum = i-1;
				break;
			}else if (lineData == "end") {
				ifstack--;
			}else if (lineData == "if") {
				ifstack++;
			}
		}

		ifstack = startstack;
	}

	bool Convo_Check_Party(int partyid){
		if(gamedata.GetComponent<Data_GameManager>().partymembers.Contains(partyid)){
			return true;
		}
		return false;
	}


	int Check_Char_Exists(string name){
		for (int i = 0; i < characters.Length; i++) {
			if (name == characters [i].charname && characters[i].inuse) {
				return i;
			}
		}
		return -1;
	}


	IEnumerator DisplayText(string messagename, string message, float speed){
		int i = 0;
		bool formatting = false;
		char format = 'b';
		string str;
		IndicatorToggle ();
		textsound.Play ();
		name.text = messagename;
		str = "";
		text.text = "";
		speed = fast ? 0 : speed;
		while( i < message.Length ){
			//Pause during text with "//p"
			if (i<message.Length-2 && message [i] == '/' && message [i + 1] == '/' && message [i + 2] == 'p') {
				i += 2;
				textsound.Pause ();
				if (messageskip ==false) {
					yield return new WaitForSeconds (speed);
				}
			} else {
				if (i < message.Length - 2 && message [i] == '<' && (message [i + 1] == 'b' || message [i + 1] == 'i') && message [i + 2] == '>') { //Deal with bold/italic/color
					format = message [i + 1];
					str += "<"+format+"></"+format+">";
					i += 3;
					formatting = true;
				} else if (i < message.Length - 3 && message [i] == '<'&& message [i + 1] == '/' && (message [i + 2] == 'b' || message [i + 2] == 'i')&& message [i + 3] == '>') { //Deal with bold/italic/color
					formatting = false;
					i+=4;
					if (i == message.Length)
						break;
				}else if ("!?(),. ".IndexOf (message [i]) > -1) { //Don't play a sound if it's a "special character"
					textsound.Stop ();
					if (messageskip ==false && "!?,. ".IndexOf (message [i]) > -1) {
						yield return new WaitForSeconds (speed*2.0f);
					}
				} else if (!textsound.isPlaying){
					textsound.Play ();
				}
				if (formatting) {
					str = str.Substring (0, str.Length - 4);
					str += message [i];
					str += "</"+format+">";
				} else {
					str += message[i];
				}
				text.text = str;
			}
			i++;
			if (messageskip == false) {
				yield return new WaitForSeconds (speed);
			}
		}
		text.text = str;
		yield return new WaitForSeconds (speed);
		messagedone = true;
		messageskip = false;
		textsound.Stop();
		IndicatorToggle ();
	}

	public void Say(string messagename, string message, float speed = 0.01f){
		messagedone = false;
		StartCoroutine (DisplayText (messagename, message, speed));
	}

	public void Speaker(int speakerPortraitID){
		ShadeAll ();
		characters[speakerPortraitID].portrait.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1f);
	}

	public void ShadeAll(){
		for (int i = 0; i < portraits.Length; i++) {
			if (characters [i].inuse) {
				characters [i].portrait.GetComponent<SpriteRenderer> ().color = new Color (0.6f, 0.6f, 0.6f, 1f);
			}
		}
	}

	public void HighlightAll(){
		for (int i = 0; i < portraits.Length; i++) {
			if (characters [i].inuse) {
				characters [i].portrait.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1f);
			}
		}
	}

	public void PlaySE(int soundID){
		effectsound.PlayOneShot (sounds [soundID],1.0f);
	}

	public void PlayBGM(int songID){
		bgmsound.clip = music [songID];
		//sources[2].Play ();
	}

	public void StopBGM(){
		bgmsound.Stop ();
	}


	public void ChangeSprite(int portraitID, int spritelistID, int spriteID){
		portraits [portraitID].GetComponent<SpriteRenderer> ().sprite = portraitSprites [spritelistID].SpriteArray[spriteID];
	}


	IEnumerator MovePortrait(int portraitID, int position, float speed) {
		canClick = false;
		Transform objectToMove = portraits [portraitID].GetComponent<Transform> ();
		Vector3 a = objectToMove.position;
		Vector3 p = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width / 8*position, Screen.height / 2));
		Vector3 b = new Vector3(p.x, objectToMove.position.y, objectToMove.position.z);
		float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
		float t = 0;
		while (t <= 1.0f) {
			t += step; // Goes from 0 to 1, incrementing by step each time
			objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
			yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
		}
		objectToMove.position = b;
		canClick =true;
	}

	public void Move(int portraitID, int position, float speed = 10.0f){
		StartCoroutine (MovePortrait (portraitID, position, speed));
	}

	public void Snap(int portraitID, int position){
		Transform objectToMove = portraits [portraitID].GetComponent<Transform> ();
		Vector2 p = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width / 8*position, Screen.height / 2));
		Vector3 b = new Vector3(p.x, objectToMove.position.y, objectToMove.position.z);
		objectToMove.position = b;
	}

	void IndicatorToggle(){
		if (indicator.activeInHierarchy) {
			indicator.SetActive (false);
		} else {
			indicator.SetActive (true);
		}
	}

	public void ChangeLocation(int locationID){
		bg.GetComponent<SpriteRenderer> ().sprite = bgSprites [locationID];
	}

	public void FadeOut(){
		ef.CrossFadeAlpha(1.0f,2.0f,false);
	}

	public void FadeIn(){
		ef.CrossFadeAlpha(0f,2.0f,false);
	}

	public void Flash(){
		StartCoroutine (FlashRoutine ());
	}

	IEnumerator FlashRoutine(){
		canClick = false;
		ef.color = new Color (1, 1, 1);
		ef.CrossFadeAlpha(1.0f,0.1f,false);
		yield return new WaitForSeconds (0.3f);
		ef.CrossFadeAlpha(0f,0.1f,false);
		yield return new WaitForSeconds (0.2f);
		ef.color = new Color (0, 0, 0);
		canClick = true;
	}

	void NextMessage()
	{
		if (auto) {
			auto = false;
			StopCoroutine ("Say");
			StartCoroutine(Execute_Convo ());
		}else if (canClick) {
			if (messagedone == false) {
				messageskip = true;
			} else if(messagedone==true){
				StopCoroutine ("Say");

				name.text = "";
				text.text = "";
				StartCoroutine(Execute_Convo ());
			}
		}
	}
}