using KerbalFuture.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KerbalFuture.Superluminal.SpaceFolder
{
    public class SpaceFolderDriveVesselModule : VesselModule
    {
        // 1 Electric Charge is equal to 1kJ\!

        // List of of drives on the vessel, updated/created on warp
        List<Part> driveList = new List<Part>();
        // Dictionary of the part and its respective participation in the warp
        Dictionary<Part, double> partECAmount = new Dictionary<Part, double>();
        protected override void OnStart()
        {
            base.OnStart();
            Debug.Log("[KF] Vessel module starting for " + Vessel.name);
        }
        public void Update()
        {
            if (Input.GetKey(KeyCode.U) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
            {
                List<Part> partList = WarpHelp.PartsWithModule(Vessel, new ModuleSpaceFolderEngine().GetType());
                foreach (Part p in partList)
                {
                    try
                    {
                        SpaceFolderDriveData data = p.Modules.GetModule<ModuleSpaceFolderEngine>().PartDriveData;
                        Debug.Log("[KF] Ye got parts with SFD's! " + p.ToString() + " " + data.MainResource);
                    }
                    catch(NullReferenceException e)
                    {
                        Debug.Log("[KF] Shucks. NRE. " + e.Source + " " + e.Message + " " + e.InnerException + " " + e.StackTrace + " " + e.HelpLink + " " + e.Data + " " + e.GetBaseException().ToString());
                    }
                }
            }
        }
        //Internal testing code
        internal bool WarpVessel(Vector3d location)
        {
            Debug.Log("[KF] Internal Warp Triggered for vessel " + Vessel.name + "!");
            Vessel.SetPosition(location);
            return true;
        }
        /*
        // Warps the vessel, using resources
        public bool WarpVessel(SpaceFolderWarpData warpData, out int fault)
        {
            Debug.Log("[KF] Warp triggered from an external source for " + Vessel.name);
            driveList = WarpHelp.PartsWithModule(Vessel, new ModuleSpaceFolderEngine());
            int internFault = fault = SpaceFolderWarpChecks.WarpAvailable(warpData, Vessel);
            if (internFault != 0)
            {
                Debug.Log("[KF] Fault discovered in warp checks with code " + internFault);
                return false;
            }
            Debug.Log("[KF] Warp checks successful, warping vessel " + Vessel.name + " now.");
            UseWarpResources();
            Vessel.SetPosition(warpData.WarpLocation);
            DistributeHeat();
            return true;
        }
        // Uses resources from the drives in driveList
        private void UseWarpResources()
        {
            Debug.Log("[KF] Using warp resources");
            List <SpaceFolderDriveData> driveData = new List<SpaceFolderDriveData>();
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
        */
        //Calculates the amount of main resource used
        private double MainResourceWarpCalc(double diameter, double multiplier)
            => Math.Pow(Math.E, diameter * multiplier / 5) * 300;
    }
}
