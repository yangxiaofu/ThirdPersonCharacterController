using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.MovementEngine{
	public class ThirdPersonCharacterLogic {

		public bool CanJump(bool jump, bool isCrouching, bool isGrounded) 
		{
			if (!jump) return false;

			if (!isGrounded) return false;

			if (isCrouching) return false;

			return true;
		}
	}
}

