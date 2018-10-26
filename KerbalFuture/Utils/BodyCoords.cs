namespace KerbalFuture.Utils
{
	public struct BodyCoords
	{
		public BodyCoords(double lat, double lon, CelestialBody cb)
		{
			Lat = lat;
			Lon = lon;
			Body = cb;
		}
		public double Lat;
		public double Lon;
		public CelestialBody Body;
	}
}