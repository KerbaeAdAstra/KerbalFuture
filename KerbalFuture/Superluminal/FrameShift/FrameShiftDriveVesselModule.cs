using System.Collections.Generic;
using System;
using UnityEngine;
using KerbalFuture.Utils;

namespace KerbalFuture.Superluminal.FrameShift
{
	public class FrameShiftDriveVesselModule : VesselModule
	{
		public static FrameShiftDriveVesselModule RequestCurrentVesselModule() => currentVessel;
		private static FrameShiftDriveVesselModule currentVessel;
		// We'll use these as a sort of latch in the FixedUpdate() loop
		bool isWarping = false;
		bool wasWarping = false;
		// In m/s (tested)
		double warpVelocity;
		List<Part> partsWithoutModuleCoreHeat = new List<Part>();
		public VesselResourceSimulation vrsInstance = new VesselResourceSimulation();
		double vesVel;
		void VesselPartCountChangedHandler(Vessel v)
		{
			Debug.Log("[KF] Detected part count change");
			if (isWarping)
			{
				CheckInWarpAvalibility();
			}
		}
		//Fires on Vessel startup
		protected override void OnStart()
		{
			base.OnStart();
			GameEvents.onVesselPartCountChanged.Add(VesselPartCountChangedHandler);
			Debug.Log("[KF] FSD Vessel module starting for " + Vessel.GetDisplayName());
		}
		private void Update()
		{
			if (Vessel == FlightGlobals.ActiveVessel)
			{
				currentVessel = this;
			}
		}
		private void FixedUpdate()
		{
			// Called when the vessel itnitially starts warping
			if (isWarping && !wasWarping)
			{
				Debug.Log("[KF] Vesel warp called for vessel " + Vessel.GetDisplayName());
				wasWarping = true;
				vesVel = Vessel.obt_speed;
				//Should change the velocity of the vessel in the vessel's facing direction
				Vessel.ChangeWorldVelocity(WarpHelp.GetFacing(Vessel).Vector * warpVelocity);
				UseResourcesBeforeWarp(warpVelocity, vrsInstance);
			}
			// Called during the warp
			else if (isWarping && wasWarping)
			{
				CheckInWarpAvalibility();
				UseResourcesDuringWarp(warpVelocity);
			}
			// Called right after the warp has finished
			else if (!isWarping && wasWarping)
			{
				Debug.Log("[KF] Vessel warp ended for vessel " + Vessel.GetDisplayName());
				wasWarping = false;
				Vessel.ChangeWorldVelocity(WarpHelp.GetFacing(Vessel).Vector * (-1 * warpVelocity));
				DistributeHeatAfterWarp();
			}
		}
		public bool WarpVessel(FrameShiftWarpData warpData, out Error err)
		{
			Debug.Log("[KF] FSD Warp called for vessel " + Vessel.GetDisplayName());
			warpVelocity = warpData.warpVelocity;
			Error internFault = err = FrameShiftWarpChecks.WarpAvalible(Vessel, warpData.warpVelocity, out vrsInstance);
			if (internFault == 0)
			{
				Debug.Log("[KF] Warp cleared for vessel " + Vessel.GetDisplayName() + ", warping now.");
				isWarping = true;
				return true;
			}
			else
			{
				isWarping = false;
				return false;
			}
		}
		public void StopWarp()
		{
			Debug.Log("[KF] Warp halted");
			isWarping = false;
		}
		// Uses resources before warping including drive efficiencies
		void UseResourcesBeforeWarp(double velocity, VesselResourceSimulation vrs)
		{
			Debug.Log("[KF] Using resources before warp!");
			double U_F = FrameShiftWarpChecks.FieldEnergyCalc(velocity, WarpHelp.VesselDiameterCalc(Vessel) / 2);
			foreach (KeyValuePair<FrameShiftDriveData, double> kvp in vrs.PartECPercent)
			{
				WarpHelp.UseResource(kvp.Key.DriveDataPart, (kvp.Value * U_F / 1000) / kvp.Key.Efficiency, kvp.Key.MainResource);
				WarpHelp.UseResource(kvp.Key.DriveDataPart, ((kvp.Value * U_F / 1000) * kvp.Key.XMMultiplier) / kvp.Key.Efficiency, kvp.Key.Catalyst);
			}
			DistributeHeatBeforeWarp(U_F);
		}
		// Uses resources during warp, called once a fixedupdate using efficiency
		void UseResourcesDuringWarp(double velocity)
		{
			Debug.Log("[KF] Using resources during warp!");
			// Calculates the total change in field energy
			double dU_FkJ = (Math.Pow(velocity, 3) * Math.Pow(WarpHelp.VesselDiameterCalc(vessel) / 2, 3) * FrameShiftWarpChecks.HYPERSPACE_DRAG_CONSTANT) / 1000;
			List<FrameShiftDriveData> driveDatas = new List<FrameShiftDriveData>(FSWarpHelp.DriveDataList(Vessel));
			double sigmaCapacity = 0;
			foreach (FrameShiftDriveData dd in driveDatas)
			{
				sigmaCapacity += dd.Capacity;
			}
			foreach (FrameShiftDriveData dd in driveDatas)
			{
				// Calculates the contribution of the drive in kJ/second using efficiency
				double contribution = ((dd.Capacity / sigmaCapacity) * dU_FkJ / dd.Efficiency);
				// deltaTime is the time between the last frame and now
				// Using it in this context, it means that it was x number of seconds between the last frame and this one
				// Unity says to multiply it to your value to get a 'x units per second' effect, but I'm not sure, it doesn't seem right
				// because now we should have kJ*sec instead of kJ/sec
				// FIXME
				Debug.Log("[KF] Time since last frame was " + Time.deltaTime + " seconds.");
				Debug.Log("[KF] Drive usage should be " + contribution * Time.deltaTime + " per second from contribution " + contribution + ".");
				WarpHelp.UseResource(dd.DriveDataPart, contribution * Time.deltaTime, dd.MainResource);
				WarpHelp.UseResource(dd.DriveDataPart, contribution * dd.XMMultiplier * Time.deltaTime, dd.Catalyst);
			}
			DistributeHeatDuringWarp(dU_FkJ * 1000, sigmaCapacity);
		}
		void DistributeHeatBeforeWarp(double U_F)
		{
			Debug.Log("[KF] Distributing lost heat before warp.");
			AddHeatModulesToParts();
			foreach (KeyValuePair<FrameShiftDriveData, double> kvp in vrsInstance.PartECPercent)
			{
				//J each part is putting in
				double contributionJ = (kvp.Value * U_F) / kvp.Key.Efficiency + ((kvp.Value * U_F) * kvp.Key.XMMultiplier) / kvp.Key.Efficiency;
				((ModuleCoreHeat)kvp.Key.DriveDataPart.Modules["ModuleCoreHeat"]).AddEnergyToCore(contributionJ);
			}
		}
		void DistributeHeatDuringWarp(double dU_F, double sigmaCapacity)
		{
			Debug.Log("[KF] Distributing heat during warp.");
			foreach (FrameShiftDriveData dd in FSWarpHelp.DriveDataList(Vessel))
			{
				double contributionJ = ((dd.Capacity / sigmaCapacity) * dU_F / dd.Efficiency) + ((dd.Capacity / sigmaCapacity) * dU_F * dd.XMMultiplier / dd.Efficiency);
				((ModuleCoreHeat)dd.DriveDataPart.Modules["ModuleCoreHeat"]).AddEnergyToCore(contributionJ);
			}
		}
		void DistributeHeatAfterWarp()
		{
			Debug.Log("[KF] Distributing field energy as heat after warp.");
			double U_F = FrameShiftWarpChecks.FieldEnergyCalc(warpVelocity, WarpHelp.VesselDiameterCalc(Vessel) / 2);
			foreach (KeyValuePair<FrameShiftDriveData, double> kvp in vrsInstance.PartECPercent)
			{
				double contributionJ = (kvp.Value * U_F) / kvp.Key.Efficiency + ((kvp.Value * U_F) * kvp.Key.XMMultiplier) / kvp.Key.Efficiency;
				((ModuleCoreHeat)kvp.Key.DriveDataPart.Modules["ModuleCoreHeat"]).AddEnergyToCore(contributionJ);
			}
			//D_RemoveHeatModulesFromParts();
		}
		//All KF drives will have ModuleCoreHeat added to them through a MM patch
		void AddHeatModulesToParts()
		{
			Debug.Log("[KF] Adding heat modules to all vessel FSDs.");
			//partsWithoutModuleCoreHeat.Clear();
			foreach (Part p in Vessel.Parts)
			{
				if (!p.Modules.Contains("ModuleCoreHeat"))
				{
					//partsWithoutModuleCoreHeat.Add(p);
					p.AddModule("ModuleCoreHeat");
				}
			}
		}
		void D_RemoveHeatModulesFromParts()
		{
			Debug.Log("[KF] Removing heat modules from parts originally without heat modules.");
			foreach (Part p in partsWithoutModuleCoreHeat)
			{
				p.Modules.Remove(p.Modules["ModuleCoreHeat"]);
			}
		}
		void CheckWarpAvalibility()
		{
			Debug.Log("[KF] Checking warp avalibility...");
			VesselResourceSimulation vrs = new VesselResourceSimulation();
			Error err = FrameShiftWarpChecks.WarpAvalible(Vessel, warpVelocity, out vrs);
			vrsInstance = vrs;
			if (err == Error.ClearForWarp)
			{
				return;
			}
			else
			{
				isWarping = false;
				return;
			}
		}
		void CheckInWarpAvalibility()
		{
			Dictionary<string, double> resUsage = vrsInstance.TotalResourceUsagePerSecond;
			foreach (KeyValuePair<string, double> kvp in resUsage)
			{
				if (WarpHelp.ResourceAmountOnVessel(Vessel, kvp.Key) < kvp.Value)
				{
					isWarping = false;
				}
			}
		}
		void MoveVessel(double vel)
		{
			Vessel.SetPosition(Vessel.GetWorldPos3D() + ((vel + vesVel) * Time.deltaTime * WarpHelp.GetFacing(Vessel).Vector));
		}
	}
}