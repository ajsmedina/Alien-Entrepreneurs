using UnityEngine;
using System.Collections;

public class MessageEvent : MonoBehaviour {
	public GameObject manager;
	private Manage ev;
	private int mess;

	void Awake(){
		ev = manager.GetComponent<Manage> ();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int Next(int messagenum){
		int oldmess = -1;
		mess = messagenum;
		while (oldmess != mess) {
			oldmess = mess;
			StartCoroutine (Message ());
		}
		return mess;
	}

	IEnumerator Message(){
		switch (mess) {
		case 1:
			ev.Clear ();
			ev.FadeIn ();
			ev.ChangeLocation (0);
			ev.Snap (0, 6);
			ev.Snap (1, 2);
			yield return new WaitForSeconds (5.0f);
			break;
		case 2:
			ev.PlayBGM (0);
			ev.ChangeSprite (0, 0, 1);
			ev.Speaker (0);
			ev.Say ("Atsuro", "Mari,//p you <i>stole</i> my//p cookies!");
			break;
		case 3:
			ev.Move (0, 6);
			ev.ChangeSprite (1, 1, 1);
			ev.Speaker (1);
			ev.Say ("Mari", "<b>Atsuro</b>! No//p I//p <b>didn't!</b>");
			break;
		case 4:
			ev.ShadeAll ();
			ev.ToggleIndicator ();
			ev.Automatic(true, 0.2f);
			ev.StopBGM ();
			ev.Move (1, 3);
			ev.Clear ();
			break;
		case 5:
			ev.Flash ();
			ev.PlaySE (0);
			yield return new WaitForSeconds (1.0f);
			break;
		case 6:
			ev.HighlightAll ();
			ev.Automatic(false);
			ev.ToggleIndicator ();
			ev.Say ("Mari", "Meanie!");
			break; 
		case 7:
			ev.ChangeSprite (0, 0, 0);
			ev.Move (0, 9);
			ev.Say ("Atsuro", "Bye!");
			ev.FadeOut ();
			break; 
		case 8:
			ev.FadeIn ();
			ev.ChangeLocation (1);
			ev.Snap (1, -2);
			ev.Snap (0, -2);
			ev.Move (0, 4);
			ev.Say ("Atsuro", "Wow! New location!");
			break;
		case 9:
			ev.ChangeSprite (0, 0, 1);
			ev.Say ("Atsuro", "Okay, so does text flow nicely if I talk a lot? What //p. //p. //p. is that?");
			break;
		}
	}
}
