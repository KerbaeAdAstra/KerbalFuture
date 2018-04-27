using System;
using System.Collections.Generic;

namespace KerbalFuture.Utils
{
	public class WarpHelp
	{
        public static void ConvertVector3dToXYZCoords(Vector3d v3d, ref double x, ref double y, ref double z)
        {
            x = v3d.x;
            y = v3d.y;
            z = v3d.z;
        }
        public static Vector3d ConvertXYZCoordsToVector3d(double x, double y, double z) => new Vector3d(x, y, z);
        public static void ConvertXYZCoordsToVector3d(double x, double y, double z, ref Vector3d v3d)
            => v3d = new Vector3d(x, y, z);
        public static double Distance(double x1, double y1, double z1, double x2, double y2, double z2)
			=> Math.Pow(Math.Pow(x1-x2, 2) + Math.Pow(y1-y2, 2) + Math.Pow(z1-z2, 2), 0.5);	
		public static double Distance(Vector3d v1, Vector3d v2)
		{
			double x2, y1, y2, z1, z2;
			double x1 = x2 = y1 = y2 = z1 = z2 = 0;
			ConvertVector3dToXYZCoords(v1, ref x1, ref y1, ref z1);
			ConvertVector3dToXYZCoords(v2, ref x2, ref y2, ref z2);
			return Math.Pow(Math.Pow(x1-x2, 2) + Math.Pow(y1-y2, 2) + Math.Pow(z1-z2, 2), 0.5);
		}
		public static double Distance(Vector3d v1, double x2, double y2, double z2)
		{
			double y1, z1;
			double x1 = y1 = z1 = 0;
			ConvertVector3dToXYZCoords(v1, ref x1, ref y1, ref z1);
			return Math.Pow(Math.Pow(x1-x2, 2) + Math.Pow(y1-y2, 2) + Math.Pow(z1-z2, 2), 0.5);
		}
		public static void UseResource(Part part, double amount, string resource)
			=> part.RequestResource(resource, amount);
		public static double ResourceAmountOnVessel(string resource, Vessel v)
	    	=> FinePrint.Utilities.VesselUtilities.VesselResourceAmount(resource, v);
		public static double GetVesselAltitude(bool includePlanetRadius, Vessel v)
		{
			if (includePlanetRadius)
			{
				return v.altitude + v.mainBody.Radius;
			}
			return v.altitude;
		}
		public static List<Part> PartsWithModule(Vessel v, PartModule pm)
		{
			List<Part> returnList = new List<Part>();
			foreach(Part p in v.Parts)
			{
				if(p.Modules.Contains(pm.ClassID))
				{
					returnList.Add(p);
				}
			}
			return returnList;
		}
		public static double VessselResourceTotal(Vessel v, string resource)
		{
			double maxAmount = 0;
			v.GetConnectedResourceTotals(new PartResourceDefinition(resource).id, out var amount, out maxAmount, false);
			return amount;
		}
		public static double CalculateGravPot(CelestialBody cb, Vessel v)
			=> cb.gravParameter / GetVesselAltitude(true, v);

	}
}