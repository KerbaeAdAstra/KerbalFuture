namespace StutterWarp
{
	class Weaver : PartModule
	{
		[KSPField(isPersistant = true, guiActive = false)]
		public float electricityConsumption;
		[KSPField(isPersistant = true, guiActive = false)]
		public float segfaultMultiplier;
		
		public double[] ValueList()
		{
			double[] array = new double[2] {(double)this.electricityConsumption, 
				(double)this.segfaultMultiplier};
			return array;
		}
	}
}