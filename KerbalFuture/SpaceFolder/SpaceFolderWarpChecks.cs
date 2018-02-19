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
			VesselData vesData = new VesselData(v);//creates a custom vesseldata instance
			//Get the engine info
			List<Part> engineList = new List<Part>();
			engineList = EngineList(v);//sets the list of parts to the vessel part list
			//Tuples from custom class Tuple.cs, as .NET 3.5 doesn't have tuples yet
			List<Tuple<Part, double, string, string>> warpTupleList = new List<Tuple<Part, double, string, string>>();//used later, filled from engineTupleList below
			List<Tuple<Part, double>> engineTupleList = new List<Tuple<Part, double>>();//That's a part and the corrosponding amount of electricity needed for it to run
			// If vessel !have Spatiofibrin, return
			/*
			double spatiofibrinNeeded = SpatiofibrinWarpCalc(engineList);
			if (vesData.ResourceAmountOnVessel("Spatiofibrin", v) < spatiofibrinNeeded)
			{
				return false;
			}
			//Taken care of with CheckResources()
			*/ 
			double electricityNeeded = ElectricityWarpCalc(engineList, false, out engineTupleList);//sets engineTupleList with a list of Part, amount
			warpTupleList = PopulateWarpTupleList(engineTupleList);//creates the quaduple with <Part, amount, mainResource, catalyst>
			if(!CheckResources(vesData, warpTupleList))
				return false;
			/*
			if (vesData.ResourceAmountOnVessel("ElectricCharge", v) 
				< electricityNeeded)
			{
				return false;
			}
			*/
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
		bool CheckResources(VesselData vesData, List<Tuple<Part, double, string, string>> warpTupleList)
		{
			HashSet<string> resourcesUsed = new HashSet<string>();//HashSets only have one of each element, repeats are destroyed
			HashSet<string> catalystsUsed = new HashSet<string>();
			for(int i = 0; i < warpTupleList.Count; i++)//populates hashsets
			{
				resourcesUsed.Add(warpTupleList[i].item3);
				catalystsUsed.Add(warpTupleList[i].item4);
			}
			List<Dictionary<string, double>> vesselResourceAmounts = new List<Dictionary<string, double>>();//New List<Dictionary<>> for the resource and the amount
			for(int i = 0; i < resourcesUsed.Count; i++)
			{
				Dictionary<string, double> tempDict = new Dictionary<string, double>(resourcesUsed[i], vesData.ResourceAmountOnVessel(resourcesUsed[i]));
				vesselResourceAmounts.Add(tempDict);
			}
			//Runs simulation of resources being used
			for(int i = 0; i < resourcesUsed.Count; i++)//We're counting on the resources being in the right order, since they were created from each other
			{
				for(int j = 0; j < warpTupleList.Count; j++)
				{
					if(vesselResourceAmounts[i][0] == warpTupleList[j].item3)
					{
						vesselResourceAmounts[i][1] -= warpTupleList[j].item2;//'removes' resources from the 'vessel'
					}
				}
			}
			//Checks to make sure none of the resources are in the red
			for(int i = 0; i < vesselResourceAmounts.Count; i++)
			{
				if(vesselResourceAmounts[i][1] < 0)
					return false;
			}
			return true;//return true if everything went well
		}
		List<Tuple<Part, double, string, string>> PopulateWarpTupleList(List<Tuple<Part, double>> engineListWithECUsage)
		{
			List<Tuple<Part, double, string, string>> returnList = new List<Tuple<Part, double, string, string>>();
			for(int i = 0; i < engineListWithECUsage.Count; i++)
			{
				Tuple<Part, double, string, string> tempTup = new Tuple<Part, double, string, string>(engineListWithECUsage[i].item1, engineListWithECUsage[i].item2, engineListWithECUsage[i].item1.Modules["SpaceFolderEngine"].mainResource, engineListWithECUsage[i].item1.Modules["SpaceFolderEngine"].catalyst);
				returnList.Add(tempTup);
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
				if(System.Object.ReferenceEquals(vms[i].GetType(), TEMPfd.GetType()))
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
			double[] returnList = new double[2];
			if(p.Modules.Contains("SpaceFolderEngine"))
			{
				returnList[0] = p.Modules["SpaceFolderEngine"].warpDriveDiameter;
				returnList[1] = p.Modules["SpaceFolderEngine"].engineMultiplier;
				return returnList;
			}
			else
			{
				returnList[0] = returnList[1] = null;
				return returnList;
			}
		}
		double SpatiofibrinWarpCalc(Part engine)
		{
			double[] engineSize = new double[2];
			engineSize[0] = GetEngineValues(engine)[0];
			engineSize[1] = GetEngineValues(engine)[1];
			return Math.Pow(Math.E, (engineSize[0]*engineSize[1])/5)
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
		double ElectricityWarpCalc(Part engine)
		{
			double returnAmount = 0;
			double[] engineInfo = new double[2];
			engineInfo[0] = GetEngineValues(engine)[0];
			engineInfo[1] = GetEngineValues(engine)[1];
			double modifiedVal = engineInfo[0]*engineInfo[1];
			returnAmount = Math.Pow(Math.E, modifiedVal/5)*300;
			return returnAmount;
		}
		double ElectricityWarpCalc(List<Part> engines, bool outAsPercent, out List<Tuple<Part, double>> partECPercent)
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
			if(outAsPercent)//returns the List<Tuple<Part, double>> with the double being a percentage of the total EC used
			{
				for(int i = 0; i < partECUsage.Count; i++)
				{
					Tuple<Part, double> temptup = new Tuple<Part, double>(partECUsage[i].item1, partECUsage[i].item2/returnAmount);
					partECPercentReturn.Add(temptup);
				}
				partECPercent = partECPercentReturn;
				return returnAmount;
			}
			else//returns List<Tuple<Part, double>> with the amount of EC used
			{
				partECPercent = partECUsage;
				return returnAmount;
			}
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
