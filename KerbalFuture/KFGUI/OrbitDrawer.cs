using System.Collections;
using UnityEngine;

namespace KerbalFuture.KFGUI
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class OrbitDrawer : MonoBehaviour
	{
		#region Singleton
		private static OrbitDrawer instance = null;
		public static OrbitDrawer Instance
		{
			get
			{
				if(instance == null)
				{
					return new OrbitDrawer();
				}
				else
				{
					return instance;
				}
			}
		}
		private OrbitDrawer()
		{

		}
		#endregion
		public bool IsReady { get; private set; } = false;
		Vessel sfdWarpLineVessel;
		Orbit sfdWarpLineOrbit;
		IEnumerator spawnCoroutine;
		void Start()
		{
			IsReady = true;
			instance = this;
		}
		internal void RefreshFields()
		{
			sfdWarpLineOrbit = null;
			sfdWarpLineVessel = null;
		}
		internal void DestroyVessel()
		{
			if(sfdWarpLineVessel != null)
			{
				sfdWarpLineVessel.Die();
			}
		}
		internal void UpdateSFDWarpOrbit(Vector3d warpPos, Vector3d currVel, CelestialBody warpBody)
		{
			if(sfdWarpLineOrbit == null)
			{
				sfdWarpLineOrbit = new Orbit();
			}
			sfdWarpLineOrbit.UpdateFromStateVectors(warpPos, currVel, warpBody, Planetarium.GetUniversalTime());
		}
		//Thanks to @wasml on the KSPF for this nice bit of code, posted in https://forum.kerbalspaceprogram.com/index.php?/topic/164792-how-to-create-a-vessel/
		public void DrawSFDWarpLineWithVessel(Vector3d pos, Vector3d vel, CelestialBody cb)
		{
			uint flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
			ConfigNode[] partNodes = new ConfigNode[1];
			partNodes[0] = ProtoVessel.CreatePartNode("PotatoRoid", flightID, null); //Changed part
			ConfigNode[] additionalNodes = new ConfigNode[0];
			UpdateSFDWarpOrbit(pos, vel, cb);
			ConfigNode protoVesselNode = ProtoVessel.CreateVesselNode("SFD Warp Orbit", VesselType.Unknown, sfdWarpLineOrbit, 0, partNodes, additionalNodes);
			double lat, lon, alt;
			cb.GetLatLonAltOrbital(pos, out lat, out lon, out alt);
			protoVesselNode﻿﻿.SetValue﻿("lon﻿﻿﻿", lon);
			protoVesselNode.SetValue("lat", lat);
			protoVesselNode.SetValue("alt", alt);

			ProtoVessel protoVessel = HighLogic.CurrentGame.AddVessel(protoVesselNode);

			sfdWarpLineVessel = protoVessel.vesselRef;
			if (sfdWarpLineVessel != null)
			{
				sfdWarpLineVessel.Load();
				spawnCoroutine = Spawn(pos, vel, cb);
				StartCoroutine(spawnCoroutine);
			}
		}
		IEnumerator Spawn(Vector3d pos, Vector3d vel, CelestialBody cb)
		{
			if ((sfdWarpLineVessel.Parts[0] != null) && (sfdWarpLineVessel.Parts[0].Rigidbody != null))
			{
				sfdWarpLineVessel.Parts[0].Rigidbody.isKinematic = true;
			}

			while ((sfdWarpLineVessel.packed) && (sfdWarpLineVessel.acceleration.magnitude == 0))
			{
				sfdWarpLineVessel.SetPosition(pos);
				yield return null;
			}
			sfdWarpLineVessel.SetWorldVelocity(vel);

			sfdWarpLineVessel.GoOffRails();
			sfdWarpLineVessel.IgnoreGForces(250);
		}
	}
}