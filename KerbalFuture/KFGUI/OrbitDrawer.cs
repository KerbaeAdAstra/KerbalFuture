using System.Collections;
using UnityEngine;
using KerbalFuture.Superluminal.SpaceFolder;
using System;

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
		public static bool IsReady { get; private set; } = false;
		public bool DrawLine { get; internal set; } = false;

		public void SetDrawnOrbit(Orbit o, CelestialBody cb)
		{

		}
		public void SetDrawOrbit(Vector3d vel, Vector3d pos, Vector3d cb)
		{
		}
		void Awake()
		{
			IsReady = true;
		}
		private void Update()
		{
			/*
			if(!DrawLine)
			{
				return;
			}
			*/
		}
	}
}