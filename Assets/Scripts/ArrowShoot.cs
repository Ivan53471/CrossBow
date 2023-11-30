using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowShoot : SSAction
{
    public float arrowSpeed = 40f;  // 箭的速度
    private float arrowForce; //拉弓力度

    public static ArrowShoot GetSSAction(float force)
    {
        ArrowShoot shootarrow = CreateInstance<ArrowShoot>();
        shootarrow.arrowForce = force;
        return shootarrow;
    }
    // Start is called before the first frame update
    public override void Start()
    {
        // 摆脱弓的控制
        gameObject.transform.parent = null;
        // 设成物理模式
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        // 将箭的速度设为向前
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * arrowSpeed * arrowForce;
        
    }

    // Update is called once per frame
    public override void Update()
    {
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public override void FixedUpdate()
    {
        // 回调颤抖动作
        if (gameObject.tag == "onTarget")
        {
            this.destroy = true;
            this.callback.SSActionEvent(this, this.gameObject);
        }
    }

}

