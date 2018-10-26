using System;
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
		public VesselResourceSimulation()
		{
			Debug.Log("[KF] Null VesselResourceSimulation created");
			isNull = true;
		}
		// PartResourceDefinition
		public VesselResourceSimulation(Vessel v, double vel, bool autoSim)
		{
			Debug.Log("[KF] VesselResourceSimulation created for vessel: " + v.GetDisplayName());
			isNull = false;
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
		public bool isNull { get; private set; }
		// Get only property for dictionary of the part and it's contribution prercentage
		public Dictionary<FrameShiftDriveData, double> PartECPercent { get { return partECPercent; } }
		// Backing field for PartECPercent (Get only)
		Dictionary<FrameShiftDriveData, double> partECPercent = new Dictionary<FrameShiftDriveData, double>();
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
		//List of resources and amounts needed
		List<VesselResource> totalResNeeded = new List<VesselResource>();
		public List<VesselResource> TotalResNeeded { get { return totalResNeeded; } }
		//Resource usage per second
		Dictionary<string, double> totalResourceUsagePerSecond = new Dictionary<string, double>();
		public Dictionary<string, double> TotalResourceUsagePerSecond { get { return totalResourceUsagePerSecond; } }
		// The status of the current simulation
		public SimulationStatus Status { get; private set; }
		// The max warp time (get to calculate)
		public double MaxWarpTime { get { return FindMaxWarpTime(); } }
		// Fills the HashSets, creates resource dictionaries
		void PopulateResourceSets()
		{
			// Freshen them up if being called again
			resHash.Clear();
			resList.Clear();
			totalResNeeded.Clear();
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
				//identical lists at this point in terms of the resource order
				resList.Add(new VesselResource(s, WarpHelp.ResourceAmountOnVessel(vessel, s)));
				totalResNeeded.Add(new VesselResource(s, 0));
			}
		}
		// Runs a full simulation of resource usage for a warp
		void RunSimulation()
		{
			isNull = false;
			Debug.Log("[KF] [VRS] Running simulation");
			double U_F = FrameShiftWarpChecks.FieldEnergyCalc(velocity, WarpHelp.VesselDiameterCalc(vessel) / 2);
			double sigmaCapacity = 0;
			foreach (FrameShiftDriveData dd in driveDatas)
			{
				sigmaCapacity += dd.Capacity;
			}
			partECPercent.Clear();
			foreach (FrameShiftDriveData dd in driveDatas)
			{
				// This is the total contribution of the drive in Joules. Remove the U_F to get the percent contribution
				// However, drives do not have 100% efficiency so we need to multiply it by the reciprical of the drive's efficiency
				double contribution = (dd.Capacity / sigmaCapacity) * U_F * dd.Efficiency;
				Debug.Log("[KF] 1");
				partECPercent.Add(dd, (dd.Capacity / sigmaCapacity));
				Debug.Log("[KF] 2");
				for (int i = 0; i < resList.Count; i++)
				{
					Debug.Log("[KF] Reslist[" + i + "] is " + resList[i].ToString());
					if (dd.MainResource == resList[i].resource)
					{
						// last part turns the contribution (in Joules) to EC (in kJ)
						resList[i] = new VesselResource(resList[i].resource, resList[i].amount - (contribution / 1000));
						totalResNeeded[i] = new VesselResource(resList[i].resource, (contribution / 1000));
						Debug.Log("[KF] Reslist[" + i + "] is now " + resList[i].ToString() + " after main res usage");
					}
					if (dd.Catalyst == resList[i].resource)
					{
						resList[i] = new VesselResource(resList[i].resource, resList[i].amount - ((contribution / 1000) * dd.XMMultiplier));
						totalResNeeded[i] = new VesselResource(resList[i].resource, (contribution / 1000) * dd.XMMultiplier);
						Debug.Log("[KF] Reslist[" + i + "] is now" + resList[i].ToString() + " after catalyst usage");
					}
					if (resList[i].amount < 0)
					{
						Debug.Log("[KF] Reslist[" + i + "] is < 0, " + resList[i].ToString());
						Status = SimulationStatus.Failed;
					}
				}
			}
			Debug.Log("[KF] 7");
			if(Status != SimulationStatus.Failed)
			{
				Status = SimulationStatus.Succeeded;
			}
			return;
		}
		double FindMaxWarpTime()
		{
			Debug.Log("[KF] Finding the max warp time...");
			if (Status == SimulationStatus.Failed)
			{
				// Return immediatly if the sim failed to begin with
				return 0.0;
			}
			// We'll be working in kJ for this funciton because I feel like it
			// This is deltaFieldEnergy for one 't' :V
			double dU_FkJ = (Math.Pow(velocity, 3) * Math.Pow(WarpHelp.VesselDiameterCalc(vessel) / 2, 3) * FrameShiftWarpChecks.HYPERSPACE_DRAG_CONSTANT) / 1000;
			// So now we have the change in energy per second
			// We need to figure out how long we can sustain this
			// Time is in seconds, EC is in kJ, and dU_F is in J. Units!
			double U_FkJ = (FrameShiftWarpChecks.FieldEnergyCalc(velocity, WarpHelp.VesselDiameterCalc(vessel) / 2)) / 1000;
			double sigmaCapacitykJ = 0;
			foreach (FrameShiftDriveData dd in driveDatas)
			{
				// Adds the converted capacity (J -> kJ) to the sigmaCapacity
				sigmaCapacitykJ += dd.Capacity / 1000;
			}
			// Now we have the capacity of the drive, the capacity avalible to provide by all the drives, and the change in capacity every second
			Dictionary<FrameShiftDriveData, double> partResourceUsagePerSecond = new Dictionary<FrameShiftDriveData, double>();
			foreach(FrameShiftDriveData dd in driveDatas)
			{
				//Adds the FSDD and the amount of energy needed per drive per 't' in kJ
				partResourceUsagePerSecond.Add(dd, ((dd.Capacity / 1000) / sigmaCapacitykJ) * dU_FkJ);
			}
			foreach(KeyValuePair<FrameShiftDriveData, double> kvp in partResourceUsagePerSecond)
			{
				// If the resource dictionary already contains the resource
				if (totalResourceUsagePerSecond.ContainsKey(kvp.Key.MainResource))
				{
					// Stores the value of the current key temporarily
					double currentKVPKeyMainResource = totalResourceUsagePerSecond[kvp.Key.MainResource];
					// Removes that key-value pair
					totalResourceUsagePerSecond.Remove(kvp.Key.MainResource);
					// Adds it back with the new value
					totalResourceUsagePerSecond.Add(kvp.Key.MainResource, currentKVPKeyMainResource + kvp.Value);
				}
				// If it does just add it
				else
				{
					totalResourceUsagePerSecond.Add(kvp.Key.MainResource, kvp.Value);
				}
				// Same stuff for the catalyst, but multiplied by the XMMultiplier value on the addition
				if(totalResourceUsagePerSecond.ContainsKey(kvp.Key.Catalyst))
				{
					double currentKVPKeyCatalyst = totalResourceUsagePerSecond[kvp.Key.Catalyst];
					totalResourceUsagePerSecond.Remove(kvp.Key.Catalyst);
					totalResourceUsagePerSecond.Add(kvp.Key.MainResource, currentKVPKeyCatalyst + kvp.Value * kvp.Key.XMMultiplier);
				}
				else
				{
					totalResourceUsagePerSecond.Add(kvp.Key.Catalyst, kvp.Value * kvp.Key.XMMultiplier);
				}
			}
			// Set to MaxValue to begin with because it is compared against later
			// We want the first match to always overwrite it
			// And I don't want to use another variable to check if it's the first run through
			double smallestTime = double.MaxValue;
			// Not freshening the resource sets because we still want the vessel resource values
			foreach(VesselResource vr in resList)
			{
				foreach(KeyValuePair<string, double> kvp in totalResourceUsagePerSecond)
				{
					// Compares the resource in the VesselResource to the resource in the resUsage
					if(vr.resource == kvp.Key)
					{
						// amount / (amount / second) gives seconds
						double time = vr.amount / kvp.Value;
						if(time < smallestTime)
						{
							smallestTime = time;
						}
					}
				}
			}
			Debug.Log("[KF] Max warp time is " + smallestTime);
			// smallestTime is now the max time that the drive can run for without the vessel running out of resources
			return smallestTime;
		}
	}
}