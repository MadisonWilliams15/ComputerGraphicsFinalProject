using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightColor { Green, Yellow };

public interface iLightController
{
    LightDirection GetLightDirection();
    void SetLightDirection(LightDirection d);
    LightColor GetLightColor();
    void SetLightColor(LightColor c);
    int GetFrameChanged();
}
