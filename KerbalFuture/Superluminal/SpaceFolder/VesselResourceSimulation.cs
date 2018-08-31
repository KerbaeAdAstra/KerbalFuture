using System;
using System.Collections.Generic;
using KerbalFuture.Utils;
using UnityEngine;

namespace KerbalFuture.Superluminal.SpaceFolder
{
	public class VesselResourceSimulation
	{
		// PartResourceDefinition
		public VesselResourceSimulation(Vessel v, bool autoSim)
		{
			Debug.Log("[KF] VesselResourceSimulation created for vessel: " + v.GetDisplayName());
			Status = SimulationStatus.Working;
			vessel = v;
			driveDatas = SFWarpHelp.DriveDataList(v);
			Debug.Log("[KF] Number of drive data's is: " + driveDatas.Count);
			PopulateResourceSets();
            if (autoSim)
            {
				Debug.Log("[KF] [VRS] autoSim is true, running simulation");
				RunSimulation();
            }
		}
        // The vessel being simulated
		Vessel vessel;
        // A list of SpaceFolderDrives on the vessel
		List<Part> drives = new List<Part>();
        // A list of SpaceFolderDriveDatas for the list of parts
		List<SpaceFolderDriveData> driveDatas = new List<SpaceFolderDriveData>();
        // Hashset for the main resources and catalysts
		HashSet<string> resHash = new HashSet<string>();
		// Resource dictionary with the name and the amount
		List<VesselResource> resList = new List<VesselResource>();
        // The status of the current simulation
		public SimulationStatus Status { get; private set; }
        // Fills the HashSets, creates resource dictionaries
		void PopulateResourceSets()
		{
			Debug.Log("[KF] [VRS] Populating resource hash sets");
			foreach (SpaceFolderDriveData d in driveDatas)
			{
				Debug.Log("[KF] [VRS] Data is: " + d.DriveDataPart.persistentId + " " + d.MainResource + " " + d.Catalyst);
				resHash.Add(d.MainResource);
				resHash.Add(d.Catalyst);
			}
			CreateResourceDictionary();
		}
        // Creates the Main Resource dictionary
		void CreateResourceDictionary()
		{
			Debug.Log("[KF] [VRS] Creating resource dictionary");
			foreach (string s in resHash)
			{
				resList.Add(new VesselResource(s, WarpHelp.ResourceAmountOnVessel(vessel, s)));
			}
		}
		//too lazy to change all the references, so I just changed these two
		// Calculates the resources needed for a drive of a given diameter and multiplier
		double MainResCalc(double diameter, double multiplier) => SpaceFolderDriveVesselModule.MainResourceWarpCalc(diameter, multiplier);
        // Calculates the amount of catalyst needed for a drive of a given diameter and multiplier
		double CatCalc(double diameter, double multiplier) => SpaceFolderDriveVesselModule.CatalystWarpCalc(diameter, multiplier);
        // Runs a full simulation of resource usage for a warp
		public void RunSimulation()
		{
			Debug.Log("[KF] [VRS] Running simulation");
			double diam = WarpHelp.VesselDiameterCalc(vessel);
			List<Part> drives = SFWarpHelp.PartsWithModuleSFD(vessel);
			// Dictionary of the actual diameter that the part is putting out
			Dictionary <SpaceFolderDriveData, double> partRelDiamDict = new Dictionary<SpaceFolderDriveData, double>();
			double percentCalc = diam / SpaceFolderWarpChecks.MaxWarpHoleSize(drives);
			foreach(SpaceFolderDriveData dd in driveDatas)
			{
				partRelDiamDict.Add(dd, dd.Diameter * percentCalc);
			}
			//Runs through all the resources in resList, removing when the resources match up
			for(int i = 0; i < resList.Count; i++)
			{
				foreach(KeyValuePair<SpaceFolderDriveData, double> kvp in partRelDiamDict)
				{
					if(resList[i].resource == kvp.Key.MainResource)
					{
						//Creates a local var to set the VesselResource struct in the list to the new value
						double amt = resList[i].amount - MainResCalc(kvp.Value, kvp.Key.Multiplier);
						resList[i] = new VesselResource(resList[i].resource, amt);
					}
					if(resList[i].resource == kvp.Key.Catalyst)
					{
						double amt = resList[i].amount - CatCalc(kvp.Value, kvp.Key.Multiplier);
						resList[i] = new VesselResource(resList[i].resource, amt);
					}
				}
				//checks after removing all resources for the resource in the for() loop (not the foreach)
				//This cuts down on the amount of times it has to run through the list if the resources aren't all there
				if(resList[i].amount < 0)
				{
					Status = SimulationStatus.Failed;
					return;
				}
			}
			Status = SimulationStatus.Succeeded;
			return;
		}
	}
}