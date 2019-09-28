using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ScreenShot : MonoBehaviour
{
    string filePath = "C:\\Users\\Jordan\\Desktop\\TestName.png";
    public GameObject canvas;
    public CanvasGroup cg;

    public Camera charCam;
    public Camera canCam;

    bool flash = false;
    float delay = 0;

    enum Flash
    {
        None,
        Delay,
        Flash
    }

    Flash state = Flash.None;
    // Start is called before the first frame update
    void Start()
    {
        canCam.enabled = false;
        charCam.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScreenCapture.CaptureScreenshot(filePath);
            print("Printed screenshot");

            //state = Flash.Delay;
            flash = true;
            cg.alpha = 0.0f;
            delay = 0;
        }

        if (flash)
        {
            if (delay < .1)
            {
                delay += Time.deltaTime;
                if (delay >= .1)
                {
                    charCam.enabled = false;
                    canCam.enabled = true;
                    cg.alpha = 1.0f;

                    Texture2D tex = null;
                    byte[] fileData;

                    if (File.Exists(filePath))
                    {
                        fileData = File.ReadAllBytes(filePath);
                        tex = new Texture2D(2, 2);
                        tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.

                        canvas.GetComponent<Renderer>().material.mainTexture = tex;
                    }

                    print("Rendered Screenshot");
                }

            }
            else
            {
                cg.alpha -= Time.deltaTime * .85f;
                if (cg.alpha <= 0)
                {
                    flash = false;
                }
                    
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.

                canvas.GetComponent<Renderer>().material.mainTexture = tex;
            }

            print("Rendered Screenshot");

        }
    }
}
