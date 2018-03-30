using UnityEngine;
using System.Collections;

public class Field_Sprite : MonoBehaviour {
	public Sprite spr_water;
	public Sprite spr_metal;
	public Sprite spr_ice;
	public Sprite spr_sand;
	public Sprite spr_metwater;

	GameObject obj;
	SpriteRenderer spr;
	Field_Insulator insul;
	Field_Door door;
	Field_Hazard haz;
	Moving_Platform move;

	/**
	 * Initialize components
	 */
	void Start () {
		obj = this.gameObject;
		spr = this.gameObject.GetComponent<SpriteRenderer>();
		insul = this.gameObject.GetComponent<Field_Insulator> ();
		door = this.gameObject.GetComponent<Field_Door> ();
		haz = this.gameObject.GetComponent<Field_Hazard> ();
		move = this.gameObject.GetComponent<Moving_Platform> ();
	}

	
	/**
	 * Update sprite and colour based on variables of different components
	 */
	void Update () {

		//Choose sprite based on variables
		if (haz != null) {
			if (haz.isHazard) {
				if (obj.layer == 8 || (insul != null && insul.insulate)) {
					spr.sprite = spr_metwater;
					
				} else {
					spr.sprite = spr_water;
				}
			} else {
				spr.sprite = spr_ice;
			}
		}	else if (obj.layer == 8|| (insul != null && insul.insulate)) {
			spr.sprite = spr_metal;
		}  else {
			spr.sprite = spr_sand;
		}

		//Choose colour based on variables
		if (move != null) {

			if (door != null) {

				if (door.open) {

					spr.color = new Color (1.0f, 1.0f, 0.5f, 0.25f);
				} else {
					spr.color = new Color (0.5f, 1.0f, 0.5f, 0.9f);
				}

			} else {
				spr.color = new Color (0.5f, 1.0f, 0.5f, 1.0f);
			}
		} else if (door != null) {

			if (door.open) {
				spr.color = new Color (1.0f, 0.6f, 0.7f, 0.25f);
			} else {
				spr.color = new Color (1.0f, 0.6f, 0.7f, 0.9f);
			}
		} else {
			spr.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		}

	
	}
}
