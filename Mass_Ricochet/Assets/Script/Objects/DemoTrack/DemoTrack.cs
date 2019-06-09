using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTrack : MonoBehaviour {

	public GameObject track;
	public float speed=1f;
	private Track tr_tr;
	private Vector3 playerrot;
	private bool is_shot=false;
	private WaitForFixedUpdate fix=new WaitForFixedUpdate();
	//commands--------------------------------------
	//1st command : shot mid speed
	public void Command_1(){
		is_shot=true;
		ShotAction(0.2f);
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
	//when shot
	public void ShotAction(float speed){
		//Play Shot Animation
		track.SetActive(true);
		tr_tr.Shot(speed);
	}
	//After shot
	void OnDisable(){
		if(is_shot)
			tr_tr.Setparent_Again();
		//Play Disable Animation on the Shotted
	}
	//Cache Variables-------------------------------
	void Awake(){
		tr_tr=track.GetComponent<Track>();
		playerrot.z=GetAngle(transform.position,NoteManager.instance.playerPos);
		transform.rotation=Quaternion.Euler(playerrot);
		track.SetActive(false);
	}
	public float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;
        return Mathf.Atan2(v.y, v.x) * 57.29578f+90;
    }
}
