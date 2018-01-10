using UnityEngine;
using System.Collections;

[System.Serializable]
public class Sprites2D
{
	public Sprite[] SpriteArray;
}

public class Manage : MonoBehaviour {

	public GameObject[] portraits;
	public Sprites2D[] portraitSprites;
	public Sprite[] bgSprites;
	public AudioClip[] sounds;
	public AudioClip[] music;
	private AudioSource[] sources;
	private GameObject nameobj ;
	private GameObject textobj;
	private GameObject dialogueEvent;
	private GameObject indicator;
	private GameObject bg;
	private GameObject effect;
	private UnityEngine.UI.Text name;
	private UnityEngine.UI.Text text;
	private UnityEngine.UI.Image ef;
	private AudioSource textsound;
	private AudioSource effectsound;
	private AudioSource bgmsound;
	private int messagedone = 1; //1=message done, 0=message in progress, 2=message fast forward
	private int messagenum=0;
	private bool canClick=true;
	private bool messagechange;
	public bool auto=false;
	public bool hideIndicator=false;
	public float autospeed = 0.07f;
	private const float defautospeed = 0.07f;
	private bool autocoroutineactive=false;

	void Awake (){
		nameobj = GameObject.Find("Name");
		textobj = GameObject.Find("Text");
		dialogueEvent = GameObject.Find("DialogueEvent");
		indicator = GameObject.Find("Indicator");
		bg = GameObject.Find("Background");
		effect = GameObject.Find("Effect");
		//name = nameobj.GetComponent<UnityEngine.UI.Text> ();
		text = textobj.GetComponent<UnityEngine.UI.Text> ();
		ef = effect.GetComponent<UnityEngine.UI.Image> ();
		sources = this.gameObject.GetComponents<AudioSource> ();
		textsound = sources [0];
		effectsound = sources [1];
		bgmsound = sources [2];
	}
	// Use this for initialization
	void Start () {
		ef.color = new Color (0, 0, 0, 1.0f);
		NextMessage ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			NextMessage ();
		} else if (canClick && messagedone == 1 && !indicator.activeInHierarchy) {
			IndicatorToggle ();
			if (auto && !autocoroutineactive) {
				StartCoroutine (AutoScroll ());
				autocoroutineactive = true;
			}
		}
	}

	IEnumerator DisplayText(string messagename, string message, float speed){
		int i = 0;
		bool formatting = false;
		char format = 'b';
	 	string str;
		textsound.Play ();
		name.text = messagename;
		str = "";
		text.text = "";
		messagedone = 0;
		while( i < message.Length ){
			//Pause during text with "//p"
			if (i<message.Length-2 && message [i] == '/' && message [i + 1] == '/' && message [i + 2] == 'p') {
				i += 2;
				textsound.Pause ();
				if (messagedone ==0) {
					yield return new WaitForSeconds (0.7f);
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
					if (messagedone ==0 && "!?,. ".IndexOf (message [i]) > -1) {
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
			if (messagedone == 0) {
				yield return new WaitForSeconds (speed);
			}
		}
		text.text = str;
		messagedone = 1;
		textsound.Stop();
	}

	public void Say(string messagename, string message, float speed = 0.05f){
		StartCoroutine (DisplayText (messagename, message, speed));
	}

	public void Speaker(int speakerPortraitID){
		for (int i = 0; i < portraits.Length; i++) {
			if (i != speakerPortraitID) {
				portraits [i].GetComponent<SpriteRenderer> ().color = new Color (0.6f, 0.6f, 0.6f, 1f);
			}
		}

		portraits[speakerPortraitID].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1f);
	}

	public void ShadeAll(){
		for (int i = 0; i < portraits.Length; i++) {
			portraits [i].GetComponent<SpriteRenderer> ().color = new Color (0.6f, 0.6f, 0.6f, 1f);
		}
	}

	public void HighlightAll(){
		for (int i = 0; i < portraits.Length; i++) {
			portraits [i].GetComponent<SpriteRenderer> ().color = new Color(1.0f, 1.0f, 1.0f, 1f);
		}
	}

	public void PlaySE(int soundID){
		effectsound.PlayOneShot (sounds [soundID],1.0f);
	}

	public void PlayBGM(int songID){
		bgmsound.clip = music [songID];
		sources[2].Play ();
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
		Vector3 p = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 8*position, Screen.height / 2, Camera.main.nearClipPlane));
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

	public void Move(int portraitID, int position, float speed=10.0f){
		StartCoroutine (MovePortrait (portraitID, position, speed));
	}
	public void Snap(int portraitID, int position){
		Transform objectToMove = portraits [portraitID].GetComponent<Transform> ();
		Vector3 p = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 8*position, Screen.height / 2, Camera.main.nearClipPlane));
		Vector3 b = new Vector3(p.x, objectToMove.position.y, objectToMove.position.z);
		objectToMove.position = b;
	}

	void IndicatorToggle(){
		if (indicator.activeInHierarchy || hideIndicator) {
			indicator.SetActive (false);
		} else {
			indicator.SetActive (true);
		}
	}

	public void ToggleIndicator(){
		if (hideIndicator) {
			hideIndicator = false;
		} else {
			hideIndicator = true;
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
	public void Clear(){
		name.text = "";
		text.text = "";
	}
		
	public void Automatic(bool ison, float setautospeed = defautospeed){
		auto = ison;
		if (ison) {
			autospeed = setautospeed;
		}
	}

	IEnumerator AutoScroll(){
		yield return new WaitForSeconds (autospeed);
		NextMessage ();
	}

	void NextMessage()
	{
		if (canClick) {
			if (messagedone == 0) {
				messagedone = 2;
			} else if(messagedone==1){
				StopCoroutine ("Say");
				StopCoroutine ("AutoScroll");
				autocoroutineactive = false;
				Clear ();
				messagenum++;

				messagenum = dialogueEvent.GetComponent<MessageEvent> ().Next (messagenum);
				IndicatorToggle ();
			}
		}
	}
}