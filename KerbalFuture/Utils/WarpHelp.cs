using System;

namespace KerbalFuture.Utils
{
	internal class WarpHelp
	{
		public static double Distance(double x1, double y1, double z1, double x2, double y2, double z2)
		{
			double dis = Math.Pow(x1-x2, 2) + Math.Pow(y1-y2, 2) + Math.Pow(z1-z2, 2);
			dis = Math.Pow(dis, 0.5);
			return dis;
		}
		public static void UseElectricity(Part part, double amount, string resource)
		{
			part.RequestResource(resource, amount);
		}
		public static double Distance(Vector3d v1, Vector3d v2)
		{
			double x1, x2, y1, y2, z1, z2;
			x1 = x2 = y1 = y2 = z1 = z2 = 0; //temp values to allow for ref usage below
			Vector3dHelper.ConvertVector3dToXYZCoords(v1, ref x1, ref y1, ref z1);
			Vector3dHelper.ConvertVector3dToXYZCoords(v2, ref x2, ref y2, ref z2);
			double dis = Math.Pow(x1-x2, 2) + Math.Pow(y1-y2, 2) + Math.Pow(z1-z2, 2);//pythag!
			dis = Math.Pow(dis, 0.5);
			return dis;
		}
		public static double Distance(Vector3d v1, double x2, double y2, double z2)
		{
			double x1, y1, z1;
			x1 = y1 = z1 = 0;//temp values, described above
			Vector3dHelper.ConvertVector3dToXYZCoords(v1, ref x1, ref y1, ref z1);
			double dis = Math.Pow(x1-x2, 2) + Math.Pow(y1-y2, 2) + Math.Pow(z1-z2, 2);
			dis = Math.Pow(dis, 0.5);
			return dis;
		}
	}
}