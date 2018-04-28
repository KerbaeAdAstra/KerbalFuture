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
            vesselModule = vm;
        }
        public SpaceFolderDriveVesselModule vesselModule;
        public void Update()
        {
            if(Input.GetKey(KeyCode.U) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && vesselModule != null)
            {
                Debug.Log("[KF] Unit test 'Warp' triggered");
                vesselModule.WarpVessel(new Vector3d(40000, 2100000, 500000000));
            }
            if(Input.GetKey(KeyCode.O) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
            {
                Debug.Log("[KF] Unit test 'WarpChecks' triggered");
            }
        }
    }
}