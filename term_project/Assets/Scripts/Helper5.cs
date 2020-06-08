
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

[System.Serializable]
public class AirList
{
    public string _returnType;
    public string coGrade;
    public string coValue;
    public string dataTerm;
    public string dataTime;
    public string khaiGrade;
    public string khaiValue;
    public string mangName;
    public string no2Grade;
    public string no2Value;
    public string numOfRows;
    public string o3Grade;
    public string o3Value;
    public string pageNo;
    public string pm10Grade;
    public string pm10Grade1h;
    public string pm10Value;
    public string pm10Value24;
    public string pm25Grade;
    public string pm25Grade1h;
    public string pm25Value;
    public string pm25Value24;
    public string resultCode;
    public string resultMsg;
    public int rnum;
    public string serviceKey;
    public string sidoName;
    public string so2Grade;
    public string so2Value;
    public string stationCode;
    public string stationName;
    public string totalCount;
    public string ver;
}
[System.Serializable]
public class Parm2
{
    public string _returnType;
    public string coGrade;
    public string coValue;
    public string dataTerm;
    public string dataTime;
    public string khaiGrade;
    public string khaiValue;
    public string mangName;
    public string no2Grade;
    public string no2Value;
    public string numOfRows;
    public string o3Grade;
    public string o3Value;
    public string pageNo;
    public string pm10Grade;
    public string pm10Grade1h;
    public string pm10Value;
    public string pm10Value24;
    public string pm25Grade;
    public string pm25Grade1h;
    public string pm25Value;
    public string pm25Value24;
    public string resultCode;
    public string resultMsg;
    public int rnum;
    public string serviceKey;
    public string sidoName;
    public string so2Grade;
    public string so2Value;
    public string stationCode;
    public string stationName;
    public string totalCount;
    public string ver;
}
[System.Serializable]
public class ArpltnInforInqireSvcVo
{
    public string _returnType;
    public string coGrade;
    public string coValue;
    public string dataTerm;
    public string dataTime;
    public string khaiGrade;
    public string khaiValue;
    public string mangName;
    public string no2Grade;
    public string no2Value;
    public string numOfRows;
    public string o3Grade;
    public string o3Value;
    public string pageNo;
    public string pm10Grade;
    public string pm10Grade1h;
    public string pm10Value;
    public string pm10Value24;
    public string pm25Grade;
    public string pm25Grade1h;
    public string pm25Value;
    public string pm25Value24;
    public string resultCode;
    public string resultMsg;
    public int rnum;
    public string serviceKey;
    public string sidoName;
    public string so2Grade;
    public string so2Value;
    public string stationCode;
    public string stationName;
    public string totalCount;
    public string ver;
}
[System.Serializable]
public class RootObject4
{
    public List<AirList> list;
    public Parm2 parm;
    public ArpltnInforInqireSvcVo ArpltnInforInqireSvcVo;
    public int totalCount;
}