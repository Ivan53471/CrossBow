using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowShoot : SSAction
{
    public float arrowSpeed = 40f;  // �����ٶ�
    private float arrowForce; //��������

    public static ArrowShoot GetSSAction(float force)
    {
        ArrowShoot shootarrow = CreateInstance<ArrowShoot>();
        shootarrow.arrowForce = force;
        return shootarrow;
    }
    // Start is called before the first frame update
    public override void Start()
    {
        // ���ѹ��Ŀ���
        gameObject.transform.parent = null;
        // �������ģʽ
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        // �������ٶ���Ϊ��ǰ
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * arrowSpeed * arrowForce;
        
    }

    // Update is called once per frame
    public override void Update()
    {
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public override void FixedUpdate()
    {
        // �ص���������
        if (gameObject.tag == "onTarget")
        {
            this.destroy = true;
            this.callback.SSActionEvent(this, this.gameObject);
        }
    }

}

