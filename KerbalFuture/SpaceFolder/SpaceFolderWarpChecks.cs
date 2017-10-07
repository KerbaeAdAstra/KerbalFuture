using UnityEngine;
using KerbalFuture;

namespace SpaceFolder
{
	class SpaceFolderWarpChecks : MonoBehaviour
	{
		static bool goodToGo;
		static double vesselDiameter;
		static var vesData;
		
		public static void InitiateWarpCheck() //called by GUI, sets bool goodToGo
		{
			//constructs a new VesselData class
			vesData = new VesselData(CurrentVessel());
			//Checks if the current vessel has a SpaceFolderEngine
			if (!vesData.VesselContainsModule(CurrentVessel(), SpaceFolderEngine))
			{
				return;
			}
			//Checks the size of the SpaceFolderEngine
			
			//If vessel !have Spatiofibrin, return
			//if (ResourceAmountOnVessel("Spatiofibrin", CurrentVessel()) <= 
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