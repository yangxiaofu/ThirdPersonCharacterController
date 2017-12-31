using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.MovementEngine{
    [RequireComponent(typeof(DongerController))]
    public class CrouchAbility : Ability
    {
        [HideInInspector] public bool Crouch = false;
        public override AbilityArgs HandleAbility(AbilityArgs args)
        {
            args.Crouch = Crouch = CrossPlatformInputManager.GetButton("Crouch");
            return args;
        }
    }
}

