using UnityEngine;


public class WaterLevel : MonoBehaviour
{
	private float volume;
	private float kolb_volume = 170f;
	private float prev_time=0f;
	private float this_time, goal;
	//GameObject obj = gameObject;
	
	Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("add");
    }

    // Update is called once per frame
    void Update()
    {
		
//volume/kolb_volume
if(volume!=GetComponent<Composition>().volume){
	
	volume=GetComponent<Composition>().volume;}

		this_time=animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		goal = volume/kolb_volume;
        if(prev_time < goal && goal <= this_time){animator.SetFloat("sp",0);}
        else if(goal > this_time){animator.SetFloat("sp",1);}
        else if(goal < this_time){animator.SetFloat("sp",-1);}
        prev_time = this_time;
        //animator.GetCurrentAnimatorStateInfo(0).length
        
        //Debug.Log(volume);
    }
}
