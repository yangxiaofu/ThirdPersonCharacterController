using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DongerAssetPack.Utility;
using NUnit.Framework;

namespace DongerAssetPack.Utility.UnitTests{
	[TestFixture]
	public class DongerUtilityTests  {

		[Test]
		[TestCase(90, 1)]
		[TestCase(-90, -1)]
		[TestCase(0, 0)]
		[TestCase(-180, 0)]
		public void SignedAngleTests_ReturnsScaledValue(float angle, float result)
		{
			Assert.AreEqual(result, Tools.ScaleAngleToOne(angle));
		}

	
	}
}



