	using UnityEngine;
	using System.Collections;

	/*
	public class Moving_Platform : MonoBehaviour {
		public float direction = 1.0f;
		public float interval = 2.0f;
		public float startpercent= 0.5f;

		// Use this for initialization
		void Start () {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (2.0f*direction, 0.0f);
			GetComponent<Rigidbody2D> ().freezeRotation = true;
			InvokeRepeating ("Change_Direction",interval*startpercent,interval);
		}
		
		// Update is called once per frame
		void Update () {
		}

		void Change_Direction(){
			direction *= -1.0f;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (2.0f*direction, 0.0f);
		}
	}
	*/
	public enum Move_Types {REPEAT, TARGET};
	//3 types of platforms: 
	//REPEAT: Keeps moving back and forth as long as power is on
	//TARGET: Moves to target position when switched. Behaviour can be determined by button types.

	//there are also 3 types of buttons:
	//TOGGLE: On/Off
	//ONE: Press, power stays on forvever
	//PRESS: On as long as it's being pressed


	public class Moving_Platform : MonoBehaviour {
		public bool vertical;
		public bool power;
		public bool repeating;

		public bool defaultpower;

		public float speed;
		public float max;
		public float min;
		public bool dir;

	public bool flip;
	public bool alwaysOn;

		private float maxpos;
		private float minpos;


		// Use this for initialization
		void Start () {
		power = this.gameObject.GetComponent<Field_Power> ().defOn;
		defaultpower = power;
			if (vertical) {
				maxpos = transform.position.y + max;
				minpos = transform.position.y - min;
			} else {
				maxpos = transform.position.x + max;
				minpos = transform.position.x - min;
			}
		}

		// Update is called once per frame
	void Update () {
		if (alwaysOn) {
			power = true;
		} else if (flip) {
			power = !this.gameObject.GetComponent<Field_Power> ().isOn;
		} else {
			power = this.gameObject.GetComponent<Field_Power> ().isOn;
		}

			if (repeating) {
				if (power) {

					if (vertical) {

						if (transform.position.y > maxpos) {
							dir = false;
						} else if (transform.position.y < minpos) {
							dir = true;
						}
						if (dir == true) {
							GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, speed);
						} else {
							GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -speed);
						}
				
					} else {

						if (transform.position.x > maxpos) {
							dir = false;
						} else if (transform.position.x < minpos) {
							dir = true;
						}
						if (dir == true) {
							GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
						} else {
							GetComponent<Rigidbody2D> ().velocity = new Vector2 (-speed, 0);
						}

					}
				} else {
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
				}
			}else  {

					if (vertical) {

					if (power && (transform.position.y < maxpos)) {
							GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, speed);
					} else if (!power && transform.position.y > minpos) {
							GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -speed);
						} else {
							GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
						}
					} else {
					if (power && (transform.position.x < maxpos)) {
							GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
					} else if (!power && transform.position.x > minpos) {
							GetComponent<Rigidbody2D> ().velocity = new Vector2 (-speed, 0);
						} else {
							GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
						}

					}
				}
		}

	}