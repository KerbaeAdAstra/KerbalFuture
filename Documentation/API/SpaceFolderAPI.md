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
class LatLongHelper {}
class SpaceFolderEngine : PartModule {}
class SpaceFolderWarpChecks : MonoBehaviour {}
class FlightDrive : VesselModule {}
class Vector3dHelper : MonoBehaviour {}
class VesselData : MonoBehaviour {}
```

### `LatLongHelper`

#### Methods for `LatLongHelper`

```cs
public double XFromLatLongAlt(double _lat, double _long, double _alt);
public double YFromLatLongAlt(double _lat, double _long, double _alt);
public double ZFromLatAlt(double _lat, double _alt);
```

### SpaceFolderEngine

#### Methods for `SpaceFolderEngine`

```cs
public static int ModuleClassID();
public float WarpDriveDiameter();
```

#### Fields for `SpaceFolderEngine`

```cs
public float warpDriveDiameter;
```

### `SpaceFolderFlightDrive`

#### Methods for `SpaceFolderFlightDrive`

```cs
public static void WarpVessel(Vessel v);
private static double GetVesselAltitude(bool includePlanetRadius, Vessel v);
private static double GetVesselLongPos(Vector3d pos, Vessel v);
private static double GetVesselLatPos(Vector3d pos, Vessel v);
private static double CalculateGravPot(CelestialBody cb, Vessel v);
```

#### Fields for `SpaceFolderFlightDrive`

```cs
private static Vector3d cbPos;
private static double vesHeight;
private static CelestialBody vesBody;
private static CelestialBody warpBody;
private static double warpLong;
private static double warpLat;
private static double bodyGravPot;
```

### `SpaceFolderWarpChecks`

#### Methods for `SpaceFolderWarpChecks`

```cs
public static double SpatiofibrinWarpCalc(List<double>[] engineSizes);
public static double ElectricityWarpCalc(List<double>[] engineInfo);
public static bool GoodToGo();
public static void InitiateWarpCheck(List<double>[] engineSizes, float vesDiameter);
private static double MaxWarpHoleSize(List<double>[] engineSizes);
```

#### Fields for `SpaceFolderWarpChecks`

```cs
private static bool goodToGo;
private static double vesselDiameter;
private static double maxWarpHoleSize;
private static double spaciofibrinNeeded;
```

### `Vector3dHelper`

#### Methods for `Vector3dHelper`

```cs
public static void ConvertVector3dToXYZCoords(Vector3d v3d, ref double x, ref double y, ref double z);
public static Vector3d ConvertXYZCoordsToVector3d(double x, double y, double z);
public static void ConvertXYZCoordsToVector3d(double x, double y, double z, ref Vector3d v3d);
public double Vector3dX();
public double Vector3dY();
public double Vector3dZ();
public void SetX(double x);
public void SetY(double y);
public void SetZ(double z);
```

#### Fields for `Vector3dHelper`

```cs
private double v3dX;
private double v3dY;
private double v3dZ;
```

### `VesselData`

#### Constructors

```cs
public VesselData(Vessel vessel);
```

#### Methods for `VesselData`

```cs
public void UpdateVesselData(Vessel vessel);
public double ResourceAmountOnVessel(string resource, Vessel vessel);
```

#### Fields for `VesselData`

```cs
private PartSet vesselParts;
private HashSet<Part> vesselPartHashSet;
```
