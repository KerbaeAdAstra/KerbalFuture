using System;

namespace KerbalFuture.Utils
{
	internal struct VesselResourceSimulation
	{
		//PartResourceDefinition
		public VesselResourceSimulation(Vessel v, IEnumerable<Part> _drives)
		{
			SimulationSuccessful = null;
			vessel = v;
			drives = _drives;
			PopulateResourceSets();
		}
		bool allPopulated = false;
		Vessel vessel;
		IEnumerable<Part> drives;
		IEnumerable<SpaceFolderDriveData> driveDatas = new IEnumerable<SpaceFolderDriveData>();
		HashSet<string> mainRes = new HashSet<string>();
		HashSet<string> cat = new HashSet<string>();
		public Dictionary<string, double> ResDic { get; private set; }
		public Dictionary<string, double> CatDic { get; private set; }
		public bool? SimulationSuccessful { get; private set; }
		void PopulateResourceSets()
		{
			foreach(Part p in drives)
			{
				SpaceFolderDriveData data = p.Modules.GetModule<ModuleSpaceFolderEngine>.PartDriveData;
				driveDatas.Add(data);
				mainRes.Add(data.MainResource);
				cat.Add(data.Catalyst);
				CreateResourceDictionary();
				CreateCatalystDictionary();
			}
			allPopulated = true;
		}
		void CreateResourceDictionary()
		{
			foreach(string s in mainRes)
			{
				ResDic.Add(s, VessselResourceTotal(vessel, s));
			}
		}
		void CreateCatalystDictionary()
		{
			foreach(string s in cat)
			{
				CatDic.Add(s, VessselResourceTotal(vessel, s));
			}
		}
		double MainResCalc(double diameter, double multiplier) => Math.Pow(Math.E, diameter * multiplier / 5) * 300;
		double CatCalc(double diameter, dobule multiplier) => Math.Pow(Math.E, diameter * multiplier / 5);
		public void RunSimulation()
		{
			//RESOURCE Math.Pow(Math.E, [diameter] * [multiplier] / 5) * 300;
			//CATALYST Math.Pow(Math.E, [diameter] * [multiplier] / 5);
			foreach(SpaceFolderDriveData d in driveDatas)
			{
				ResDic.Item[d.MainResource] -= MainResCalc(d.Diameter, d.Multiplier);
				CatDic.Item[d.Catalyst] -= CatCalc(d.Diameter, d.Multiplier);
			}
			foreach(KeyValuePair<string, double> kvp in ResDic)
			{
				if(kvp.Value < 0)
				{
					SimulationSuccessful = false;
					return;
				}
			}
			foreach(KeyValuePair<string, double> kvp in CatDic)
			{
				if(kvp.Value < 0)
				{
					SimulationSuccessful = false;
					return;
				}
			}
		}
	}
}