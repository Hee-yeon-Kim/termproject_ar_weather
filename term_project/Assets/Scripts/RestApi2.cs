using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class RestApi2 : MonoBehaviour
{
    private static RestApi2 _instance;
    public static RestApi2 Instance
    {
        get
        {
            if (_instance ==null)
            {
                _instance =FindObjectOfType<RestApi2>();
                if(_instance == null)
                {
                    GameObject go2 = new GameObject();
                    go2.name =typeof(RestApi2).Name;
                    _instance=go2.AddComponent<RestApi2>();
                    DontDestroyOnLoad(go2);
                }
            }
            return _instance;
        }
    }

     public IEnumerator GetWeather2(string Url, System.Action<RootObject2> callBack)
    {
        string url=UnityWebRequest.EscapeURL(Url);
      
        using (UnityWebRequest www = UnityWebRequest.Get(Url))
        {
            //www.chunkedTransfer = false;
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                 _ShowAndroidToastMessage("에러: API 요청 거부");
                
            }
            else
            {
                if (www.isDone)
                {
                      _ShowAndroidToastMessage("API 요청 승인");
                    string jsonResult = 
                        System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    
                    //Item[] entities =
                    //   JsonHelper.getJsonArray<Item>(jsonResult);
                   // Debug.Log(jsonResult);
                   // Debug.Log("끝");
                    try{RootObject2 itemss = JsonUtility.FromJson<RootObject2>(jsonResult);
                    callBack(itemss);    }
                    catch{
                        _ShowAndroidToastMessage("에러:  잘못된 input");
                    }
                         
                }
                //ddlCountries.options.AddRange(entities.
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
     /// 토스트 띄우기
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity =
                unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>(
                            "makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }
}
