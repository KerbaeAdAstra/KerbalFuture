# SpaceFolder API

## File List

```plain
LatLongHelper.cs
SpaceFolderEngine.cs
SpaceFolderWarpChecks.cs
SpaceFolderFlightDrive.cs
Vector3dHelper.cs
VesselData.cs
```

## Classes

```cs
class LatLongHelper {…}
class SpaceFolderEngine : PartModule {…}
class SpaceFolderWarpChecks : MonoBehaviour {…}
class FlightDrive : VesselModule {…}
class Vector3dHelper : MonoBehaviour {…}
class VesselData : MonoBehaviour {…}
```

### `LatLongHelper`

#### Public Methods

```cs
public double XFromLatLongAlt(double _lat, double _long, double _alt);
public double YFromLatLongAlt(double _lat, double _long, double _alt);
public double ZFromLatAlt(double _lat, double _alt);
```

### SpaceFolderEngine

#### Public Static Methods

```cs
int ModuleClassID();
```

#### Public Methods

```cs
float WarpDriveDiameter();
```

#### Public Fields

```cs
float warpDriveDiameter;
```

### SpaceFolderFlightDrive

#### Public Static Methods

```cs
void WarpVessel(Vessel v);
```

#### Private Static Functions

```cs
double GetVesselAltitude(bool includePlanetRadius, Vessel v);
double GetVesselLongPos(Vector3d pos, Vessel v);
double GetVesselLatPos(Vector3d pos, Vessel v);
double CalculateGravPot(CelestialBody cb, Vessel v);
```

#### Private Static Fields

```cs
Vector3d cbPos;
double vesHeight;
CelestialBody vesBody;
CelestialBody warpBody;
double warpLong;
double warpLat;
double bodyGravPot;
```

### SpaceFolderWarpChecks

#### Public Static Methods

```cs
double SpatiofibrinWarpCalc(List<double>[] engineSizes);
List<double> BigToSmallSortedDoubleList(List<double> list);
List<double> SmallToBigSortedDoubleList(List<double> list);
double ElectricityWarpCalc(List<double>[] engineInfo);
bool GoodToGo();
void InitiateWarpCheck(List<double>[] engineSizes, float vesDiameter);
```

#### Private Static Methods

```cs
double MaxWarpHoleSize(List<double>[] engineSizes);
```

#### Private Static Fields

```cs
bool goodToGo;
double vesselDiameter;
double maxWarpHoleSize;
double spaciofibrinNeeded;
```

### Vector3dHelper

#### Public Static Methods

```cs
void ConvertVector3dToXYZCoords(Vector3d v3d, ref double x, ref double y, ref double z);
Vector3d ConvertXYZCoordsToVector3d(double x, double y, double z);
void ConvertXYZCoordsToVector3d(double x, double y, double z, ref Vector3d v3d);
```

#### Public Methods

```cs
double Vector3dX();
double Vector3dY();
double Vector3dZ();
void SetX(double x);
void SetY(double y);
void SetZ(double z);
```

#### Private Fields

```cs
double v3dX;
double v3dY;
double v3dZ;
```

### VesselData

#### Constructors and Deconstructors

```cs
VesselData(Vessel vessel);
```

#### Public Methods

```cs
void UpdateVesselData(Vessel vessel);
double ResourceAmountOnVessel(string resource, Vessel vessel);
```

#### Private Fields

```cs
PartSet vesselParts;
HashSet<Part> vesselPartHashSet;
```
