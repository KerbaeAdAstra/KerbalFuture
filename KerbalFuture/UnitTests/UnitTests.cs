using System;
using KerbalFuture.Superluminal.SpaceFolder;
using KerbalFuture.Utils;
using UnityEngine;

namespace KerbalFuture.UnitTests
{
    [KSPAddon(KSPAddon.Startup.Flight, false)] //starts every time player enters flight scene
    class UnitTests : MonoBehaviour
    {
        public static SpaceFolderDriveVesselModule vesselModule;
        public void Update()
        {
            if(Input.GetKey(KeyCode.U) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
            {
                Debug.Log("[KF] Unit tests triggered");
                vesselModule.WarpVessel(new Vector3d(40000, 2100000, 500000000));
            }
        }
    }
}