using UnityEngine;
using KerbalFuture;

namespace SpaceFolder
{
	class SpaceFolderWarpChecks : MonoBehaviour
	{
		static bool goodToGo;
		static float vesselDiameter;
		static var vesData;
		static float[] warpEngineSizes;
		static float maxWarpHoleSize;
		static double spaciofibrinNeeded;
		
		[KSPField(isPersistant = true)]
		static float quadCoefficent;

		[KSPField(isPersistant = true)]
		static float quadH;

		[KSPField(isPersistant = true)]
		static float quadK;

		public static double SpaciofibrinWarpCalc(float[] engineSizes)
		{
			double returnAmount;
			foreach(float f in engineSizes)
			{
				returnAmount += QuadCalc(f);
			}
			return returnAmount;
		}
		private static double QuadCalc(float engineSize)
		{
			engineSize = (double)engineSize;
			quadH = (double)quadH;
			quadK = (double)quadK;
			quadCoefficent = (double)quadCoefficent;
			return (quadCoefficent * Math.Pow((engineSize - quadH), 2) + quadK);
		}
		public static void InitiateWarpCheck(float[] engineSizes, float vesDiameter) //called by GUI, sets bool goodToGo
		{
			warpEngineSizes.Clear();
			vesselDiameter = vesDiameter;
			goodToGo = false;
			foreach(float f in engineSizes)
			{
				warpEngineSizes.Add(f);
			}
			//constructs a new VesselData class
			vesData = new VesselData(FlightGlobals.activeVessel);
			int spaceFolderClassID = SpaceFolderEngine.ModuleClassID();
			//Checks if the current vessel has a SpaceFolderEngine
			if (!vesData.VesselContainsModule(FlightGlobals.activeVessel, spaceFolderClassID))
			{
				return;
			}
			//Checks the size of the SpaceFolderEngine module
			
			//If vessel !have Spatiofibrin, return
			spaciofibrinNeeded = SpaciofibrinWarpCalc(engineSizes);
			if(ResourceAmountOnVessel("Spaciofibrin", FlightGlobals.activeVessel) < spaciofibrinNeeded)
			{
				return;
			}
			// ResourceAmountNeeded())
			//	return;
			// If warpDrive.diameter < vesselSize, return
			//
			// WarpVessel();
			goodToGo = true;
		}
	}
}
