﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScreenShot : MonoBehaviour
{
    string filePath = "TestName.png";
    public CanvasGroup cg;
    public UnityEngine.UI.Image theImage;

    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController firstPerson;
    public Camera charCam;
    public Camera canCam;

    public Sprite[] suffering;
    public UnityEngine.UI.Image[] images;

    bool flash = false;
    bool display = false;
    float delay = 0;
    float picTime = 0;

    int corruption = 0;


    // Start is called before the first frame update
    void Start()
    {
        canCam.enabled = false;
        charCam.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !display)
        {
            ScreenCapture.CaptureScreenshot(filePath);
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
                    firstPerson.EnableMove(false);
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

                        theImage.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }

                    picTime = 3f;
                    display = true;

                    for (int i = 0; i < images.Length; i++)
                    {
                        images[i].sprite = suffering[i];
                    }
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

        if (display)
        {
            picTime -= Time.deltaTime;

            if (picTime <= 0)
            {
                firstPerson.EnableMove(true);
                display = false;
                charCam.enabled = true;
                canCam.enabled = false;
            }
        }

    }
}
