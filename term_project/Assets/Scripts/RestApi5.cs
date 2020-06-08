using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class RestApi5 : MonoBehaviour
{
    
    private static RestApi5 _instance;
    public static RestApi5 Instance
    {
        get
        {
            if (_instance ==null)
            {
                _instance =FindObjectOfType<RestApi5>();
                if(_instance == null)
                {
                    GameObject go = new GameObject();
                    go.name =typeof(RestApi5).Name;
                    _instance=go.AddComponent<RestApi5>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public IEnumerator GetWeather5(string Url, System.Action<RootObject4> callBack)
    {
        string url=UnityWebRequest.EscapeURL(Url);
      
        using (UnityWebRequest www = UnityWebRequest.Get(Url))
        {
            //www.chunkedTransfer = false;
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                 _ShowAndroidToastMessage("에러: API5 요청 거부");
                
            }
            else
            {
                if (www.isDone)
                {
                      _ShowAndroidToastMessage("API 5요청 승인");
                    string jsonResult = 
                        System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    
                    //Item[] entities =
                    //   JsonHelper.getJsonArray<Item>(jsonResult);
                   // Debug.Log(jsonResult);
                   // Debug.Log("끝");
                    try{RootObject4 itemss = JsonUtility.FromJson<RootObject4>(jsonResult);
                    callBack(itemss);    }
                    catch{
                          _ShowAndroidToastMessage("에러:  잘못된 input5");
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
