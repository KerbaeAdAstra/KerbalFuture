namespace KerbalFuture.Utils
{
	internal class Vector3dHelper
	{
		public static void ConvertVector3dToXYZCoords(Vector3d v3d, ref double x, ref double y, ref double z)
		{
			x = v3d.x;
			y = v3d.y;
			z = v3d.z;
		}
		public static Vector3d ConvertXYZCoordsToVector3d(double x, double y, double z) => new Vector3d(x, y, z);//returning Vector3d, lambda appropriate
		public static void ConvertXYZCoordsToVector3d(double x, double y, double z, ref Vector3d v3d)
		{
			v3d = new Vector3d(x, y, z);//not returning, lambda not used
		}
	}
}