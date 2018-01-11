using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.IceEngine{
	public abstract class Ability : MonoBehaviour {
		///<summary>This will handle abilities that are attached to the gameobject</summary>
		public abstract AbilityArgs HandleAbility(AbilityArgs args);
	}

	public class AbilityArgs
	{
		public Vector3 Move = Vector3.zero;
		public bool Jump = false;
		public float JumpPower = 0f;
		public bool Crouch = false;
		public bool Shooting = false;
	}
}

