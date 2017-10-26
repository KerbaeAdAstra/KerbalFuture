# SpaceFolder API
## File List
* LatLongHelper.cs
* SpaceFolderEngine.cs
* SpaceFolderWarpChecks.cs
* SpaceFolderFlightDrive.cs
* Vector3dHelper.cs
* VesselData.cs

## Classes
### LatLongHelper
* LatLongHelper
### SpaceFolderEngine
* SpaceFolderEngine : PartModule
### SpaceFolderWarpChecks
* SpaceFolderWarpChecks : MonoBehaviour
### SpaceFolderFlightDrive
* FlightDrive : VesselModule
### Vector3dHelper
* Vector3dHelper : MonoBehaviour
### VesselData
* VesselData : MonoBehaviour

### LatLongHelper
#### Public Methods
* public double XFromLatLongAlt(double _lat, double _long, double _alt)
* public double YFromLatLongAlt(double _lat, double _long, double _alt)
* public double ZFromLatAlt(double _lat, double _alt)

### SpaceFolderEngine
#### Public Static Methods
* int ModuleClassID()
#### Public Methods
* float WarpDriveDiameter()
#### Public Fields
* float warpDriveDiameter

### SpaceFolderFlightDrive
#### Public Static Methods
* void WarpVessel(Vessel v)
#### Private Static Functions
* double GetVesselAltitude(bool includePlanetRadius, Vessel v)
* double GetVesselLongPos(Vector3d pos, Vessel v)
* double GetVesselLatPos(Vector3d pos, Vessel v)
* double CalculateGravPot(CelestialBody cb, Vessel v)
#### Private Static Fields
* Vector3d cbPos
* double vesHeight
* CelestialBody vesBody
* CelestialBody warpBody
* double warpLong
* double warpLat
* double bodyGravPot

### SpaceFolderWarpChecks
#### Public Static Methods
* double SpaciofibrinWarpCalc(List<double>[] engineSizes)
* List<double> BigToSmallSortedDoubleList(List<double> list)
* List<double> SmallToBigSortedDoubleList(List<double> list)
* double ElectricityWarpCalc(List<double>[] engineInfo)
* bool GoodToGo()
* void InitiateWarpCheck(List<double>[] engineSizes, float vesDiameter)
#### Private Static Methods
* double MaxWarpHoleSize(List<double>[] engineSizes)
#### Private Static Fields
* bool goodToGo
* double vesselDiameter
* double maxWarpHoleSize
* double spaciofibrinNeeded

### Vector3dHelper
#### Public Static Methods
* void ConvertVector3dToXYZCoords(Vector3d v3d, ref double x, ref double y, ref double z)
* Vector3d ConvertXYZCoordsToVector3d(double x, double y, double z)
* void ConvertXYZCoordsToVector3d(double x, double y, double z, ref Vector3d v3d)
#### Public Methods
* double Vector3dX()
* double Vector3dY()
* double Vector3dZ()
* void SetX(double x)
* void SetY(double y)
* void SetZ(double z)
#### Private Fields
* double v3dX
* double v3dY
* double v3dZ

### VesselData
#### Constructors and Deconstructors
* VesselData(Vessel vessel)
#### Public Methods
* void UpdateVesselData(Vessel vessel)
* double ResourceAmountOnVessel(string resource, Vessel vessel)
#### Private Fields
* PartSet vesselParts
* HashSet<Part> vesselPartHashSet
