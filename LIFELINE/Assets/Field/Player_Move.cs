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

	Rigidbody2D rb;
	bool can_move = true;
	bool grounded=false;
	bool teleporting=false;
	public Vector2 rel_vel = Vector2.zero;
	public Vector2 true_vel = Vector2.zero;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		if (Time.timeScale == 0.0f) {
		}
		else if (keybind[1] == "1" ? Input.GetKey(KeyCode.LeftArrow) : Input.GetKey (keybind[1])) {
			Move (-1);
		} else if (keybind[3] == "3" ? Input.GetKey(KeyCode.RightArrow) : Input.GetKey (keybind[3])) {
			Move (1);
		} else {
			Horz_Drag ();
		}
		if (Time.timeScale != 0.0f && keybind[0] == "0" ? Input.GetKeyDown(KeyCode.UpArrow) : Input.GetKeyDown (keybind[0])) {
			//rel_vel = Vector2.zero;
			Jump ();
		}

		if (jump_count != 0) {
			//rel_vel = Vector2.zero;
		}
			
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

	/* 
	 * Move up to max speed(based on divinity) left/right if possible
	 * dir: direction to move (-1 left, 1 Right)
	 */
	void Move (int dir){
		if (!can_move) {
			return;
		}
		rb.gravityScale = 2.0f;
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



	/* 
	 * Functions Accessed by Player_Mov to modify variables in this script
	 */

	/* 
	 * Slow horz movement if not moving left/right to create decceleration
	 */
	void Horz_Drag(){
		if (!can_move || !grounded) {
			return;
		}
		Vector2 vel = rb.velocity;
		if (vel.x > 0) {
			vel.x-=0.1f;
		}
		else if (vel.x < 0) {
			vel.x += 0.1f;
		}
		rb.velocity = vel;
	}

	/* 
	 * If possible, jump
	 */
	void Jump (){
		if (!can_move || rb.gravityScale==0.0f) {
			return;
		}
		if (jump_count < max_jump) {
			transform.position = new Vector2 (transform.position.x, transform.position.y + 0.1f);
			rb.velocity = new Vector2 (0.0f, jump_height);
			if (jump_count > 0) {
				jump_count++;
			}
		}
	}
		
	/* 
	 * Resets jump/grab/move variables if grounded
	 */
	void OnCollisionEnter2D(Collision2D coll) {
		can_move = true;

		if ((coll.gameObject.tag == "Floor" || coll.gameObject.tag == "Floor_Move" || coll.gameObject.tag == "Object" ||  coll.gameObject.tag == "Door" ) && transform.position.y > coll.gameObject.transform.position.y) {
			jump_count = 0;
			grounded = true;
			rb.gravityScale = 2.0f;
			rb.velocity = new Vector2 (0, 0);

			if (coll.gameObject.tag == "Floor_Move" || coll.gameObject.tag == "Object") {
				rel_vel = coll.gameObject.GetComponent<Rigidbody2D> ().velocity;
			}
		}

		if ((coll.gameObject.tag == "Player") && transform.position.y > coll.gameObject.transform.position.y) {
			jump_count = 0;
			grounded = true;
			rb.gravityScale = 2.0f;
			rb.velocity = new Vector2 (0, 0);
		}

		if (coll.gameObject.GetComponent<Field_Hazard> () != null && coll.gameObject.GetComponent<Field_Hazard> ().isHazard) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

	}

	void OnCollisionStay2D (Collision2D coll){

		if ((coll.gameObject.tag == "Floor_Move" || (coll.gameObject.tag == "Object" ) )   && transform.position.y > coll.gameObject.transform.position.y && jump_count == 0)  {
			rel_vel = coll.gameObject.GetComponent<Rigidbody2D> ().velocity;
		}

		if (coll.gameObject.GetComponent<Field_Hazard> () != null && coll.gameObject.GetComponent<Field_Hazard> ().isHazard) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

	/* 
	 * Stop the player from using grapples to slide quickly
	 */

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Teleporter" && !teleporting) {
			teleporting = true;
			StartCoroutine (Teleport_Wait (coll.gameObject.GetComponent<Field_Teleporter> ().target.transform.position));
		} else if (coll.gameObject.tag == "Teleporter" && teleporting) {
			teleporting = false;
		} else if (coll.gameObject.tag == "Collect") {
			coll.gameObject.GetComponent<Field_Collectable> ().Collected ();
		}
	}

	IEnumerator Teleport_Wait(Vector2 target){
		yield return new WaitForSeconds (0.3f);
		rb.velocity = Vector2.zero;
		transform.position = target;
	}


	public void Freeze(){
		rb.isKinematic = true;
		GetComponent<BoxCollider2D> ().isTrigger = true;
	}


	public void UnFreeze(){
		rb.isKinematic = false;
		GetComponent<BoxCollider2D> ().isTrigger = false;
	}

	/* 
	 * If leaving ground, count as first jump and possibly first grapple if grappling
	 */
	void OnCollisionExit2D(Collision2D coll) {
		if ((coll.gameObject.tag == "Floor" || coll.gameObject.tag == "Floor_Move" || coll.gameObject.tag == "Player" || coll.gameObject.tag == "Object" ||  coll.gameObject.tag == "Door" ) && transform.position.y > coll.gameObject.transform.position.y) {
			jump_count++;
			grounded = false;
		}
		if (coll.gameObject.tag == "Enemy") {
			Physics2D.IgnoreCollision (coll.gameObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), false);
		}
		if (coll.gameObject.tag == "Floor_Move" || coll.gameObject.tag == "Object")  {
			rel_vel = Vector2.zero;
		}

	}


	/* 
	 * Prevents user input during grapple flight
	 * wait: how long to prevent input
	 */
	IEnumerator Disable_Move(float wait){
		can_move=false;
		yield return new WaitForSeconds(wait);
		can_move=true;
	}
}
