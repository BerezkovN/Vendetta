using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSetup : MonoBehaviour
{
    [SerializeField] public RenderTexture renderTexture;
    [SerializeField] public GameObject plane;
    [SerializeField] public Camera waterCamera;

    [Range(1, 10)]
    public float quality;

    // Start is called before the first frame update
    void Start()
    {
        renderTexture.width = (int)(Screen.width / quality);
        renderTexture.height = (int)(Screen.height / quality);

        Vector3 screenBottomRight = waterCamera.ScreenToWorldPoint(new Vector3(waterCamera.pixelWidth, waterCamera.pixelHeight));
        Vector3 screenTopLeft = waterCamera.ScreenToWorldPoint(new Vector3(0, 0));

        Vector3 screenSizeInUnits = screenBottomRight - screenTopLeft;
        plane.transform.localScale = new Vector3(screenSizeInUnits.x /10, screenSizeInUnits.y/10, 1);
    }
}
