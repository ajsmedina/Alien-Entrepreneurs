using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player_Move : MonoBehaviour {
	public int jump_count = 0;
	public int max_speed;
	public int max_jump;
	public int jump_height;
	public string[] keybind = { "w", "a", "s", "d" };
	public GameObject field;

	public int playerid;

	Rigidbody2D rb;
	bool teleporting=false;
	public Vector2 rel_vel = Vector2.zero;

	/**
	 * Initialize Variables
	 */
	void Start () {
		field = GameObject.Find ("FieldManager");
		rb = GetComponent<Rigidbody2D> ();
	}

	/**
	 * Update is called once per frame and used for movement
	 */
	void Update () {
		//If game is paused, ignore all movement
		//Otherwise, move in the specified direction if the key is pressed
		if (Time.timeScale == 0.0f) {
		} else if (keybind [1] == "1" ? Input.GetKey (KeyCode.LeftArrow) : Input.GetKey (keybind [1])) {
			Move (-1);
		} else if (keybind [3] == "3" ? Input.GetKey (KeyCode.RightArrow) : Input.GetKey (keybind [3])) {
			Move (1);
		}

		if (Time.timeScale != 0.0f && keybind [0] == "0" ? Input.GetKeyDown (KeyCode.UpArrow) : Input.GetKeyDown (keybind [0])) {
			Jump ();
		}

		//If standing still and unpaused, change velocity to relative velocity (velocity of platform standing on)
		if (
			rel_vel != Vector2.zero &&
			!(keybind[1] == "1" ? Input.GetKey(KeyCode.LeftArrow) : Input.GetKey (keybind[1])) &&
			!(keybind[3] == "3" ? Input.GetKey(KeyCode.RightArrow) : Input.GetKey (keybind[3])) &&
			! (Time.timeScale != 0.0f && keybind[0] == "0" ? Input.GetKeyDown(KeyCode.UpArrow) : Input.GetKeyDown (keybind[0])) &&
			jump_count == 0
			) {
			rb.velocity = rel_vel;
		}

	}
	/** 
	 * Move up to max speed
	 * dir: direction to move (-1 left, 1 Right)
	 */
	void Move (int dir){
		if (dir == -1) {
			if (rb.velocity.x > -6.0f) {
				rb.velocity += new Vector2 (-1, 0);
			}
		}
		if (dir == 1) {

			if (rb.velocity.x < 6.0f) {
				rb.velocity += new Vector2 (1, 0);
			}
		}
	}



	/**
	 * If possible, jump
	 */
	void Jump (){
		if (jump_count < max_jump) {
			transform.position = new Vector2 (transform.position.x, transform.position.y + 0.1f);
			rb.velocity = new Vector2 (rb.velocity.x, jump_height);
			if (jump_count > 0) {
				jump_count++;
			}
		}
	}
		
	/** 
	 * Resets jump variables and relative velocity when colliding with object. 
	 * Kills player when contacting hazard.
	 * coll: collider of other object
	 */
	void OnCollisionEnter2D(Collision2D coll) {
		string coltag = coll.gameObject.tag;

		//Land on another platform
		if ((coll.gameObject.tag == "Floor" ||coltag == "Floor_Move" ||coltag == "Player" ||coltag == "Object" || coltag == "Door" )){// && transform.position.y > coll.gameObject.transform.position.y) {
			jump_count = 0;
			rb.velocity = Vector2.zero;

			if (coll.gameObject.tag == "Floor_Move" ||coltag == "Object") {
				rel_vel = coll.gameObject.GetComponent<Rigidbody2D> ().velocity;
			}
		}

		//Reset level if colliding with a hazard
		if (coll.gameObject.GetComponent<Field_Hazard> () != null && coll.gameObject.GetComponent<Field_Hazard> ().isHazard) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

	}

	/**
	 * Matches velocity with the other object collided with.
	 * Kills player when contacting hazard. (Used when hazard switches to dangerous when player is standing on it)
	 * coll: collider of other object
	 */
	void OnCollisionStay2D (Collision2D coll){
		string coltag = coll.gameObject.tag;
		//Modify velocity to match moving object
		if ((coll.gameObject.tag == "Floor_Move" || (coll.gameObject.tag == "Object" ) )   && transform.position.y > coll.gameObject.transform.position.y && jump_count == 0)  {
			rel_vel = coll.gameObject.GetComponent<Rigidbody2D> ().velocity;
		}

		if (coll.gameObject.GetComponent<Field_Hazard> () != null && coll.gameObject.GetComponent<Field_Hazard> ().isHazard) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}


	/**
	 * Used on trigger collision (teleporters/collectables)
	 * coll: collider of other object
	 */
	void OnTriggerEnter2D(Collider2D coll){
		string coltag = coll.gameObject.tag;
		if (coll.gameObject.tag == "Teleporter" && !teleporting) {
			teleporting = true;
			StartCoroutine (Teleport_Wait (coll.gameObject.GetComponent<Field_Teleporter> ().target.transform.position));
		} else if (coll.gameObject.tag == "Teleporter" && teleporting) {
			teleporting = false;
		} else if (coll.gameObject.tag == "Collect") {
			coll.gameObject.GetComponent<Field_Collectable> ().Collected ();
		} else if (coll.gameObject.tag == "Hat") {
			Debug.Log (playerid);
			field.GetComponent<Data_Field>().Change_Hat(playerid, coll.gameObject.GetComponent<Field_Hat>().hatid);
		} else if (coll.gameObject.tag == "Gate") {
			SceneManager.LoadScene(coll.gameObject.GetComponent<Field_Gate>().levelname);
		}
	}

	/**
	 * Teleports played. Called when colliding with a teleporter.
	 * target: the position to teleport to
	 */
	IEnumerator Teleport_Wait(Vector2 target){
		yield return new WaitForSeconds (0.2f);
		rb.velocity = Vector2.zero;
		transform.position = target;
	}


	/**
	 * Add a jump when leaving the ground
	 * coll: collider of other object
	 */
	void OnCollisionExit2D(Collision2D coll) {
		string coltag = coll.gameObject.tag;
		if ((coll.gameObject.tag == "Floor" ||coltag == "Floor_Move" ||coltag == "Player" ||coltag == "Object" || coltag == "Door" )){// && transform.position.y > coll.gameObject.transform.position.y) {
			jump_count++;
		}
		if (coll.gameObject.tag == "Floor_Move" ||coltag == "Object")  {
			rel_vel = Vector2.zero;
		}

	}

	/**
	 * If character falls out of camera, jump to leader.
	 * Used in HUB only
	 */
	void OnBecameInvisible(){
		if( field.GetComponent<Data_Field>().isHub)
			this.gameObject.transform.position = field.GetComponent<Data_Field>().getLeaderPos ();
	}

}
