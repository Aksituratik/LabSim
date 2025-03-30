using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
    using System;
public class AddObject : MonoBehaviour
{
	public GameObject Prefab;
	public GameObject Camera1;
	public Camera Camera2;
	private float distance = 1.0f;
	private float max_distance = 2.5f;
	private float min_distance = 0.5f;
	private bool moving = false;
	private bool moving1 = false;
	private GameObject newObject;
	private TMP_Text canvas_ph ;
	private TMP_Text canvas_c ;
	private TMP_Text canvas_m ;
	private TMP_Text canvas_sub ;
	private TMP_Text canvas_ph1 ;
	private TMP_Text canvas_c1 ;
	private TMP_Text canvas_m1 ;
	private TMP_Text canvas_sub1 ;
	private string ph,m,sub,ph1,m1,sub1,c,c1;
	private float volume,volume1;
private GameObject panel,panel1,panel_obj,panel1_obj;
    void Start()
    {
		canvas_ph=GameObject.Find("Canvas/Panel/Ph").GetComponent<TMP_Text>();
		canvas_c=GameObject.Find("Canvas/Panel/C").GetComponent<TMP_Text>();
		canvas_m=GameObject.Find("Canvas/Panel/M").GetComponent<TMP_Text>();
		canvas_sub=GameObject.Find("Canvas/Panel/Substance").GetComponent<TMP_Text>();
				canvas_ph1=GameObject.Find("Canvas/Panel1/Ph").GetComponent<TMP_Text>();
		canvas_c1=GameObject.Find("Canvas/Panel1/C").GetComponent<TMP_Text>();
		canvas_m1=GameObject.Find("Canvas/Panel1/M").GetComponent<TMP_Text>();
		canvas_sub1=GameObject.Find("Canvas/Panel1/Substance").GetComponent<TMP_Text>();
		panel=GameObject.Find("Canvas/Panel");
				panel1=GameObject.Find("Canvas/Panel1");
	}
    void Update()
    {

		float degree= transform.rotation.eulerAngles.y/180*Mathf.PI;
        float degree1= Camera1.transform.rotation.eulerAngles.x/180*Mathf.PI;
		if (moving == false){
			  	  // Создаем луч из позиции камеры в направлении впереди
        Ray ray = Camera2.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
 
    	RaycastHit hit;
       
        // Проверяем, пересекается ли луч с чем-либо на сцене
        if (Physics.Raycast(ray, out hit))
    	{
        	// Если пересечение есть, вы можете получить информацию о нем
        	Transform objectHit = hit.transform;
        	if(objectHit.CompareTag("movable") ){
				panel.gameObject.SetActive(true);
								
			}else{
			panel.gameObject.SetActive(false);	
			}
			
			if(newObject==null && objectHit.CompareTag("movable")){
			panel_obj=objectHit.gameObject;
			}else{panel_obj=newObject;}
        	if(panel_obj!=null){
				
				m="";
				sub="";
				c ="";
				for(int i = 0; i < panel_obj.GetComponent<Composition>().subList.Count; i++){
					string name = panel_obj.GetComponent<Composition>().subList[i].name;
				m=m + Math.Round(panel_obj.GetComponent<Composition>().subList[i].quantity,5)+", ";
				sub=sub + name+", ";
				if(panel_obj.GetComponent<Composition>().subList[i].ind==true){
					c = c+"- , ";
				}else{
				c = c+ panel_obj.GetComponent<Composition>().subList[i].quantity/panel_obj.GetComponent<Composition>().volume*1000/Composition.CountM(name,panel_obj.GetComponent<Composition>().elemList)+", ";
					
				}
				}
				if(panel_obj.GetComponent<Composition>().subList.Count>0){
					m=m.Substring(0, m.Length-2 );
					sub=sub.Substring(0, sub.Length-2 );
					}
				canvas_ph.text="Ph: "+Composition.CountPh(panel_obj);
				canvas_c.text= "C: " + c;
				canvas_m.text="M (гр): " +  m;
				canvas_sub.text=sub;
			}
			if((moving1 == true )&& objectHit.CompareTag("movable")){
			panel1_obj=objectHit.gameObject;}else{panel1_obj=null;}
        	if(panel1_obj!=null){
				if(true){ph1="9";}
				m1="";
				sub1="";
				c1 ="";
				for(int i = 0; i < panel1_obj.GetComponent<Composition>().subList.Count; i++){
					string name = panel1_obj.GetComponent<Composition>().subList[i].name;
				m1=m1 + Math.Round(panel1_obj.GetComponent<Composition>().subList[i].quantity,5)+", ";
				sub1=sub1 + name+", ";
				
				if(panel1_obj.GetComponent<Composition>().subList[i].ind==true){
					c1 = c1+"- , ";
				}else{
				c1 = c1+ panel1_obj.GetComponent<Composition>().subList[i].quantity/panel1_obj.GetComponent<Composition>().volume*1000/Composition.CountM(name,panel1_obj.GetComponent<Composition>().elemList)+", ";
				}
				}
				if(panel1_obj.GetComponent<Composition>().subList.Count>0){
					m1=m1.Substring(0, m1.Length-2 );
					sub1=sub1.Substring(0, sub1.Length-2 );}
					
				canvas_ph1.text="Ph: "+Composition.CountPh(panel1_obj);
				canvas_c1.text= "C: " + c1;
				canvas_m1.text= "M (гр): " + m1;
				canvas_sub1.text=sub1;
			}
        	
        	if(objectHit.CompareTag("movable") && Input.GetMouseButton(0) ){
				//Debug.Log("Попал в объект: " + objectHit.name);
				if( (moving1 == false && moving == false)){
				newObject = objectHit.gameObject;
				newObject.GetComponent<Rigidbody>().isKinematic=true;
				moving1 = true;
			}
			}
			if( Input.GetKey(GetComponent<FirstPersonMovement>().freezeKey) && (moving==true ||  moving1==true)){
				panel.gameObject.SetActive(true);
				if(objectHit.CompareTag("movable") && objectHit.gameObject!=newObject ){
				panel1.gameObject.SetActive(true);
				
				if( Input.GetMouseButtonDown(1) ){
				//Debug.Log(objectHit.gameObject.GetComponent<Composition>().volume  );
				//Debug.Log(newObject.GetComponent<Composition>().volume  );
				volume=0;
				volume1=0;
				for(int i = 0; i < newObject.GetComponent<Composition>().subList.Count; i++){
				volume+=newObject.GetComponent<Composition>().subList[i].quantity;
				}
				for(int i = 0; i < objectHit.gameObject.GetComponent<Composition>().subList.Count; i++){
				volume1+=objectHit.gameObject.GetComponent<Composition>().subList[i].quantity;
				}
				float add_volume;
				string add_name;
				if(volume>=10 && volume1<=170-10){
				for(int i = 0; i < newObject.GetComponent<Composition>().subList.Count; i++){
					add_volume=10*newObject.GetComponent<Composition>().subList[i].quantity/volume;
				newObject.GetComponent<Composition>().subList[i].quantity-=add_volume;
				add_name=newObject.GetComponent<Composition>().subList[i].name;
				if(objectHit.gameObject.GetComponent<Composition>().subList.Exists(x=>x.name == add_name)){
				objectHit.gameObject.GetComponent<Composition>().subList.Find(x=>x.name == add_name).quantity+=add_volume;
				}else{
					Composition.Sub sub_obj = newObject.GetComponent<Composition>().subList[i];
					objectHit.gameObject.GetComponent<Composition>().subList.Add(new Composition.Sub(){name = add_name, quantity = add_volume,PK = sub_obj.PK,is_weak = sub_obj.is_weak,ind_color = sub_obj.ind_color,ind_points = sub_obj.ind_points, ind=sub_obj.ind});
				
				
				}
				}
				//for(int i = 0; i < objectHit.gameObject.GetComponent<Composition>().subList.Count; i++){
				//objectHit.gameObject.GetComponent<Composition>().subList[i].quantity+=10*objectHit.gameObject.GetComponent<Composition>().subList[i].quantity/volume1;
				//}
				
				
				//newObject.GetComponent<Composition>().volume-=10;
				//objectHit.gameObject.GetComponent<Composition>().volume+=10;
			}
			}}else{panel1.gameObject.SetActive(false);}
        	}else{panel1.gameObject.SetActive(false);}
    	}
    	if(Input.GetMouseButtonUp(0) && newObject!=null){
			moving1 = false;
			newObject.GetComponent<Rigidbody>().isKinematic=false;
			newObject=null;
			
		}
    	
			
		}



        
        if (Input.GetKeyDown(KeyCode.F) && moving == false  && moving1 == false){
			moving = true;
        Prefab.GetComponent<Rigidbody>().isKinematic=true;
        //Instantiate(Prefab, new Vector3(-1, 0, 0), Quaternion.identity);
        //Camera.transform.Rotation.x

        newObject = Instantiate(Prefab, transform.position + new Vector3(Mathf.Sin(degree)*Mathf.Cos(degree1), Camera1.transform.position.y-Mathf.Sin(degree1), Mathf.Cos(degree)*Mathf.Cos(degree1))  , Quaternion.identity);
        
        
        //Debug.Log(Camera.transform.position.y);
        //Debug.Log(degree1);
        //Debug.Log();
    	}
    	else if (Input.GetKeyDown(KeyCode.F) && ( moving == true) && newObject != null){
			moving = false;
        newObject.GetComponent<Rigidbody>().isKinematic=false;
		newObject=null;
    	}
    	if((moving1==true || moving == true) && newObject != null && !Input.GetKey(GetComponent<FirstPersonMovement>().freezeKey)){
			float distance_change=(Input.GetAxis("Mouse ScrollWheel"));
			if(distance+distance_change<min_distance){distance=min_distance;}
			else if(distance+distance_change>max_distance){distance=max_distance;}
			else{distance+=distance_change;}
			newObject.transform.position = transform.position + new Vector3(Mathf.Sin(degree)*Mathf.Cos(degree1)*distance, Camera1.transform.position.y-transform.position.y-Mathf.Sin(degree1), Mathf.Cos(degree)*Mathf.Cos(degree1)*distance);
		panel.gameObject.SetActive(true);
		}
    	
    	
    	
    	//Debug.Log();
    	
    }
}
