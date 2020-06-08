using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  
[System.Serializable]
public class GetAddress
{
    public int cdX;
    public int cdY;
    public string address1;
    public string address2;
    
}
[System.Serializable]
public class Header
{
    public string resultCode ;
    public string resultMsg ;
}
[System.Serializable]
public class Item
{
    public string baseDate ;
    public string baseTime ;
    public string category ;
    public int nx ;
    public int ny ;
    public string obsrValue ;
}
[System.Serializable]
public class Items
{
    public List<Item> item ;
}
[System.Serializable]
public class Body
{
    public string dataType ;
    public Items items ;
    public int pageNo ;
    public int numOfRows ;
    public int totalCount ;
}
[System.Serializable]
public class Response
{
    public Header header ;
    public Body body ;
}
[System.Serializable]
public class RootObject
{
    public Response response;
}