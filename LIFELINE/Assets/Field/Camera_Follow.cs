using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
	public class Camera_Follow : MonoBehaviour
	{
		public Transform target;
		public float damping = 1;
		public float lookAheadFactor = 3;
		public float lookAheadReturnSpeed = 0.5f;
		public float lookAheadMoveThreshold = 0.1f;

		private float m_OffsetZ;
		private Vector3 m_LastTargetPosition;
		private Vector3 m_CurrentVelocity;
		private Vector3 m_LookAheadPos;
		public Data_Field field;

		// Use this for initialization
		private void Start()
		{
		}


		/**
		 * Move camera to follow the leader.
		 * Used only in HUB
		 */
		private void Update()
		{
			target = field.players [field.getLeader ()].transform;


			Vector3 newPos = Vector3.SmoothDamp(transform.position, new Vector2(target.position.x, transform.position.y), ref m_CurrentVelocity, damping);

			transform.position = newPos;

			m_LastTargetPosition = target.position;

			Vector3 pos = transform.position;
			pos.z = -10;
			transform.position = pos;
		}

	}
}
