using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSetup : MonoBehaviour
{
    public RenderTexture renderTexture;
    public GameObject plane;
    [Range(1, 10)]
    public float quality;

    // Start is called before the first frame update
    void Start()
    {
        renderTexture.width = (int)(Screen.width / quality);
        renderTexture.height = (int)(Screen.height / quality);

        Vector3 screenBottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        Vector3 screenTopLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));

        Vector3 screenSizeInUnits = screenBottomRight - screenTopLeft;
        plane.transform.localScale = new Vector3(screenSizeInUnits.x /10, screenSizeInUnits.y/10, 1);
    }
}
