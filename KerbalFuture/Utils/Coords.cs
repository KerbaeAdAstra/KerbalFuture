using System;

namespace KerbalFuture.Utils
{
	public struct Coords
	{
		public Coords(double _lat, double _lon, double _alt, CelestialBody _cb)
		{
			Lat = _lat;
			Lon = _lon;
			Alt = _alt;
			Body = _cb;
		}
		public double Lat { get; }
		public double Lon { get; }
		public double Alt { get; }
		public CelestialBody Body { get; }
		public double XLoc => Body.GetWorldSurfacePosition(Lat, Lon, Alt).x;
		public double YLoc => Body.GetWorldSurfacePosition(Lat, Lon, Alt).y;
		public double ZLoc => Body.GetWorldSurfacePosition(Lat, Lon, Alt).z;
		public Vector3d WorldSpace => Body.GetWorldSurfacePosition(Lat, Lon, Alt);
	}
}