using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.Utility{

	///<summary>Takes the angle in degrees and scales it to 1. If it is a -90 degree angle, it should return -1.  If it's a 90 degree angle, then it should return 1.  If it's directly in front, it'll return 0.!-- </summary>
	public static class Tools{
		public static float ScaleAngleToOne(float angleInDegrees){
			//Need to recode thsi so that it's refactored d
			if (angleInDegrees > 90) return 1;
			if (angleInDegrees < -90) return -1;

			
			return angleInDegrees/90;
		}
	}

}