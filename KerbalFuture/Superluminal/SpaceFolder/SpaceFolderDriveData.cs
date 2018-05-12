namespace KerbalFuture.Superluminal.SpaceFolder
{
	public struct SpaceFolderDriveData
	{
        // Constructor
		public SpaceFolderDriveData(Part part, double diameter, double multiplier, string mainRes, string cat)
		{
			DriveDataPart = part;
			Diameter = diameter;
			Multiplier = multiplier;
			MainResource = mainRes;
			Catalyst = cat;
			IsNull = false;
		}
		public SpaceFolderDriveData(bool isNull)
		{
			DriveDataPart = new Part();
			Diameter = double.NaN;
			Multiplier = double.NaN;
			MainResource = string.Empty;
			Catalyst = string.Empty;
			IsNull = true;
		}
		
        // Properties of struct, readonly
		public Part DriveDataPart { get; }
		public double Diameter { get; }
		public double Multiplier { get; }
		public string MainResource { get; }
		public string Catalyst { get; }
		public bool IsNull { get; }
	}
}