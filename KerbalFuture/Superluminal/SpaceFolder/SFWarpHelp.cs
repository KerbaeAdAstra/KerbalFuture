using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KerbalFuture.Superluminal.SpaceFolder
{
	public class SFWarpHelp
	{
		//Gets the list of parts on a vessel with ModuleSpaceFolderDrive
		public static List<Part> PartsWithModuleSFD(Vessel v)
		{
			List<Part> returnList = new List<Part>();
			foreach (Part p in v.Parts)
			{
				IEnumerator ting = p.Modules.GetEnumerator();
				bool tingStatus = ting.MoveNext();
				for (int i = 0; i < p.Modules.Count; i++)
				{
					if (ting.Current.GetType() == typeof(ModuleSpaceFolderDrive))
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
		public static ModuleSpaceFolderDrive SFDModuleFromPart(Part p)
		{
			Debug.Log("[KF] Getting ModuleSFEngine from part " + p.name);
			IEnumerator ting = p.Modules.GetEnumerator();
			bool tingStatus = ting.MoveNext();
			for (int i = 0; i < p.Modules.Count; i++)
			{
				Debug.Log("[KF] Trying to find the PartModule...");
				if (ting.Current.GetType() == new ModuleSpaceFolderDrive().GetType())
				{
					Debug.Log("[KF] Found the PartModule for part " + p.name + "!");
					return (ModuleSpaceFolderDrive)ting.Current;
				}
				else if (tingStatus)
				{
					tingStatus = ting.MoveNext();
				}
			}
			Debug.Log("[KF] PartModule not found in part " + p.name);
			return null;
		}
		//Gets a list of SFDriveDatas from a vessel
		public static List<SpaceFolderDriveData> DriveDataList(Vessel v)
		{
			List<SpaceFolderDriveData> returnList = new List<SpaceFolderDriveData>();
			foreach (Part p in v.Parts)
			{
				if(!p.Modules.Contains("ModuleSpaceFolderDrive"))
				{
					continue;
				}
				ModuleSpaceFolderDrive em = SFDModuleFromPart(p);
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
		public static List<SpaceFolderDriveData> SortDriveData(List<SpaceFolderDriveData> inList)
		{
			IEnumerable<SpaceFolderDriveData> queryDiameters = from dd in inList
															   orderby dd.Diameter descending
															   select dd;
			return new List<SpaceFolderDriveData>(queryDiameters);
		}
		//Welcome... To the graveyard
		/*
		//Drive data from part
		public static SpaceFolderDriveData PartSFDData(Part p)
		{
			if (p.Modules.Contains("ModuleSpaceFolderDrive"))
			{
				ModuleSpaceFolderDrive d = (ModuleSpaceFolderDrive)p.Modules["ModuleSpaceFolderDrive"];
				return d.PartDriveData;
			}
			else
			{
				return new SpaceFolderDriveData(true);
			}
		}
		//Parts with SFD's from vessel
		public static List<Part> SFDOnVessel(Vessel v)
		{
			IEnumerable<Part> sfds = from sfd in v.Parts
									 where sfd.Modules.Contains("ModuleSpaceFolderDrive") == true
									 select sfd;
			return new List<Part>(sfds);
		}
		//Gets a list of DriveData's from a vessel
		public static List<SpaceFolderDriveData> SFDDFromVessel(Vessel v)
		{
			if (PartsWithModuleSFD(v).Count == 0)
			{
				return new List<SpaceFolderDriveData>();
			}
			List<SpaceFolderDriveData> returnList = new List<SpaceFolderDriveData>();
			foreach (Part p in PartsWithModuleSFD(v))
			{
				returnList.Add(SFDModuleFromPart(p).PartDriveData);
			}
			return returnList;
		}
		*/
	}
}
