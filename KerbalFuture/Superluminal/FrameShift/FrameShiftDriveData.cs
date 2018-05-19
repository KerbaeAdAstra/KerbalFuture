namespace KerbalFuture.Superluminal.FrameShift
{
	public class FrameShiftDriveData
	{
		public FrameShiftDriveData(Part part, double capacity, double efficiency, string mainResource, string catalyst)
		{
			DriveDataPart = part;
			Capacity = capacity;
			Efficiency = efficiency;
			MainResource = mainResource;
			Catalyst = catalyst;
		}
		public Part DriveDataPart { get; }
		public double Capacity { get; }
		public double Efficiency { get; }
		public string MainResource { get; }
		public string Catalyst { get; }
	}
}