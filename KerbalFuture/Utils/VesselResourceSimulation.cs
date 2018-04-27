using System;
using System.Collections.Generic;
using KerbalFuture.Superluminal.SpaceFolder;

namespace KerbalFuture.Utils
{
	internal class VesselResourceSimulation
	{
		//PartResourceDefinition
		public VesselResourceSimulation(Vessel v, List<Part> _drives)
		{
			Status = SimulationStatus.Working;
			vessel = v;
			drives = _drives;
			PopulateResourceSets();
		}
		bool allPopulated = false;
		Vessel vessel;
		List<Part> drives;
		List<SpaceFolderDriveData> driveDatas = new List<SpaceFolderDriveData>();
		HashSet<string> mainRes = new HashSet<string>();
		HashSet<string> cat = new HashSet<string>();
		public Dictionary<string, double> ResDic { get; private set; }
		public Dictionary<string, double> CatDic { get; private set; }
		public SimulationStatus Status { get; private set; }
		void PopulateResourceSets()
		{
			foreach(Part p in drives)
			{
				SpaceFolderDriveData data = p.Modules.GetModule<ModuleSpaceFolderEngine>().PartDriveData;
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
				ResDic.Add(s, VesselResourceAmount(vessel, s));
			}
		}
		void CreateCatalystDictionary()
		{
			foreach(string s in cat)
			{
				CatDic.Add(s, VesselResourceAmount(vessel, s));
			}
		}
        double VesselResourceAmount(Vessel v, string res)
        {
            double totalAmount = 0;
            foreach(Part p in v.Parts)
            {
                totalAmount += p.Resources.Get(res).amount;
            }
            return totalAmount;
        }
		double MainResCalc(double diameter, double multiplier) => Math.Pow(Math.E, diameter * multiplier / 5) * 300;
		double CatCalc(double diameter, double multiplier) => Math.Pow(Math.E, diameter * multiplier / 5);
		public void RunSimulation()
		{
			//RESOURCE Math.Pow(Math.E, [diameter] * [multiplier] / 5) * 300;
			//CATALYST Math.Pow(Math.E, [diameter] * [multiplier] / 5);
			foreach(SpaceFolderDriveData d in driveDatas)
			{
				//ResDic.Item[d.MainResource] -= MainResCalc(d.Diameter, d.Multiplier);
				//CatDic.Item[d.Catalyst] -= CatCalc(d.Diameter, d.Multiplier);
                foreach(KeyValuePair<string, double> kvp in ResDic)
                {
                    if(kvp.Key == d.MainResource)
                    {
                        ResDic.Remove(kvp.Key);
                        ResDic.Add(d.MainResource, kvp.Value - MainResCalc(d.Diameter, d.Multiplier));
                    }
                }
                foreach(KeyValuePair<string, double> kvp in CatDic)
                {
                    if (kvp.Key == d.Catalyst)
                    {
                        CatDic.Remove(kvp.Key);
                        CatDic.Add(d.Catalyst, kvp.Value - CatCalc(d.Diameter, d.Multiplier));
                    }
                }
			}
			foreach(KeyValuePair<string, double> kvp in ResDic)
			{
				if(kvp.Value < 0)
				{
					Status = SimulationStatus.Failed;
					return;
				}
			}
			foreach(KeyValuePair<string, double> kvp in CatDic)
			{
				if(kvp.Value < 0)
				{
                    Status = SimulationStatus.Succeeded;
					return;
				}
			}
		}
	}
}