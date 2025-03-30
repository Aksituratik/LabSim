using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing;
using System.Linq;
public class Composition : MonoBehaviour
{
	private string line,line1;
	private int start = 1;
	public float bas_volume = 20;
	public float[] add_volume = {20};
	public float volume =0;
	public string[] content={"H2O"};
	public string[] elem;
	public int[] elem_mass;
	public List<Sub> subList = new List<Sub>();
	public List<elements> elemList = new List<elements>();
	
		public class elements{
			public string elem;
			public float elem_mass;}
	
	public class Sub{
			public string name;
			public float quantity;
			public bool ind=false;
			public float[] ind_points={0,0};
			public string[] ind_color={"","",""};
			public bool is_weak = false;
			public float PK;
			public void info(){
				Debug.Log(name);
				Debug.Log(quantity);
			}
			
		}
		
		
		public static float CountM(string name,List<elements> elemList1){
			float add_mass;
			int elem_num=1;
			float mass=0f;
			bool in_bracket = false;
			////Debug.Log(name);
			//mass+=elemList1.Find(x=>x.elem==name[0].ToString()).elem_mass;
			for(int i=0; i<name.Length;i++){
				////Debug.Log(i);
				bool Char_lower=false;
				if(name.Length-1<=i){Char_lower=false;}else{ Char_lower=Char.IsLower(name[i+1]);}
				if( Char_lower && name[i]!='(' && name[i]!=')' ){
					
					add_mass=(elemList1.Find(x=>x.elem==name.Substring(i, 2)).elem_mass);
					if(name.Length>=i+3 && Char.IsDigit(name[i+2])){elem_num=int.Parse(name[i+2].ToString());i++;}else{elem_num=1;}
					//Debug.Log(elemList1.Find(x=>x.elem==name).elem);
					if(in_bracket){elem_num=elem_num*(name[1+name.IndexOf(')')]);}
					mass+=add_mass*elem_num;
					i++;
					}else if (name[i]!='(' && name[i]!=')' ){
						//Debug.Log(name.Substring(i, 1));
						add_mass=elemList1.Find(x=>x.elem==name.Substring(i, 1)).elem_mass;
						//Debug.Log(elem_num);
						
						if(name.Length>=i+2 && Char.IsDigit(name[i+1])){elem_num=int.Parse(name[i+1].ToString());i++;}else{elem_num=1;}
						//Debug.Log( 1*(name[1+name.IndexOf(')')]) );
						if(in_bracket){elem_num=elem_num*( int.Parse(  name[1+name.IndexOf( ')' )].ToString()  ) );}
						mass+=add_mass*elem_num;
						}else{if(name[i]=='('){in_bracket=true;}else if(name[i]==')'){in_bracket=false;i++;}}
				//Debug.Log(add_mass);
				//Debug.Log(elem_num);
				
			}
			Debug.Log(mass);
			return mass;
		}
		
		
	public static string CountPh(GameObject obj){
		float ph=7;
		float ph_volume=0;
		float M;
		int last_ind=-1;
		List<Sub> fluidList=obj.GetComponent<Composition>().subList;
		List<elements> elemList=obj.GetComponent<Composition>().elemList; 
		GameObject liquid=obj.GetComponent<Transform>().Find("water").gameObject;
		//.GetComponent<TMP_Text>()
		

				//obj.GetComponent<Renderer>().material.SetColor("_BaseColor",c);
		
		if(fluidList.Count!=0){
		for(int i=0; i<fluidList.Count;i++){
			ph_volume+=fluidList[i].quantity;
			if (fluidList[i].ind==true){last_ind=i;}
		}
		int H_num=1;
		float[] H_plus= new float[fluidList.Count];
		
		for(int i=0; i<fluidList.Count;i++){
			
			
			
			if(fluidList[i].name!="H2O" && fluidList[i].ind==false){
			if(fluidList[i].name[0]=='H' ) {
				M=CountM(fluidList[i].name,elemList);

				if(fluidList[i].name[1]=='2'){ H_num=2; }else{ H_num=1;}
				
				if(fluidList[i].is_weak){
				H_plus[i]=(float)Math.Pow(10,-(fluidList[i].PK-(float)(Math.Log10(H_num*fluidList[i].quantity/ph_volume*1000/M)))/2);
				}else{
					H_plus[i]=(float)(H_num*fluidList[i].quantity/ph_volume*1000/M);
				}
				
				
				}
			else if(fluidList[i].name.Substring(fluidList[i].name.Length-2,2)=="OH" || fluidList[i].name.Substring(fluidList[i].name.Length-5,4)=="(OH)" ){
				//Debug.Log(fluidList[i].name.Substring(fluidList[i].name.Length-2,2));
				M=CountM(fluidList[i].name,elemList);
				if(fluidList[i].name[fluidList[i].name.Length-1]=='2'){ H_num=2; }else{ H_num=1;}
				if(fluidList[i].is_weak){
					//Debug.Log();
					H_plus[i]=(float)Math.Pow(10,-14+(fluidList[i].PK-(float)(Math.Log10(H_num*fluidList[i].quantity/ph_volume*1000/M)))/2);
				}else{
				H_plus[i]=(float)((Math.Pow(10,-14))/(H_num*fluidList[i].quantity/ph_volume*1000/M));
				}
			}
				
				
				}
				
		}
		if(H_plus.Sum()!=0){ph=(float)-Math.Log10(H_plus.Sum());}else{ph=7;}
		//else{}

		if(last_ind!=-1){
			string htmlColor;
			//Debug.Log(fluidList[last_ind].ind_color.Length);
			if(ph<fluidList[last_ind].ind_points[0]){htmlColor=fluidList[last_ind].ind_color[0];}
			else if(ph<fluidList[last_ind].ind_points[1]){htmlColor=fluidList[last_ind].ind_color[1];}
			else {htmlColor=fluidList[last_ind].ind_color[2];}
		
				
		                     UnityEngine.Color c =  new UnityEngine.Color((float)Convert.ToInt32(htmlColor.Substring(0, 2), 16)/255,
                                       (float)Convert.ToInt32(htmlColor.Substring(2, 2), 16)/255,
                                       (float)Convert.ToInt32(htmlColor.Substring(4, 2), 16)/255,0.6117f);
		
		
		
				liquid.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_BaseColor",c);
		
		
		
			
		}
		
		
		
		
		
		
		
		return ph.ToString();
		}else{return "-";}
		
	}

	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		
		string [] line1_1;
		
