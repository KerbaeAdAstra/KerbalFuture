using System;

namespace Fuck
{
	class LatLongHelper
	{
		public double XFromLatLongAlt(double _lat, double _long, double _alt)
        => _alt * Math.Cos(_lat) * Math.Cos(_long);

		public double YFromLatLongAlt(double _lat, double _long, double _alt)
        => _alt * Math.Cos(_lat) * Math.Sin(_long);

		public double ZFromLatAlt(double _lat, double _alt)
        => _alt * Math.Cos(_lat);
	}
}
