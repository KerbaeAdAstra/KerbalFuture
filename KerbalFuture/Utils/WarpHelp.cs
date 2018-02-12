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
		// FIXME
		/*
		public static double Distance(Vector3d v1, Vector3d v2)
		{
			double x1, x2, y1, y2, z1, z2;
			Vector3dHelper.ConvertVector3dToXYZCoords(v1, ref x1, ref y1, ref z1);
			Vector3dHelper.ConvertVector3dToXYZCoords(v2, ref x2, ref y2, ref z2);
			double dis = Math.Pow(x1-x2, 2) + Math.Pow(y1-y2, 2) + Math.Pow(z1-z2, 2);
			dis = Math.Pow(dis, 0.5);
			return dis;
		}
		public static double Distance(Vector3d v1, double x2, double y2, double z2)
		{
			double x1, y1, z1;
			Vector3dHelper.ConvertVector3dToXYZCoords(v1, ref x1, ref y1, ref z1);
			double dis = Math.Pow(x1-x2, 2) + Math.Pow(y1-y2, 2) + Math.Pow(z1-z2, 2);
			dis = Math.Pow(dis, 0.5);
			return dis;
		}
		*/
	}
}