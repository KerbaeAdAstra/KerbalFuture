using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;
using KerbalFuture;

namespace KerbalFuture
{
	class SpaceFolderVslChecks : MonoBehaviour
	{
		private bool gudToGo;
		private double vesselDiameter;
		public static void InitiateWarpCheck()//called by GUI, sets bool gudToGo
		{
			//If vessel !have Spatiofibrin, return
			//if (ResourceAmountOnVessel("Spatiofibrin", CurrentVessel()) < ResourceAmountNeeded())
			//	return;
			//If warpDrive.diameter < vesselSize, return
			//
			WarpVessel();
		}
		public static bool WarpIsGo()//used by FlightDrive to allow warp
		{
			if(gudToGo)
				return true;
			else
				return false;
		}
	}
}