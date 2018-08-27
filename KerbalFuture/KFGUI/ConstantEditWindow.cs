using UnityEngine;
using KerbalFuture.Utils;
using System.IO;
using KerbalFuture.Superluminal;
using KerbalFuture.Superluminal.FrameShift;
using KerbalFuture.Superluminal.SpaceFolder;

namespace KerbalFuture.KFGUI
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class ConstantEditWindow : MonoBehaviour
	{
		double constOfSpaceTime = 0;
		double constOfContraction = 0;
		double constOfHyperspaceDrag = 0;
		double constOfXMUsage = 0;

		double constOfReactantUsage = 0;


		string stringConstSpaceTime = "";
		string stringConstContraction = "";
		string stringConstHyperspaceDrag = "";
		string stringConstXMUsage = "";

		string stringConstReactantUsage = "";

		Rect constantsRect;

		ConfigNode constants = ConfigNode.Load(Path.Combine(KFGUI.dllLoc, "Constants.cfg"));

		private void Start()
		{
			constantsRect = new Rect(50, 50, (GameSettings.SCREEN_RESOLUTION_WIDTH / 1920) * 300, (GameSettings.SCREEN_RESOLUTION_HEIGHT / 1080) * 500);
			UpdateConstants();
		}
		public static bool drawWindow = false;
		private void OnGUI()
		{
			if(drawWindow)
			{
				constantsRect = GUILayout.Window(37, constantsRect, DrawConstantEditWindow, "Constants");
			}
		}
		private void DrawConstantEditWindow(int id)
		{
			GUI.DragWindow(new Rect(0, 0, constantsRect.width, 20 * (GameSettings.SCREEN_RESOLUTION_HEIGHT / 1080)));
			GUILayout.Label("FSD");
			GUILayout.BeginHorizontal();
			GUILayout.Label("constSpaceTime");
			stringConstSpaceTime = GUILayout.TextField(stringConstSpaceTime);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("constContraction");
			stringConstContraction = GUILayout.TextField(stringConstContraction);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("constHyperspaceDrag");
			stringConstHyperspaceDrag = GUILayout.TextField(stringConstHyperspaceDrag);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("constXMUsage");
			stringConstXMUsage = GUILayout.TextField(stringConstXMUsage);
			GUILayout.EndHorizontal();
			GUILayout.Space(30 * (GameSettings.SCREEN_RESOLUTION_HEIGHT / 1080));
			GUILayout.Label("SFD");
			GUILayout.BeginHorizontal();
			GUILayout.Label("constReactantUsage");
			stringConstReactantUsage = GUILayout.TextField(stringConstReactantUsage);
			GUILayout.EndHorizontal();
			GUILayout.Space(30 * (GameSettings.SCREEN_RESOLUTION_HEIGHT / 1080));
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Save fields"))
			{
				SaveConstants();
			}
			if(GUILayout.Button("Update fields"))
			{
				UpdateConstants();
			}
			GUILayout.EndHorizontal();
		}
		private void SaveConstants()
		{
			constOfSpaceTime = double.Parse(stringConstSpaceTime, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowExponent);
			constOfContraction = double.Parse(stringConstContraction, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowExponent);
			constOfHyperspaceDrag = double.Parse(stringConstHyperspaceDrag, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowExponent);
			constOfXMUsage = double.Parse(stringConstXMUsage, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowExponent);
			constOfReactantUsage = double.Parse(stringConstReactantUsage, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowExponent);
			FrameShiftWarpChecks.ALCUBIERRE_CONSTANT_OF_SPACETIME = constOfSpaceTime;
			FrameShiftWarpChecks.CONTRACTION_CONSTANT_OF_SPACETIME = constOfContraction;
			FrameShiftWarpChecks.HYPERSPACE_DRAG_CONSTANT = constOfHyperspaceDrag;
			FrameShiftWarpChecks.DRIVE_USAGE_CONSTANT_OF_XM = constOfXMUsage;
			SpaceFolderDriveVesselModule.SPACEFOLDER_CONSTANT_OF_REACTANT_USAGE = constOfReactantUsage;
			if (!GUIUtils.SaveConstant(constants, "constSpaceTime", constOfSpaceTime, "Constants.cfg"))
			{
				Debug.Log("[KF] Failed to save constant constSpaceTime.");
			}
			if (!GUIUtils.SaveConstant(constants, "constContraction", constOfContraction, "Constants.cfg"))
			{
				Debug.Log("[KF] Failed to save constant constContraction.");
			}
			if (!GUIUtils.SaveConstant(constants, "constHyperspaceDrag", constOfHyperspaceDrag, "Constants.cfg"))
			{
				Debug.Log("[KF] Failed to save constant constHyperspaceDrag.");
			}
			if (!GUIUtils.SaveConstant(constants, "constXMUsage", constOfXMUsage, "Constants.cfg"))
			{
				Debug.Log("[KF] Failed to save constant constXMUsage.");
			}
			if (!GUIUtils.SaveConstant(constants, "constReactantUsage", constOfReactantUsage, "Constants.cfg"))
			{
				Debug.Log("[KF] Failed to save constant constReactantUsage.");
			}
		}
		private void UpdateConstants()
		{
			if(!GUIUtils.TryLoadConstant(constants, "constSpaceTime", ref constOfSpaceTime))
			{
				Debug.Log("[KF] Failed to load constant constSpaceTime");
			}
			stringConstSpaceTime = constOfSpaceTime.ToString();
			if(!GUIUtils.TryLoadConstant(constants, "constContraction", ref constOfContraction))
			{
				Debug.Log("[KF] Failed to load constant constContraction");
			}
			stringConstContraction = constOfContraction.ToString();
			if(!GUIUtils.TryLoadConstant(constants, "constHyperspaceDrag", ref constOfHyperspaceDrag))
			{
				Debug.Log("[KF] Failed to load constant constHyperspaceDrag");
			}
			stringConstHyperspaceDrag = constOfHyperspaceDrag.ToString();
			if(!GUIUtils.TryLoadConstant(constants, "constXMUsage", ref constOfXMUsage))
			{
				Debug.Log("[KF] Failed to load constant constXMUsage");
			}
			stringConstXMUsage = constOfXMUsage.ToString();
			if(!GUIUtils.TryLoadConstant(constants, "constReactantUsage", ref constOfReactantUsage))
			{
				Debug.Log("[KF] Failed to load constant constReactantUsage");
			}
			stringConstReactantUsage = constOfReactantUsage.ToString();
		}
	}
}