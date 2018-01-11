namespace Hlpr
{
	class Vector3dHelper
	{
		double v3dX, v3dY, v3dZ;

		public static void ConvertVector3dToXYZCoords(Vector3d v3d, ref double x, 
		    ref double y, ref double z)
		    {
		    	x = v3d.x;
		    	y = v3d.y;
		    	z = v3d.z;
	        	}
		public static Vector3d ConvertXYZCoordsToVector3d(double x, double y,
			double z)
	    	=> new Vector3d(x, y, z);
		public static void ConvertXYZCoordsToVector3d(double x, double y,
			double z, ref Vector3d v3d)
	    	=> v3d = new Vector3d(x, y, z);
		public double Vector3dX() => v3dX;
		public double Vector3dY() => v3dY;
		public double Vector3dZ() => v3dZ;
		public void SetX(double x) {v3dX = x;}
		public void SetY(double y) {v3dY = y;}
		public void SetZ(double z) {v3dZ = z;}
	}
}