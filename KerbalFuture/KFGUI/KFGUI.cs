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
using System.Collections;
using System.Globalization;

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
		bool sfdOrbitStats = false;

		bool keplerian = true, kerbalian = false, keplerianAdvanced = false; //start with one on

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
			heightMultiplier = GameSettings.SCREEN_RESOLUTION_HEIGHT / 1080f;
			widthMultiplier = GameSettings.SCREEN_RESOLUTION_WIDTH / 1920f; //looks good on my screen, so we'll multiply the rect's sizes by this to get them the same relative size everwhere
			ftlRect = new Rect(50, 50, 500 * widthMultiplier, 300 * heightMultiplier);
			pdRect = new Rect(50, 50, 500 * widthMultiplier, 300 * heightMultiplier);
			optRect = new Rect(50, 50, 500 * widthMultiplier, 300 * heightMultiplier);
			winSelectRect = new Rect(10, 50, 150 * widthMultiplier, 100 * heightMultiplier);
			Debug.Log("[KF] heightMultiplier=" + heightMultiplier + ", widthMultiplier=" + widthMultiplier);
		}
		Vector2 bodySelectScrollPos = new Vector2();
		Orbit sfdLineOrbit = null;
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
				GUILayout.BeginArea(new Rect(ftlRect.x + ftlRect.width, ftlRect.y, 150 * widthMultiplier, 500 * heightMultiplier), "Select a body", "box");
				//GUILayout.BeginArea(new Rect(ftlRect.x + ftlRect.width, ftlRect.y, 150, 500), "Select a body", "box");
				GUILayout.Space(20 * widthMultiplier);
				//GUILayout.Space(20);
				bodySelectScrollPos = GUILayout.BeginScrollView(bodySelectScrollPos);
				foreach (CelestialBody cb in cbList)
				{
					if (GUILayout.Button(cb.name))
					{
						Debug.Log("[KF] Selected body " + cb.name);
						sfdBodySelection = false;
						SFDCB = cb;
					}
				}
				GUILayout.EndScrollView();
				GUILayout.EndArea();
			}
			else if (sfdBodySelection && !sfdState || !FTLActive) //checks to see either if it was just turned off or the window was switched off
			{
				sfdBodySelection = false;
			}
			#endregion
			#region SFDFinalOrbitStats
			if(sfdOrbitStats && sfdState)
			{
				Rect rectToUse;
				bool writeWindow = true;
				if(sfdBodySelection)
				{
					rectToUse = new Rect(ftlRect.x + ftlRect.width + 150 * widthMultiplier, ftlRect.y, 300 * widthMultiplier, 300 * heightMultiplier);
				}
				else
				{
					rectToUse = new Rect(ftlRect.x + ftlRect.width, ftlRect.y, 300 * widthMultiplier, 300 * heightMultiplier);
				}
				GUILayout.BeginArea(rectToUse, "SFD Warp Orbital Parameters", "box");
				GUILayout.Space(20 * widthMultiplier);
				if(SpaceFolderDriveVesselModule.CurrentVesselModule.Vessel.LandedOrSplashed)
				{
					writeWindow = false;
				}
				if (writeWindow)
				{
					#region OrbitStatToolbar
					GUILayout.BeginHorizontal();
					if (GUILayout.Button("Keplarian"))
					{
						kerbalian = keplerianAdvanced = false;
						keplerian = true;
					}
					if (GUILayout.Button("Kerbalian"))
					{
						keplerian = keplerianAdvanced = false;
						kerbalian = true;
					}
					if (GUILayout.Button("Keplerian Advanced"))
					{
						kerbalian = keplerian = false;
						keplerianAdvanced = true;
					}
					GUILayout.EndHorizontal();
					#endregion
					string toWrite = "";
					double lat, lon, tempDouble;
					lat = lon = tempDouble = 0;
					bool inputsGood = true;
					if (!double.TryParse(SFDLat, out tempDouble))
					{
						inputsGood = false;
					}
					if (!double.TryParse(SFDLon, out tempDouble))
					{
						inputsGood = false;
					}
					if (inputsGood)
					{
						lat = double.Parse(SFDLat, System.Globalization.NumberStyles.AllowDecimalPoint);
						lon = double.Parse(SFDLon, System.Globalization.NumberStyles.AllowDecimalPoint);
						Coords c = new Coords(lat, lon, WarpHelp.GravPotAltitude(SpaceFolderDriveVesselModule.CurrentVesselModule.Vessel, SFDCB), SFDCB);
						if(sfdLineOrbit == null)
						{
							//create copy of the vessel's orbit, otherwise we're working directly with it :o
							sfdLineOrbit = new Orbit(SpaceFolderDriveVesselModule.CurrentVesselModule.Vessel.orbit);
						}
						sfdLineOrbit.UpdateFromStateVectors(c.WorldSpace, SpaceFolderDriveVesselModule.CurrentVesselModule.Vessel.GetObtVelocity(), SFDCB, Planetarium.GetUniversalTime());
						//convert the orbital parameters into the string toWrite
						toWrite += "Latitude: " + Math.Round(lat, 1) + "\n";
						toWrite += "Longitude: " + Math.Round(lon, 1) + "\n";
						toWrite += "Celestial Body: " + SFDCB.GetName() + "\n";
						toWrite += "Orbit is ";
						//if the Pe is below the radius + atmosphere of the body
						if(sfdLineOrbit.semiMajorAxis * (1 - Math.Abs(sfdLineOrbit.eccentricity)) < SFDCB.radiusAtmoFactor)
						{
							toWrite += "suborbital";
						}
						//if the 
						else if(sfdLineOrbit.eccentricity > 0 && sfdLineOrbit.eccentricity < 1)
						{
							toWrite += "elliptical";
						}
						else if(sfdLineOrbit.eccentricity == 1)
						{
							toWrite += "parabolic escape";
						}
						else if(sfdLineOrbit.eccentricity > 1)
						{
							toWrite += "hyperbolic escape";
						}
						toWrite += "\nORBITAL PARAMETERS\n";
						if (keplerian || keplerianAdvanced)
						{
							toWrite += "SMA: " + Math.Round(sfdLineOrbit.semiMajorAxis, 2) + "\n";
							toWrite += "E: " + Math.Round(sfdLineOrbit.eccentricity, 2) + "\n";
							toWrite += "I: " + Math.Round(sfdLineOrbit.inclination, 2);
							if (keplerianAdvanced)
							{
								toWrite += "\n";
								toWrite += "Ω: " + Math.Round(sfdLineOrbit.LAN, 2) + "\n";
								toWrite += "ω: " + Math.Round(sfdLineOrbit.argumentOfPeriapsis, 2) + "\n";
								toWrite += "θ: " + Math.Round(sfdLineOrbit.trueAnomaly, 2);
							}
						}
						else if (kerbalian)
						{
							toWrite += "Ap: " + Math.Round(sfdLineOrbit.semiMajorAxis * (1 + Math.Abs(sfdLineOrbit.eccentricity)), 2) + "\n";
							toWrite += "Pe: " + Math.Round(sfdLineOrbit.semiMajorAxis * (1 - Math.Abs(sfdLineOrbit.eccentricity)), 2) + "\n";
							toWrite += "I: " + Math.Round(sfdLineOrbit.inclination, 2);
						}
					}
					GUILayout.Label(toWrite);
				}
				GUILayout.EndArea();
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
		bool sfdMapLine = false;
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
				//if (double.TryParse(FSDVelocity, out outDouble))
				if(decimal.TryParse(FSDVelocity, NumberStyles.Float, CultureInfo.InvariantCulture, out _)) //discard the value, we don't need it
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
								new BodyCoords(double.Parse(SFDLat, System.Globalization.NumberStyles.AllowDecimalPoint), 
									double.Parse(SFDLon, System.Globalization.NumberStyles.AllowDecimalPoint), 
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
				sfdOrbitStats = GUILayout.Toggle(sfdOrbitStats, "Warp Orbit Statistics", "button");
				#region OldCodeForOrbitDrawer
				/*
				if (inputsGood && sfdMapLine && OrbitDrawer.IsReady)
				{
					//if the state is different
					if (prevMapState != sfdMapLine)
					{
						//If the line was just turned on
						if (sfdMapLine)
						{
							/*
							OrbitDrawer.Instance.DestroyVessel();
							OrbitDrawer.Instance.RefreshFields();
							OrbitDrawer.Instance.DrawSFDWarpLineWithVessel(OrbitDrawer.CalculateOrbit(new Coords(double.Parse(SFDLat, System.Globalization.NumberStyles.AllowDecimalPoint), double.Parse(SFDLon, System.Globalization.NumberStyles.AllowDecimalPoint), WarpHelp.CalculateGravPot(SFDCB, currVM.Vessel), SFDCB).WorldSpace,
								currVM.Vessel.velocityD,
								SFDCB));
							Debug.Log("[KF] Drew SFD line with new vessel");
							FlightGlobals.SetActiveVessel(currVM.Vessel);
							*
							OrbitDrawer.Instance.SetDrawnOrbit(/*orbit to draw here*);
							OrbitDrawer.Instance.DrawLine = true;
						}
						//If the line was just turned off
						else
						{
							//OrbitDrawer.Instance.DestroyVessel();
							OrbitDrawer.Instance.DrawLine = false;
						}
					}
					//if the state is the same and the line is turned on
					else if (sfdMapLine)
					{
						/*
						OrbitDrawer.Instance.UpdateSFDWarpOrbit(new Coords(double.Parse(SFDLat, System.Globalization.NumberStyles.AllowDecimalPoint), double.Parse(SFDLon, System.Globalization.NumberStyles.AllowDecimalPoint), WarpHelp.CalculateGravPot(SFDCB, currVM.Vessel), SFDCB).WorldSpace,
								currVM.Vessel.velocityD,
								SFDCB);
						*
						OrbitDrawer.Instance.SetDrawnOrbit(/*orbit to draw here, CelestialBody*);
					}
					//Don't do anything if the state is the same and the line is turned off
				}
				else if(!inputsGood && sfdMapLine && OrbitDrawer.Instance.IsReady)
				{
					output = "Cannot draw map\nline, inputs bad";
				}
				//should never be used, but it's an edge case
				else if(inputsGood && sfdMapLine && !OrbitDrawer.Instance.IsReady)
				{
					Debug.Log("[KF] Attempting orbit line drawing, but OrbitDrawer is not ready...");
					sfdMapLine = false; //turns the button back off
				}
				*/
				#endregion
				#region Output
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
				#endregion
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