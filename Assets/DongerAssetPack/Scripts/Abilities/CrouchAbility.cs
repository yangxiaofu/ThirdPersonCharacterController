using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.MovementEngine{
    public class CrouchAbility : Ability
    {
        public override AbilityArgs HandleAbility(AbilityArgs args)
        {
            args.Crouch = CrossPlatformInputManager.GetButton("Crouch");
			return args;
        }
    }
}

