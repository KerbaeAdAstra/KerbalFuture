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
			VesselToWarp = vesselToWarp;
			CurrentLocation = currentLocation;
			WarpLocation = warpLocation;
			TimeToWarp = timeToWarp;
			CurrentCelestialBody = currentCelestialBody;
			WarpCelestialBody = warpCelestialBody;
		}

		public Vessel VesselToWarp { get; }
		public Vector3d CurrentLocation { get; }
		public Vector3d WarpLocation { get; }
		public double TimeToWarp { get; }
		public CelestialBody CurrentCelestialBody { get; }
		public CelestialBody WarpCelestialBody { get; }

		public double WarpDistance => WarpHelp.Distance(CurrentLocation, WarpLocation);
		public double WarpLocationXValue => WarpLocation.x;
		public double WarpLocationYValue => WarpLocation.y;
		public double WarpLocationZValue => WarpLocation.z;
	}
}