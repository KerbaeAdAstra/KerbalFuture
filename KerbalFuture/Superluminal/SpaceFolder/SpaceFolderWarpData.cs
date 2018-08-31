using KerbalFuture.Utils;

namespace KerbalFuture.Superluminal.SpaceFolder
{
	public struct SpaceFolderWarpData
	{
		public SpaceFolderWarpData(Vessel vesselToWarp,
			BodyCoords warpLocation,
			double timeToWarp)
		{
			VesselToWarp = vesselToWarp;
			CurrentLocation = vesselToWarp.GetWorldPos3D();
			WarpLocation = warpLocation;
			TimeToWarp = timeToWarp;
			CurrentCelestialBody = vesselToWarp.mainBody;
			WarpCelestialBody = warpLocation.Body;
		}

		public Vessel VesselToWarp { get; }
		public Vector3d CurrentLocation { get; }
		public BodyCoords WarpLocation { get; }
		public double TimeToWarp { get; }
		public CelestialBody CurrentCelestialBody { get; }
		public CelestialBody WarpCelestialBody { get; }
	}
}