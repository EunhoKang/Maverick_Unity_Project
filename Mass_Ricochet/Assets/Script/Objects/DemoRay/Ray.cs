using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour {
	public GameObject m_Parent;
	private WaitForSeconds shot_delay=new WaitForSeconds(0.01f);
	private Vector3 myscale=new Vector3(1,1,1);
	private Vector3 addVec=new Vector3(0f,2f,0f);
	bool isnt_inverse=true;
	void OnDisable(){
		StopAllCoroutines();
	}
	// Use this for initialization
	public void Shot(float time){
		transform.parent=null;
		m_Parent.SetActive(false);
		StartCoroutine(DuringRay(time));
	}
	public void Setparent_Again(){
		//play the end animation
		transform.SetParent(NoteManager.instance.recycle.transform);
		gameObject.SetActive(false);
	}
	
	IEnumerator DuringRay(float time){
		while(transform.localScale.y<40&&isnt_inverse){
			transform.localScale+=addVec;
			yield return shot_delay;
		}
	}
}
