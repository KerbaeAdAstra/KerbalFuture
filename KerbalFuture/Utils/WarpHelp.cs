using KerbalFuture.Superluminal.SpaceFolder;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KerbalFuture.Utils
{
	public class WarpHelp
	{
        // Converts a Vector3d int XYZ CartCoords, with ref's
        public static void ConvertVector3dToXYZCoords(Vector3d v3d, ref double x, ref double y, ref double z)
        {
            x = v3d.x;
            y = v3d.y;
            z = v3d.z;
        }
        // Returns a Vector3d from XYZ coords
        public static Vector3d ConvertXYZCoordsToVector3d(double x, double y, double z) => new Vector3d(x, y, z);
        // Converts XYZ coords into a Vector3d, with ref's
        public static void ConvertXYZCoordsToVector3d(double x, double y, double z, ref Vector3d v3d)
            => v3d = new Vector3d(x, y, z);
        // Gets the distance between two points (cart)
        public static double Distance(double x1, double y1, double z1, double x2, double y2, double z2)
			=> Math.Pow(Math.Pow(x1-x2, 2) + Math.Pow(y1-y2, 2) + Math.Pow(z1-z2, 2), 0.5);
        // Gets the distance between two points (Vector3d)
		public static double Distance(Vector3d v1, Vector3d v2)
		{
			double x2, y1, y2, z1, z2;
			double x1 = x2 = y1 = y2 = z1 = z2 = 0;
			ConvertVector3dToXYZCoords(v1, ref x1, ref y1, ref z1);
			ConvertVector3dToXYZCoords(v2, ref x2, ref y2, ref z2);
			return Math.Pow(Math.Pow(x1-x2, 2) + Math.Pow(y1-y2, 2) + Math.Pow(z1-z2, 2), 0.5);
		}
        // Gets the distance bewteen two points (Vector3d and XYZ)
		public static double Distance(Vector3d v1, double x2, double y2, double z2)
		{
			double y1, z1;
			double x1 = y1 = z1 = 0;
			ConvertVector3dToXYZCoords(v1, ref x1, ref y1, ref z1);
			return Math.Pow(Math.Pow(x1-x2, 2) + Math.Pow(y1-y2, 2) + Math.Pow(z1-z2, 2), 0.5);
		}
        // Uses a resource on a part
		public static void UseResource(Part part, double amount, string resource)
			=> part.RequestResource(resource, amount);
        // Gets the resource amount on a vessel
		public static double ResourceAmountOnVessel(Vessel v, string resource)
	    	=> FinePrint.Utilities.VesselUtilities.VesselResourceAmount(resource, v);
        // Gets the altitude of the vessel, option to include the planet radius for distance from planet center or core
		public static double GetVesselAltitude(bool includePlanetRadius, Vessel v)
		{
			if (includePlanetRadius)
			{
				return v.altitude + v.mainBody.Radius;
			}
			return v.altitude;
		}
		// Gets the gravitation potential for a vessel above a celestial body
		public static double CalculateGravPot(CelestialBody cb, Vessel v)
			=> cb.gravParameter / GetVesselAltitude(true, v);
	}
}