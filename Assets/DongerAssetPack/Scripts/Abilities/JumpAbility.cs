using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.MovementEngine{
	public class JumpAbility : Ability {

		public override AbilityArgs HandleAbility(AbilityArgs args)
		{	
            args.Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			return args;
		}
	}
}

