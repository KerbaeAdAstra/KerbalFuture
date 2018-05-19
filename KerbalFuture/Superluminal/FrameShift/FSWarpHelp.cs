using System.Collections.Generic;
using System.Collections;
using System.Linq; //Might need later !shrug
//see below TODO
using UnityEngine;

namespace KerbalFuture.Superluminal.FrameShift
{
	public class FSWarpHelp
	{
		//TODO: Do over functions in this and SFWarpHelp with LINQ?
		public static List<Part> PartsWithModuleFSD(Vessel v)
		{
			//Test at a later date
			/*
			IEnumerable<Part> parts = from part in v.Parts
									  where part.Modules.Contains("ModuleFrameShiftDrive") == true
									  select part;
			return new List<Part>(parts);
			*/
			List<Part> returnList = new List<Part>();
			foreach (Part p in v.Parts)
			{
				IEnumerator ting = p.Modules.GetEnumerator();
				bool tingStatus = ting.MoveNext();
				for (int i = 0; i < p.Modules.Count; i++)
				{
					if (ting.Current.GetType() == typeof(ModuleFrameShiftDrive))
					{
						returnList.Add(p);
					}
					if (tingStatus)
					{
						tingStatus = ting.MoveNext();
					}
				}
			}
			return returnList;
		}
		//Gets the PartModule pm from a provided Part p
		public static ModuleFrameShiftDrive FSDModuleFromPart(Part p)
		{
			Debug.Log("[KF] Getting ModuleFSD from part " + p.name);
			IEnumerator ting = p.Modules.GetEnumerator();
			bool tingStatus = ting.MoveNext();
			for (int i = 0; i < p.Modules.Count; i++)
			{
				Debug.Log("[KF] Trying to find the PartModule...");
				if (ting.Current.GetType() == typeof(ModuleFrameShiftDrive))
				{
					Debug.Log("[KF] Found the PartModule for part " + p.name + "!");
					return (ModuleFrameShiftDrive)ting.Current;
				}
				else if (tingStatus)
				{
					tingStatus = ting.MoveNext();
				}
			}
			Debug.Log("[KF] PartModule not found in part " + p.name);
			return null;
		}
		//Gets a list of FSDriveDatas from a vessel
		public static List<FrameShiftDriveData> DriveDataList(Vessel v)
		{
			List<FrameShiftDriveData> returnList = new List<FrameShiftDriveData>();
			foreach (Part p in v.Parts)
			{
				if (!p.Modules.Contains("ModuleSpaceFolderEngine"))
				{
					continue;
				}
				ModuleFrameShiftDrive em = FSDModuleFromPart(p);
				if (em != null)
				{
					returnList.Add(em.PartDriveData);
				}
				else
				{
					continue;
				}
			}
			return returnList;
		}
	}
}