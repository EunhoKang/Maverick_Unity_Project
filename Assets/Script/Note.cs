using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {
	private float speed=10f;
	//Update NoteSpeed
	void Awake(){
		speed=NoteManager.instance.NoteSpeed;
	}
	//Falling Note
	void Update(){
		transform.Translate(Vector3.down*speed*Time.deltaTime);
	}
	//SendMessage to NoteManager
	public void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Perfect")){
			NoteManager.instance.Judge(true);
			gameObject.SetActive(false);
			//Debug.Log(Time.time); //use it to check falling time
			//Debug.Log("PERFECT");
		}else if(other.CompareTag("Good")){
			NoteManager.instance.Judge(false);
			gameObject.SetActive(false);
			//Debug.Log("GOOD");
		}else if(other.CompareTag("Miss")){
			gameObject.SetActive(false);
			NoteManager.instance.combo_manage(0);
			//Debug.Log("MISS");
		}
	}
}
