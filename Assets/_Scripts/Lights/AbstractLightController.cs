using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractLightController : MonoBehaviour, iLightController
{
    public LightDirection lightDirection = LightDirection.CrossNorthSouth;
    public LightColor lightColor = LightColor.Green;
    public IntersectionTwoLane intersection;
    public int frameChanged;//Updated when lightColor set

    public LightDirection GetLightDirection()
    {
        return lightDirection;
    }

    public void SetLightDirection(LightDirection d)
    {
        lightDirection = d;
    }

    public LightColor GetLightColor()
    {
        return lightColor;
    }

    public void SetLightColor(LightColor c)
    {
        lightColor = c;
        if (c != LightColor.Yellow)
        {
            frameChanged = Time.frameCount;
        }
    }

    public int GetFrameChanged()
    {
        return frameChanged;
    }

    public void Awake()
    {
        intersection = this.transform.GetComponent<IntersectionTwoLane>();//Make intersection accessible from light controller
    }


}
