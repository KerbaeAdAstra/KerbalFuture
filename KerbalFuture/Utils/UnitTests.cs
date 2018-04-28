using System;
using KerbalFuture.Superluminal.SpaceFolder;
using UnityEngine;

namespace KerbalFuture.Utils
{
    [KSPAddon(KSPAddon.Startup.Flight, false)] //starts every time player enters flight scene
    class UnitTests : MonoBehaviour
    {
        public UnitTests(SpaceFolderDriveVesselModule vm)
        {
            Debug.Log("[KF] UnitTest created from vessel " + vm.Vessel.name.ToString());
            vesselModule = vm;
        }
        public SpaceFolderDriveVesselModule vesselModule;
        public void Update()
        {
            Debug.Log("[KF] UnitTest " + this.ToString() + " updating");
            if(Input.GetKey(KeyCode.U) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && vesselModule != null)
            {
                Debug.Log("[KF] UnitTest " + this.ToString() + " 'Warp' triggered");
                vesselModule.WarpVessel(new Vector3d(40000, 2100000, 500000000));
            }
        }
    }
}