using Godot;
using System;

public static class VectorMath {
  public static double GetRotationFromVector(Vector2 v) {
    // Math found at https://www.desmos.com/3d/ec9955337c, it is the same except I inverted the y, and for some reason 360 - angle is now 270
    double _angle = Math.Acos(v.Y / Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2)));
    if (v.X > 0) { 
      return _angle;
    }
    else {
      return 270 - _angle; // very scary magic number, usually it's 360, but it works when I changed it to 270. Need to unit test this
    }
  }
}
