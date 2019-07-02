using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

		public static GameManager gamemanager=null;
		private void Awake() {
			if(gamemanager==null){
				gamemanager=this;
			}else if(gamemanager!=this){
				Destroy(gameObject);
			}
		}
		class SongInfo{
			public AudioClip music;
			public stageInfo stagefile;
		}
		public GameObject ins;
		private NoteManager notemanager;
		public AudioClip[] musics;
		public List<stageInfo> stages=new List<stageInfo>();
		//Generate NoteManager, Send Data to it
		private Dictionary<string, SongInfo> SongDic;
		public GameObject MainScreen;
		public GameObject SongSelectScreen;
		public GameObject ResultScreen;
		private string current_scene;
		private string prev_scene;
		private Dictionary<string, GameObject> SceneDic;
		[HideInInspector]public int LastPlayerScore;
		void Start ()
		{
			//Add Songs in Dictionary
			string name;
			SongInfo songinfo=new SongInfo();
			SongDic=new Dictionary<string,SongInfo>();

			name="Firefly";
			songinfo.music=musics[0];
			songinfo.stagefile=stages[0];
			SongDic.Add(name,songinfo);

			//Add Scenes in Dictionary
			SceneDic=new Dictionary<string,GameObject>();

			name="Main Scene";
			SceneDic.Add(name,MainScreen);

			name="Song Select Scene";
			SceneDic.Add(name,SongSelectScreen);

			name="Result Scene";
			SceneDic.Add(name,ResultScreen);
			//Save Current&Previous Scene
			current_scene="Main Scene";
			prev_scene="";
			//Start Main Scene
			MainScreen.SetActive(true);
		}	
		public void SceneChange(string next){
			if(current_scene!="Playing")
				SceneDic[current_scene].SetActive(false);
			SceneDic[next].SetActive(true);
			current_scene=next;
		}

		public void StartMusic(string song_name){
			SceneDic[current_scene].SetActive(false);
			if (NoteManager.instance == null)
				ins=Instantiate(ins);
			notemanager=ins.GetComponent<NoteManager>();

			current_scene="Playing";
			notemanager.StartSetting(SongDic[song_name].music,SongDic[song_name].stagefile);
		}
}
