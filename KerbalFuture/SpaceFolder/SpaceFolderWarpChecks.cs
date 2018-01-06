using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Hlpr

namespace SpaceFolder
{
	class SpaceFolderWarpChecks : MonoBehaviour
	{
		public bool CheckWarp(Vessel v)
		// called by GUI, sets bool goodToGo
		{
			FlightGlobals fgs = new FlightGlobals();
			ShipConstruct sc = new ShipConstruct(v.GetName(), EditorFacilities.VAB, v.Parts);//TODO get vessel launch building and use that instead of just EditorFacilities.VAB
			double vesselDiameter = sc.shipSize.x;//seen of forum post https://forum.kerbalspaceprogram.com/index.php?/topic/116071-getting-vessel-size/&do=findComment&comment=2067825 that the x value is the diameter of the ship. Thanks Thomas P.
			// constructs a new VesselData class
			VesselData vesData = new VesselData(v);
			//Get the engine info
			List<double[]> engineInfo = new List<double[]>();
			engineInfo = GetEngineValues(v);
			// If vessel !have Spatiofibrin, return
			double spatiofibrinNeeded = SpatiofibrinWarpCalc(engineInfo);
			if (vesData.ResourceAmountOnVessel("spatiofibrin", v) < spatiofibrinNeeded)
			{
				return;
			}
			double electricityNeeded = ElectricityWarpCalc(engineInfo);
			if (vesData.ResourceAmountOnVessel("Electricity", v) 
				< electricityNeeded) // TODO, name may not be right
			{
				return;
			}
			// If warpDrive.diameter < vesselSize, return
			if (MaxWarpHoleSize(engineInfo) < vesselDiameter)
			{
				return;
			}
			FlightDrive.WarpVessel(v);
		}
		List<double[]> GetEngineValues(Vessel v)
		{
			List<Part> list = new List<Part>();
			List<double[]> returnList = new List<double[]>();
			list = v.Parts;
			for(int i = 0; i < list.Count; i++)
			{
				if(list[i].Modules.Contains("SpaceFolderEngine"))
				{
					returnList.Add(list[i].Modules["SpaceFolderEngine"].SFDEngineValues());
				}
			}
			return returnList;
		}
		double SpatiofibrinWarpCalc(List<double[]> engineSizes)
		{
			double returnAmount = 0;
			for (int i = 0; i < engineSizes.GetLength(0); i++)
			{
				double modifiedVal = engineSizes[i][0] * engineSizes[i][1];
				returnAmount += Math.Pow(Math.E, modifiedVal/5);// TODO, fix values
			}
			return returnAmount;
		}
		double ElectricityWarpCalc(List<double[]> engineInfo)
		{
			double returnAmount = 0;
			for(int i = 0; i < engineInfo.GetLength(0); i++)
			{
				double modifiedVal = engineInfo[i][0] * engineInfo[i][1];
				returnAmount += Math.Pow(Math.E, modifiedVal/5);//TODO: fix values
			}
			returnAmount *= 300;
			return returnAmount;
		}
		double MaxWarpHoleSize(List<double[]> engineSizes)
		{
			double[] dividers = new double[4] { 0.8, 0.6, 0.4, 0.2 };
			double divider = 0.1;
			List<double> unmodEngineSize = new List<double>();
			unmodEngineSize.Sort();
			unmodEngineSize.Reverse();
			for (int i = 0; i < engineSizes.GetLength(0); i++)
			{
				unmodEngineSize.Add(engineSizes[i][0]);
			}
			double realSize = 0;
			for (int i = 0; i <= unmodEngineSize.GetLength(0); i++)
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
	}
}
