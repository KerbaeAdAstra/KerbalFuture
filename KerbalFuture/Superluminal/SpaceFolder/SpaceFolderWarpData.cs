using System;
using KerbalFuture.Utils;

namespace KerbalFuture.Superluminal.SpaceFolder
{
	public struct SpaceFolderWarpData
	{
		public SpaceFolderWarpData(Vessel vesselToWarp,
			Vector3d currentLocation,
			Vector3d warpLocation,
			double timeToWarp,
			CelestialBody currentCelestialBody,
			CelestialBody warpCelestialBody)
		{
			_VesselToWarp = vesselToWarp;
			_CurrentLocation = currentLocation;
			_WarpLocation = warpLocation;
			_TimeToWarp = timeToWarp;
			_CurrentCelestialBody = currentCelestialBody;
			_WarpCelestialBody = warpCelestialBody;
		}
		private Vessel _VesselToWarp;
		private Vector3d _CurrentLocation;
		private Vector3d _WarpLocation;
		private double _TimeToWarp;
		private CelestialBody _CurrentCelestialBody;
		private CelestialBody _WarpCelestialBody;
		
		public Vessel VesselToWarp
		{
			get
			{
				return _VesselToWarp;
			}
		}
		public Vector3d CurrentLocation
		{
			get
			{
				return _CurrentLocation;
			}
		}
		public Vector3d WarpLocation
		{
			get
			{
				return _WarpLocation;
			}
		}
		public double TimeToWarp
		{
			get
			{
				return _TimeToWarp;
			}
		}
		public CelestialBody CurrentCelestialBody
		{
			get
			{
                return _CurrentCelestialBody;
			}
		}
		public CelestialBody WarpCelestialBody
		{
			get
			{
				return _WarpCelestialBody;
			}
		}
		public double WarpDistance
		{
			get
			{
				return WarpHelp.Distance(CurrentLocation, WarpLocation);
			}
		}
		public double WarpLocationXValue
		{
			get
			{
				return _WarpLocation.x;
			}
		}
		public double WarpLocationYValue
		{
			get
			{
				return _WarpLocation.y;
			}
		}
		public double WarpLocationZValue
		{
			get
			{
				return _WarpLocation.z;
			}
		}
	}
}