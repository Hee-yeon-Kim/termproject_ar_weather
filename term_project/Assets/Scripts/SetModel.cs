using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SetModel : MonoBehaviour
{
     public GameObject weatherPrefab;
     public ParticleSystem Raincontrol;
    // private Text butt;//테스트용
     public TextMesh Signtext;
     public ParticleSystem Cloudcontrol;
     public ParticleSystem heatdistortion;
     public ParticleSystem groundfog;
     private weather show;
     private Controller controller;
     public Animator animator;
    PlaceOnPlane Place;
    private Toggle[] togglelist2;
   
     
   

    private void Start() {
  
        
        
        controller=GameObject.FindGameObjectWithTag("controller").GetComponent<Controller>();
        Place = GameObject.FindGameObjectWithTag("origin").GetComponent<PlaceOnPlane>();
        show=new weather(); 
        show=controller.show_weather;
        
        togglelist2 = new Toggle[11];
        togglelist2=controller.togglelist;

        for(int i=0;i<togglelist2.Length;i++)
        {
            togglelist2[i].onValueChanged.AddListener(ToggleClick2);
        }
        
        updating();
                   
    }
    
    
  
    public void ToggleClick2(bool val){
       
        if(Place.updateon)
        {
              
            while(true)
            {
                if(controller.updating)
                { show=controller.show_weather; controller.updating=false;break;}
            }
             
            updating();
             
        }
        else return;
    }

    public void updating(){
        string raintext="";
        string cloudtext="";
        string windtext="";
        string temptext = "";
        
        Raincontrol.Clear();
        Cloudcontrol.Clear();
       
        
        //비세팅
        var rainemi=Raincontrol.emission;
        var rainmain=Raincontrol.main;
        
        switch(show.form){
            case 0: rainemi.enabled=false; raintext="맑음"; break;
            case 1: //rain 
            case 4: //소나기
            
                    rainemi.enabled=true;
                    float waterhere=show.water;
                    if(show.state==1) waterhere=show.water/6;

                        if(waterhere<1)
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(3,6);
                            rainemi.rateOverTime=15; 
                            raintext="약한 비"+"("+waterhere+"mm"+")\n";
                        }
                        else if(waterhere<5)
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(5,8);
                            rainemi.rateOverTime=20;
                            raintext="약한 비"+"("+waterhere+"mm"+")";
                        }
                        else if(waterhere<10)
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(5,8);
                            rainemi.rateOverTime=50;
                            raintext="비"+"("+waterhere+"mm"+")";
                        }
                        else if(waterhere<20)
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(6,9);
                            rainemi.rateOverTime=50;
                             raintext="비"+"("+waterhere+"mm"+")";
                        }
                        else if(waterhere<40)
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(5,9);
                            rainemi.rateOverTime=100;
                             raintext="강한 비"+"("+waterhere+"mm"+")";
                        }
                        else if(waterhere<70)
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(7,11);
                            rainemi.rateOverTime=100;
                        }
                        else
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(8,16);
                            rainemi.rateOverTime=150;
                        }
                    
                    
                   
                    break;
            case 2://비 눈
                    raintext="비와 눈" ;
                    rainemi.enabled=true;
                    rainmain.startSpeed= new ParticleSystem.MinMaxCurve(4,8);
                    rainemi.rateOverTime=20;
                    
                    break;
            case 3://눈
                     raintext="눈";
                    rainemi.enabled=false;
                    break;
            default: raintext=""; break;

        }//form 세팅완료

        //구름 세팅시작
        var cloudmain=Cloudcontrol.main;
        var cloudemi=Cloudcontrol.emission;
        var fogmain = groundfog.main;
        var fogemi = groundfog.emission;
        switch (show.sky){
            
            case 1: cloudemi.enabled=false; cloudtext="구름없음"; fogemi.enabled = false; break;
            case 2: cloudemi.enabled=true; cloudmain.loop=true; cloudemi.rateOverTime=3; cloudtext="구름거의없음"; fogemi.enabled = false; break;
            case 3: cloudemi.enabled=true; cloudmain.loop=true; cloudemi.rateOverTime=8; cloudtext="구름많음"; fogemi.enabled = false; break;
            case 4: cloudemi.enabled=true; cloudmain.loop=true; cloudemi.rateOverTime=18;  cloudtext="흐림"; fogemi.enabled = true; fogmain.loop = true;  break;
            default: cloudtext=""; break;

        }//sky:cloud 세팅완료

        //온도-폭염 heat distortion
        var heatmain = heatdistortion.main;
        var heatemi = heatdistortion.emission;
        if (show.temp > 30)
        {
            heatemi.enabled = true;
            heatmain.loop = true;
            temptext = "폭염주의";
        }
        else
        {
            heatemi.enabled = false;
            
            temptext = show.temp.ToString()+"도";
        }
        //heat완료

         


        //바람세팅시작
        if (show.windV<3) {animator.speed=0; windtext="무풍";}
         else if(show.windV<5.4) {animator.speed=0.2f; windtext="미풍";}
         else if(show.windV<7.9){animator.speed=0.8f; windtext="약풍";}
         else if(show.windV<10.7){ animator.speed=1.7f; windtext="강풍";}//이제 강한바람
         else if(show.windV<13.8) {animator.speed=2.5f;windtext="강풍";}
         else {animator.speed=3.6f;windtext="강풍주의";}//강풍주의보
 
           // Color temp_color1= new Color(0,163,255,255);
            //Color temp_color2= new Color(136,200,243,255);
           // Color temp_color3= new Color(0,163,255,255);
           // var tempd=Skyrend.materials[0];
            /*tempd.SetColor("_Color1",temp_color1);
            tempd.SetColor("_Color2",temp_color2);
            tempd.SetColor("_Color3",temp_color3);*/
        //표지판 세팅
        
        int date1=int.Parse(show.date.Substring(4,2));
        
        int date2=int.Parse(show.date.Substring(6,2));
         
        int time=int.Parse(show.time.Substring(0,2));
         
        Signtext.text=date1.ToString()+"월 "+date2.ToString()+"일\r\n"+time.ToString()+"시\r\n"+temptext+' '+raintext+' '+cloudtext+' '+windtext;
        //표지판 세팅 완료
        
    }
 
    private void OnDisable()
    {
         
    }

    
}
