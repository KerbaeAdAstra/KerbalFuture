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
            instance++;
            Debug.Log("[KF] UnitTest created from vessel " + vm.Vessel.name.ToString() + ", instance " + instance);
            vesselModule = vm;
        }
        public void Awake()
        {
            Debug.Log("[KF] UnitTests awakened, instance is " + instance);
        }
        public void Start()
        {
            Debug.Log("[KF] UnitTests started, instance is " + instance);
            t = Time.realtimeSinceStartup;
        }
        public static uint instance = 0;
        public float t;
        bool alreadywarped = false;
        public SpaceFolderDriveVesselModule vesselModule;
        public void Update()
        {
            if(t + 1000 < Time.realtimeSinceStartup && !alreadywarped)
            {
                alreadywarped = true;
                Debug.Log("[KF] UnitTest " + this.ToString() + " 'Warp' triggered, instance is " + instance);
                vesselModule.WarpVessel(new Vector3d(40000, 2100000, 500000000));
            }
        }
    }
}