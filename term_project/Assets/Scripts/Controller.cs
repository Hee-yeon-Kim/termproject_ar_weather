using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
# if PLATFORM_ANDROID
using UnityEngine.Android;//for permission
# endif

/*
강수형태 pty
습도 reh
1시간 강수량 rn1
기온 섭씨 T1H
동서바람성분 UUU
풍향 VEC
남북바람성분 VVV
풍속 WSD
*/

[System.Serializable]
public class weather{
        public int state=0;
        public int inttime=0;
        public string date="1111";
        public string time="1111";
        public float hum=0.0f;
        public float temp=0.0f;
        public float EWwind=0.0f;
        public float SNwind=0.0f;
        public int form=0;
        public int windD=0;
        public float windV=0.0f;
        public float water=0.0f; 
        public float hightemp=0.0f;
        public float lowtemp=0.0f;
        public int sky=0;
        public int rainpercent=0;
        public int index;
 
 }

public class Controller : MonoBehaviour
{
  
    private string WEB_URL="";
    private string WEB_URL2="";
    private string WEB_URL3="";
    private string WEB_URL4="";
    private string WEB_URL5="";
     public weather show_weather;
    public weather[] weatherlist;
    public AirList now_air;
    public Dropdown dropdown1;
    public TextAsset myTxt;
    public Button okaybtn1;
    public Button okaybtn2;
    public InputField textinput;

   
    private List<int> selectXY;
    private List<Dictionary<string, object>> dataList;  

//UI    
    [SerializeField] private Text TapAddress;
    [SerializeField] private Text TapDate;
    [SerializeField] private Toggle tog1;
    [SerializeField] private Toggle tog2;
    [SerializeField] private Toggle tog3;
    [SerializeField]  private Toggle tog4;
    [SerializeField] private Toggle tog5;
    [SerializeField] private Toggle tog6;
    [SerializeField] private Toggle tog7;
    [SerializeField] private Toggle tog8;
    [SerializeField] private Toggle tog9;
    [SerializeField] private Toggle tog10;
    [SerializeField] private Toggle tog11;
    [SerializeField] private Toggle tog12;//for demo
    [SerializeField] private Toggle tog13;//for demo
    [SerializeField] private Toggle tog14;//for demo
    [SerializeField] private Text test;
    [SerializeField] private Sprite sun;
    [SerializeField] private Sprite rain;
    [SerializeField] private Sprite rainorsnow;
    [SerializeField] private Sprite snow;
    [SerializeField] private Sprite sudden; 
    [SerializeField] private Sprite cloud;
    [SerializeField] private Sprite suncloud;
    [HideInInspector] public Toggle[] togglelist;
    [HideInInspector] public bool updating=false;
 
    private string hourForURL1;
    private string hourForURL2;
    private string todaydateForURL;
    private string address1;
    private string address2;
    private string address3;
    private string rightdate;
    private string righthour;
    private string rightmin;



//gps 관련 변수
        double RE = 6371.00877; // 지구 반경(km)
        double GRID = 5.0; // 격자 간격(km)
        double SLAT1 = 30.0; // 투영 위도1(degree)
        double SLAT2 = 60.0; // 투영 위도2(degree)
        double OLON = 126.0; // 기준점 경도(degree)
        double OLAT = 38.0; // 기준점 위도(degree)
        double XO = 43; // 기준점 X좌표(GRID)
        double YO = 136; // 기1준점 Y좌표(GRID)
        bool gpsInit = false;
    // Start is called before the first frame update
   private void Start()
    {
        weatherlist = new weather[14];
        now_air=new AirList();
        for( int i=0; i<14; i++)
        {
            weatherlist[i]= new weather();
        }

        dataList=CSVReader.Read(myTxt);
        selectXY=new List<int>();
        dropdown1.ClearOptions();
        okaybtn1.onClick.AddListener(clickokaybtn1);
        okaybtn2.onClick.AddListener(clickokaybtn2);
        StartCoroutine("GetUserLocation");


    }

