using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.IceEngine{
	[RequireComponent(typeof(DongerController))]
	public class JumpAbility : Ability {

		[SerializeField] private float _jumpPower = 6f;
		public float JumpPower{get{return _jumpPower;}}
		public override AbilityArgs HandleAbility(AbilityArgs args)
		{	
            args.Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			args.JumpPower = _jumpPower;
			return args;
		}
    }
}

