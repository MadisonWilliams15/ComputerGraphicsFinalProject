using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float panSpeed = 20;//Camera speed
    public float scrollSpeed = 20;//Speed of zooming
    public float panBorderThickness = 30;//Border to allow camera movement with mouse
    public Vector2 panLimit;//Border of game space
    public float zoomMin = 1;//Minimum camera size
    public float zoomLimit = 20;//maximum camera size value

    void Update()
    {
        Vector3 pos = transform.position;
        Camera cam = GetComponent<Camera>();

        if (Input.GetKey("w") || Input.GetKey("up") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.GetKey("down") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.GetKey("left") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.GetKey("right") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }//Updates camera position if mouse near border, wasd, or arrow keys being used


        cam.orthographicSize += -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, -panLimit.y, panLimit.y);//Clamp camera position to game border
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, zoomMin, zoomLimit); 

        transform.position = pos;
    }
}
