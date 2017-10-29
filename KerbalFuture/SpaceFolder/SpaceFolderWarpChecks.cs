using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceFolder
{
	class SpaceFolderWarpChecks : MonoBehaviour
	{
		static bool goodToGo;
		static double vesselDiameter;
		static double spatiofibrinNeeded;
		static double electricityNeeded;
		
		public static double SpatiofibrinWarpCalc(List<double>[] engineSizes)
		{
			double returnAmount = 0;
			for (int i = 0; i < engineSizes.Count(); i++)
			{
				double modifiedVal = engineSizes[i][0] * engineSizes[i][1];
				returnAmount += Math.Pow(Math.E, modifiedVal/5);
				// TODO, fix values
			}
			return returnAmount;
		}
		static double MaxWarpHoleSize(List<double>[] engineSizes)
		{
			List<double> dividers = new List<double> { 0.8, 0.6, 0.4, 0.2 };
			double divider = 0.1;
			List<double> unmodEngineSize = new List<double>();
			unmodEngineSize.Sort();
			unmodEngineSize.Reverse();
			for (int i = 0; i < engineSizes.Count(); i++)
			{
				unmodEngineSize.Add(engineSizes[i][0]);
			}
			double totSize = 0;
			double realSize = 0;
			for (int i = 0; i < unmodEngineSize.Count(); i++)
			{
				totSize += unmodEngineSize[i];
			}
			for (int i = 0; i <= unmodEngineSize.Count(); i++)
			{
				if (i == 0)
				{
					realSize += unmodEngineSize[i];
				}
				else if (i > 0 && i < 5)
				{
					realSize += (unmodEngineSize[i] * dividers[i - 1]);
				}
				else
				{
					realSize += unmodEngineSize[i] * divider;
					divider = divider / 2;
				}
			}
			return realSize;
		}
		public static double ElectricityWarpCalc(List<double>[] engineInfo)
		{
			double returnAmount = 0;
			for(int i = 0; i < engineInfo.Count(); i++)
			{
				double modifiedVal = engineInfo[i][0] * engineInfo[i][1];
				returnAmount += Math.Pow(Math.E, modifiedVal/5);
			}
			returnAmount *= 300;
			return returnAmount;
		}
		public static bool GoodToGo() => goodToGo;
		public static void InitiateWarpCheck(List<double>[] engineSizes,
											 float vesDiameter)
		// called by GUI, sets bool goodToGo
		{
			FlightGlobals fgs = new FlightGlobals();
			vesselDiameter = vesDiameter;
			goodToGo = false;
			// constructs a new VesselData class
			VesselData vesData = new VesselData(fgs.activeVessel);
			// If vessel !have Spatiofibrin, return
			spatiofibrinNeeded = SpatiofibrinWarpCalc(engineSizes);
			if (vesData.ResourceAmountOnVessel("spatiofibrin", fgs.activeVessel)
				< spatiofibrinNeeded)
			{
				return;
			}
			electricityNeeded = ElectricityWarpCalc(engineSizes);
			if (vesData.ResourceAmountOnVessel("Electricity", fgs.activeVessel) 
				< electricityNeeded) // TODO, name may not be right
			{
				return;
			}
			// If warpDrive.diameter < vesselSize, return
			if (MaxWarpHoleSize(engineSizes) < vesselDiameter)
			{
				return;
			}
			goodToGo = true;
			FlightDrive.WarpVessel(fgs.activeVessel);
		}
	}
}
