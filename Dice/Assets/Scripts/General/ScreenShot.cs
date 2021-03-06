// ******  Notice : It doesn't works in Web Player environment.  ******
// ******    It works in PC environment.                         ******
// Default method have some problem, when you take a Screen shot for your game. 
// So add this script.
// CF Page : http://technology.blurst.com/unity-jpg-encoding-javascript/
// made by Jerry ( sdragoon@nate.com )

using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class ScreenShot : MonoBehaviour {
    public string imageNamePrefix;
    private bool canTakeScreenShot = true;

    private int count;
    private string path;

    void Start() {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        path = Application.dataPath + "/Screen Shots";

        try {
            if(!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            count = Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly).Length;
        } catch(Exception e) {
            print(e.ToString());
            enabled = false;
        }
#else
        enabled = false;
#endif
    }

    void Update() {
        if(Input.GetAxis("Screen Shot") != 0 && canTakeScreenShot) {
            canTakeScreenShot = false;
            StartCoroutine(ScreenshotEncode());
        } else if(Input.GetAxis("Screen Shot") == 0) {
            canTakeScreenShot = true;
        }
    }

    IEnumerator ScreenshotEncode() {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        // wait for graphics to render
        yield return new WaitForEndOfFrame();

        // create a texture to pass to encoding
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);

        // put buffer into texture
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        // split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;

        byte[] bytes = texture.EncodeToPNG();

        // save our test image (could also upload to WWW)
        File.WriteAllBytes(path + "/" + imageNamePrefix + count + ".png", bytes);
        count++;

        print("Took Screen Shot");

        // Added by Karl. - Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
        DestroyObject(texture);
#else
        return null;
#endif
    }
}