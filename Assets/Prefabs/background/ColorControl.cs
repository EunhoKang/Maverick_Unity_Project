using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorControl : MonoBehaviour {
	public SpriteRenderer m_Sprite;
	private WaitForSeconds FrameTime;
	void Awake(){
		FrameTime=new WaitForSeconds(0.013f);
	}
	public void ChangeCol(Color targetColor){
		StartCoroutine(ChangeBG(targetColor,13));
	}
	IEnumerator ChangeBG(Color targetColor,float duration){
		Color t_color=m_Sprite.color;
		for(int i=1;i<=1000;i++){
			m_Sprite.color = Color.Lerp(m_Sprite.color, targetColor, i*Time.deltaTime*0.01f);
			yield return FrameTime;
		}
	}
}
