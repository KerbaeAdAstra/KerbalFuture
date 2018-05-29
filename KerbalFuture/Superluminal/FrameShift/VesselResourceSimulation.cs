using System.Collections.Generic;
using KerbalFuture.Utils;
using UnityEngine;

/*
 * Notes on resource usage
 * https://hastebin.com/raw/apilelovez
*/

namespace KerbalFuture.Superluminal.FrameShift
{
	public class VesselResourceSimulation
	{
		// PartResourceDefinition
		public VesselResourceSimulation(Vessel v, double vel, bool autoSim)
		{
			Debug.Log("[KF] VesselResourceSimulation created for vessel: " + v.GetDisplayName());
			Status = SimulationStatus.Working;
			velocity = vel;
			vessel = v;
			driveDatas = FSWarpHelp.DriveDataList(v);
			Debug.Log("[KF] Number of drive data's is: " + driveDatas.Count);
			PopulateResourceSets();
			if (autoSim)
			{
				Debug.Log("[KF] [VRS] autoSim is true, running simulation");
				RunSimulation();
			}
		}
		// Velocity of the vessel
		double velocity;
		// The vessel being simulated
		Vessel vessel;
		// A list of SpaceFolderDrives on the vessel
		List<Part> drives = new List<Part>();
		// A list of SpaceFolderDriveDatas for the list of parts
		List<FrameShiftDriveData> driveDatas = new List<FrameShiftDriveData>();
		// Hashsets for the resources
		HashSet<string> resHash = new HashSet<string>();
		//List of vessel resources and amounts
		List<VesselResource> resList = new List<VesselResource>();
		// The status of the current simulation
		public SimulationStatus Status { get; private set; }
		// Fills the HashSets, creates resource dictionaries
		void PopulateResourceSets()
		{
			Debug.Log("[KF] [VRS] Populating resource hash sets");
			foreach (FrameShiftDriveData d in driveDatas)
			{
				Debug.Log("[KF] data is: " + d.DriveDataPart.persistentId + " " + d.MainResource + " " + d.Catalyst);
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
		// Runs a full simulation of resource usage for a warp
		public void RunSimulation()
		{
			Debug.Log("[KF] [VRS] Running simulation");
			double U_F = FrameShiftWarpChecks.FieldEnergyCalc(velocity, WarpHelp.VesselDiameterCalc(vessel) / 2);
			double sigmaCapacity = 0;
			foreach(FrameShiftDriveData dd in driveDatas)
			{
				sigmaCapacity += dd.Capacity;
			}
			foreach(FrameShiftDriveData dd in driveDatas)
			{
				double contribution = (dd.Capacity / sigmaCapacity) * U_F;
				for(int i = 0; i < resList.Count; i++)
				{
					if(dd.MainResource == resList[i].resource)
					{
						// last part turns the contribution (in Joules) to EC (in kJ)
						resList[i] = new VesselResource(resList[i].resource, resList[i].amount - (contribution / 1000));
					}
					if(dd.Catalyst == resList[i].resource)
					{
						resList[i] = new VesselResource(resList[i].resource, resList[i].amount - ((contribution / 1000) * dd.XMMultiplier));
					}
					if(resList[i].amount < 0)
					{
						Status = SimulationStatus.Failed;
						return;
					}
				}
			}
			Status = SimulationStatus.Succeeded;
			return;
		}
	}
}