using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    private IntersectionTwoLane intersection;
    public LightDirection direction;
    private SpriteRenderer visibleLight;
    private Color red, green, yellow;

    public void Start()
    {
        intersection = this.transform.parent.GetComponent<IntersectionTwoLane>();
        visibleLight = this.GetComponent<SpriteRenderer>();
        red = new Color32(204, 6, 5, 255);
        yellow = new Color32(250, 210, 1, 255);
        green = new Color32(51, 165, 50, 255);
    }

    public void Update()
    {
        if(intersection.lightController.GetLightDirection() == direction || (LightDirection.CrossNorthSouth == intersection.lightController.GetLightDirection() && (LightDirection.North == direction || LightDirection.South == direction)) || (LightDirection.CrossEastWest == intersection.lightController.GetLightDirection() && (LightDirection.East == direction || LightDirection.West == direction)))
        {
            if(LightColor.Green == intersection.lightController.GetLightColor())
            {
                visibleLight.color = green;
            }
            else if (LightColor.Yellow == intersection.lightController.GetLightColor())
            {
                visibleLight.color = yellow;
            }//Set light color
        }//If light is supposed to point in same direction as this light or light is a crossways light pointing in same direction as this light
        else if (true)
        {
            visibleLight.color = red;
        }//Else light is red
    }
}
