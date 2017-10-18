using UnityEngine;
using KerbalFuture;

namespace SpaceFolder
{
	class Vector3dHelper : MonoBehaviour
	{
		double v3dX, v3dY, v3dZ;

		public static void ConvertVector3dToXYZCoords(Vector3d v3d, ref double x, ref double y, ref double z)
		{
			x = v3d.x;
			y = v3d.y;
			z = v3d.z;
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
		public void SetX(double x) {v3dX = x;}
		public void SetY(double y) {v3dY = y;}
		public void SetZ(double z) {v3dZ = z;}
	}
}
