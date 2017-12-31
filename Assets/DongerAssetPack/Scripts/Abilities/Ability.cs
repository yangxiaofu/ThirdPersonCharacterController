﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.MovementEngine{
	public abstract class Ability : MonoBehaviour {
		///<summary>This will handle abilities that are attached to the gameobject</summary>
		public abstract AbilityArgs HandleAbility(AbilityArgs args);
	}

	public class AbilityArgs
	{
		public Vector3 Move = Vector3.zero;
		public bool Jump = false;
		public bool Crouch = false;

		public void Reset(){
			Move = Vector3.zero;
			Jump = false;
			Crouch = false;
		}

	}
}

