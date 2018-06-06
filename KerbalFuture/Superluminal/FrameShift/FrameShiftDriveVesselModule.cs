using System.Collections.Generic;
using System;
using UnityEngine;
using KerbalFuture.Utils;

namespace KerbalFuture.Superluminal.FrameShift
{
	public class FrameShiftDriveVesselModule : VesselModule
	{
		// We'll use these as a sort of latch in the FixedUpdate() loop
		bool isWarping = false;
		bool wasWarping = false;
		// In m/s (tested)
		double warpVelocity = 500;
		List<Part> partsWithoutModuleCoreHeat = new List<Part>();
		VesselResourceSimulation vrsInstance = new VesselResourceSimulation();
		void VesselPartCountChangedHandler(Vessel v)
		{
			if(isWarping)
			{
				CheckWarpAvalibility();
			}
		}
		//Fires on Vessel startup
		protected override void OnStart()
		{
			base.OnStart();
			GameEvents.onVesselPartCountChanged.Add(VesselPartCountChangedHandler);
			Debug.Log("[KF] FSD Vessel module starting for " + Vessel.GetDisplayName());
		}
		public void FixedUpdate()
		{
			// Called when the vessel itnitially starts warping
			if(isWarping && !wasWarping)
			{
				wasWarping = true;
				UseResourcesBeforeWarp(warpVelocity, vrsInstance);
			}
			// Called during the warp
			else if(isWarping && wasWarping)
			{
				UseResourcesDuringWarp(warpVelocity);
			}
			// Called right after the warp has finished
			else if(!isWarping && wasWarping)
			{
				wasWarping = false;
				DistributeHeatAfterWarp();
			}
		}
		public void WarpVessel(FrameShiftWarpData warpData, out Error err)
		{
			Debug.Log("[KF] FSD Warp called for vessel " + Vessel.GetDisplayName());
			warpVelocity = warpData.warpVelocity;
			Error internFault = err = FrameShiftWarpChecks.WarpAvalible(Vessel, warpData.warpVelocity, out vrsInstance);
			if(internFault == 0)
			{
				Debug.Log("[KF] Warp cleared for vessel " + Vessel.GetDisplayName() + ", warping now.");
				isWarping = true;
				return;
			}
			else
			{
				isWarping = false;
				return;
			}
		}
		// Uses resources before warping including drive efficiencies
		void UseResourcesBeforeWarp(double velocity, VesselResourceSimulation vrs)
		{
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
			foreach(FrameShiftDriveData dd in FSWarpHelp.DriveDataList(Vessel))
			{
				double contributionJ = ((dd.Capacity / sigmaCapacity) * dU_F / dd.Efficiency) + ((dd.Capacity / sigmaCapacity) * dU_F * dd.XMMultiplier / dd.Efficiency);
				((ModuleCoreHeat)dd.DriveDataPart.Modules["ModuleCoreHeat"]).AddEnergyToCore(contributionJ);
			}
		}
		void DistributeHeatAfterWarp()
		{
			double U_F = FrameShiftWarpChecks.FieldEnergyCalc(warpVelocity, WarpHelp.VesselDiameterCalc(Vessel) / 2);
			foreach(KeyValuePair<FrameShiftDriveData, double> kvp in vrsInstance.PartECPercent)
			{
				double contributionJ = (kvp.Value * U_F) / kvp.Key.Efficiency + ((kvp.Value * U_F) * kvp.Key.XMMultiplier) / kvp.Key.Efficiency;
				((ModuleCoreHeat)kvp.Key.DriveDataPart.Modules["ModuleCoreHeat"]).AddEnergyToCore(contributionJ);
			}
			RemoveHeatModulesFromParts();
		}
		void AddHeatModulesToParts()
		{
			partsWithoutModuleCoreHeat.Clear();
			foreach(Part p in Vessel.Parts)
			{
				if(!p.Modules.Contains("ModuleCoreHeat"))
				{
					partsWithoutModuleCoreHeat.Add(p);
					p.AddModule("ModuleCoreHeat");
				}
			}
		}
		void RemoveHeatModulesFromParts()
		{
			foreach(Part p in partsWithoutModuleCoreHeat)
			{
				p.Modules.Remove(p.Modules["ModuleCoreHeat"]);
			}
		}
		void CheckWarpAvalibility()
		{
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
	}
}