    public void clickokaybtn1(){
        List<string> options1=new List<string>();
        if(dropdown1.options.Count!=0) dropdown1.ClearOptions();
        if(selectXY.Count!=0){selectXY.Clear();selectXY=new List<int>();} 
        string select1=textinput.text;
        for(var i=0;i<dataList.Count;i++)
        {
            if((dataList[i]["AD3"].ToString()).Equals(select1)){
                string tmp=dataList[i]["AD1"].ToString()+" "+dataList[i]["AD2"].ToString();
                selectXY.Add(i);
                options1.Add(tmp);
            }
            
        }
        dropdown1.AddOptions(options1);
        
    }

    public void clickokaybtn2(){

        int tmp_value=dropdown1.value;
        int index=selectXY[tmp_value];

        address1=dataList[index]["AD1"].ToString();
        address2=dataList[index]["AD2"].ToString();
        address3=dataList[index]["AD3"].ToString();
        string cdX= dataList[index]["COx"].ToString();
        string cdY= dataList[index]["COy"].ToString();
        setTime();

        WEB_URL="http://apis.data.go.kr/1360000/VilageFcstInfoService/getUltraSrtNcst?serviceKey=w6hMMFdaqKyFWywg%2F0XFBLk3dXev9ujSBriCDatFX6mLTgINou81cYHgyQUR0nsA2sTTmneuZ1oRRjZ%2FzmSfWA%3D%3D&numOfRows=9&pageNo=1"+
                            "&dataType=JSON"+
                            "&base_date="+todaydateForURL+
                            "&base_time="+hourForURL1+"00"+
                            "&nx="+cdX+"&ny="+cdY;
                
        WEB_URL2="http://apis.data.go.kr/1360000/VilageFcstInfoService/getVilageFcst?serviceKey=w6hMMFdaqKyFWywg%2F0XFBLk3dXev9ujSBriCDatFX6mLTgINou81cYHgyQUR0nsA2sTTmneuZ1oRRjZ%2FzmSfWA%3D%3D&numOfRows=100&pageNo=1"+
                            "&dataType=JSON"+
                            "&base_date="+todaydateForURL+
                            "&base_time="+hourForURL2+"00"+
                            "&nx="+cdX+"&ny="+cdY;
        WEB_URL3="http://openapi.airkorea.or.kr/openapi/services/rest/MsrstnInfoInqireSvc/getTMStdrCrdnt?umdName="
        +address3+"&pageNo=1&numOfRows=10&_returnType=json&ServiceKey=w6hMMFdaqKyFWywg%2F0XFBLk3dXev9ujSBriCDatFX6mLTgINou81cYHgyQUR0nsA2sTTmneuZ1oRRjZ%2FzmSfWA%3D%3D";

        StartCoroutine(RestApi.Instance.GetWeather(WEB_URL,GetnowItems));
               
        StartCoroutine(RestApi2.Instance.GetWeather2(WEB_URL2,GetnowItems2));
        settingDemo();
        Show();
        
        string tmp=address1+"/"+address2+"/"+address3+"로 변경되었습니다.";
        _ShowAndroidToastMessage(tmp);

        StartCoroutine(RestApi3.Instance.GetWeather3(WEB_URL3,GetnowItems3));
 
    }
    void settingDemo(){
        weatherlist[11].date="20000319";
        weatherlist[11].form=1;//rain
        weatherlist[11].sky=3;//흐림
        weatherlist[11].temp=3;
        weatherlist[11].windV=30;//강한바람
        weatherlist[11].time="1400";
        weatherlist[11].water=5;
        weatherlist[11].state=1;

        weatherlist[12].date="20000320";
        weatherlist[12].form=0;//맑음
        weatherlist[12].sky=3;//흐림
        weatherlist[12].temp=9;
        weatherlist[12].windV=4;//바람
        weatherlist[12].time="1400";
        weatherlist[12].water=0;
        weatherlist[12].state=1;

        weatherlist[13].date="20000321";
        weatherlist[13].form=4;//rain
        weatherlist[13].sky=2;//약간흐림
        weatherlist[13].temp=11;
        weatherlist[13].windV=70;//강한바람
        weatherlist[13].time="1400";
        weatherlist[13].water=0;
        weatherlist[13].state=1;
        
    }

