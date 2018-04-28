using System;
using System.Collections.Generic;
using KerbalFuture.Utils;
using KerbalFuture.UnitTests;

namespace KerbalFuture.Superluminal.SpaceFolder
{
	[KSPAddon(KSPAddon.Startup.Flight, true)]
	public class SpaceFolderDriveVesselModule : VesselModule
	{
		// 1 Electric Charge is equal to 1kJ\!
		
		// List of of drives on the vessel, updated/created on warp
		List<Part> driveList = new List<Part>();
		// Dictionary of the part and its respective participation in the warp
		Dictionary<Part, double> partECAmount = new Dictionary<Part, double>();
        // Warps the vessel, using resources
        public void Awake()
        {
            UnitTests.UnitTests.vesselModule = this;
        }
        internal bool WarpVessel(Vector3d location)
        {
            vessel.SetPosition(location);
            return true;
        }
        public bool WarpVessel(SpaceFolderWarpData warpData, out int fault)
		{
			driveList = WarpHelp.PartsWithModule(Vessel, new ModuleSpaceFolderEngine());
			int internFault = fault = SpaceFolderWarpChecks.WarpAvailable(warpData, Vessel);
			if (internFault != 0)
			{
				return false;
			}
			UseWarpResources();
			vessel.SetPosition(warpData.WarpLocation);
			DistributeHeat();
			return true;
		}
		// Uses resources from the drives in driveList
		private void UseWarpResources()
		{
			List<SpaceFolderDriveData> driveData = new List<SpaceFolderDriveData>();
			foreach (Part p in driveList)
			{
				driveData.Add(((ModuleSpaceFolderEngine)p.Modules["ModuleSpaceFolderEngine"]).PartDriveData);
			}
			foreach (SpaceFolderDriveData d in driveData)
			{
				double tempEC = MainResourceWarpCalc(d.Diameter, d.Multiplier);
				partECAmount.Add(d.DriveDataPart, tempEC);
				WarpHelp.UseResource(d.DriveDataPart, tempEC, d.MainResource);
			}
		}
		// Heat distribution for after warp, using ModuleCoreHeat put in place by a MM patch
		private void DistributeHeat()
		{
			// Adds heat
			foreach (KeyValuePair<Part, double> kvp in partECAmount)
			{
				bool hadModuleCoreHeat = true;
				// Iff MM doesn't work or this engine for whatever reason doesn't have a ModuleCoreHeat, we add one
				if (!kvp.Key.Modules.Contains("ModuleCoreHeat"))
				{
					hadModuleCoreHeat = false;
					kvp.Key.AddModule("ModuleCoreHeat", true);
				}
				// p.Modules.GetModule<ModuleSpaceFolderEngine>.PartDriveData
				((ModuleCoreHeat)kvp.Key.Modules["ModuleCoreHeat"]).AddEnergyToCore(kvp.Value);
				// Removes ModuleCoreHeat if the part didn't already have it
				if (hadModuleCoreHeat)
				{
					kvp.Key.RemoveModule(kvp.Key.Modules["ModuleCoreHeat"]);
				}
			}
		}
        //Calculates the amount of main resource used
        private double MainResourceWarpCalc(double diameter, double multiplier)
	        => Math.Pow(Math.E, diameter * multiplier / 5)* 300;
    }
}