using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Hlpr;

namespace SpaceFolder
{
	class SpaceFolderWarpChecks : MonoBehaviour
	{
		public bool CheckWarp(Vessel v, bool warpVesselAfterCheck)//called by GUI, returns true or false based on if warpchecks got all the way through or not
		{
			ShipConstruct sc = new ShipConstruct(v.GetName(), EditorFacilities.VAB, v.Parts);//TODO get vessel launch building and use that instead of just EditorFacilities.VAB
			double vesselDiameter = sc.shipSize.x;//seen of forum post https://forum.kerbalspaceprogram.com/index.php?/topic/116071-getting-vessel-size/&do=findComment&comment=2067825 that the x value is the diameter of the ship. Thanks Thomas P!
			// constructs a new VesselData class
			VesselData vesData = new VesselData(v);
			//Get the engine info
			List<Part> engineList = new List<Part>();
			engineList = EngineList(v);
			// If vessel !have Spatiofibrin, return
			double spatiofibrinNeeded = SpatiofibrinWarpCalc(engineList);
			if (vesData.ResourceAmountOnVessel("Spatiofibrin", v) < spatiofibrinNeeded)
			{
				return false;
			}
			List<Tuple<Part, double>> warpTupleList = new List<Tuple<Part, double>>();
			double electricityNeeded = ElectricityWarpCalc(engineList, out warpTupleList);
			if (vesData.ResourceAmountOnVessel("ElectricCharge", v) 
				< electricityNeeded)
			{
				return false;
			}
			// If warpDrive.diameter < vesselSize, return
			if (MaxWarpHoleSize(engineList) < vesselDiameter)
			{
				return false;
			}
			if(warpVesselAfterCheck)
			{
				bool vmexists = false;
				VesselModule vm = GetVMInstance(v, ref vmexists);
				if(vmexists && (vm != null))//double check just to make sure it works
				{
					vm.WarpVessel(warpTupleList, electricityNeeded);//switchable so that it can be called to check if the warp is valid without actually warping
					return true;
				}
				else
					return false;
			}
		}
		VesselModule GetVMInstance(Vessel v, ref bool exists)
		{
			List<VesselModule> vms = new List<VesselModule>();
			vms = v.VesselModules;
			//Create a temp instance of FlightDrive for checking
			FlightDrive TEMPfd = new FlightDrive();
			for(int i = 0; i < vms.Count; i++)
			{
				if(Object.ReferenceEquals(vms[i].GetType(), TEMPfd.GetType()))
					return vms[i];
			}
			return null;//returns null if an instance doesn't exist
		}
		List<Part> EngineList(Vessel v)//use list[i].Modules["SpaceFolderEngine"].field; to get specific values
		{
			List<Part> list = new List<Part>();
			List<Part> returnList = new List<Part>();
			list = v.Parts;
			for(int i = 0; i < list.Count; i++)
			{
				if(list[i].Modules.Contains("SpaceFolderEngine"))
					returnList.Add(list[i]);
			}
			return returnList;
		}
		double[] GetEngineValues(Part p)//array is in order of {engineSize, modifier}
		{
			List<double[]> returnList = new List<double[]>();
			for(int i = 0; i < list.Count; i++)
			{
				if(list[i].Modules.Contains("SpaceFolderEngine"))
				{
					returnList.Add(list[i].Modules["SpaceFolderEngine"].SFDEngineValues());
				}
			}
			return returnList;
		}
		double SpatiofibrinWarpCalc(List<Part> engines)
		{
			double returnAmount = 0;
			List<double[]> engineSizes = new List<double[]>();
			for(int i = 0; i < engines.Count; i++)
				engineSizes.Add(GetEngineValues(engines[i]));
			for (int i = 0; i < engineSizes.GetLength(0); i++)
			{
				double modifiedVal = engineSizes[i][0] * engineSizes[i][1];
				returnAmount += Math.Pow(Math.E, modifiedVal/5);
			}
			return returnAmount;
		}
		double ElectricityWarpCalc(List<Part> engines, out List<Tuple<Part, double>> partECPercent)
		{
			double returnAmount = 0;
			List<Tuple<Part, double>> partECUsage = new List<Tuple<Part, double>>();
			List<double[]> engineInfo = new List<double[]>();
			for(int i = 0; i < engines.Count; i++)
				engineInfo.Add(GetEngineValues(engines[i]));
			for(int i = 0; i < engineInfo.GetLength(0); i++)
			{
				double modifiedVal = engineInfo[i][0] * engineInfo[i][1];
				Tuple<Part, double> tempTup = new Tuple<Part, double>(engines[i], Math.Pow(Math.E, modifiedVal/5)*300);
				partECUsage.Add(tempTup);
				returnAmount += Math.Pow(Math.E, modifiedVal/5)*300;//TODO: fix values
			}
			List<Tuple<Part, double>> partECPercentReturn = new List<Tuple<Part, double>>();
			for(int i = 0; i < partECUsage.Count; i++)
			{
				Tuple<Part, double> temptup = new Tuple<Part, double>(partECUsage[i].item1, partECUsage[i].item2/returnAmount);
				partECPercentReturn.Add(temptup);
			}
			partECPercent = partECPercentReturn;
			return returnAmount;
		}
		double MaxWarpHoleSize(List<Part> engines)
		{
			double[] dividers = new double[4] { 0.8, 0.6, 0.4, 0.2 };
			double divider = 0.1;
			List<double> unmodEngineSize = new List<double>();
			List<double[]> engineSizes = new List<double[]>();
			for(int i = 0; i < engines.Count; i++)
				engineSizes.Add(GetEngineValues(engines[i]));
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
			unmodEngineSize.Sort();
			unmodEngineSize.Reverse();
			return realSize;
		}
	}
}
