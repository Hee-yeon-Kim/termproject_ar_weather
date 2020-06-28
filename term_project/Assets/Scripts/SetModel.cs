using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
 
public class SetModel : MonoBehaviour
{
    public GameObject weatherPrefab;
    public ParticleSystem Raincontrol;
    public TextMesh Signtext;
    public ParticleSystem heatdistortion;
    private weather show;
    private Controller controller;
    public Animator animator;
    PlaceOnPlane Place;
    public Light mylight;
    public GameObject moon;
    public GameObject Lowcloud;
    public GameObject Highcloud;
    public ParticleSystem groundfog;
    public ParticleSystem groundfog2;
    private Material cloudmat1;
    private Material cloudmat2;
    public GameObject game;
    public GameObject Spring;
    public GameObject Summer;
    public GameObject Fall;
    public GameObject Winter;
    public Material treeA;
    public Material treeW;
    public Material treeS;
    public Material skymat;
    public Animator characteranim;
    bool summer = false;
    private void Start() {
   
        controller=GameObject.FindGameObjectWithTag("controller").GetComponent<Controller>();
        Place = GameObject.FindGameObjectWithTag("origin").GetComponent<PlaceOnPlane>();
        show =new weather(); 
        show=controller.show_weather;
        cloudmat1=Lowcloud.GetComponent<MeshRenderer>().material;
        cloudmat2 = Highcloud.GetComponent<MeshRenderer>().material;
        
        updating();
                   
    }
    private void Update()
    {
        if (Place.updateon) return;
        
        if (controller.TriggergamemodeOn)
        {
            characteranim.SetTrigger("cheer");
            controller.TriggergamemodeOn = false;
            game.SetActive(true);

        }
        if (controller.TriggergamemodeOff)
        {
            controller.TriggergamemodeOff = false;
            foreach(ParticleSystem par in game.GetComponentsInChildren<ParticleSystem>())
            {
                par.Clear();
            }
            game.SetActive(false);
        }
         
        if ( controller.updating)//다중모드가 아닐때 사용자가 볼 시간을 바꿨다면 updating
        {
            show = controller.show_weather; 
            controller.updating = false;
            updating();
        }
        

        return;
    }


