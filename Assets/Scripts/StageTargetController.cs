using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTargetController : MonoBehaviour
{
    public MainController scene;
    public ScoreRecorder sc_recorder;
    public bool isShot;
    // Start is called before the first frame update
    void Start()
    {
        scene = SSDirector.getInstance().currentController;
        sc_recorder = scene.recorder;
        isShot = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        GameObject arrow = collision.gameObject;
        //有箭中靶
        if (arrow.CompareTag("arrow") && !isShot)
        {
            // 将箭的速度设为0
            arrow.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // 使用运动学运动控制
            arrow.GetComponent<Rigidbody>().isKinematic = true;
            // 计分
            sc_recorder.RecordScore(1);
            //标记箭为中靶
            arrow.tag = "onTarget";
            isShot = true;
            gameObject.GetComponent<Animator>().SetTrigger("isShot");
        }
    }
}
