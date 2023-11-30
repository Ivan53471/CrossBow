using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SecondCamera : MonoBehaviour
{
    public float show_time = 3f; // ���չʾʱ��
    public bool show = false;
    private GameObject curArrow; // ��ǰ����ļ�
    private Vector3 offset; // ƫ����
    void Start()
    {
        show = false;
        curArrow = null;
        offset = new Vector3(0, 0, -2);
    }
    void Update()
    {
        if (show == true)
        {
            show_time -= Time.deltaTime;
            // �����λ�����ڼ�������
            transform.position = curArrow.transform.position + curArrow.transform.forward + offset;
            // չʾʱ�䳬��3�룬ֹͣչʾ
            if (show_time < 0)
            {
                curArrow = null;
                show = false;
                this.gameObject.SetActive(false);
            }
        }
    }
    // չʾ�����
    public void ShowCamera(GameObject arrow)
    {
        curArrow = arrow;
        show = true;
        show_time = 3f;
        this.gameObject.SetActive(true);
    }
}
