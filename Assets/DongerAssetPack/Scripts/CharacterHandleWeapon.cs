using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.MovementEngine{

	//This will be added if the character has a weapon
	[RequireComponent(typeof(Animator))]
	public class CharacterHandleWeapon : MonoBehaviour {
		Animator _anim;
		private const int WeaponUpLayerIndex = 1;
		void Start()
		{
			_anim = GetComponent<Animator>();
            _anim.SetLayerWeight(1, 1);
		}
	}
}

