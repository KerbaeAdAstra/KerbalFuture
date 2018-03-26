using System;
using System.Collections.Generic;
using System.Linq;
using KerbalFuture.SpaceFolder;
using KerbalFuture.Utils;

namespace SpaceFolder
{
	class SpaceFolderWarpChecks
	{
		public bool CheckWarp(Vessel v, bool warpVesselAfterCheck)//called by GUI, returns true or false based on if warpchecks got all the way through or not
		{
			ShipConstruct sc = new ShipConstruct(v.GetName(), EditorFacility.VAB, v.Parts); //TODO get vessel launch building and use that instead of just EditorFacilities.VAB
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
			double electricityNeeded = ElectricityWarpCalc(engineList, out engineTupleList);//sets engineTupleList with a list of Part, amount
			warpTupleList = PopulateWarpTupleList(engineTupleList);//creates the quaduple with <Part, amount, mainResource, catalyst>
			if (!CheckResources(vesData, warpTupleList))
			{
				return false;
			}
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
				return false;//L43
			}
			if (!warpVesselAfterCheck)
			{
				return false;
			}
			bool vmexists = false;
			SpaceFolderFlightDrive vm = GetVMInstance(v, ref vmexists);
			if (!vmexists || vm == null)
			{
				return false;
			}
			vm.WarpVessel(warpTupleList, electricityNeeded);//switchable so that it can be called to check if the warp is valid without actually warping
			return true;

		}
		bool CheckResources(VesselData vesData, List<Tuple<Part, double, string, string>> warpTupleList)
		{
			HashSet<string> resourcesUsed = new HashSet<string>();//HashSets only have one of each element, repeats are destroyed
			HashSet<string> catalystsUsed = new HashSet<string>();
			foreach (Tuple<Part, double, string, string> t in warpTupleList)
			{
				resourcesUsed.Add(t.item3);
				catalystsUsed.Add(t.item4);
			}
			Dictionary<string, double> vesselResourceAmounts = new Dictionary<string, double>();//New List<Dictionary<>> for the resource and the amount
			for (int i = 0; i < resourcesUsed.Count; i++)
			{
				vesselResourceAmounts.Add(resourcesUsed.ToArray()[i],
					vesData.ResourceAmountOnVessel(resourcesUsed.ToArray()[i]));
			}
			//Runs simulation of resources being used
			foreach (KeyValuePair<string, double> kvp in vesselResourceAmounts)
			{
				foreach (Tuple<Part, double, string, string> t in warpTupleList)
				{
					if (kvp.Key != t.item3) 
					{
						continue;
					}
					double resourceAmount = kvp.Value;
					resourceAmount -= t.item2;//'removes' resources from the 'vessel'
					vesselResourceAmounts[kvp.Key] = resourceAmount;
				}
			}
			//Checks to make sure none of the resources are in the red
			foreach (KeyValuePair<string, double> kvp in vesselResourceAmounts)
			{
				if (kvp.Value < 0)
				{
					return false;
				}
			}
			return true;//return true if everything went well
		}
		List<Tuple<Part, double, string, string>> PopulateWarpTupleList(IEnumerable<Tuple<Part, double>> engineListWithECUsage)
		{
			List<Tuple<Part, double, string, string>> returnList = new List<Tuple<Part, double, string, string>>();
			for(int i = 0; i < engineListWithECUsage.Count; i++)
			{
				Tuple<Part, double, string, string> tempTup = new Tuple<Part, double, string, string>(engineListWithECUsage[i].item1, engineListWithECUsage[i].item2, engineListWithECUsage[i].item1.Modules["SpaceFolderEngine"].mainResource, engineListWithECUsage[i].item1.Modules["SpaceFolderEngine"].catalyst);
				returnList.Add(tempTup);
			}
		}
		//TODO
		SpaceFolderFlightDrive GetVMInstance(Vessel v, ref bool exists)
		{
			List<VesselModule> vms = v.vesselModules;
			return (SpaceFolderFlightDrive) vms.FirstOrDefault(
				t => ReferenceEquals(t.GetType(), typeof(SpaceFolderFlightDrive)));
		}
		//TODO
		List<Part> EngineList(IShipconstruct v)//use list[i].Modules["SpaceFolderEngine"].field; to get specific values
			=> v.Parts.Where(t => t.Modules.Contains("SpaceFolderEngine")).ToList();
		double[] GetEngineValues(Part p)//array is in order of {engineSize, modifier}
		{
			double[] returnList = new double[2];
			if (!p.Modules.Contains("SpaceFolderEngine")) return returnList;
			returnList[0] = ((SpaceFolderEngine)p.Modules["SpaceFolderEngine"]).warpDriveDiameter;
			returnList[1] = ((SpaceFolderEngine)p.Modules["SpaceFolderEngine"]).engineMultiplier;
			return returnList;
		}
		double SpatiofibrinWarpCalc(Part engine)
		{
			double[] engineSize = new double[2];
			engineSize[0] = GetEngineValues(engine)[0];
			engineSize[1] = GetEngineValues(engine)[1];
			return Math.Pow(Math.E, engineSize[0] * engineSize[1] / 5);
		}
		//TODO
		double SpatiofibrinWarpCalc(IEnumerable<Part> engines)
		{
			List<double[]> engineSizes = engines.Select(GetEngineValues).ToList();
			return engineSizes.Select(t => t[0] * t[1]).Select(modifiedVal => Math.Pow(Math.E, modifiedVal / 5)).Sum();
		}
		double ElectricityWarpCalc(Part engine)
		{
			double[] engineInfo = new double[2];
			engineInfo[0] = GetEngineValues(engine)[0];
			engineInfo[1] = GetEngineValues(engine)[1];
			double returnAmount = Math.Pow(Math.E, engineInfo[0]*engineInfo[1]/5)*300;
			return returnAmount;
		}
		double ElectricityWarpCalc(IList<Part> engines, bool outAsPercent, out List<Tuple<Part, double>> partECPercent)
		{
			double returnAmount = 0;
			List<Tuple<Part, double>> partECUsage = new List<Tuple<Part, double>>();
			List<double[]> engineInfo = engines.Select(GetEngineValues).ToList();//TODO
			for(int i = 0; i < engineInfo.Count; i++)
			{
				double modifiedVal = engineInfo[i][0] * engineInfo[i][1];
				Tuple<Part, double> tempTup = new Tuple<Part, double>(engines[i], Math.Pow(Math.E, modifiedVal/5)*300);
				partECUsage.Add(tempTup);
				returnAmount += Math.Pow(Math.E, modifiedVal/5)*300;//TODO: fix values
			}
			List<Tuple<Part, double>> partECPercentReturn = new List<Tuple<Part, double>>();
			if (outAsPercent)//returns the List<Tuple<Part, double>> with the double being a percentage of the total EC used
			{
				partECPercentReturn.AddRange(partECUsage.Select(
					t => new Tuple<Part, double>(t.item1, t.item2 / returnAmount)));//TODO
				partECPercent = partECPercentReturn;
				return returnAmount;
			}
			partECPercent = partECUsage;
			return returnAmount;
		}
		double ElectricityWarpCalc(IList<Part> engines, out List<Tuple<Part, double>> partECPercent)
		{
			double returnAmount = 0;
			List<Tuple<Part, double>> partECUsage = new List<Tuple<Part, double>>();
			List<double[]> engineInfo = engines.Select(GetEngineValues).ToList();//TODO
			for(int i = 0; i < engineInfo.Count; i++)
			{
				double modifiedVal = engineInfo[i][0] * engineInfo[i][1];
				Tuple<Part, double> tempTup = new Tuple<Part, double>(engines[i], Math.Pow(Math.E, modifiedVal/5)*300);
				partECUsage.Add(tempTup);
				returnAmount += Math.Pow(Math.E, modifiedVal/5)*300;//TODO: fix values
			}
			//TODO
			List<Tuple<Part, double>> partECPercentReturn =
				partECUsage.Select(t => new Tuple<Part, double>(t.item1, t.item2 / returnAmount)).ToList();
			partECPercent = partECPercentReturn;
			return returnAmount;
		}
		double MaxWarpHoleSize(IEnumerable<Part> engines)
		{
			double[] dividers = { 0.8, 0.6, 0.4, 0.2 };
			double divider = 0.1;
			List<double> unmodEngineSize = new List<double>();
			List<double[]> engineSizes = engines.Select(GetEngineValues).ToList();
			foreach (double[] t in engineSizes)
			{
				unmodEngineSize.Add(t[0]);
			}
			double realSize = 0;
			for (int i = 0; i <= unmodEngineSize.Count; i++)
			{
				if (i == 0)
				{
					realSize += unmodEngineSize[i];
				}
				else if (i > 0 && i < 5)
				{
					realSize += unmodEngineSize[i] * dividers[i - 1];
				}
				else
				{
					realSize += unmodEngineSize[i] * divider;
					divider = divider / 2;
				}
			}
			goodToGo = true;
			FlightDrive.WarpVessel(fgs.activeVessel, MaxWarpHoleSize(engineSizes));
		}
	}
}
