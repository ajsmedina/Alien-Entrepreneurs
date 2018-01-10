using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class laserScript : MonoBehaviour {
	public LineRenderer laserLineRenderer;
	public GameObject partner;
		public float laserWidth = 0.1f;
		public float laserMaxLength = 5f;

		void Start() {
			Vector3[] initLaserPositions = new Vector3[ 2 ] { Vector3.zero, Vector3.zero };
			laserLineRenderer.SetPositions( initLaserPositions );
			laserLineRenderer.SetWidth( laserWidth, laserWidth );
		}

		void Update() 
		{
		ShootLaserFromTargetPosition (transform.position);
			
		}

		void ShootLaserFromTargetPosition( Vector3 targetPosition )
	{
			Vector3 endPosition = partner.transform.position;

		RaycastHit2D hit =  Physics2D.Raycast(transform.position, (endPosition - targetPosition).normalized, (endPosition - targetPosition).magnitude, LayerMask.GetMask("Insulator", "Player"));

		if (hit.collider != null) {

			if (hit.collider.gameObject.tag == "Player") {
				if(hit.collider.gameObject.Equals(partner) == false)
					SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}else {
				endPosition = hit.point;
			}
		}
		

			laserLineRenderer.SetPosition( 0, targetPosition );
			laserLineRenderer.SetPosition( 1, endPosition );
		}

	/**public Transform startPoint;
	public Transform endPoint;
	LineRenderer laserLine;
	// Use this for initialization
	void Start () {
		laserLine = GetComponentInChildren<LineRenderer> ();
		laserLine.SetWidth (.2f, .2f);
	}
	
	// Update is called once per frame
	void Update () {
		laserLine.SetPosition (0, startPoint.position);
		laserLine.SetPosition (1, endPoint.position);

	}*/
}