    public void updating(){
        string raintext="";
        string cloudtext="";
        string windtext="";
        string temptext = "";
        Raincontrol.Clear();
        groundfog.Clear();
        groundfog2.Clear();
        heatdistortion.Clear();
        Summer.SetActive(false);
        Fall.SetActive(false);
        Winter.SetActive(false);
        Spring.SetActive(false);
        characteranim.gameObject.transform.localPosition = new Vector3(1.78f, 0.0f, 0.0f);
        characteranim.gameObject.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
     

        int date1 = int.Parse(show.date.Substring(4, 2));
;
        int date2 = int.Parse(show.date.Substring(6, 2));

        int time = int.Parse(show.time.Substring(0, 2));

        

        //월에 따른 풍경 세팅
        if (6 <= date1 && date1 <= 8) // 여름
        {
            treeA.SetFloat("_SwitchColumn", 0);
            treeA.SetFloat("_SwitchRow", 0);
            Summer.SetActive(true);
            summer = true;

        }
        else if (date1 == 12 || date1 <= 2)//겨울
        {
            treeW.SetFloat("_SwitchColumn", 0);
            treeW.SetFloat("_SwitchRow", 0);
            Winter.SetActive(true);
            characteranim.SetTrigger("spin");
        }
        else if (date1 >= 9 && date1 <= 11)//가을
        {
            treeA.SetFloat("_SwitchColumn", 1);
            treeA.SetFloat("_SwitchRow", 0);

            Fall.SetActive(true);
            characteranim.SetTrigger("dizzy");
        }
        else//봄
        {
            treeS.SetFloat("_SwitchColumn", 0);
            treeS.SetFloat("_SwitchRow", 1);
            characteranim.SetTrigger("cheer");

            Spring.SetActive(true);
        }

        //시간에 따른 하늘세팅
        if (time < 6 || time > 18)//밤
        {
            mylight.intensity = 0.7f;
            moon.SetActive(true);
            skymat.SetTextureOffset("_MainTex", new Vector2(0.05f, 0));
        }
        else if (6 <= time && time <= 15)//아침&낮
        {
            mylight.intensity = 1.5f;
            moon.SetActive(false);
            skymat.SetTextureOffset("_MainTex", new Vector2(0.6f, 0));

        }
        else// 늦은 오후
        {
            mylight.intensity = 1.0f;
            skymat.SetTextureOffset("_MainTex", new Vector2(0.4f, 0));
            moon.SetActive(false);
        }

        //현재 미세먼지
        string airquality = "";
        var finedustemi = groundfog2.emission;
        var finedustmain = groundfog2.main;
        finedustemi.enabled = false;
        if (show.state==0&&controller.now_air!=null)
        {
            int todayair = int.Parse(controller.now_air.pm10Grade);
            switch (todayair)
            {
                case 1:
                    airquality = "오늘> 미세먼지-좋음";
                    break;
                case 2:
                    airquality = "오늘> 미세먼지-보통";
                    break;
                case 3:
                    airquality = "오늘> 미세먼지-나쁨";
                    finedustemi.enabled = true;
                    finedustmain.loop = true;
                    break;
                case 4:
                    airquality = "오늘> 미세먼지-매우나쁨";
                    finedustemi.enabled = true;
                    finedustmain.loop = true;
                    break;
            }
            int todayair2 = int.Parse(controller.now_air.pm25Grade);
            switch (todayair2)
            {
                case 1:
                    airquality += "&초미세먼지-좋음\n";
                    break;
                case 2:
                    airquality += "&초미세먼지-보통\n";
                    break;
                case 3:
                    airquality += "&초미세먼지-나쁨\n";
                    break;
                case 4:
                    airquality += "&초미세먼지-매우나쁨\n";
                    break;
            }
            int nowair = int.Parse(controller.now_air.pm10Grade1h);
            switch (nowair)
            {
                case 1:
                    airquality += "현재> 미세먼지-좋음";
                    break;
                case 2:
                    airquality += "현재> 미세먼지-보통";
                    break;
                case 3:
                    airquality += "현재> 미세먼지-나쁨";
                    finedustemi.enabled = true;
                    finedustmain.loop = true;
                    break;
                case 4:
                    airquality += "현재> 미세먼지-매우나쁨";
                    finedustemi.enabled = true;
                    finedustmain.loop = true;
                    break;
            }
            int nowair2 = int.Parse(controller.now_air.pm25Grade1h);
            switch (nowair2)
            {
                case 1:
                    airquality += "&초미세먼지-좋음\n";
                    break;
                case 2:
                    airquality += "&초미세먼지-보통\n";
                    break;
                case 3:
                    airquality += "&초미세먼지-나쁨\n";
                    finedustemi.enabled = true;
                    finedustmain.loop = true;
                    break;
                case 4:
                    airquality += "&초미세먼지-매우나쁨\n";
                    finedustemi.enabled = true;
                    finedustmain.loop = true;
                    break;
            }

        }
      
        //비세팅
        var rainemi=Raincontrol.emission;
        var rainmain=Raincontrol.main;
        
        switch(show.form){
            case 0: rainemi.enabled=false; raintext="맑음";
                if (summer)
                {
                    if(time%2==0)characteranim.SetTrigger("happy");
                    else characteranim.SetTrigger("dancing");
                }
                break;
            case 1: //rain 
            case 4: //소나기
                    rainemi.enabled=true;
                    float waterhere=show.water;
                    if(show.state==1) waterhere=show.water/6;

                        if(waterhere<1)
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(3,6);
                            rainemi.rateOverTime=60; 
                            raintext="약한 비";
                            if (summer) characteranim.SetTrigger("lookingaround");
                        }
                        else if(waterhere<5)
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(5,8);
                            rainemi.rateOverTime=80;
                            raintext="약한 비"+"("+ string.Format("{0:0.##}", waterhere) + "mm"+")";
                             if (summer) characteranim.SetTrigger("lookingaround");
                        }
                        else if(waterhere<20)
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(5,8);
                            rainemi.rateOverTime=200;
                            raintext="비"+"("+ string.Format("{0:0.##}", waterhere) + "mm"+")";
                             if (summer) characteranim.SetTrigger("offensive");
                         }
                        else if(waterhere<30)
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(6,9);
                            rainemi.rateOverTime=300;  
                             raintext="강한 비"+"("+ string.Format("{0:0.##}", waterhere) + "mm"+")";
                             if (summer) characteranim.SetTrigger("offensive");
                        }
                        else if(waterhere<40)
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(5,9);
                            rainemi.rateOverTime=400;
                             raintext="강한 비"+"("+ string.Format("{0:0.##}", waterhere) + "mm"+")";
                            if (summer) characteranim.SetTrigger("offensive");
                         }
                        else if(waterhere<70)
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(7,11);
                            rainemi.rateOverTime=500;
                            raintext = "폭우" + "(" + string.Format("{0:0.##}", waterhere) + "mm" + ")";
                             if (summer) characteranim.SetTrigger("offensive");
                           }
                        else
                        {
                            rainmain.startSpeed= new ParticleSystem.MinMaxCurve(8,16);
                            rainemi.rateOverTime=500;
                            raintext = "폭우" + "(" + string.Format("{0:0.##}", waterhere) + "mm" + ")";
                             if (summer) characteranim.SetTrigger("offensive");
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
            default: raintext="";
                    break;

        }//form 세팅완료

        //구름 세팅시작
        
        var fogmain = groundfog.main;
        var fogemi = groundfog.emission;
        
        cloudmat1.SetFloat("_Speed", 0.37f); cloudmat2.SetFloat("_Speed", 0.37f);
        Color ccolor = cloudmat1.color;
        Color ccolor2 = cloudmat2.color;
        fogemi.enabled = false;
        switch (show.sky){
            
            case 1:  cloudtext="구름없음"; ccolor.a=  0f;  ccolor2.a =  0f;  mylight.intensity *= 1.1f; cloudmat1.SetFloat("_Density", 2.0f); cloudmat2.SetFloat("_Density", 2.0f); break;
            case 2:  cloudtext = "구름거의없음"; ccolor.a = 150.0f; ccolor2.a = 150f; cloudmat1.SetFloat("_Density", 1.8f); cloudmat2.SetFloat("_Density", 1.8f); break;
            case 3:  cloudtext="구름많음"; ccolor.a = 255.0f; ccolor2.a = 255f; mylight.intensity *= 0.9f; cloudmat1.SetFloat("_Density", 1.5f); cloudmat2.SetFloat("_Density", 1.5f); break;
            case 4:  cloudtext="구름많음"; ccolor.a = 255.0f; ccolor2.a = 255f; cloudmat1.SetFloat("_Density", 1.5f); cloudmat2.SetFloat("_Density", 1.5f);  mylight.intensity *= 0.8f; fogemi.enabled = true; fogmain.loop = true;  break;
            default: cloudtext=""; break;

        }
        cloudmat1.color = ccolor;
        cloudmat2.color = ccolor2;
        //sky:cloud 세팅완료

        //온도-폭염 heat distortion
        var heatmain = heatdistortion.main;
        var heatemi = heatdistortion.emission;
        if (show.temp > 33)
        {
            heatemi.enabled = true;
            heatmain.loop = true;
            if(show.temp>35) temptext = string.Format("{0:0.#}", show.temp) + "도 "+"폭염경보";
            else temptext = string.Format("{0:0.#}", show.temp) + "도 " + "폭염주의";
            characteranim.SetTrigger("dizzy");
        }
        else
        {
            heatemi.enabled = false;
            temptext = string.Format("{0:0.#}", show.temp) + "도";
        }
        //heat완료

         


        //바람세팅시작
        if (show.windV<3) {animator.speed=0; windtext="무풍"; treeA.SetFloat("_MotionSpeed", 0); treeA.SetFloat("_MotionRange", 0); }
         else if(show.windV<5.4) {animator.speed=0.2f; windtext="미풍"; treeA.SetFloat("_MotionSpeed", 0.57f); treeA.SetFloat("_MotionRange", 1); }
        else if(show.windV<7.9){animator.speed=0.8f; windtext="약풍"; treeA.SetFloat("_MotionSpeed", 0.57f); treeA.SetFloat("_MotionRange", 1); }
        else if(show.windV<10.7){ animator.speed=1.7f; windtext="강풍"; treeA.SetFloat("_MotionSpeed", 1.8f); treeA.SetFloat("_MotionRange", 1.8f); }//이제 강한바람
        else if(show.windV<13.8) {animator.speed=2.5f;windtext="강풍"; treeA.SetFloat("_MotionSpeed", 2.4f); treeA.SetFloat("_MotionRange", 2.0f); }
        else {animator.speed=3.6f;windtext="강풍주의"; treeA.SetFloat("_MotionSpeed", 2.99f); treeA.SetFloat("_MotionRange", 2.25f); }//강풍주의보

        Signtext.text = date1.ToString() + "월 " + date2.ToString() + "일\r\n" + time.ToString() + "시\r\n" + temptext + "\n" + raintext + ' ' + cloudtext + ' ' + windtext;
        if(show.state==0) Signtext.text+="\n" + airquality;
        //표지판 세팅 완료
    }

    private void OnDisable()
    {
         
    }
    

}
