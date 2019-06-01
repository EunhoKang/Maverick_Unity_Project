using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour {
	//Singletone
	public int stagenum;
	public static NoteManager instance=null;
	private void Awake() {
		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
	}
	//Objects which needs for stage
	public GameObject player;
	public Vector3 playerPos;
	private Player playerFunc;
	public GameObject Hp;
	public Vector3 HpPos;
	private GameObject Tempobj;
	//Judge Line
	public GameObject Perf;
	public GameObject Good;
	public GameObject Miss;
	//MusicSource
	public AudioSource audiosource;
	public AudioClip music;
	public float SPB;
	public float StartDelay;
	//Object Pool & Data List for Game
	public List<GameObject> _Objs=new List<GameObject>();
	public List<GameObject> Objs=new List<GameObject>();
	public List<Vector3> Objpos=new List<Vector3>();
	public List<Vector3> Objrot=new List<Vector3>();
	public List<float> _Objtime=new List<float>();
	public List<float> _Notetime=new List<float>();
	public List<Vector3> ObjCommand=new List<Vector3>();
	public List<WaitForSeconds> Objtime=new List<WaitForSeconds>();
	public List<WaitForSeconds> Notetime=new List<WaitForSeconds>();
	//Note Info
	private List<GameObject> Notes;
	public int NotePoolCount=16;
	public GameObject NoteType;
	public Vector3 NotePos;
	public float NoteSpeed;
	private WaitForSeconds FrameTime;
	public float noteInterval=1f;

	//Set TimeScale, Generate Judge Line
	void OnEnable() {
		FrameTime=new WaitForSeconds(noteInterval*0.01f);

		Perf.SetActive(false);
		Good.SetActive(false);
		Miss.SetActive(false);
		
	}
	//Receive Stage Data, Pooling Notes & Objects, Caching Datas, Instantiate Objects, Start Music
	public void StartSetting(AudioClip stagemusic,GameObject O){
		music=stagemusic;
		audiosource.clip=music;
		stageInfo T=O.GetComponent<stageInfo>();
		_Objs=T.Objs;
		Objpos=T.Objpos;
		Objrot=T.Objrot;
		_Objtime=T.Objtime;
		_Notetime=T.Notetime;
		NoteSpeed=T.Notespeed;
		ObjCommand=T.ObjCommand;
		SPB=60/T.BPM;
		StartDelay=T.StartDelay;

		Notes=new List<GameObject>();
		for(int i=0; i<NotePoolCount;i++){
			GameObject _obj=Instantiate(NoteType);
			_obj.SetActive(false);
			Notes.Add(_obj);
		}

		GameObject obj;
		for(int i=0; i<_Objs.Count;i++){
			obj=Instantiate(_Objs[i]);
			obj.SetActive(false);
			Objs.Add(obj);
		}

		for(int i=0; i<_Objtime.Count;i++){
			Objtime.Add(new WaitForSeconds(_Objtime[i]*SPB));
		}
		for(int i=0; i<_Notetime.Count;i++){
			Notetime.Add(new WaitForSeconds(_Notetime[i]*SPB));
		}

		Hp=Instantiate(Hp,HpPos,Quaternion.identity)as GameObject;
		player=Instantiate(player,playerPos,Quaternion.identity)as GameObject;
		playerFunc=player.GetComponent<Player>();

		StartCoroutine(MusicStart(stagenum));
	}
	//Wait for some sec, Edit Objs, Edit Notes, Then Start the Game
	IEnumerator MusicStart(int stagenum){
		float iv=10f/NoteSpeed;//To sync the Note

		//where notes generated	
		for(int i=0;i<Notetime.Count;i++)
			StartCoroutine(NoteGenerate(i));

		yield return new WaitForSeconds(1f*iv);

		//where obj edited
		//(TimeNumber,ObjNumber,CommandType)
		//All number has to be int
		//TimeNumber : Recommanding TimeNumber to be increased rather than decreased
		//ObjNumber : 
		//CommandType : 0 to Activate, -1 to Deactivate, Others are Custom Command
		for(int i=0; i<ObjCommand.Count;i++){
			StartCoroutine(CommandObj((int)ObjCommand[i].x,(int)ObjCommand[i].y,(int)ObjCommand[i].z));
		}
		yield return new WaitForSeconds(StartDelay);
		//System.GC.Collect();
		audiosource.Play();
	}
	//-------------------------------------------------------------------------------------

	//Waiting for Input for Dash
	void FixedUpdate(){
		if(Input.GetButton("Hit")){
			StartingJudge();
		}
	}
	//Move Player according to Judge
	public void Judge(bool isPerf){
		if(isPerf){
			playerFunc.StartMove(true);
		}else{
			playerFunc.StartMove(false);
		}
	}
	//Judging the Note
	public void StartingJudge(){

		StartCoroutine(DuringJudge());
	}
	IEnumerator DuringJudge(){	
		Perf.SetActive(true);
		Good.SetActive(true);
		Miss.SetActive(true);
		yield return FrameTime;
		Perf.SetActive(false);
		Good.SetActive(false);
		Miss.SetActive(false);
	}

	//-------------------------------------------------------------------------------------

	//Generate Note and Wait assigned time
	IEnumerator NoteGenerate(int notecount){
		yield return Notetime[notecount];
		//Debug.Log(Time.time); //use it to check falling time

		Tempobj=getNote();
		Tempobj.SetActive(true);
		Tempobj.transform.position=NotePos;
	}

	//Get Note from Pool
	public GameObject getNote(){
		for(int i=0; i<Notes.Count;i++){
			if(!Notes[i].activeInHierarchy){
				return Notes[i];
			}
		}
		GameObject obj=Instantiate(NoteType);
		obj.SetActive(false);
		Notes.Add(obj);
		return obj;
	}

	//Command Objs. 0 for Activate, -1 for Deactivate, else for Costom Action
	IEnumerator CommandObj(int Timecount,int Objcount,int Mode){
		if(Mode==0){
			yield return Objtime[Timecount];
			//change this after complete
			Objs[Objcount].SetActive(true);
			Objs[Objcount].transform.position=Objpos[Timecount];
			Objs[Objcount].transform.rotation=Quaternion.Euler(Objrot[Timecount]);
		}else if(Mode==-1){
			yield return Objtime[Timecount];

			Objs[Objcount].SetActive(false);
		}
		else if(Mode==1){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_1");
			
		}else if(Mode==2){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_2");
			
		}else if(Mode==3){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_3");
			
		}
	}
}
