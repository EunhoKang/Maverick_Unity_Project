using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {

	public GameObject m_Parent;
	private Vector3 Delta;
	public Rigidbody2D rb_tr;
	private WaitForSeconds shot_delay=new WaitForSeconds(0.01f);
	bool isnt_inverse=true;
	void OnDisable(){
		StopAllCoroutines();
	}
	public void Shot(float speed){
		transform.parent=null;
		m_Parent.SetActive(false);
		StartCoroutine(DuringShot(speed));
	}
	public void Setparent_Again(){
		//play the end animation
		transform.SetParent(NoteManager.instance.recycle.transform);
		gameObject.SetActive(false);
	}
	IEnumerator DuringShot(float speed){
		while(isnt_inverse){
			Delta=(NoteManager.instance.playerPos-m_Parent.transform.position);
			rb_tr.MovePosition(transform.position+Delta.normalized*speed);
			yield return shot_delay;
		}
		/* 
		while(!isnt_inverse){

		}
		*/
		
	}
}
