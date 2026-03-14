using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectRatio : MonoBehaviour
{
    // The aspect ratio you want (e.g., 16/9f = 1.777f)
    public Vector2 targetRatio = new Vector2(16, 9);

    void Start()
    {
        SetCamera();
    }

    void SetCamera()
    {
        Camera camera = GetComponent<Camera>();
        float targetAspectRatio = targetRatio.x / targetRatio.y;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspectRatio;

        if (scaleHeight < 1.0f)
        {
            // Letterbox (black bars on top/bottom)
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            camera.rect = rect;
        }
        else
        {
            // Pillarbox (black bars on sides)
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = camera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
        }
    }
}
