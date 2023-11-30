using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour
{
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    public void RecordScore(int ringscore)
    {
        //增加新的值
        score += ringscore;
    }
}
