using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour {
	public List<ColorControl> Sprites;
	public GameObject[] SprObj;
	public Color[] colors;
	public GameObject particle;
	private WaitForSeconds FrameTime;
	void Awake () {
		Sprites=new List<ColorControl>();
		Sprites.Clear();
		FrameTime=new WaitForSeconds(0.013f);
		for(int i=0; i<SprObj.Length;i++){
			Sprites.Add(SprObj[i].GetComponent<ColorControl>());
		}
	}
	public void Command_1(){
		Sprites[0].ChangeCol(colors[0]);
	}
	public void Command_2(){
		Sprites[0].ChangeCol(colors[1]);
	}
	public void Command_3(){
		Sprites[0].ChangeCol(colors[2]);
	}
	public void Command_4(){
		Sprites[0].ChangeCol(colors[3]);
	}
	public void Command_5(){
		Sprites[1].ChangeCol(colors[4]);
	}
	public void Command_6(){
		Sprites[1].ChangeCol(colors[5]);
	}
	public void Command_7(){
		Sprites[1].ChangeCol(colors[6]);
	}
	public void Command_8(){
		Sprites[1].ChangeCol(colors[7]);
	}
	public void Command_9(){
		Sprites[2].ChangeCol(colors[8]);
	}
	public void Command_10(){
		Sprites[2].ChangeCol(colors[9]);
	}
	public void Command_11(){
		Sprites[2].ChangeCol(colors[10]);
	}
	public void Command_12(){
		Sprites[2].ChangeCol(colors[11]);
	}
	public void Command_13(){
		ParticleOn();
	}
	public void Command_14(){
		ParticleOff();
	}
	public void Command_15(){
		transform.position=new Vector3(0,0,0);
	}
	void ParticleOn(){
		particle.SetActive(true);
	}
	void ParticleOff(){
		particle.SetActive(false);
	}
	void ChangeColor(){
		
	}
}
