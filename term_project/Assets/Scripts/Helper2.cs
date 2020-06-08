using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

[System.Serializable]
 public class Header2
{
    public string resultCode ;
    public string resultMsg ;
}
[System.Serializable]
public class Item2
{
    public string baseDate ;
    public string baseTime ;
    public string category;
    public string fcstDate ;
    public string fcstTime;
    public string fcstValue;
    public int nx ;
    public int ny ;
}
[System.Serializable]
public class Items2
{
    public List<Item2> item ;
}
[System.Serializable]
public class Body2
{
    public string dataType;
    public Items2 items ;
    public int pageNo ;
    public int numOfRows;
    public int totalCount ;
}
[System.Serializable]
public class Response2
{
    public Header2 header;
    public Body2 body ;
}
[System.Serializable]
public class RootObject2
{
    public Response2 response ;
}