    private void Show()
    {
         TapAddress.text=address1+" "+address2+"\r\n"+address3;
         TapDate.text= rightdate+"\r\n"+righthour+"시 "+rightmin+"분 기준";
         show_weather=weatherlist[0]; show_weather.index=0;
         
         togglelist = new Toggle[14]{tog1,tog2,tog3,tog4,tog5,tog6,tog7,tog8,tog9,tog10,tog11,tog12,tog13,tog14};
         int i=0;
         int temp_count=0;
         int temp_int=0;
         foreach(Toggle tog in togglelist)
         {
             //string tmp = weatherlist[i].temp.ToString();
             //tog.GetComponentsInChildren<Text>()[0].text=tmp+"도";
            if(i==0)
            {
                tog.GetComponentsInChildren<Text>()[0].text="현재";
            }
            else if(i>10)
            {
                tog.GetComponentsInChildren<Text>()[0].text="데모용";
            }
            else
            {
                temp_int=int.Parse( weatherlist[i].time.Substring(0,2));
                if(temp_int==0)
                {
                    if(temp_count==0)
                    { tog.GetComponentsInChildren<Text>()[0].text="내일 "+temp_int.ToString()+"시"; temp_count++;}
                    else
                    tog.GetComponentsInChildren<Text>()[0].text="내일모레 "+temp_int.ToString()+"시";
                }
                else
                {
                    tog.GetComponentsInChildren<Text>()[0].text=temp_int.ToString()+"시";
                }
            }
            if(weatherlist[i].form==0){
                
                switch(weatherlist[i].sky){
                case 1: tog.GetComponentInChildren<Image>().sprite=sun; break;
                case 2: tog.GetComponentInChildren<Image>().sprite=suncloud; break;
                case 3: tog.GetComponentInChildren<Image>().sprite=suncloud; break;
                case 4: tog.GetComponentInChildren<Image>().sprite=cloud; break;
                default: break;
                }
            }
            else{
             switch(weatherlist[i].form){
                case 0: tog.GetComponentInChildren<Image>().sprite=sun; break;
                case 1: tog.GetComponentInChildren<Image>().sprite=rain; break;
                case 2: tog.GetComponentInChildren<Image>().sprite=rainorsnow; break;
                case 3: tog.GetComponentInChildren<Image>().sprite=snow; break;
                case 4: tog.GetComponentInChildren<Image>().sprite=sudden; break;
                default: break;
            }
            

        }
             int tmp2=i;
             togglelist[i].onValueChanged.AddListener((t)=> ToggleClick(tmp2));
             i++;
         }
    }

    public void ToggleClick( int i){

        show_weather=weatherlist[i];
        show_weather.index=i;
        //test.text=show_weather.date;//테스트
        if(i==0) show_weather.state=0; 
        else show_weather.state=1;
        updating=true;

    }

/// <summary>
///  위치 gps 허가를 받기 위함
/// </summary>
    IEnumerator GetUserLocation()
    {
        yield return new WaitForEndOfFrame();
        if(!Input.location.isEnabledByUser)
        {
           Permission.RequestUserPermission(Permission.FineLocation);
            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => Application.isFocused == true);
            if(Input.location.isEnabledByUser)
            {
                StartCoroutine("GetLatLonUsingGPS");
            }
            else{
                _ShowAndroidToastMessage("위치 인식거부: 돋보기 아이콘을 눌러 직접 장소를 지정해주세요.");
                yield break;
            }
        }
        else
        {
            StartCoroutine("GetLatLonUsingGPS");
        }
                    
