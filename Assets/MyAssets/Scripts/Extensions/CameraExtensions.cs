using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraExtensions
{
    public static Vector2 OrthographicBoundsVector2(this Camera camera)
    {
        Vector2 bounds = Vector2.zero;
        Bounds _bounds = OrthographicBounds(camera);
        bounds.x = _bounds.extents.x;
        bounds.y = _bounds.extents.y;
#if UNITY_EDITOR
        Debug.Log("Bounds: " + bounds);
#endif
        return bounds;
    }

    public static Bounds OrthographicBounds(this Camera camera)
    {
#if UNITY_EDITOR
        float screenAspect = (float)Screen.width / (float)Screen.height;
#else
        float screenAspect = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
#endif
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}