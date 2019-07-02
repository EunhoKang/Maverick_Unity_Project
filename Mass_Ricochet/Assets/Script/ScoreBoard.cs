using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public Text ScoreUI;
    private WaitForSeconds FTime=new WaitForSeconds(0.0001f);
    private void OnEnable() {
        ScoreUI.text=GameManager.gamemanager.LastPlayerScore.ToString();
    }
    IEnumerator ScoreUp(int score){
        /* 
        yield return new WaitForSeconds(0.4f);
        int temp=0;
        for(int i=0;i<score;i++){
            if(temp>10)
                temp+=10;
            if
            ScoreUI.text=temp.ToString();
            yield return FTime;
        }
        */
        yield return null;
    }
}
