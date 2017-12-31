using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.MovementEngine{
	public abstract class Ability : MonoBehaviour {

		public abstract void HandleAbility();
	}

	public class AbilityArgs
	{
		public Vector3 Move;
		public bool Jump;
		public bool Crouch;

	}
}

