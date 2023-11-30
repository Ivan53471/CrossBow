using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SecondCamera : MonoBehaviour
{
    public float show_time = 3f; // 相机展示时间
    public bool show = false;
    private GameObject curArrow; // 当前跟随的箭
    private Vector3 offset; // 偏移量
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
            // 将相机位置设在箭的正后方
            transform.position = curArrow.transform.position + curArrow.transform.forward + offset;
            // 展示时间超过3秒，停止展示
            if (show_time < 0)
            {
                curArrow = null;
                show = false;
                this.gameObject.SetActive(false);
            }
        }
    }
    // 展示副相机
    public void ShowCamera(GameObject arrow)
    {
        curArrow = arrow;
        show = true;
        show_time = 3f;
        this.gameObject.SetActive(true);
    }
}
