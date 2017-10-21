using UnityEngine;
using System.Math;

namespace SpaceFolder
{
	class SpaceFolderWarpChecks : MonoBehaviour
	{
		static bool goodToGo;
		static double vesselDiameter;
		static double maxWarpHoleSize;
		static double spaciofibrinNeeded;
		
		public static double SpaciofibrinWarpCalc(double[] engineSizes)
		{
			double returnAmount;
			foreach(double d in engineSizes)
			{
				returnAmount += Math.pow(Math.E, d/5);//TODO, fix values
			}
			return returnAmount;
		}
		private static double MaxWarpHoleSize(double[] engineSizes)
		{
			List<double> dividers = new List<double>;
			double divider = 0.1;
			dividers.Add(0.8); dividers.Add(0.6); dividers.Add(0,4); dividers.Add(0.2);
			int engineCount = 0;
			double totSize = 0;
			double realSize = 0;
			foreach(double d in engineSizes)
			{
				totSize += d;
			}
			engineCount = engineSizes.Count;
			engineSizes = SortedDoubleList(engineSizes);
			for(int i = 0; i <= engineCount; i++)
			{
				if (i == 0)
				{
					realSize += engineSizes[i];
				}
				else if(i > 0 && i < 5)
				{
					realSize += (engineSizes[i] * dividers[i+1]);
				}
				else
				{
					realSize += engineSizes[i] * divider;
					divider = divider/2;
				}
			}
		}
		public static double[] BigToSmallSortedDoubleList(double[] list)
		{
			List<double> sortedList = new List<double>();
			while(list.Count != 0)
			{
				sortedList.Add(list.Min());
				list.Remove(list.Min());
			}
			sortedList.Reverse();
			return sortedList;
		}
		public static double[] SmallToBigSortedDoubleList(double[] list)
		{
			List<double> sortedList = new List<double>();
			while(list.Count != 0)
			{
				sortedList.Add(list.Min());
				list.Remove(list.Min());
			}
			return sortedList;
		}
		public static bool GoodToGo()
		{
			return goodToGo;
		}
		public static void InitiateWarpCheck(double[] engineSizes, float vesDiameter) //called by GUI, sets bool goodToGo
		{
			FlightGlobals fgs = new FlightGlobals();
			vesselDiameter = vesDiameter;
			goodToGo = false;
			//constructs a new VesselData class
			VesselData vesData = new VesselData(fgs.activeVessel);
			int spaceFolderClassID = SpaceFolderEngine.ModuleClassID();
			//Checks if the current vessel has a SpaceFolderEngine
			if (!vesData.VesselContainsModule(fgs.activeVessel, spaceFolderClassID))
			{
				return;
			}
			//If vessel !have Spatiofibrin, return
			spaciofibrinNeeded = SpaciofibrinWarpCalc(engineSizes);
			if(vesData.ResourceAmountOnVessel("spatiofibrin", fgs.activeVessel) < spaciofibrinNeeded)
			{
				return;
			}
			// If warpDrive.diameter < vesselSize, return
			if(MaxWarpHoleSize(engineSizes) < vesselDiameter)
			{
				return;
			}
			goodToGo = true;
			WarpVessel(fgs.activeVessel);
		}
	}
}
