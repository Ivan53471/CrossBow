using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTremble : SSAction
{
    float tremble_radius = 1.5f;// 颤抖的程度
    float tremble_time = 1.0f; // 颤抖的时间

    Vector3 arrow_pos;   // 箭的原位置

    public static ArrowTremble GetSSAction()
    {
        ArrowTremble tremble_action = CreateInstance<ArrowTremble>();
        return tremble_action;

    }

    // Start is called before the first frame update
    public override void Start()
    {
        // 得到箭中靶时的位置
        arrow_pos = this.transform.position;
    }
    // 实现箭的颤抖动作
    // Update is called once per frame
    public override void Update()
    {   // 更新时间，得到剩余的颤抖时间
        tremble_time -= Time.deltaTime;

        if (tremble_time > 0)
        {
            // 获取头部的位置
            Vector3 head_pos = this.transform.Find("HitPos").position;
            // 围绕箭头颤抖
            this.transform.Rotate(head_pos, tremble_radius);
        }
        else
        {
            // 将箭返回一开始的位置
            transform.position = arrow_pos;
            // 开始销毁动作
            this.destroy = true;
            // 回调为空
            this.callback.SSActionEvent(this);
        }
    }

    public override void FixedUpdate()
    {

    }
}

