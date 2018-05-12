using System;
using System.Collections.Generic;
using KerbalFuture.Utils;
using UnityEngine;
using KerbalFuture.Superluminal.SpaceFolder;
using System.Linq;
using System.Collections;

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
        // Hashsets for the main resources and catalysts
		HashSet<string> mainRes = new HashSet<string>();
		HashSet<string> cat = new HashSet<string>();
		// Resource dictionaries with the name and the amount
		Dictionary<string, double> ResDic = new Dictionary<string, double>();
		Dictionary<string, double> CatDic = new Dictionary<string, double>();
        // The status of the current simulation
		public SimulationStatus Status { get; private set; }
        // Fills the HashSets, creates resource dictionaries
		void PopulateResourceSets()
		{
			Debug.Log("[KF] [VRS] Populating resource hash sets");
			foreach (SpaceFolderDriveData d in driveDatas)
			{
				Debug.Log("[KF] data is: " + d.DriveDataPart.persistentId + " " + d.MainResource + " " + d.Catalyst);
				mainRes.Add(d.MainResource);
				cat.Add(d.Catalyst);
			}
			CreateResourceDictionary();
			CreateCatalystDictionary();
		}
        // Creates the Main Resource dictionary
		void CreateResourceDictionary()
		{
			Debug.Log("[KF] [VRS] Creating resource dictionary");
			foreach (string s in mainRes)
			{
				ResDic.Add(s, WarpHelp.ResourceAmountOnVessel(vessel, s));
			}
		}
        // Creates the Catalyst dictionary
		void CreateCatalystDictionary()
		{
			Debug.Log("[KF] [VRS] Creating catalyst dictionary");
			foreach (string s in cat)
			{
				CatDic.Add(s, WarpHelp.ResourceAmountOnVessel(vessel, s));
			}
		}
        // Calculates the resources needed for a drive of a given diameter and multiplier
		double MainResCalc(double diameter, double multiplier) => Math.Pow(Math.E, diameter * multiplier / 5) * 300;
        // Calculates the amount of catalyst needed for a drive of a given diameter and multiplier
		double CatCalc(double diameter, double multiplier) => Math.Pow(Math.E, diameter * multiplier / 5);
        // Runs a full simulation of resource usage for a warp
		public void RunSimulation()
		{
			Debug.Log("[KF] [VRS] Running simulation");
			Dictionary<string, double> ResDicCopy = new Dictionary<string, double>();
			Dictionary<string, double> CatDicCopy = new Dictionary<string, double>();
			// RESOURCE Math.Pow(Math.E, [diameter] * [multiplier] / 5) * 300;
			// CATALYST Math.Pow(Math.E, [diameter] * [multiplier] / 5);
			foreach (SpaceFolderDriveData d in driveDatas)
			{
				// ResDic.Item[d.MainResource] -= MainResCalc(d.Diameter, d.Multiplier);
				// CatDic.Item[d.Catalyst] -= CatCalc(d.Diameter, d.Multiplier);
                foreach (KeyValuePair<string, double> kvp in ResDic)
                {
					if (kvp.Key != d.MainResource)
					{
						continue;
					}
					ResDicCopy.Add(d.MainResource, kvp.Value - MainResCalc(d.Diameter, d.Multiplier));
                }
                foreach (KeyValuePair<string, double> kvp in CatDic)
                {
					if (kvp.Key != d.Catalyst)
					{
						continue;
					}
					CatDicCopy.Add(d.Catalyst, kvp.Value - CatCalc(d.Diameter, d.Multiplier));
				}
			}
			foreach (KeyValuePair<string, double> kvp in ResDicCopy)
			{
				Debug.Log("[KF] In sector 4, looping");
				if (kvp.Value >= 0)
				{
					Debug.Log("[KF] In sector 4, continuing");
					continue;
				}
				Debug.Log("[KF] In sector 4, sim failed");
				Status = SimulationStatus.Failed;
				return;
			}
			foreach (KeyValuePair<string, double> kvp in CatDicCopy)
			{
				Debug.Log("[KF] In sector 5, looping");
				if (kvp.Value >= 0)
				{
					Debug.Log("[KF] In sector 5, continuing");
					continue;
				}
				Debug.Log("[KF] In sector 5, sim failed");
				Status = SimulationStatus.Failed;
				return;
			}
			Status = SimulationStatus.Succeeded;
			return;
		}
	}
}