namespace StutterWarp
{
	class Weaver : PartModule
	{
		[KSPField(isPersistant = true, guiActive = false)]
		public float electricityConsumption;
		[KSPField(isPersistant = true, guiActive = false)]
		public float segfaultMultiplier;
		
		public float[] ValueList()
		{
			float[] array = new float[2] {this.electricityConsumption, this.segfaultMultiplier};
			return array;
		}
	}
}