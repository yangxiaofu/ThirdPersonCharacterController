using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.IceEngine{ 

	//This will be added if the character has a weapon
	[RequireComponent(typeof(Animator))]
	public class CharacterHandleWeapon : MonoBehaviour {

		[Tooltip("This is the transfrom that the character will rotate around when aiming their weapon.")]
		protected Animator _anim;
		protected CameraRaycaster _raycaster;
		private const int WEAPONUP_LAYER_INDEX = 1;

		
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
			_anim = GetComponent<Animator>();
            _anim.SetLayerWeight(WEAPONUP_LAYER_INDEX, 1);

			_raycaster = FindObjectOfType<CameraRaycaster>();
			_raycaster.OnLayerHit += OnLayerHit;
		}

        private void OnLayerHit(RaycastHit hit)
        {
            _lookAtDirection = hit.point;
        }

	}
}

