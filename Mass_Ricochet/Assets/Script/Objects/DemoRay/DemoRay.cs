using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoRay : MonoBehaviour {
	public GameObject ray;
	private Ray ray_ray;
	private Vector3 playerrot;
	private bool is_shot=false;
	private WaitForFixedUpdate fix=new WaitForFixedUpdate();
	//commands--------------------------------------
	//1st command : shot ray
	void Command_1(){
		is_shot=true;
		ShotAction(0.1f);
	}
	//Passive---------------------------------------
	//Before shot
	void Start(){
		//start begin animation
		StartCoroutine(track_player());
	}
	IEnumerator track_player(){
		while(!is_shot){
			playerrot.z=GetAngle(transform.position,NoteManager.instance.playerPos);
			transform.rotation=Quaternion.Euler(playerrot);
			yield return fix;
		}
	}
	//When Shot
	public void ShotAction(float time){
		//Play Shot Animation
		ray.SetActive(true);
		ray_ray.Shot(time);
	}
	//After Shot
	void OnDisable(){
		ray_ray.Setparent_Again();
		//Play Disable Animation on the Shotted
	}
	//Cache Variables-------------------------------
	void Awake(){
		ray_ray=ray.GetComponent<Ray>();
		playerrot.z=GetAngle(transform.position,NoteManager.instance.playerPos);
		transform.rotation=Quaternion.Euler(playerrot);
		ray.SetActive(false);
	}
	public float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;
        return Mathf.Atan2(v.y, v.x) * 57.29578f+90;
    }
}
