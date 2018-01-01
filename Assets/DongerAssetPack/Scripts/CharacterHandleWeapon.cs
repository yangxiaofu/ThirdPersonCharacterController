using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.IceEngine{ 

	//This will be added if the character has a weapon
	[RequireComponent(typeof(Animator))]
	public class CharacterHandleWeapon : MonoBehaviour {

		[Tooltip("This is the transfrom that the character will rotate around when aiming their weapon.")]
		[SerializeField] bool _rotateArmsToTarget = false;
		[SerializeField] Transform _spine;
		protected Animator _anim;
		protected CameraRaycaster _raycaster;
		private const int WEAPONUP_LAYER_INDEX = 1;

		LeftHandGrip _leftGrip;
		RightHandGrip _rightGrip;
		
		[Tooltip("Used to determine if the left hand will grab the weapon.")]
		[SerializeField] bool _useLeftHandGrip = false;
		[Range(0, 1)]
		[SerializeField] float _leftHandWeight = 1f;
		[Tooltip("Used to determine if the right hand will grab the weapon.")]
		[SerializeField] bool _useRightHandGrip = false;
		[Range(0, 1)]
		[SerializeField] float _rightHandWeight = 1f;
		Vector3 _lookAtDirection;


		[Header("Debugging Purposes")]
		[SerializeField] bool _targetIsGameObject = true;

		[Tooltip("Used for debugging purposes for now.  ")]
		[SerializeField] GameObject _target;
		[SerializeField] Transform _weaponSocket;

		protected virtual void Start()
		{
			//Find the hand grips on the weapon.
			if (_useLeftHandGrip) _leftGrip = GetComponentInChildren<LeftHandGrip>();
			if (_useRightHandGrip) _rightGrip = GetComponentInChildren<RightHandGrip>();

			_anim = GetComponent<Animator>();
            _anim.SetLayerWeight(WEAPONUP_LAYER_INDEX, 1);

			//Find the camera raycaster if the rotation is true.
			if (_rotateArmsToTarget)
			{
				_raycaster = FindObjectOfType<CameraRaycaster>();
				_raycaster.OnLayerHit += OnLayerHit;
			}
		}

        private void OnLayerHit(RaycastHit hit)
        {
            _lookAtDirection = hit.point;
        }

		void LateUpdate()
        {
            RotateSpine();
        }

		//TODO: remove later for debugging.
        private void RotateSpine()
        {
            if (_targetIsGameObject)
            {
                var direction = (_target.transform.position - this.gameObject.transform.position).normalized;
                //Move the spine instead.

                _spine.up = -direction;

                print("Forward Pos " + _spine.forward);
            }
        }

        void OnAnimatorIK(int layerIndex)
		{
			if (_rotateArmsToTarget)
			{
				_anim.SetIKPosition(AvatarIKGoal.LeftHand, _leftGrip.transform.position);
				_anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, _leftHandWeight);

				_anim.SetIKPosition(AvatarIKGoal.RightHand, _rightGrip.transform.position);
				_anim.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightHandWeight);
			}
		}

		void OnDrawGizmos()
		{
			// Gizmos.color = Color.green;
			// Gizmos.DrawRay(_spine.transform.position, -_spine.up);

			// Gizmos.color = Color.red;
			// Gizmos.DrawRay(_spine.transform.position, -_spine.forward);

			Gizmos.color = Color.red;
			var testDirection = new Vector3(0, 0.5f, 0.5f);
			Gizmos.DrawRay(this.transform.position, testDirection);

			Gizmos.color = Color.red;
			Gizmos.DrawRay(this.transform.position, Vector3.up);

			Gizmos.color = Color.green;
			var projection = Vector3.ProjectOnPlane(this.transform.position + testDirection, Vector3.up);
		}
	}
}

