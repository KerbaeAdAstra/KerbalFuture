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
		public double XLoc
		{
			get
			{
				return Alt*Math.Sin(Lat)*Math.Cos(Lon);
			}
		}
		public double YLoc
		{
			get
			{
				return Alt*Math.Sin(Lat)*Math.Sin(Lon);
			}
		}
		public double ZLoc
		{
			get
			{
				return Alt*Math.Cos(Lat);
			}
		}
	}
}