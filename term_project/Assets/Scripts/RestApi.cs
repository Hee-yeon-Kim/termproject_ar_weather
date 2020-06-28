using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class RestApi : MonoBehaviour
{
    
    private static RestApi _instance;
    public static RestApi Instance
    {
        get
        {
            if (_instance ==null)
            {
                _instance =FindObjectOfType<RestApi>();
                if(_instance == null)
                {
                    GameObject go = new GameObject();
                    go.name =typeof(RestApi).Name;
                    _instance=go.AddComponent<RestApi>();
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

    public IEnumerator GetWeather(string Url, System.Action<RootObject> callBack)
    {
        string url=UnityWebRequest.EscapeURL(Url);
      
        using (UnityWebRequest www = UnityWebRequest.Get(Url))
        {
            //www.chunkedTransfer = false;
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                 _ShowAndroidToastMessage("네트워크 문제 또는 API 제공가능시간이 아니므로 현재 날씨의 API 요청이 거부되었습니다.");
                
            }
            else
            {
                if (www.isDone)
                {
                      _ShowAndroidToastMessage("API 요청 중");
                    string jsonResult = 
                        System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    
                    //Item[] entities =
                    //   JsonHelper.getJsonArray<Item>(jsonResult);
                   // Debug.Log(jsonResult);
                   // Debug.Log("끝");
                    try{RootObject itemss = JsonUtility.FromJson<RootObject>(jsonResult);
                    callBack(itemss);    }
                    catch{
                        _ShowAndroidToastMessage("지금은 일시적으로 현재 날씨 정보를 받아올 수 없습니다.");
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
