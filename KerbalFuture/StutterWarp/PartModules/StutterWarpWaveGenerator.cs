namespace StutterWarp
{
	class WaveGenerator : PartModule
	{
		[KSPField(isPersistant = true, guiActive = false)]
		public float spatiofibrinNeeded;
		[KSPField(isPersistant = true, guiActive = false)]
		public float maxWavelength;
		[KSPField(isPersistant = true, guiActive = false)]
		public float minWavelength;
		[KSPField(isPersistant = true, guiActive = false)]
		public float maxAmplitude;
		[KSPField(isPersistant = true, guiActive = false)]
		public float minAmplitude;
		[KSPField(isPersistant = true, guiActive = false)]
		public float efficiency; //between 0 and 1
		[KSPField(isPersistant = true, guiActive = false)]
		public float interference; //greater than 1
		
		public double[] ValueList()
		{
			double[] array = new double[8] {(double)this.spatiofibrinNeeded, 
				(double)this.electricityNeeded, 
				(double)this.maxWavelength, 
				(double)this.minWavelength, 
				(double)this.maxAmplitude, 
				(double)this.minAmplitude,
				(double)this.efficiency,
				(double)this.interference};
			return array;
		}
	}
}