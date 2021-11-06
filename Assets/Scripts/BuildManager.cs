using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_STANDALONE
public class BuildManager : MonoBehaviour
{
    public Camera mainCamera;
    public float aspect;
    
    // Start is called before the first frame update
    void Start()
    {
        aspect = (float)4 / 3;
        mainCamera.aspect = aspect;
        Screen.SetResolution(1280, 960, FullScreenMode.Windowed);
        //Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape")) { Application.Quit(); }
    }
}

#endif
