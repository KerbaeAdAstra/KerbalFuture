using UnityEngine;

namespace KerbalFuture
{
	class SpaceFolderWarpChecks : MonoBehaviour
	{
		static bool goodToGo;
		static double vesselDiameter;
		public static void InitiateWarpCheck() //called by GUI, sets bool goodToGo
		{
			//If vessel !have Spatiofibrin, return
			//if (ResourceAmountOnVessel("Spatiofibrin", CurrentVessel()) < 
			// ResourceAmountNeeded())
			//	return;
			// If warpDrive.diameter < vesselSize, return
			//
			// WarpVessel();
		}
		public static bool WarpIsGo()
		{
			return goodToGo;
		}
	}
}