		//"Assets/Resources/Mol_Mass.txt"
		using (var sr1 = new StringReader(Resources.Load<TextAsset>("Mol_Mass").text)){
		string elem_mass1;
		while(line1 != null || start == 1){
			start =0;
			line1 = sr1.ReadLine();
			if(line1!=null){
				
				line1_1=line1.Split("	");
				//float.TryParse(line1_1[1]+"f",out elem_mass1);
				//try{
					elem_mass1="1,0078";
   elemList.Add(new elements(){elem=line1_1[0],elem_mass=float.Parse(line1_1[1])});
				
//}catch(Exception e){Debug.Log(line1_1[1]+"f");}
				
				
				//Debug.Log(elemList[elemList.Count-1].elem_mass);
				//Debug.Log(line1_1[1]+"f");
			}
			
			
			//(float )(line1_1[1])
		}
		}
		//float mass=elemList.Find(x=>x.elem==name).elem_mass;
		
		
		
		using (var sr = new StringReader(Resources.Load<TextAsset>("Chem").text)){
        
        start = 1;
        while (line != null || start == 1)
        
    {
		start = 0;
        line = sr.ReadLine();
        string[] split_line;
        if(line!=null){
			split_line=line.Split(" ");
					
			//Debug.Log(split_line[1].Substring(1, split_line[2].Length-1));
			
			//Debug.Log(split_line.Length==2 && Array.IndexOf(content,split_line[0])!=-1);
			if((split_line.Length==1 || split_line.Length==2 )&& Array.IndexOf(content,split_line[0])!=-1 ){
				
				float ind_volume=(add_volume.Length>Array.IndexOf(content,split_line[0]) ? add_volume[ Array.IndexOf(content,split_line[0])]:bas_volume);
			subList.Add(new Sub(){name = split_line[0], quantity = ind_volume});
			if(split_line.Length==2){
				//Debug.Log(split_line[1].Substring(1, split_line[1].Length-1));
				subList[subList.Count-1].is_weak=true;
				subList[subList.Count-1].PK=(float.Parse(split_line[1].Substring(1, split_line[1].Length-1), CultureInfo.InvariantCulture.NumberFormat));
			}
			
			
			} else if(split_line[0]=="ind" && Array.IndexOf(content,split_line[1])!=-1){
				Debug.Log(split_line[0]);	
				//Debug.Log(add_volume.Length);
				//Debug.Log(Array.IndexOf(content,split_line[1]));
				float ind_volume=(add_volume.Length>Array.IndexOf(content,split_line[1]) ? add_volume[ Array.IndexOf(content,split_line[1])]:bas_volume);
				subList.Add(new Sub(){name = split_line[1], quantity = ind_volume});
				subList[subList.Count-1].ind=true;
				
				string[] str_ind_points=split_line[2].Substring(1, split_line[2].Length-2).Split(",");
				//float[] a =new float[float.Parse(str_ind_points[0], CultureInfo.InvariantCulture.NumberFormat),float.Parse(str_ind_points[1], CultureInfo.InvariantCulture.NumberFormat)];
				//Debug.Log(split_line[2].Substring(1, split_line[2].Length-1));
				subList[subList.Count-1].ind_points=new float[2]{float.Parse(str_ind_points[0], CultureInfo.InvariantCulture.NumberFormat),float.Parse(str_ind_points[1], CultureInfo.InvariantCulture.NumberFormat)};
				subList[subList.Count-1].ind_color=split_line[3].Substring(1, split_line[3].Length-2).Split(",");
				}
			//subList[subList.Count-1].info();
			}
        }
        }
        //Debug.Log(subList[0].info());
        
        
        string ph = CountPh(gameObject);
        
     }
       
    

    // Update is called once per frame
    void Update()
    {
		//gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor",new UnityEngine.Color(255,0,0,1) );
		volume=0;
       for(int i=0; i<subList.Count;i++){
		volume+=subList[i].quantity;
	   }
       
    }
}
