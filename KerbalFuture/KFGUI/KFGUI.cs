using System.IO;
using UnityEngine;
using System.Collections.Generic;
using KSP.UI.Screens;
using System.Linq;
using System;
using System.Reflection;
using KerbalFuture.Superluminal.FrameShift;
using KerbalFuture.Superluminal.SpaceFolder;
using KerbalFuture.Superluminal;
using KerbalFuture.Utils;

namespace KerbalFuture.KFGUI
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class KFGUI : MonoBehaviour
	{
		#region Singleton
		private static KFGUI instance = null;
		public static KFGUI Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new KFGUI();
				}
				return instance;
			}
		}
		private KFGUI()
		{

		}
		#endregion
		string FSDVelocity = "";
		string SWDAmplitude = "";
		string SWDWavelength = "";
		string SFDLat = "";
		string SFDLon = "";
		CelestialBody SFDCB;
		string PSDLayer = "";
		List<CelestialBody> cbList = new List<CelestialBody>();
		//Window 0
		public bool FTLActive { get; set; }
		// Window 1
		public bool PlanetaryDestructionActive { get; set; }
		// Window 2
		public bool OptionsMenuActive { get; set; }
		public bool WindowSelectionActive { get; private set; } = false;
		public Color GUIStandardContentColor = GUI.contentColor;

		public static string dllLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		
		float heightMultiplier;
		float widthMultiplier;
		Rect ftlRect;
		Rect pdRect;
		Rect optRect;
		Rect winSelectRect;

		// Debug.Log("[KF] ");
		bool fsdState, sfdState, swdState, psdState;
		bool ARUActive = false;
		bool sfdBodySelection = false;

		public Color sfdEndpointColor = new Color(255, 35, 181, 1);

		private void Start()
		{
			Debug.Log("[KF] GUI starting");
			fsdState = sfdState = swdState = psdState = false;
			GUIStandardContentColor = GUI.contentColor;
			SFDCB = FlightGlobals.ActiveVessel.mainBody;
			cbList.Clear();
			cbList.AddRange(FlightGlobals.Bodies);
			Debug.Log("[KF] Toolbar button enabled.");
			try
			{
				logo = GameDatabase.Instance.GetTexture(toolbarLogoLoc, false);
			}
			catch (Exception)
			{
				Debug.Log("[KF] Failed to load KF logo from mod directory.");
			}
			//Calls the constant loading functions
			Debug.Log("[KF] Loading of constants " + (LoadFSDWarpConstants() ? "failed" : "succeded") + ".");
			Debug.Log("[KF] Loading of constants " + (LoadSFDWarpConstants() ? "failed" : "succeded") + ".");
			//Subscribes to the events
			GameEvents.onGUIApplicationLauncherReady.Add(CreateButton);
			GameEvents.onGUIApplicationLauncherDestroyed.Add(DestroyButton);
			//Gets the height multiplier and the width multiplier to fix up the GUI size
			heightMultiplier = GameSettings.SCREEN_RESOLUTION_HEIGHT / 1080;
			widthMultiplier = GameSettings.SCREEN_RESOLUTION_WIDTH / 1920; //looks good on my screen, so we'll multiply the rect's sizes by this to get them the same relative size everwhere
			ftlRect = new Rect(50, 50, 500 * widthMultiplier, 300 * heightMultiplier);
			pdRect = new Rect(50, 50, 500 * widthMultiplier, 300 * heightMultiplier);
			optRect = new Rect(50, 50, 500 * widthMultiplier, 300 * heightMultiplier);
			winSelectRect = new Rect(10, 50, 150 * widthMultiplier, 100 * heightMultiplier);
		}
		Vector2 bodySelectScrollPos = new Vector2();
		private void OnGUI()
		{
			if (FTLActive)
			{
				ftlRect = GUILayout.Window(0, ftlRect, DrawFTLInternals, "Faster Than Light", GUILayout.Width(500), GUILayout.Height(300));
			}
			if (PlanetaryDestructionActive)
			{
				pdRect = GUILayout.Window(1, pdRect, DrawPlanetDestructionInternals, "Planetary Destruction", GUILayout.Width(500), GUILayout.Height(300));
			}
			if (OptionsMenuActive)
			{
				optRect = GUILayout.Window(2, optRect, DrawOptionsMenuInternals, "Kerbal Future Options", GUILayout.Width(500), GUILayout.Height(300));
			}
			if (WindowSelectionActive)
			{
				winSelectRect = GUILayout.Window(3, winSelectRect, DrawWindowSelector, "Window Selector", GUILayout.Width(150), GUILayout.Height(100));
			}
			#region SFD body selection scroll
			if (sfdBodySelection && sfdState)
			{
				GUILayout.BeginArea(new Rect(ftlRect.x + ftlRect.width, ftlRect.y, 150, 500), "Select a body", "box");
				GUILayout.Space(20);
				bodySelectScrollPos = GUILayout.BeginScrollView(bodySelectScrollPos);
				foreach (CelestialBody cb in cbList)
				{
					if (GUILayout.Button(cb.name))
					{
						sfdBodySelection = false;
						SFDCB = cb;
					}
				}
				GUILayout.EndScrollView();
				GUILayout.EndArea();
			}
			else if (sfdBodySelection && !sfdState)
			{
				sfdBodySelection = false;
			}
			#endregion
		}
		bool[] windowSaveStates = new bool[3] { false, false, false };
		private void DisableAllWindows(bool saveStates = false)
		{
			if (saveStates)
			{
				windowSaveStates[0] = FTLActive;
				windowSaveStates[1] = PlanetaryDestructionActive;
				windowSaveStates[2] = OptionsMenuActive;
			}
			FTLActive = false;
			PlanetaryDestructionActive = false;
			OptionsMenuActive = false;
		}
		private void EnableWindows()
		{
			FTLActive = windowSaveStates[0];
			PlanetaryDestructionActive = windowSaveStates[1];
			OptionsMenuActive = windowSaveStates[2];
		}
		private void DrawWindowSelector(int id)
		{
			GUI.DragWindow(new Rect(0, 0, winSelectRect.width, 20 * heightMultiplier));
			GUILayout.BeginVertical();
			FTLActive = GUILayout.Toggle(FTLActive, "FTL", "button");
			PlanetaryDestructionActive = GUILayout.Toggle(PlanetaryDestructionActive, "PD", "button");
			OptionsMenuActive = GUILayout.Toggle(OptionsMenuActive, "Options", "button");
			ConstantEditWindow.drawWindow = GUILayout.Toggle(ConstantEditWindow.drawWindow, "Constants", "button");
			GUILayout.EndVertical();
		}
		Vector2 ftlScrollPos = new Vector2();
		Superluminal.FrameShift.VesselResourceSimulation vrs = new Superluminal.FrameShift.VesselResourceSimulation();
		//make it have something to try to limit NRE's, can be shared between all different drive GUI's because it's only going to be used one at a time
		//we will reset it when they switch drives tho
		string output = "";
		Superluminal.FrameShift.Error fsdErr = Superluminal.FrameShift.Error.Null;
		Superluminal.SpaceFolder.Error sfdErr = Superluminal.SpaceFolder.Error.Null;
		List<VesselResource> vrUDWList;
		List<VesselResource> vrList;
		double outDouble = 0; //useless, need for TryParse
		private void DrawFTLInternals(int id)
		{
			GUI.DragWindow(new Rect(0, 0, ftlRect.width, 20 * heightMultiplier));
			ftlScrollPos = GUILayout.BeginScrollView(ftlScrollPos);
			#region toolbar
			bool temp;
			GUILayout.BeginHorizontal();
			temp = fsdState;
			fsdState = GUILayout.Toggle(fsdState, "FSD", "button"); // 0
			if (temp != fsdState)
			{
				swdState = sfdState = psdState = false;
				ScreenMessages.PostScreenMessage(new ScreenMessage("Frameshift drive selected", 10, new ScreenMessageStyle()));
			}
			if (fsdState == false && temp == true)
			{
				fsdState = true;
			}
			temp = swdState;
			swdState = GUILayout.Toggle(swdState, "SWD", "button"); // 1
			if (temp != swdState)
			{
				fsdState = sfdState = psdState = false;
				ScreenMessages.PostScreenMessage(new ScreenMessage("Stutterwarp drive selected", 10, new ScreenMessageStyle()));
			}
			if (swdState == false && temp == true)
			{
				swdState = true;
			}
			temp = sfdState;
			sfdState = GUILayout.Toggle(sfdState, "SFD", "button"); // 2
			if (temp != sfdState)
			{
				swdState = fsdState = psdState = false;
				ScreenMessages.PostScreenMessage(new ScreenMessage("Spacefolder drive selected", 10, new ScreenMessageStyle()));
			}
			if (sfdState == false && temp == true)
			{
				sfdState = true;
			}
			temp = psdState;
			psdState = GUILayout.Toggle(psdState, "PSD", "button"); // 3
			if (temp != psdState)
			{
				swdState = sfdState = fsdState = false;
				ScreenMessages.PostScreenMessage(new ScreenMessage("Polyspacial drive selected", 10, new ScreenMessageStyle()));
			}
			if (psdState == false && temp == true)
			{
				psdState = true;
			}
			GUILayout.EndHorizontal();
			#endregion toolbar
			#region FSD
			if (fsdState)
			{
				bool formatOkay = true;
				#region Velocity input
				GUILayout.BeginHorizontal();
				GUILayout.Label("Desired velocity");
				if (double.TryParse(FSDVelocity, out outDouble))
				{
					GUI.contentColor = GUIStandardContentColor;
				}
				else
				{
					formatOkay = false;
					GUI.contentColor = Color.red;
				}
				//Max length is 42 digits, that's when the box starts stretching and things get kinda weird
				FSDVelocity = GUILayout.TextField(FSDVelocity, 42);
				GUI.contentColor = GUIStandardContentColor;
				GUILayout.Label("m/s");
				GUILayout.EndHorizontal();
				#endregion
				try
				{
					GUILayout.BeginHorizontal();
					FrameShiftDriveVesselModule currVM = FrameShiftDriveVesselModule.RequestCurrentVesselModule();
					GUIStyle style = new GUIStyle();
					style.wordWrap = true;
					double velocity = 0;
					bool warpSuccess = false;
					if (formatOkay)
					{
						velocity = double.Parse(FSDVelocity, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowExponent);
					}
					if (GUILayout.Button("Warp Vessel"))
					{
						if (formatOkay)
						{
							warpSuccess = currVM.WarpVessel(new FrameShiftWarpData(currVM.Vessel, velocity), out fsdErr);
							vrs = currVM.vrsInstance;
							vrList = FSDResourceUsageBeforeWarp(velocity, vrs, currVM.Vessel);
							vrList = AlphabetizeVRList(vrList).ToList();
							vrUDWList = FSDResourceUsageDuringWarp(velocity, currVM.Vessel);
							vrUDWList = AlphabetizeVRList(vrUDWList).ToList();
						}
					}
					if (GUILayout.Button("Analyse warp"))
					{
						if (formatOkay)
						{
							fsdErr = FrameShiftWarpChecks.WarpAvalible(currVM.Vessel, velocity, out vrs);
							vrList = FSDResourceUsageBeforeWarp(velocity, vrs, currVM.Vessel);
							vrList = AlphabetizeVRList(vrList).ToList();
							vrUDWList = FSDResourceUsageDuringWarp(velocity, currVM.Vessel);
							vrUDWList = AlphabetizeVRList(vrUDWList).ToList();
						}
					}
					if (GUILayout.Button("Stop warp"))
					{
						fsdErr = Superluminal.FrameShift.Error.Null;
						vrUDWList.Clear();
						vrList.Clear();
						currVM.StopWarp();
					}
					if (warpSuccess && fsdErr == Superluminal.FrameShift.Error.ClearForWarp)
					{
						output = "Warped!";
					}
					else if (!warpSuccess && fsdErr == Superluminal.FrameShift.Error.ClearForWarp)
					{
						output = "Checks passed";
					}
					else if (fsdErr == Superluminal.FrameShift.Error.Null || !formatOkay)
					{
						output = "";
					}
					else
					{
						output = fsdErr.ToString();
					}
					GUILayout.Label("Warp status: " + output, style);
					GUILayout.EndHorizontal();
				}
				//this is just in case, I don't know if it's ever going to throw an exception
				catch (Exception e)
				{
					Debug.Log("[KF] Caught exception " + e.ToString());
				}
			}
			#endregion
			#region SWD
			if (swdState)
			{
				#region Inputs
				GUILayout.BeginHorizontal();
				GUILayout.BeginVertical();
				GUILayout.Label("Amplitude");
				GUILayout.Label("Wavelength");
				GUILayout.EndVertical();
				GUILayout.BeginVertical();
				if (double.TryParse(SWDAmplitude, out outDouble))
				{
					GUI.contentColor = GUIStandardContentColor;
				}
				else
				{
					GUI.contentColor = Color.red;
				}
				SWDAmplitude = GUILayout.TextField(SWDAmplitude, 21);
				GUI.contentColor = GUIStandardContentColor;
				GUILayout.BeginHorizontal();
				if (double.TryParse(SWDWavelength, out outDouble))
				{
					GUI.contentColor = GUIStandardContentColor;
				}
				else
				{
					GUI.contentColor = Color.red;
				}
				SWDWavelength = GUILayout.TextField(SWDWavelength, 21);
				GUI.contentColor = GUIStandardContentColor;
				GUILayout.Label("meters");
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
				#endregion
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Attempt warp"))
				{
					//TODO Try warp
				}
				if (GUILayout.Button("Check warp"))
				{
					//TODO Check warp
				}
				GUILayout.BeginVertical();
				GUILayout.Label("Output");
				//TODO GUILayout.Label() //have output warp checks status/errors
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
			}
			#endregion
			#region SFD
			if (sfdState)
			{
				SpaceFolderDriveVesselModule currVM = SpaceFolderDriveVesselModule.CurrentVesselModule;
				bool inputsGood = true;
				#region Inputs
				GUILayout.BeginHorizontal();
				GUILayout.BeginVertical();
				GUILayout.Label("Latitude");
				GUILayout.Label("Longitude");
				GUILayout.Label("Celestial Body");
				GUILayout.EndVertical();
				GUILayout.BeginVertical();
				if (double.TryParse(SFDLat, out outDouble))
				{
					GUI.contentColor = GUIStandardContentColor;
				}
				else
				{
					inputsGood = false;
					GUI.contentColor = Color.red;
				}
				SFDLat = GUILayout.TextField(SFDLat, 6);
				GUI.contentColor = GUIStandardContentColor;
				if (double.TryParse(SFDLon, out outDouble))
				{
					GUI.contentColor = GUIStandardContentColor;
				}
				else
				{
					inputsGood = false;
					GUI.contentColor = Color.red;
				}
				SFDLon = GUILayout.TextField(SFDLon, 6);
				GUI.contentColor = GUIStandardContentColor;
				if (GUILayout.Button(SFDCB.name))
				{
					sfdBodySelection = !sfdBodySelection;
				}
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
				#endregion
				GUILayout.BeginHorizontal();
				bool warpSuccess = false;
				if (GUILayout.Button("Attempt warp"))
				{
					//TODO Try warp
					if(inputsGood)
					{
						warpSuccess = currVM.WarpVessel(
							new SpaceFolderWarpData(currVM.Vessel, 
								new BodyCoords(double.Parse(SFDLat, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowExponent), 
									double.Parse(SFDLon, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowExponent), 
									SFDCB), 
								0), 
							out sfdErr);
						if(warpSuccess)
						{
							currVM.Vessel.UpdatePosVel();
							currVM.Vessel.orbitDriver.enabled = true;
							currVM.Vessel.orbitDriver.RecalculateOrbit(SFDCB);
						}
					}
				}
				if (GUILayout.Button("Check warp"))
				{
					//TODO Check warp
					if(inputsGood)
					{
						sfdErr = SpaceFolderWarpChecks.WarpAvailable(currVM.Vessel);
					}
				}
				if(GUILayout.Button("Toggle map line"))
				{
					sfdLineDrawn = !sfdLineDrawn;
				}
				GUILayout.BeginVertical();
				GUILayout.Label("Output");
				if(warpSuccess && sfdErr == Superluminal.SpaceFolder.Error.ClearForWarp)
				{
					output = "Warped!";
				}
				else if(!warpSuccess && sfdErr == Superluminal.SpaceFolder.Error.ClearForWarp)
				{
					output = "Checks passed!";
				}
				else if(sfdErr == Superluminal.SpaceFolder.Error.Null || !inputsGood)
				{
					output = "";
				}
				else
				{
					output = sfdErr.ToString();
				}
				GUILayout.Label(output);
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
			}
			#endregion
			#region PSD
			if (psdState)
			{
				#region Inputs
				GUILayout.BeginHorizontal();
				GUILayout.Label("Layer");
				if (double.TryParse(SWDWavelength, out outDouble))
				{
					GUI.contentColor = GUIStandardContentColor;
				}
				else
				{
					GUI.contentColor = Color.red;
				}
				PSDLayer = GUILayout.TextField(PSDLayer, 2);
				GUI.contentColor = GUIStandardContentColor;
				GUILayout.EndHorizontal();
				#endregion
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Attempt warp"))
				{
					//TODO Try warp
				}
				if (GUILayout.Button("Check warp"))
				{
					//TODO Check warp
				}
				GUILayout.BeginVertical();
				GUILayout.Label("Output");
				//TODO GUILayout.Label() //have output warp checks status/errors
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
			}
			#endregion
			#region ARU
			if (fsdState || sfdState || swdState || psdState)
			{
				ARUActive = GUILayout.Toggle(ARUActive, "Advanced resource usage", "button");
			}
			if (ARUActive)
			{
				/*
				if (fsdState)
				{
					foreach (FrameShiftDriveData dd in vesselDriveDatas)
					{

					}
				}
				*/
			}
			#endregion
			GUILayout.EndScrollView();
		}
		IEnumerable<VesselResource> AlphabetizeVRList(List<VesselResource> vrl)
		{
			IEnumerable<VesselResource> v = from vesRes in vrl
											orderby vesRes.resource ascending
											select vesRes;
			return v;
		}
		// Math taken straight from the FSDVesselModule method UseResourcesBeforeWarp
		List<VesselResource> FSDResourceUsageBeforeWarp(double velocity, Superluminal.FrameShift.VesselResourceSimulation vrs, Vessel v)
		{
			HashSet<string> vesResHash = new HashSet<string>();
			List<VesselResource> vesResList = new List<VesselResource>();
			double U_F = FrameShiftWarpChecks.FieldEnergyCalc(velocity, WarpHelp.VesselDiameterCalc(v) / 2);
			foreach (KeyValuePair<FrameShiftDriveData, double> kvp in vrs.PartECPercent)
			{
				vesResHash.Add(kvp.Key.MainResource);
				vesResHash.Add(kvp.Key.Catalyst);
			}
			foreach (string s in vesResHash)
			{
				vesResList.Add(new VesselResource(s, 0));
			}
			foreach (KeyValuePair<FrameShiftDriveData, double> kvp in vrs.PartECPercent)
			{
				for (int i = 0; i < vesResList.Count; i++)
				{
					if (vesResList[i].resource == kvp.Key.MainResource)
					{
						vesResList[i] = new VesselResource(vesResList[i].resource, vesResList[i].amount + (kvp.Value * U_F / 1000) / kvp.Key.Efficiency);
					}
					if (vesResList[i].resource == kvp.Key.Catalyst)
					{
						vesResList[i] = new VesselResource(vesResList[i].resource, vesResList[i].amount + ((kvp.Value * U_F / 1000) * kvp.Key.XMMultiplier) / kvp.Key.Efficiency);
					}
				}
			}
			return vesResList;
		}
		List<VesselResource> FSDResourceUsageDuringWarp(double velocity, Vessel v)
		{
			HashSet<string> vesResHash = new HashSet<string>();
			List<VesselResource> vesResList = new List<VesselResource>();
			double dU_FkJ = (Math.Pow(velocity, 3) * Math.Pow(WarpHelp.VesselDiameterCalc(v) / 2, 3) * FrameShiftWarpChecks.HYPERSPACE_DRAG_CONSTANT) / 1000;
			List<FrameShiftDriveData> driveDatas = new List<FrameShiftDriveData>(FSWarpHelp.DriveDataList(v));
			double sigmaCapacity = 0;
			foreach (FrameShiftDriveData dd in driveDatas)
			{
				sigmaCapacity += dd.Capacity;
				vesResHash.Add(dd.MainResource);
				vesResHash.Add(dd.Catalyst);
			}
			foreach (string s in vesResHash)
			{
				vesResList.Add(new VesselResource(s, 0));
			}
			foreach (FrameShiftDriveData dd in driveDatas)
			{
				for (int i = 0; i < vesResList.Count; i++)
				{
					if (vesResList[i].resource == dd.MainResource)
					{
						// Calculates the contribution of the drive in kJ/second using efficiency
						vesResList[i] = new VesselResource(dd.MainResource, ((dd.Capacity / sigmaCapacity) * dU_FkJ / dd.Efficiency) + vesResList[i].amount);
					}
					if (vesResList[i].resource == dd.Catalyst)
					{
						vesResList[i] = new VesselResource(dd.Catalyst, (((dd.Capacity / sigmaCapacity) * dU_FkJ / dd.Efficiency) * dd.XMMultiplier) + vesResList[i].amount);
					}
				}
			}
			return vesResList;
		}
		private void DrawPlanetDestructionInternals(int id)
		{
			GUI.DragWindow(new Rect(0, 0, pdRect.width, 20 * heightMultiplier));
			GUILayout.Label("Nothing here yet, check back after a new mod release!");
		}
		private void DrawOptionsMenuInternals(int id)
		{
			GUI.DragWindow(new Rect(0, 0, optRect.width, 20 * heightMultiplier));
			GUILayout.Label("Nothing here yet, check back after a new mod release!");
		}
		#region Toolbar button
		// When we want the toolbar button avalible
		static string toolbarLogoLoc = "KerbalFuture/PluginData/toolbarLogo";
		private const ApplicationLauncher.AppScenes visibleInScenes =
			ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW;
		private Texture2D logo;
		private ApplicationLauncherButton button;

		private void OnDisable()
		{
			Debug.Log("[KF] Toolbar button disabled.");
			GameEvents.onGUIApplicationLauncherReady.Remove(CreateButton);
			GameEvents.onGUIApplicationLauncherDestroyed.Remove(DestroyButton);
			DestroyButton();
		}
		private void CreateButton()
		{
			Debug.Log("[KF] Toolbar button being created.");
			if (ApplicationLauncher.Ready && button == null)
			{
				if (logo == null)
				{
					logo = LoadToolbarTextureManually();
				}
				button = ApplicationLauncher.Instance.AddModApplication(OnButtonSelected, OnButtonDeselected,
					null, null,
					null, null,
					visibleInScenes, logo);
				Debug.Log("[KF] " + button.transform.position);
			}
		}
		private void DestroyButton()
		{
			Debug.Log("[KF] Toolbar button being destroyed.");
			if (button != null)
			{
				ApplicationLauncher.Instance.RemoveModApplication(button);
				button = null;
			}
		}
		private void OnButtonSelected()
		{
			Debug.Log("[KF] Toolbar button selected.");
			WindowSelectionActive = true;
			EnableWindows();
		}
		private void OnButtonDeselected()
		{
			Debug.Log("[KF] Toolbar button deselected.");
			WindowSelectionActive = false;
			DisableAllWindows(true);
		}
		private Texture2D LoadToolbarTextureManually()
		{
			Debug.Log("[KF] PluginResources found at " + Path.Combine(dllLoc, "PluginData"));
			Texture2D t = new Texture2D(38, 38, TextureFormat.RGBA32, false);
			if (t.LoadImage(File.ReadAllBytes(Path.Combine(Path.Combine(dllLoc, "PluginData"), "toolbarLogo.png"))))
			{
				Debug.Log("[KF] Succeeded in manually loading toolbar icon texture.");
			}
			else
			{
				Debug.Log("[KF] ERROR LOADING TOOLBAR ICON TEXTURE");
			}
			return t;
		}
		#endregion
		#region SFDEndLine
		bool sfdLineDrawn = false;
		LineRenderer line = null;
		GameObject obj = new GameObject("Line");
		PlanetariumCamera planetariumCamera = null;
		void DrawSFDLatLongLine(bool inputsGood)
		{
			if (!inputsGood || !MapView.MapIsEnabled || !sfdLineDrawn)
			{
				return;
			}
			planetariumCamera = (PlanetariumCamera)FindObjectOfType(typeof(PlanetariumCamera));
			List<Vector3> linePoints = new List<Vector3>();
			linePoints.Add(SFDCB.position);
			linePoints.Add(new Coords(double.Parse(SFDLat), double.Parse(SFDLon), SFDCB.Radius * 3, SFDCB).WorldSpace);
			obj.layer = 9;
			line = obj.AddComponent<LineRenderer>();
			line.useWorldSpace = true;
			line.material = new Material(Shader.Find("Particles/Additive"));
			line.startColor = sfdEndpointColor;
			line.endColor = sfdEndpointColor;
			line.startWidth = (float)(0.005 * planetariumCamera.Distance);
			line.endWidth = (float)(0.005 * planetariumCamera.Distance);
			line.positionCount = 2;
			line.SetPositions(linePoints.ToArray());
			line.enabled = true;
		}
		#endregion
		#region Constant loading
		//Loads the FSD warp constants
		//returns false if no problems were encountered, and true if problems were encountered
		bool LoadFSDWarpConstants()
		{
			bool failedLoad = false;
			Debug.Log("[KF] Loading warp constants...");
			ConfigNode constants = ConfigNode.Load(Path.Combine(dllLoc, "Constants.cfg"));
			if (!GUIUtils.TryLoadConstant(constants, "constSpaceTime", ref FrameShiftWarpChecks.constOfSpaceTime))
			{
				FrameShiftWarpChecks.ALCUBIERRE_CONSTANT_OF_SPACETIME = 0.000001;
				failedLoad = true;
			}
			if (!GUIUtils.TryLoadConstant(constants, "constContraction", ref FrameShiftWarpChecks.constOfContraction))
			{
				FrameShiftWarpChecks.CONTRACTION_CONSTANT_OF_SPACETIME = 0.000000000001;
				failedLoad = true;
			}
			if (!GUIUtils.TryLoadConstant(constants, "constHyperspaceDrag", ref FrameShiftWarpChecks.constOfDrag))
			{
				failedLoad = true;
				FrameShiftWarpChecks.HYPERSPACE_DRAG_CONSTANT = 0.00000000000000000000001;
			}
			if (!GUIUtils.TryLoadConstant(constants, "constXMUsage", ref FrameShiftWarpChecks.constOfXMUsage))
			{
				failedLoad = true;
				FrameShiftWarpChecks.DRIVE_USAGE_CONSTANT_OF_XM = 400;
			}
			return failedLoad;
		}
		//Loads the SFD warp constants
		//returns false if no problems were encountered, true if problems were encountered
		bool LoadSFDWarpConstants()
		{
			bool failedLoad = false;
			Debug.Log("[KF] Loading SFD warp constants...");
			ConfigNode constants = ConfigNode.Load(Path.Combine(dllLoc, "Constants.cfg"));
			if(!GUIUtils.TryLoadConstant(constants, "constReactantUsage", ref SpaceFolderDriveVesselModule.sfConstReactantUsage))
			{
				failedLoad = true;
				SpaceFolderDriveVesselModule.SPACEFOLDER_CONSTANT_OF_REACTANT_USAGE = 30000;
			}
			return failedLoad;
		}
#endregion
	}
}