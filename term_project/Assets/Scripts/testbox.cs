using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class testbox : MonoBehaviour
{
    
    
    //private Toggle toggle1;
    //private Toggle toggle2;
   // public GameObject game1;
   // public GameObject game2;
    // 앵커 생성 후 인스턴스 되며 이 클래스 실행

    //경우 정리
    //1. 날씨토글만 체크돼있고 터치  
    //2. 그래프토글만 체크돼있고 터치  
    //3. 두개의 토글이 모두 체크돼있고 터치
    //4. 둘다 언체크
   
    void Start()
    {
       
       // toggle1=GameObject.FindGameObjectWithTag("weathertap").GetComponent<Toggle>();
       // toggle2=GameObject.FindGameObjectWithTag("graphtap").GetComponent<Toggle>();
        //weather 토글이 체크->날씨 시뮬 모델 활성화되고 모델프리팹생성// 언체크->비활성화 모델안만들어짐
        /*if(toggle1.isOn){
            game1.SetActive(true);
                
        }else game1.SetActive(false);//풍차 하나당 제어하는거 잊지마
        //그래프토글체크시 센터 앵커 인위적 추가 후 이 클래스 실행됨 graph 토글 체크-> 그래프 메쉬 생성하는 프리팹 활성화
        if(toggle2.isOn){
            //그래프모드
            game2.SetActive(true);
        }else game2.SetActive(false);
        
          
        if(anchorCreator.AnchorG!=null){
            anchorG=anchorCreator.AnchorG;
            
        }*/
    }
  

    // Update is called once per frame
    void Update()
    {
        
    }
}