        /* if( !Input.location.isEnabledByUser ) //FIRST IM CHACKING FOR PERMISSION IF "true" IT MEANS USER GAVED PERMISSION FOR USING LOCATION INFORMATION
        {
           // statusTxt.text = "No Permission";
           Permission.RequestUserPermission(Permission.FineLocation);
      
        }
           else
        {
            //statusTxt.text = "Ok Permission";
            StartCoroutine("GetLatLonUsingGPS");
        }*/
    }
    void setTime(){

                string right_now=DateTime.Now.ToString(("HHmm"));
                string todaydate=DateTime.Now.ToString(("yyyyMMdd"));
                todaydateForURL=todaydate;
                rightdate=DateTime.Now.ToString(("yyyy-MM-dd"));
                righthour=DateTime.Now.ToString(("HH"));
                rightmin=DateTime.Now.ToString(("mm"));
                int m = int.Parse(rightmin);
                int h = int.Parse(righthour);
                
                DateTime tmp_d;
                int tmp_h=h;
                int tmp_h2=(h+1)/3;

               if(m<=30) {
                  tmp_h=h-1;
                   if(tmp_h<10){hourForURL1="0"+tmp_h.ToString();}
                   else {hourForURL1=tmp_h.ToString();}
               } else {hourForURL1=tmp_h.ToString();}

               //WEB 동네예보
                
                switch(tmp_h2)
                {
                    case 0://h=0,1
                         tmp_d=DateTime.Now.AddDays(-1);
                         todaydateForURL=tmp_d.ToString(("yyyyMMdd"));
                         hourForURL2="23";
                        
                        break;
                   case 1://h=2,3,4
                        if(h==2&&m<10){
                            tmp_d=DateTime.Now.AddDays(-1);
                            todaydateForURL=tmp_d.ToString(("yyyyMMdd"));
                            hourForURL2="23";
                            
                        }
                        else{
                            hourForURL2="02";
                        }
                         
                        break;
                   case 2://h=5,6,7
                         if(h==5&&m<10){
                             
                            hourForURL2="02";
                        }
                        else{
                            hourForURL2="05";
                        }
                       
                        break;
            
                   case 3://h=8,9,10
                        if(h==8&&m<10){
                             
                            hourForURL2="05";
                        }
                        else{
                            hourForURL2="08";
                        }
                      

                        break;
                   case 4://11,12,13
                         if(h==11&&m<10){
                            
                           hourForURL2="08";
                        }
                        else{
                            hourForURL2="11";
                        }
                     
                        break;
                   case 5://14.15.16
                        if(h==14&&m<10){
                            
                            hourForURL2="11";
                        }
                        else{
                            hourForURL2="14";}
                           
                        break;
                   case 6://17,18,19
                     if(h==17&&m<10){
                            
                            hourForURL2="14";
                        }
                        else{
                            hourForURL2="17";
                        }
                         
                        break;
                   case 7://20.21.22
                     if(h==20&&m<10){
                            
                            hourForURL2="17";
                        }
                        else{
                            hourForURL2="20";
                        }
                        
                        break;
                   case 8://23.24
                     if(h==23&&m<10){
                            
                            hourForURL2="20";
                        }
                        else{
                            hourForURL2="23";
                        }
                          
                        break;
                    default: hourForURL2="02"; break;
                }
    }

    IEnumerator GetLatLonUsingGPS()
    {
        Input.location.Start();
        int maxWait = 5;
            while( Input.location.status == LocationServiceStatus.Initializing && maxWait > 0 )
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            if( maxWait < 1 )
            {
                //statusTxt.text = "Failed To Iniyilize in 10 seconds";
                yield break;
            }
            if( Input.location.status == LocationServiceStatus.Failed )
            {
               // statusTxt.text = "Failed To Initialize";
                yield break;
            }
            else
            {
                double log = Input.location.lastData.longitude;
                double lat  = Input.location.lastData.latitude;
                setTime();

                //WEB 실황단기예보
                Dictionary<string, double> LatLngToXY = new Dictionary<string, double>(); 
                
                LatLngToXY =dfs_xy_conf("toXY",lat,log);      

                WEB_URL="http://apis.data.go.kr/1360000/VilageFcstInfoService/getUltraSrtNcst?serviceKey=w6hMMFdaqKyFWywg%2F0XFBLk3dXev9ujSBriCDatFX6mLTgINou81cYHgyQUR0nsA2sTTmneuZ1oRRjZ%2FzmSfWA%3D%3D&numOfRows=9&pageNo=1"+
                            "&dataType=JSON"+
                            "&base_date="+todaydateForURL+
                            "&base_time="+hourForURL1+"00"+
                            "&nx="+LatLngToXY["x"].ToString()+"&ny="+LatLngToXY["y"].ToString();
                
                WEB_URL2="http://apis.data.go.kr/1360000/VilageFcstInfoService/getVilageFcst?serviceKey=w6hMMFdaqKyFWywg%2F0XFBLk3dXev9ujSBriCDatFX6mLTgINou81cYHgyQUR0nsA2sTTmneuZ1oRRjZ%2FzmSfWA%3D%3D&numOfRows=100&pageNo=1"+
                            "&dataType=JSON"+
                            "&base_date="+todaydateForURL+
                            "&base_time="+hourForURL2+"00"+
                            "&nx="+LatLngToXY["x"].ToString()+"&ny="+LatLngToXY["y"].ToString();
              

                yield return StartCoroutine(RestApi.Instance.GetWeather(WEB_URL,GetnowItems));
               
                yield return StartCoroutine(RestApi2.Instance.GetWeather2(WEB_URL2,GetnowItems2));

                for(var i=0;i<dataList.Count;i++)
                {
                    if((dataList[i]["COx"].ToString()).Equals(LatLngToXY["x"].ToString())){
                        if((dataList[i]["COy"].ToString()).Equals(LatLngToXY["y"].ToString()))
                        {
                            address1=dataList[i]["AD1"].ToString();
                            address2=dataList[i]["AD2"].ToString();
                            address3=dataList[i]["AD3"].ToString();
                        }
                    }
                    
                }
                 Input.location.Stop();
                 settingDemo();
                 Show();

                 WEB_URL3="http://openapi.airkorea.or.kr/openapi/services/rest/MsrstnInfoInqireSvc/getTMStdrCrdnt?umdName="
                         +address3+"&pageNo=1&numOfRows=10&ServiceKey=w6hMMFdaqKyFWywg%2F0XFBLk3dXev9ujSBriCDatFX6mLTgINou81cYHgyQUR0nsA2sTTmneuZ1oRRjZ%2FzmSfWA%3D%3D&_returnType=json";

                 yield return StartCoroutine(RestApi3.Instance.GetWeather3(WEB_URL3,GetnowItems3));
            }      
    }


    // Update is called once per frame
    void Update()
    {
        //_UpdateApplicationLifecycle();
    }

    Dictionary<string, double> dfs_xy_conf(string code, double v1, double v2)
    {
            double DEGRAD = System.Math.PI / 180.0;
            double RADDEG = 180.0 / System.Math.PI;

            double re = RE / GRID;
            double slat1 = SLAT1 * DEGRAD;
            double slat2 = SLAT2 * DEGRAD;
            double olon = OLON * DEGRAD;
            double olat = OLAT * DEGRAD;

            double sn = System.Math.Tan((System.Math.PI * 0.25f + slat2 * 0.5f)) / System.Math.Tan(System.Math.PI * 0.25f + slat1 * 0.5f);
            sn = System.Math.Log(System.Math.Cos(slat1) / System.Math.Cos(slat2)) / System.Math.Log(sn);
            double sf = System.Math.Tan(System.Math.PI * 0.25f + slat1 * 0.5f);
            sf = System.Math.Pow(sf, sn) * System.Math.Cos(slat1) / sn;
            double ro = System.Math.Tan(System.Math.PI * 0.25f + olat * 0.5f);
            ro = re * sf / System.Math.Pow(ro, sn);

            Dictionary<string, double> rs = new Dictionary<string, double>();
            double ra, theta;

            if (code == "toXY")
            {
                rs["lat"] = v1;
                rs["lng"] = v2;
                ra = System.Math.Tan(System.Math.PI * 0.25f + (v1) * DEGRAD * 0.5f);
                ra = re * sf / System.Math.Pow(ra, sn);
                theta = v2 * DEGRAD - olon;
                if (theta > System.Math.PI) theta -= 2.0f * System.Math.PI;
                if (theta < -System.Math.PI) theta += 2.0f * System.Math.PI;
                theta *= sn;
                rs["x"] = System.Math.Floor(ra * System.Math.Sin(theta) + XO + 0.5f);
                rs["y"] = System.Math.Floor(ro - ra * System.Math.Cos(theta) + YO + 0.5f);
                
            }
            else
            {
                rs["x"] = v1;
                rs["y"] = v2;
                double xn = v1 - XO;
                double yn = ro - v2 + YO;
                ra = System.Math.Sqrt(xn * xn + yn * yn);
                if (sn < 0.0f) ra = -ra;
                double alat = System.Math.Pow((re * sf / ra), (1.0f/ sn));
                alat = 2.0f * System.Math.Atan(alat) - System.Math.PI * 0.5f;

                if (System.Math.Abs(xn) <= 0.0)
                {
                    theta = 0.0f;
                }
                else
                {
                    if (System.Math.Abs(yn) <= 0.0)
                    {
                        theta = System.Math.PI * 0.5f;
                        if (xn < 0.0f) theta = -theta;
                    }
                    else theta = System.Math.Atan2(xn, yn);
                }
                double alon = theta / sn + olon;
                rs["lat"] = alat * RADDEG;
                rs["lng"] = alon * RADDEG;
            }
            return rs;
    }
    public void GetnowItems3(RootObject3 root){
        //test.text=root.list[0].sggName;
        string tmx="";
        string tmy="";
        for(int i=0; i<root.list.Count;i++) 
        {
            if((root.list[i].sidoName).Equals(address1)){
                tmx=root.list[i].tmX;
                tmy=root.list[i].tmY;
               
                break;
            }
            if(i==root.list.Count-1){
                _ShowAndroidToastMessage("미세먼지 정보를 원하시면 탐색버튼을 눌러 다른 읍/면/동을 선택해 주세요.");
               
                return;
            }
        }
        
        WEB_URL4="http://openapi.airkorea.or.kr/openapi/services/rest/MsrstnInfoInqireSvc/getNearbyMsrstnList?"+
        "tmX="+tmx+"&tmY="+tmy+"&_returnType=json&ServiceKey=w6hMMFdaqKyFWywg%2F0XFBLk3dXev9ujSBriCDatFX6mLTgINou81cYHgyQUR0nsA2sTTmneuZ1oRRjZ%2FzmSfWA%3D%3D";
        StartCoroutine(RestApi3.Instance.GetWeather3(WEB_URL4,GetnowItems4));
         
          
    }
    public void GetnowItems4(RootObject3 root){

        string station=root.list[0].stationName;
    
        WEB_URL5="http://openapi.airkorea.or.kr/openapi/services/rest/ArpltnInforInqireSvc/getMsrstnAcctoRltmMesureDnsty?"
        +"stationName="+station+"&dataTerm=daily&pageNo=1&numOfRows=50&ServiceKey=w6hMMFdaqKyFWywg%2F0XFBLk3dXev9ujSBriCDatFX6mLTgINou81cYHgyQUR0nsA2sTTmneuZ1oRRjZ%2FzmSfWA%3D%3D&ver=1.3&_returnType=json";
        StartCoroutine(RestApi5.Instance.GetWeather5(WEB_URL5,GetnowItems5));
        
    }
    public void GetnowItems5(RootObject4 root){
      
        now_air=root.list[0];
       
    }
    public void GetnowItems(RootObject root){
     
        foreach(Item itemm in root.response.body.items.item)
        {
               
            
            //초단기 실황
            switch(itemm.category){

                //강수형태
                case "PTY" :
                    try
                    {
                        weatherlist[0].form= int.Parse(itemm.obsrValue);
                        
                    }
                    catch (FormatException e)
                    {
                        Debug.Log(e.Message);return;
                    }
                    weatherlist[0].date=itemm.baseDate;
                    weatherlist[0].time=itemm.baseTime;
                break;

                //습도 %
                case "REH" :
                    try
                    { 
                        weatherlist[0].hum= int.Parse(itemm.obsrValue);
                    }
                    catch (FormatException e)
                    {
                        Debug.Log(e.Message);return;
                    }
                    
                break;
                //1시간 강수량 mm
                case "RN1" :
                    try
                    {
                        weatherlist[0].water= float.Parse(itemm.obsrValue);
                    }
                    catch (FormatException e)
                    {
                        Debug.Log(e.Message);return;
                    }
                break;

                //기온 섭씨
                case "T1H" :
                try
                    {
                        weatherlist[0].temp= float.Parse(itemm.obsrValue);
                    }
                    catch (FormatException e)
                    {
                        Debug.Log(e.Message);return;
                    }
                break;

                //동서바람성분
                case "UUU" :
                try
                    {
                        weatherlist[0].EWwind=float.Parse(itemm.obsrValue);
                    }
                    catch (FormatException e)
                    {
                        Debug.Log(e.Message);return;
                    }
                break;

                //풍향
                case "VEC" :
                try
                    {
                        weatherlist[0].windD=int.Parse(itemm.obsrValue);
                    }
                    catch (FormatException e)
                    {
                        Debug.Log(e.Message);return;
                    }
                break;

                //남북바람성분
                case "VVV" :
                try
                    {
                        weatherlist[0].SNwind=float.Parse(itemm.obsrValue);
                    }
                    catch (FormatException e)
                    {
                        Debug.Log(e.Message);return;
                    }
                break;

                //풍속
                case "WSD" :
                try
                    {
                        weatherlist[0].windV= float.Parse(itemm.obsrValue);
                    }
                    catch (FormatException e)
                    {
                        Debug.Log(e.Message);
                     return;
                    }
                break;

                default :
                // Debug.Log("NO_DATA!!");
                break;
            
            }


        }
     
    }
    public void GetnowItems2(RootObject2 root)
    {
          //동네예보정보분류
        
         int i=1;
        
        foreach(Item2 itemm in root.response.body.items.item){
           
            switch(itemm.category)
            {
                case "POP": weatherlist[i].rainpercent=int.Parse(itemm.fcstValue); 
                            weatherlist[i].date=itemm.fcstDate;
                            weatherlist[i].time=itemm.fcstTime;
                            break;
                case "PTY": weatherlist[i].form=int.Parse(itemm.fcstValue); break;
                case "REH": weatherlist[i].hum=float.Parse(itemm.fcstValue); break;
                case "SKY": weatherlist[i].sky=int.Parse(itemm.fcstValue); break;
                case "T3H": weatherlist[i].temp=float.Parse(itemm.fcstValue); break;
                case "UUU": weatherlist[i].EWwind=float.Parse(itemm.fcstValue); break;
                case "VEC": weatherlist[i].windD=int.Parse(itemm.fcstValue); break;
                case "VVV": weatherlist[i].SNwind=float.Parse(itemm.fcstValue); break;
                case "WSD": weatherlist[i].windV=float.Parse(itemm.fcstValue);
                            if(int.Parse(weatherlist[i].date)%6!=0)//R06은 6시간간격으로만 나오므로 
                               if(i==1) weatherlist[i].water=weatherlist[i+1].water;
                               else weatherlist[i].water=weatherlist[i-1].water;
                            i++; 
                            break;
                case "R06": weatherlist[i].water=float.Parse(itemm.fcstValue); break;
                case "TMN" : weatherlist[i].lowtemp=float.Parse(itemm.fcstValue); break;
                case "TMX" : weatherlist[i].hightemp=float.Parse(itemm.fcstValue); break;
                default: break;
                             
            }
            if(i==11) break;
           
        }
        if(weatherlist[0].form==0) weatherlist[0].sky=weatherlist[1].sky;             
    }  
    
    
    /*private void _UpdateApplicationLifecycle()
    {
            //종료 Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // 대기 or 진행 Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;// 화면이 꺼지지 않도록 prevent screen dimming
            }

            if (m_IsQuitting) //에러 때문에 종료되는 상황
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to
            // appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);// 0.5f time 후에 _DoQuit 함수 실행하라
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage(
                    "ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
     }*/
    private void OnDisable()
    {
            StopAllCoroutines();
    }
    private void _DoQuit()
    {
        Application.Quit();
    }
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
