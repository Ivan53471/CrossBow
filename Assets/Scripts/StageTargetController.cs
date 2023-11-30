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
        //�м��а�
        if (arrow.CompareTag("arrow") && !isShot)
        {
            // �������ٶ���Ϊ0
            arrow.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // ʹ���˶�ѧ�˶�����
            arrow.GetComponent<Rigidbody>().isKinematic = true;
            // �Ʒ�
            sc_recorder.RecordScore(1);
            //��Ǽ�Ϊ�а�
            arrow.tag = "onTarget";
            isShot = true;
            gameObject.GetComponent<Animator>().SetTrigger("isShot");
        }
    }
}
