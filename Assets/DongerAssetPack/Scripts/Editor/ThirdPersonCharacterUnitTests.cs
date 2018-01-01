using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

namespace DongerAssetPack.IceEngine.UnitTests{
	[TestFixture]
	public class ThirdPersonCharacterUnitTests {

		[Test]
		[TestCase(false, false, false, false)]
		[TestCase(false, false, true, false)]
		[TestCase(false, true, false, false)]
		[TestCase(false, true, true, false)]
		[TestCase(true, false, false, false)]
		[TestCase(true, false, true, true)]
		[TestCase(true, true, false, false)]
		[TestCase(true, true, true, false)]
		public void ThirdPersonCharacterLogic_CanJump_RetunsIfCanJump(bool jump, bool isCrouching, bool isGrounded, bool result)
		{
			var logic = new ThirdPersonCharacterLogic();	
			Assert.AreEqual(result, logic.CanJump(jump, isCrouching, isGrounded));
		}
	}

}

