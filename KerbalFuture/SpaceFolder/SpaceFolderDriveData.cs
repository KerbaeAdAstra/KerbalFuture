namespace KerbalFuture.Superluminal.SpaceFolder
{
	public struct SpaceFolderDriveData
	{
		public SpaceFolderDriveData(Part part, double diameter, double multiplier, string mainRes, string cat)
		{
			DriveDataPart = part;
			Diameter = diameter;
			Multiplier = multiplier;
			MainResource = mainRes;
			Catalyst = cat;
		}
		public Part DriveDataPart { get; }
		public double Diameter { get; }
		public double Multiplier { get; }
		public string MainResource { get; }
		public string Catalyst { get; }
	}
}