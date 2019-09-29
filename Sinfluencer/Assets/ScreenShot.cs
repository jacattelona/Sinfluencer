using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.IO;
using UnityEngine.Rendering.PostProcessing;

public class ScreenShot : MonoBehaviour
{
    string filePath = "";
    public CanvasGroup cg;
    public UnityEngine.UI.Image theImage;

    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController firstPerson;
    public Camera charCam;
    public Camera canCam;

    public Sprite[] suffering;
    public UnityEngine.UI.Image[] images;

    public UnityEngine.Audio.AudioMixer mixer;

    bool flash = false;
    bool display = false;
    float delay = 0;
    float picTime = 0;

    float corruption = 0f;
    public PostProcessVolume volume;
    ChromaticAberration chroma;
    Vignette vignette;
    LensDistortion whoops;
    Bloom bloomboy;

    public CanvasGroup pfordayquil;


    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.dataPath + "\\TestName.png";
        canCam.enabled = false;
        charCam.enabled = true;
        suffering = Resources.LoadAll<Sprite>("");
        mixer.SetFloat("ChorusDepth", corruption);
        volume.profile.TryGetSettings(out chroma);
        volume.profile.TryGetSettings(out vignette);
        volume.profile.TryGetSettings(out whoops);
        volume.profile.TryGetSettings(out bloomboy);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !display)
        {
            ScreenCapture.CaptureScreenshot(filePath);
            pfordayquil.alpha = 0f;
            flash = true;
            cg.alpha = 0.0f;
            delay = -1f;
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

                    if (System.IO.File.Exists(filePath))
                    {
                        fileData = System.IO.File.ReadAllBytes(filePath);
                        tex = new Texture2D(2, 2);
                        tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.

                        theImage.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }

                    picTime = 3f;
                    display = true;

                    foreach(UnityEngine.UI.Image i in images)
                    {
                        int rand = Random.Range(0, suffering.Length);
                        i.sprite = suffering[rand];
                        RandomizeImage(i);
                    }
                    UpdateMusic();
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
            if (Input.GetMouseButtonDown(0))
            {
                firstPerson.EnableMove(true);
                display = false;
                charCam.enabled = true;
                canCam.enabled = false;
                pfordayquil.alpha = 1f;
            }
        }

    }

    void UpdateMusic()
    {
        corruption += .125f;
        mixer.SetFloat("ChorusDepth", corruption);
        chroma.intensity.value = corruption;
        vignette.intensity.value = corruption;
        whoops.intensity.value += 3;
        bloomboy.intensity.value += 2;
    }

    void RandomizeImage(UnityEngine.UI.Image i)
    {
        RectTransform rect = i.GetComponent<RectTransform>();
        rect.Rotate(new Vector3(0, 0, Random.Range(-45, 45)));
        rect.anchoredPosition = new Vector2(Random.Range(-400, 400), Random.Range(-150, 150));
    }
}
