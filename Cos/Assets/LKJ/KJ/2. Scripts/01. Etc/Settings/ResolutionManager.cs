using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    /* 1920x1080 */
    public void SetGameFHD(int width, int height, bool isFullScreen)
    {
        Debug.Log("FHD 해상도로 변경!");
        Screen.SetResolution(1920, 1080, true);
    }

    /* 2560x1440 */
    public void SetGameQHD(int width, int height, bool isFullScreen)
    {
        Debug.Log("QHD 해상도로 변경!");
        Screen.SetResolution(2560, 1440, true);
    }
}
