using System;

namespace KerbalFuture.Utils
{
	internal class LatLongHelper
	{
		public double XFromLatLongAlt(double _lat, double _long, double _alt)
        => _alt * Math.Cos(_lat) * Math.Cos(_long);

		public double YFromLatLongAlt(double _lat, double _long, double _alt)
        => _alt * Math.Cos(_lat) * Math.Sin(_long);

		public double ZFromLatAlt(double _lat, double _alt)
        => _alt * Math.Cos(_lat);
		
		public double GetVesselAltitude(bool includePlanetRadius, Vessel v)
		{
			if (includePlanetRadius) return v.altitude + v.mainBody.Radius;
			return v.altitude;
		}
	}
}
