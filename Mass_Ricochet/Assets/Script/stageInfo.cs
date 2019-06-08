using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stageInfo : MonoBehaviour {
	public List<GameObject> Objs=new List<GameObject>();
	public List<Vector3> Objpos=new List<Vector3>();
	public List<Vector3> Objrot=new List<Vector3>();
	public List<float> Objtime=new List<float>();
	[Header("#Time/Obj/Command")]
	public List<Vector3> ObjCommand=new List<Vector3>();
	[Header("#Notes")]
	public List<float> Notetime=new List<float>();
	public float Notespeed=1;
	[Header("#BPM")]
	public float BPM;
	public float StartDelay;
	[Header("#RandomPlace")]
	public List<Vector3> Randomplace=new List<Vector3>();
}
