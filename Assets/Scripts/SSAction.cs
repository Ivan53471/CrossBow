using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject
{
    public bool enable = true;                      //�Ƿ����ڽ��д˶���
    public bool destroy = false;                    //�Ƿ���Ҫ������
    public GameObject gameObject;                   //��������
    public Transform transform;                     //���������transform
    public ISSActionCallback callback;              //������ɺ����Ϣ֪ͨ��

    protected SSAction() { }
    //�������ʹ����������������
    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }
    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
    public virtual void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }
}

