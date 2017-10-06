using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;
using KerbalFuture;

namespace KerbalFuture
{
	class Vector3dHelper : MonoBehaviour
	{
		private double v3dX, v3dY, v3dZ;
		
		public static void ConvertVector3dToXYZCoords(Vector3d v3d)
		{
			v3dX = v3d.x;
			v3dY = v3d.y;
			v3dZ = v3d.z;
		}
		public static Vector3d ConvertXYZCoordsToVector3d(double x, double y, double z)
		{
			return Vector3d.Vector3d(x, y, z);
		}
		public double Vector3dX()
		{
			return v3dX;
		}
		public double Vector3dY()
		{
			return v3dY;
		}
		public double Vector3dZ()
		{
			return v3dZ;
		}
		public double SetX(double x) {v3dX = x;}
		public double SetY(double y) {v3dY = y;}
		public double SetZ(double z) {v3dZ = z;}
	}
}