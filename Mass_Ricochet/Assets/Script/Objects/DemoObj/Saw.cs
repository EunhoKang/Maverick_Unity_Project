using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour {
	public GameObject m_Parent;
	public Transform origin;
	public Rigidbody2D rb_saw;
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
		transform.SetParent(NoteManager.instance.recycle.transform);//Bug!
		gameObject.SetActive(false);
	}
	IEnumerator DuringShot(float speed){
		while(isnt_inverse){
			rb_saw.MovePosition(transform.position+(m_Parent.transform.position-origin.position)*speed);
			yield return shot_delay;
		}
		/* 
		while(!isnt_inverse){

		}
		*/
		
	}
}
