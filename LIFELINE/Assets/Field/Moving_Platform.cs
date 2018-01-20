	using UnityEngine;
	using System.Collections;

	/**
	 * Move_Types
	 * REPEAT: Keeps moving back and forth as long as power is on
	 * TARGET: Moves to target position when switched. Behaviour can be determined by button types.
	 */
	public enum Move_Types {REPEAT, TARGET};


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

	Field_Power pow;
	Rigidbody2D rb;

	/**
	 * Initialize power and target positions
	 */
	void Start () {
		pow = this.gameObject.GetComponent<Field_Power> ();
		rb = this.gameObject.GetComponent<Rigidbody2D> ();
		power = pow.defOn;
		defaultpower = power;
		if (vertical) {
				maxpos = transform.position.y + max;
				minpos = transform.position.y - min;
		} else {
				maxpos = transform.position.x + max;
				minpos = transform.position.x - min;
		}
	}

	/**
	 * Update the power and direction of the platform
	 */
	void Update () {

		//Update power
		if (alwaysOn) {
			power = true;
		} else if (flip) {
			power = !pow.isOn;
		} else {
			power = pow.isOn;
		}

		//When repeating, move only if power is on
		if (repeating) {
			if (power) {

				//Change direction based on the position
				if (vertical) {

					if (transform.position.y > maxpos) {
						dir = false;
					} else if (transform.position.y < minpos) {
						dir = true;
					}
					if (dir == true) {
						rb.velocity = new Vector2 (0, speed);
					} else {
						rb.velocity = new Vector2 (0, -speed);
					}
			
				} else {

					if (transform.position.x > maxpos) {
						dir = false;
					} else if (transform.position.x < minpos) {
						dir = true;
					}
					if (dir == true) {
						rb.velocity = new Vector2 (speed, 0);
					} else {
						rb.velocity = new Vector2 (-speed, 0);
					}

				}
			} else {
				rb.velocity = new Vector2 (0, 0);
			}

		}else  {
			//When set to target mode, move between two positions based on if
			//power is on or off

			if (vertical) {

				if (power && (transform.position.y < maxpos)) {
						rb.velocity = new Vector2 (0, speed);
				} else if (!power && transform.position.y > minpos) {
						rb.velocity = new Vector2 (0, -speed);
					} else {
						rb.velocity = new Vector2 (0, 0);
					}
				} else {
				if (power && (transform.position.x < maxpos)) {
						rb.velocity = new Vector2 (speed, 0);
				} else if (!power && transform.position.x > minpos) {
						rb.velocity = new Vector2 (-speed, 0);
					} else {
						rb.velocity = new Vector2 (0, 0);
					}

				}
			}
		}

	}