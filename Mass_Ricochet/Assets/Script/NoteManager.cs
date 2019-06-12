using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
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
	public GameObject Heart;
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
	public List<GameObject> _Objs;
	public List<GameObject> Objs;
	public List<Vector3> Objpos;
	public List<float> _Objtime;
	public List<float> _Notetime;
	public List<Vector3> ObjCommand;
	public List<WaitForSeconds> Objtime;
	public List<WaitForSeconds> Notetime;
	public List<Vector3> RandomPlace;
	public string Name_Objtime;
	public string Name_Notetime;
	public string Name_ObjCommand;
	public string Name_RandomPlace;
	//Note Info
	private List<GameObject> Notes;
	public int NotePoolCount;
	public GameObject NoteType;
	public Vector3 NotePos;
	public float NoteSpeed;
	private WaitForSeconds FrameTime;
	public float noteInterval;
	//make recycle bin
	public GameObject recycle;
	//user score
	public int score;
	public int amount;
	public int combo_time;
	public bool is_GameEnd;
	//UI
	public Text scoreText;
	public Text diffText;
	//Set TimeScale, Generate Judge Line
	void OnEnable() {
		FrameTime=new WaitForSeconds(noteInterval*0.01f);

		Perf.SetActive(false);
		Good.SetActive(false);
		Miss.SetActive(false);

		NotePoolCount=10;
		noteInterval=1f;
		score=0;
		is_GameEnd=false;
		amount=1;
		combo_time=0;

		_Objs=new List<GameObject>();
		Objs=new List<GameObject>();
		Objpos=new List<Vector3>();
		_Objtime=new List<float>();
		_Notetime=new List<float>();
		ObjCommand=new List<Vector3>();
		Objtime=new List<WaitForSeconds>();
		Notetime=new List<WaitForSeconds>();
		Notes=new List<GameObject>();
		RandomPlace=new List<Vector3>();

		_Objs.Clear();
		Objs.Clear();
		Objpos.Clear();
		_Objtime.Clear();
		_Notetime.Clear();
		ObjCommand.Clear();
		Objtime.Clear();
		Notetime.Clear();
		Notes.Clear();
		RandomPlace.Clear();
	}
	//Receive Stage Data, Pooling Notes & Objects, Caching Datas, Instantiate Objects, Start Music
	public void StartSetting(AudioClip stagemusic,GameObject O){
		music=stagemusic;
		audiosource.clip=music;
		stageInfo T=O.GetComponent<stageInfo>();
		_Objs=T.Objs;
		NoteSpeed=T.Notespeed;
		Name_Notetime=T.Name_Notetime;
		Name_ObjCommand=T.Name_ObjCommand;
		Name_Objtime=T.Name_Objtime;
		Name_RandomPlace=T.Name_RandomPlace;

		TextAsset textAsset= Resources.Load<TextAsset>(Name_Notetime);
		StringReader reader=new StringReader(textAsset.text);
		int Max=int.Parse(reader.ReadLine());
		for(int i=0;i<Max;i++){
			_Notetime.Add(float.Parse(reader.ReadLine()));
		}
		textAsset= Resources.Load<TextAsset>(Name_Objtime);
		reader=new StringReader(textAsset.text);
		Max=int.Parse(reader.ReadLine());
		for(int i=0;i<Max;i++){
			_Objtime.Add(float.Parse(reader.ReadLine()));
		}
		textAsset= Resources.Load<TextAsset>(Name_ObjCommand);
		reader=new StringReader(textAsset.text);
		Max=int.Parse(reader.ReadLine());
		string temp;
		string[] temps;
		Vector3 tempVec=new Vector3(0,0,0);
		for(int i=0;i<Max;i++){
			temp=reader.ReadLine();
			temps=temp.Split(' ');
			tempVec.x=float.Parse(temps[0]);
			tempVec.y=float.Parse(temps[1]);
			tempVec.z=float.Parse(temps[2]);
			ObjCommand.Add(tempVec);
		}
		textAsset= Resources.Load<TextAsset>(Name_RandomPlace);
		reader=new StringReader(textAsset.text);
		Max=int.Parse(reader.ReadLine());
		for(int i=0;i<Max;i++){
			temp=reader.ReadLine();
			temps=temp.Split(' ');
			tempVec.x=float.Parse(temps[0]);
			tempVec.y=float.Parse(temps[1]);
			tempVec.z=float.Parse(temps[2]);
			RandomPlace.Add(tempVec);
		}
		
		for(int i=0;i<ObjCommand.Count;i++){
			Objpos.Add(RandomPlace[Random.Range(0,RandomPlace.Count)]);
		}	
		SPB=60/T.BPM;
		StartDelay=T.StartDelay;

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
		
		Heart=Instantiate(Heart,HpPos,Quaternion.identity)as GameObject;
		player=Instantiate(player,playerPos,Quaternion.identity)as GameObject;
		playerFunc=player.GetComponent<Player>();
		
		StartCoroutine(MusicStart(stagenum));
	}
	//Wait for some sec, Edit Objs, Edit Notes, Then Start the Game
	IEnumerator MusicStart(int stagenum){
		Objs[Objs.Count-1].SetActive(true);
		Objs[Objs.Count-1].transform.position=new Vector3(0,0,0);
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
		//Debug.Log(Time.time);
		StartCoroutine(ScoreSet());
		StartCoroutine(Game_End());
	}
	//-------------------------------------------------------------------------------------

	//Waiting for Input for Dash
	void FixedUpdate(){
		if(Input.GetButton("Hit")){
			StartingJudge();
		}
	}
	//add score
	IEnumerator ScoreSet(){
		while(!is_GameEnd){
			score+=amount;
			combo_time++;
			show_scorein_UI();
			if(combo_time>500){
				combo_time=0;
				if(amount<=625)
					combo_manage(1);
			}
			//Debug.Log(score);
			yield return FrameTime;
		}
	}
	IEnumerator Game_End(){
		yield return new WaitForSeconds(258f);
		is_GameEnd=true;
	}
	public void combo_manage(int mode){
		if(mode==-1){
			amount=1;
			score-=1000;
			combo_time=0;
			show_diffin_UI(-1);
		}else if(mode==0){
			amount=1;
			combo_time=0;
			show_diffin_UI(0);
		}else if(mode==1){
			amount*=5;
			show_diffin_UI(1);
		}
	}
	public void show_scorein_UI(){
		scoreText.text=score.ToString();
	}
	public void show_diffin_UI(int mode){
		if(mode==-1){
			diffText.text="(-1000) Miss";
		}else if(mode==0){
			diffText.text="(x1) Combo Break";
		}else if(mode==1){
			if(amount==5){
				diffText.text="(x5) Combo LV 2";
			}else if(amount==25){
				diffText.text="(x25) Combo LV 3";
			}else if(amount==125){
				diffText.text="(x125) Combo LV 4";
			}else if(amount==625){
				diffText.text="(x625) Combo LV MAX";
			}
		}
		Invoke("reset_diffin_UI",2f);
	}
	public void reset_diffin_UI(){
		diffText.text=null;
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
			Objs[Objcount].transform.position=RandomPlace[Random.Range(0,RandomPlace.Count)];
			
			//Objs[Objcount].transform.rotation=Quaternion.Euler(Objrot[Timecount]);
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
			
			
		}else if(Mode==4){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_4");
			
			
		}else if(Mode==5){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_5");
			
		}else if(Mode==6){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_6");
			
			
		}else if(Mode==7){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_7");
			
			
		}else if(Mode==8){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_8");
			
			
		}else if(Mode==9){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_9");
		
			
		}else if(Mode==10){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_10");
			
			
		}else if(Mode==11){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_11");
			
			
		}else if(Mode==12){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_12");
			
			
		}else if(Mode==13){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_13");
			
			
		}else if(Mode==14){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_14");
			
			
		}else if(Mode==15){
			yield return Objtime[Timecount];

			Objs[Objcount].SendMessage("Command_15");
			
			
		}
	}
}
