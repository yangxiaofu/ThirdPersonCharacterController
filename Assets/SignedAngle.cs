using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DongerAssetPack.Utility;

namespace Game.Core{
	public class SignedAngle : MonoBehaviour {

		float angle = 0;
		[SerializeField] GameObject _target;
		// Use this for initialization=
		
		// Update is called once per frame
		void Update () 
		{
			var directionToTarget = (_target.transform.position - this.transform.position).normalized;
			angle = Vector3.SignedAngle(this.transform.forward, directionToTarget, Vector3.up);
		}

		void OnGUI()
		{
			GUILayout.Label("Angle is " + angle);
			GUILayout.Space(100);
			GUILayout.Label("Fixed Rotation " + Tools.ScaleAngleToOne(angle));
		}

		/// <summary>
		/// Callback to draw gizmos that are pickable and always drawn.
		/// </summary>
		void OnDrawGizmos()
		{
			Gizmos.DrawRay(new Ray(this.transform.position, this.transform.forward));
		}
		
	}

}
