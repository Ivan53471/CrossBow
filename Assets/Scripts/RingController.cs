using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour
{
    public int RingScore = 0; // ��ǰ���ķ�ֵ
    public MainController scene;
    public ScoreRecorder sc_recorder;
    // Start is called before the first frame update
    void Start()
    {
        scene = SSDirector.getInstance().currentController;
        sc_recorder = scene.recorder;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject arrow = collision.gameObject;
        //�м��а�
        if (arrow.CompareTag("arrow"))
        {
            // �������ٶ���Ϊ0
            arrow.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // ʹ���˶�ѧ�˶�����
            arrow.GetComponent<Rigidbody>().isKinematic = true;
            // �Ʒ�
            sc_recorder.RecordScore(RingScore);
            //��Ǽ�Ϊ�а�
            arrow.tag = "onTarget";
        }
    }
}

