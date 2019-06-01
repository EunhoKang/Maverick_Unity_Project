using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public GameObject ins;
	private NoteManager notemanager;
	public AudioClip[] musics;
	public List<GameObject> stages=new List<GameObject>();

	//Generate NoteManager, Send Data to it
	void Awake ()
    {
        if (NoteManager.instance == null)
            ins=Instantiate(ins);
			notemanager=ins.GetComponent<NoteManager>();

			notemanager.StartSetting(musics[0],stages[0]);
	}	
}
