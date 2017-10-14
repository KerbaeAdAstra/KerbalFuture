using UnityEngine;
using KerbalFuture;

namespace SpaceFolder
{
	class SpaceFolderWarpChecks : MonoBehaviour
	{
		static bool goodToGo;
		static double vesselDiameter;
		static var vesData;
		
		public static double VesselDiameter(Vessel vessel)
		{
			//TODO
		}
		public static void InitiateWarpCheck() //called by GUI, sets bool goodToGo
		{
			//constructs a new VesselData class
			vesData = new VesselData(FlightGlobals.activeVessel);
			int spaceFolderClassID = SpaceFolderEngine.ModuleClassID();
			//Checks if the current vessel has a SpaceFolderEngine
			if (!vesData.VesselContainsModule(FlightGlobals.activeVessel, SpaceFolderEngine.spaceFolderClassID))
			{
				return;
			}
			//Checks the size of the SpaceFolderEngine module
			
			//If vessel !have Spatiofibrin, return
			//if (ResourceAmountOnVessel("Spatiofibrin", CurrentVessel()) <= 
			//if (ResourceAmountOnVessel("Spatiofibrin", FlightGlobals.activeVessel) < 
			// ResourceAmountNeeded())
			//	return;
			// If warpDrive.diameter < vesselSize, return
			//
			// WarpVessel();
		}
		public static bool WarpIsGo()
		{
			InitiateWarpCheck();
			return goodToGo;
		}
	}
}
