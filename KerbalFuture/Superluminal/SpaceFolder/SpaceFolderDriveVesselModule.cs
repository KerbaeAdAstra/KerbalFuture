using KerbalFuture.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KerbalFuture.Superluminal.SpaceFolder
{
	public class SpaceFolderDriveVesselModule : VesselModule
    {
		internal static double sfConstReactantUsage = 0.0;
		public static double SPACEFOLDER_CONSTANT_OF_REACTANT_USAGE
		{
			get
			{
				return sfConstReactantUsage;
			}
			internal set
			{
				sfConstReactantUsage = value;
			}
		}
		internal static SpaceFolderDriveVesselModule CurrentVesselModule { get; private set; }
		// 1 Electric Charge is equal to 1kJ\!

		// Dictionary of the part and its respective participation in the warp
		// Created by UseWarpResources for DistributeHeat
		Dictionary<Part, double> partECAmount = new Dictionary<Part, double>();

		//Fires on Vessel startup
		protected override void OnStart()
		{
			base.OnStart();
			Debug.Log("[KF] SFD Vessel module starting for " + Vessel.GetDisplayName());
		}
		private void Update()
		{
			if(Vessel == FlightGlobals.ActiveVessel && CurrentVesselModule != this)
			{
				CurrentVesselModule = this;
			}
			if (Input.GetKey(KeyCode.U) && (Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt)) && KFGUI.ConstantEditWindow.advancedMode)
			{
				Debug.Log("[KF] " + Vessel.mainBody.GetDisplayName() + "'s gravitational parameter is " + Vessel.mainBody.gravParameter);
			}
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
            Vessel.SetPosition(new Coords(warpData.WarpLocation.Lat, warpData.WarpLocation.Lon, WarpHelp.GravPotAltitude(Vessel, warpData.WarpCelestialBody) - warpData.WarpCelestialBody.Radius, warpData.WarpLocation.Body).WorldSpace);
            DistributeHeat();
			//update everything I guess?
			Vessel.UpdateCaches();
			Vessel.UpdateLandedSplashed();
			Vessel.UpdatePosVel();
			Vessel.UpdateDistanceTraveled();
            return true;
        }
		void UseWarpResources()
		{
			Debug.Log("[KF] Using resources for warp");
			partECAmount.Clear();
			double diam = WarpHelp.VesselDiameterCalc(vessel);
			Debug.Log("[KF] Vessel diameter is " + diam);
			List<Part> drives = SFWarpHelp.PartsWithModuleSFD(vessel);
			Debug.Log("[KF] Found " + drives.Count + "  drives in " + vessel.GetDisplayName());
			// Dictionary of the actual diameter that the part is putting out
			Dictionary<SpaceFolderDriveData, double> partRelDiamDict = new Dictionary<SpaceFolderDriveData, double>();
			double percentCalc = diam / SpaceFolderWarpChecks.MaxWarpHoleSize(drives);
			Debug.Log("[KF] percentCalc is " + percentCalc);
			List<SpaceFolderDriveData> driveDatas = new List<SpaceFolderDriveData>(SFWarpHelp.DriveDataList(Vessel));
			foreach (SpaceFolderDriveData dd in driveDatas)
			{
				partRelDiamDict.Add(dd, dd.Diameter * percentCalc);
			}
			Debug.Log("[KF] There are " + partRelDiamDict.Count + " values in partRelDiamDict");
			foreach (KeyValuePair<SpaceFolderDriveData, double> kvp in partRelDiamDict)
			{
				// Creating the partECAmount dictionary for heat distribution in kJ
				partECAmount.Add(kvp.Key.DriveDataPart, MainResourceWarpCalc(kvp.Value, kvp.Key.Multiplier));
				//using the resources
				Debug.Log("[KF] Using " + MainResourceWarpCalc(kvp.Value, kvp.Key.Multiplier) + " " + kvp.Key.MainResource);
				WarpHelp.UseResource(kvp.Key.DriveDataPart, MainResourceWarpCalc(kvp.Value, kvp.Key.Multiplier), kvp.Key.MainResource);
			}
		}
        // Heat distribution for after warp, using ModuleCoreHeat hopefully put in place by a MM patch
		// otherwise adds it in the function
        void DistributeHeat()
        {
            Debug.Log("[KF] Distributing heat to vessel " + Vessel.name);
            // Adds heat
            foreach (KeyValuePair<Part, double> kvp in partECAmount)
            {
                // Iff MM doesn't work or this engine for whatever reason doesn't have a ModuleCoreHeat, we add one
				// Don't bother removing it, cause we're going to keep using it later
                if (!kvp.Key.Modules.Contains("ModuleCoreHeat"))
                {
                    Debug.Log("[KF] Part " + kvp.Key.name + " does not contain ModuleCoreHeat. Adding");
                    kvp.Key.AddModule("ModuleCoreHeat", true);
                }
				// p.Modules.GetModule<ModuleSpaceFolderDrive>.PartDriveData
				Debug.Log("[KF] Adding " + kvp.Value + "kJ of heat to " + kvp.Key.name + ". Final part temperature is " + (kvp.Key.temperature + kvp.Value).ToString() + "C");
                ((ModuleCoreHeat)kvp.Key.Modules["ModuleCoreHeat"]).AddEnergyToCore(kvp.Value);
            }
        }
		//Calculates the amount of main resource used in kJ
		public static double MainResourceWarpCalc(double diameter, double multiplier)
            => Math.Pow(Math.E, diameter * multiplier / 5) * 300 / 1000 * 30000;
		public static double CatalystWarpCalc(double diameter, double multiplier)
			=> Math.Pow(Math.E, diameter * multiplier / 5) / 1000;
    }
}
