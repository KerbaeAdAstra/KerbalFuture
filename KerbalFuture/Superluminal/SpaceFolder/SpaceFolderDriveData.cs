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
		public Part DriveDataPart { get; internal set; }
		public double Diameter { get; internal set; }
		public double Multiplier { get; internal set; }
		public string MainResource { get; internal set; }
		public string Catalyst { get; internal set; }
	}
}