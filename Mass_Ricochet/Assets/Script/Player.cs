
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private bool invincible=false;
	private Vector3 mouse;
	private Vector3 currentVelocity;
	private WaitForFixedUpdate tptime=new WaitForFixedUpdate();
	private float playerHP=100;
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
		for(int i=1;i<=25;i++){
			transform.position=Vector3.SmoothDamp(transform.position,target,ref currentVelocity,0.04f*i);
			yield return tptime;
		}
		invincible=false;
	}
	//if hit by obj, reduce hp
	void OnTriggerEnter2D(Collider2D other) {
		if(other.CompareTag("Enemy")){
			playerHP-=10f;
			Debug.Log(playerHP);
		}
	}
}

