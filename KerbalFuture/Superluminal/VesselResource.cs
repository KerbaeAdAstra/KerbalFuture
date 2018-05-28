namespace KerbalFuture.Superluminal
{
	public struct VesselResource
	{
		public VesselResource(string res, double amt)
		{
			resource = res;
			amount = amt;
		}
		public string resource;
		public double amount;
	}
}