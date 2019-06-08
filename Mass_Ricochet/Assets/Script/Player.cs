
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private bool invincible;
	private Vector3 mouse;
	private WaitForFixedUpdate tptime=new WaitForFixedUpdate();
	void Awake(){
		invincible=false;
	}
	//Send player's pos
	void FixedUpdate() {
		NoteManager.instance.playerPos=transform.position;
	}

	//Dash to Desired Point
	public void StartMove(bool isInvincible){
		mouse=Camera.main.ScreenToWorldPoint((Input.mousePosition));
		mouse.z=0;
		StartCoroutine(Move(isInvincible,mouse));
	}
	IEnumerator Move(bool isInvincible, Vector3 target){
		if(isInvincible)
			invincible=true;
		for(int i=1;i<=8;i++){
			transform.position=Vector3.Lerp(transform.position,target,0.125f*i);
			//transform.position=Vector3.SmoothDamp(transform.position,target,ref currentVelocity,0.04f*i);
			yield return tptime;
		}
		invincible=false;
	}
	//if hit by obj, reduce hp
	void OnTriggerEnter2D(Collider2D other) {
		if(other.CompareTag("Enemy")){
			NoteManager.instance.combo_manage(-1);
		}
	}
}

