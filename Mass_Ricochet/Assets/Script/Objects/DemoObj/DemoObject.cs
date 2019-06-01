using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoObject : MonoBehaviour {
	public GameObject saw;
	private Saw saw_saw;
	private Vector3 playerrot;
	private bool is_shot=false;
	private WaitForFixedUpdate fix=new WaitForFixedUpdate();
	//commands--------------------------------------
	//1st command : shot mid speed
	void Command_1(){
		is_shot=true;
		ShotAction(0.5f);
	}
	//Passive---------------------------------------
	//Before Shot
	void Start(){
		//Play Start Animation
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
	public void ShotAction(float speed){
		//Play Shot Animation
		saw.SetActive(true);
		saw_saw.Shot(speed);
	}
	//After Shoted
	void OnDisable(){
		saw_saw.Setparent_Again();
		//Play Disable Animation on the Shotted
	}
	//Cache Variables-------------------------------
	void Awake(){
		saw_saw=saw.GetComponent<Saw>();
		playerrot.z=GetAngle(transform.position,NoteManager.instance.playerPos);
		transform.rotation=Quaternion.Euler(playerrot);
		saw.SetActive(false);
	}
	public float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;
        return Mathf.Atan2(v.y, v.x) * 57.29578f+90;
    }
}
