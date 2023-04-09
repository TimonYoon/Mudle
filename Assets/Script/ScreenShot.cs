using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShot : MonoBehaviour
{
    Coroutine coroutine;
    public void OnClickTakeScreenShotAndShare()
    {
        if (coroutine != null)
            return;
        coroutine = StartCoroutine(TakeScreenshot());
    }
    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);

        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);

        screenTexture.Apply();

        byte[] dataToSave = screenTexture.EncodeToPNG();
        string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
        File.WriteAllBytes(destination, dataToSave);

        Destroy(screenTexture);
        if (!Application.isEditor)
        {
            //new NativeShare().AddFile(destination).SetSubject("Subject goes here").SetText("Hellow Meowdle! \nThank you for sharing :) \n" + "#MEOWDLE https://play.google.com/store/apps/details?id=com.blackout.plojectA").Share();
            //AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            //AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            //intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));


            //AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");

            //// 디바이스 파일에서 캡쳐 이미지를 찾는다.
            //AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "content://" + destination);
            //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

            //intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "https://play.google.com/store/apps/details?id=com.blackout.plojectA");
            //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "MEOWDLE Share");
            //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "MEOWDLE");

            //intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            //AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            //AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            //AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");

            //Debug.Log("MEOWDLE _ Debug ( END )");
            //currentActivity.Call("startActivity", jChooser);

        }
        coroutine = null;
    }
}