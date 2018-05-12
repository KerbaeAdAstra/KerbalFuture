using KerbalFuture.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
			Debug.Log("[KF] Vessel module starting for " + Vessel.GetDisplayName());
		}
        // Warps the vessel, using resources
        public bool WarpVessel(SpaceFolderWarpData warpData, out Error fault)
        {
            Debug.Log("[KF] Warp triggered from an external source for " + Vessel.name);
            Error internFault = fault = SpaceFolderWarpChecks.WarpAvailable(Vessel);
            if (internFault != 0)
            {
                Debug.Log("[KF] Fault discovered in warp checks with code " + internFault.ToString());
                return false;
            }
            Debug.Log("[KF] Warp checks successful, warping vessel " + Vessel.GetDisplayName() + " now.");
            UseWarpResources();
            Vessel.SetPosition(warpData.WarpLocation);
            DistributeHeat();
            return true;
        }
		//TODO implement selective drives
		//i.e. smart usage to pick smaller drives that get the hole size closer to the vessel size
		// Uses resources from the drives in driveList
		void UseWarpResources()
		{
			List<SpaceFolderDriveData> pSortedList = new List<SpaceFolderDriveData>();
			Debug.Log("[KF] Using warp resources for vessel " + Vessel.GetDisplayName());
			double vesselDiameter = SpaceFolderWarpChecks.VesselDiameterCalc(
				ShipConstruction.CalculateCraftSize(
					new ShipConstruct(Vessel.name, EditorFacility.VAB, Vessel.Parts)));
			Debug.Log("[KF] Calculated craft diameter as " + vesselDiameter + " meters");
			if (SFWarpHelp.DriveDataList(Vessel).Count > 1)
			{
				List<SpaceFolderDriveData> sortedList = new List<SpaceFolderDriveData>(SFWarpHelp.SortDriveData(SFWarpHelp.DriveDataList(Vessel)));
				List<SpaceFolderDriveData> testList = new List<SpaceFolderDriveData>();
				Debug.Log("[KF] Trying to test additions to testList");
				foreach (SpaceFolderDriveData dd in sortedList)
				{
					testList.Add(dd);
					IEnumerable<Part> ddPartList = from dda in testList
												   select dda.DriveDataPart;
					double size = SpaceFolderWarpChecks.MaxWarpHoleSize(ddPartList);
					if (vesselDiameter < size)
					{
						Debug.Log("[KF] testList's Warp Hole Size is above vessel diameter! Removing last element");
						//testList now contains just under the diameter of the warp bubble needed
						testList.RemoveAt(testList.Count - 1);
						break;
					}
				}
				if (testList.Count + 1 != sortedList.Count)
				{
					Debug.Log("[KF] Warp does not need all drives avalible");
					sortedList.Reverse();
					foreach (SpaceFolderDriveData dd in sortedList)
					{
						testList.Add(dd);
						IEnumerable<Part> ddPartList = from dda in testList
													   select dda.DriveDataPart;
						double size = SpaceFolderWarpChecks.MaxWarpHoleSize(ddPartList);
						if (vesselDiameter < size)
						{
							//breaks when it reaches a small enough drive
							pSortedList.Clear();
							pSortedList.AddRange(testList);
							break;
						}
					}
				}
				else
				{
					pSortedList.Clear();
					pSortedList.AddRange(sortedList);
				}
			}
			else if (SFWarpHelp.DriveDataList(Vessel).Count == 1)
			{
				Debug.Log("[KF] One drive on vessel");
				pSortedList.Clear();
				pSortedList.Add(SFWarpHelp.DriveDataList(Vessel)[0]);
			}
			else
			{
				Debug.Log("[KF] No drives on vessel, aborting");
				return;
			}
			foreach (SpaceFolderDriveData dd in pSortedList)
			{
				Debug.Log("[KF] Using warp resources for " + dd.DriveDataPart.persistentId);
				double tempEC = MainResourceWarpCalc(dd.Diameter, dd.Multiplier);
				partECAmount.Clear();
				partECAmount.Add(dd.DriveDataPart, tempEC);
				WarpHelp.UseResource(dd.DriveDataPart, tempEC, dd.MainResource);
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
		//Calculates the amount of main resource used
		double MainResourceWarpCalc(double diameter, double multiplier)
            => Math.Pow(Math.E, diameter * multiplier / 5) * 300;
    }
}
