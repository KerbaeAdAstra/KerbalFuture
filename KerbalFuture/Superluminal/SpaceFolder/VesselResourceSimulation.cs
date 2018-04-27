using System;
using System.Collections.Generic;
using KerbalFuture.Utils;

namespace KerbalFuture.Superluminal.SpaceFolder
{
	internal class VesselResourceSimulation
	{
		//PartResourceDefinition
		public VesselResourceSimulation(Vessel v, List<Part> _drives, bool autoSim)
		{
			Status = SimulationStatus.Working;
			vessel = v;
			drives = _drives;
			PopulateResourceSets();
            if(autoSim)
            {
                RunSimulation();
            }
		}
        //The vessel being simulated
		Vessel vessel;
        //A list of SpaceFolderDrives on the vessel
		List<Part> drives;
        //A list of SpaceFolderDriveDatas for the list of parts
		List<SpaceFolderDriveData> driveDatas = new List<SpaceFolderDriveData>();
        //Hashsets for the main resources and catalysts
		HashSet<string> mainRes = new HashSet<string>();
		HashSet<string> cat = new HashSet<string>();
        //Resource dictionaries with the name and the amount
		public Dictionary<string, double> ResDic { get; private set; }
		public Dictionary<string, double> CatDic { get; private set; }
        //The status of the current simulation
		public SimulationStatus Status { get; private set; }
        //Fills the HashSets, creates resource dictionaries
		void PopulateResourceSets()
		{
			foreach(Part p in drives)
			{
				SpaceFolderDriveData data = p.Modules.GetModule<ModuleSpaceFolderEngine>().PartDriveData;
				driveDatas.Add(data);
				mainRes.Add(data.MainResource);
				cat.Add(data.Catalyst);
				CreateResourceDictionary();
				CreateCatalystDictionary();
			}
		}
        //Creates the Main Resource dictionary
		void CreateResourceDictionary()
		{
			foreach(string s in mainRes)
			{
				ResDic.Add(s, WarpHelp.ResourceAmountOnVessel(vessel, s));
			}
		}
        //Creates the Catalyst dictionary
		void CreateCatalystDictionary()
		{
			foreach(string s in cat)
			{
				CatDic.Add(s, WarpHelp.ResourceAmountOnVessel(vessel, s));
			}
		}
        //Calculates the resources needed for a drive of a given diameter and multiplier
		double MainResCalc(double diameter, double multiplier) => Math.Pow(Math.E, diameter * multiplier / 5) * 300;
        //Calculates the amount of catalyst needed for a drive of a given diameter and multiplier
		double CatCalc(double diameter, double multiplier) => Math.Pow(Math.E, diameter * multiplier / 5);
        //Runs a full simulation of resource usage for a warp
		public void RunSimulation()
		{
			//RESOURCE Math.Pow(Math.E, [diameter] * [multiplier] / 5) * 300;
			//CATALYST Math.Pow(Math.E, [diameter] * [multiplier] / 5);
			foreach (SpaceFolderDriveData d in driveDatas)
			{
				//ResDic.Item[d.MainResource] -= MainResCalc(d.Diameter, d.Multiplier);
				//CatDic.Item[d.Catalyst] -= CatCalc(d.Diameter, d.Multiplier);
                foreach (KeyValuePair<string, double> kvp in ResDic)
                {
                    if (kvp.Key != d.MainResource)
                    {
                        continue;
                    }
	                ResDic.Remove(kvp.Key);
	                ResDic.Add(d.MainResource, kvp.Value - MainResCalc(d.Diameter, d.Multiplier));
                }
                foreach (KeyValuePair<string, double> kvp in CatDic)
                {
                    if (kvp.Key != d.Catalyst)
                    {
                        continue;
                    }
	                CatDic.Remove(kvp.Key);
	                CatDic.Add(d.Catalyst, kvp.Value - CatCalc(d.Diameter, d.Multiplier));
                }
			}
			foreach(KeyValuePair<string, double> kvp in ResDic)
			{
                if (!(kvp.Value < 0))
                {
                    continue;
                }
				Status = SimulationStatus.Failed;
				return;
			}
			foreach(KeyValuePair<string, double> kvp in CatDic)
			{
                if (!(kvp.Value < 0))
                {
                    continue;
                }
				Status = SimulationStatus.Succeeded;
				return;
			}
		}
	}
}