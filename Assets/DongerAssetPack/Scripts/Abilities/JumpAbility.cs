using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.MovementEngine{
	public class JumpAbility : Ability {

		bool _jump;

		public override void HandleAbility()
		{
			if (!_jump)
            {
                _jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
		}
	}
}

