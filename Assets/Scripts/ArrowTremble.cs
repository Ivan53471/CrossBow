using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTremble : SSAction
{
    float tremble_radius = 1.5f;// �����ĳ̶�
    float tremble_time = 1.0f; // ������ʱ��

    Vector3 arrow_pos;   // ����ԭλ��

    public static ArrowTremble GetSSAction()
    {
        ArrowTremble tremble_action = CreateInstance<ArrowTremble>();
        return tremble_action;

    }

    // Start is called before the first frame update
    public override void Start()
    {
        // �õ����а�ʱ��λ��
        arrow_pos = this.transform.position;
    }
    // ʵ�ּ��Ĳ�������
    // Update is called once per frame
    public override void Update()
    {   // ����ʱ�䣬�õ�ʣ��Ĳ���ʱ��
        tremble_time -= Time.deltaTime;

        if (tremble_time > 0)
        {
            // ��ȡͷ����λ��
            Vector3 head_pos = this.transform.Find("HitPos").position;
            // Χ�Ƽ�ͷ����
            this.transform.Rotate(head_pos, tremble_radius);
        }
        else
        {
            // ��������һ��ʼ��λ��
            transform.position = arrow_pos;
            // ��ʼ���ٶ���
            this.destroy = true;
            // �ص�Ϊ��
            this.callback.SSActionEvent(this);
        }
    }

    public override void FixedUpdate()
    {

    }
}

