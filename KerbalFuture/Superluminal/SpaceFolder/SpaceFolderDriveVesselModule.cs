using KerbalFuture.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KerbalFuture.Superluminal.SpaceFolder
{
	public class SpaceFolderDriveVesselModule : VesselModule
    {
		// 1 Electric Charge is equal to 1kJ\!

		// Dictionary of the part and its respective participation in the warp
		Dictionary<Part, double> partECAmount = new Dictionary<Part, double>();

		//Fires on Vessel startup
		protected override void OnStart()
		{
			base.OnStart();
			Debug.Log("[KF] SFD Vessel module starting for " + Vessel.GetDisplayName());
		}
        // Warps the vessel, using resources
        public bool WarpVessel(SpaceFolderWarpData warpData, out Error fault)
        {
            Debug.Log("[KF] SFD warp triggered from an external source for " + Vessel.name);
            Error internFault = fault = SpaceFolderWarpChecks.WarpAvailable(Vessel);
            if (internFault != 0)
            {
                Debug.Log("[KF] Fault discovered in SFD warp checks with code " + internFault.ToString());
                return false;
            }
            Debug.Log("[KF] SFD warp checks successful, warping vessel " + Vessel.GetDisplayName() + " now.");
            UseWarpResources();
            Vessel.SetPosition(warpData.WarpLocation);
            DistributeHeat();
            return true;
        }
		void UseWarpResources()
		{
			partECAmount.Clear();
			double diam = WarpHelp.VesselDiameterCalc(vessel);
			List<Part> drives = SFWarpHelp.PartsWithModuleSFD(vessel);
			// Dictionary of the actual diameter that the part is putting out
			Dictionary<SpaceFolderDriveData, double> partRelDiamDict = new Dictionary<SpaceFolderDriveData, double>();
			double percentCalc = diam / SpaceFolderWarpChecks.MaxWarpHoleSize(drives);
			List<SpaceFolderDriveData> driveDatas = new List<SpaceFolderDriveData>(SFWarpHelp.DriveDataList(Vessel));
			foreach (SpaceFolderDriveData dd in driveDatas)
			{
				partRelDiamDict.Add(dd, dd.Diameter * percentCalc);
			}
			Debug.Log("[KF] Using resources for warp");
			foreach(KeyValuePair<SpaceFolderDriveData, double> kvp in partRelDiamDict)
			{
				// Creating the partECAmount dictionary for heat distribution in kJ
				partECAmount.Add(kvp.Key.DriveDataPart, MainResourceWarpCalc(kvp.Value, kvp.Key.Multiplier));
				//using the resources
				WarpHelp.UseResource(kvp.Key.DriveDataPart, MainResourceWarpCalc(kvp.Value, kvp.Key.Multiplier), kvp.Key.MainResource);
			}
			/*
			double tempEC = MainResourceWarpCalc(d.Diameter, d.Multiplier);
            partECAmount.Add(d.DriveDataPart, tempEC);
            WarpHelp.UseResource(d.DriveDataPart, tempEC, d.MainResource);
			*/
		}
        // Heat distribution for after warp, using ModuleCoreHeat put in place by a MM patch
        void DistributeHeat()
        {
            Debug.Log("[KF] Distributing heat to vessel " + Vessel.name);
            // Adds heat
            foreach (KeyValuePair<Part, double> kvp in partECAmount)
            {
                bool hadModuleCoreHeat = true;
                // Iff MM doesn't work or this engine for whatever reason doesn't have a ModuleCoreHeat, we add one
                if (!kvp.Key.Modules.Contains("ModuleCoreHeat"))
                {
                    Debug.Log("[KF] Part " + kvp.Key.name + " does not contain ModuleCoreHeat. Adding");
                    hadModuleCoreHeat = false;
                    kvp.Key.AddModule("ModuleCoreHeat", true);
                }
                // p.Modules.GetModule<ModuleSpaceFolderEngine>.PartDriveData
                Debug.Log("[KF] Adding " + kvp.Value + "kJ of heat to " + kvp.Key.name + ". Final part temperature is " + (kvp.Key.temperature + kvp.Value).ToString() + "C");
                ((ModuleCoreHeat)kvp.Key.Modules["ModuleCoreHeat"]).AddEnergyToCore(kvp.Value);
                // Removes ModuleCoreHeat if the part didn't already have it
                if (hadModuleCoreHeat)
                {
                    Debug.Log("[KF] Part " + kvp.Key.name + " did not have ModuleCoreHeat. Removing.");
                    kvp.Key.RemoveModule(kvp.Key.Modules["ModuleCoreHeat"]);
                }
            }
        }
		//Calculates the amount of main resource used in kJ
		double MainResourceWarpCalc(double diameter, double multiplier)
            => Math.Pow(Math.E, diameter * multiplier / 5) * 300 / 1000;
		double CatalystWarpCalc(double diameter, double multiplier)
			=> Math.Pow(Math.E, diameter * multiplier / 5) / 1000;
    }
}
