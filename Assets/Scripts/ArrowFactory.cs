using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFactory : MonoBehaviour
{
    private List<GameObject> dirtyArrow; // 正在被使用的arrow
    private List<GameObject> freeArrow; // 没有被使用的arrow

    public GameObject arrow = null;

    void Start()
    {
        dirtyArrow = new List<GameObject>();
        freeArrow = new List<GameObject>();
    }
    public GameObject GetArrow()
    {
        int arrowCount = freeArrow.Count;
        if (arrowCount == 0)
        {
            arrow = GameObject.Instantiate(
                Resources.Load<GameObject>("Prefabs/Arrow"), Vector3.zero, Quaternion.identity);
        }
        else
        {
            arrow = freeArrow[freeArrow.Count - 1];
            freeArrow.RemoveAt(freeArrow.Count - 1);
            // 箭在靶子上需要把标签改回来
            if (arrow.tag == "onTarget")
            {
                arrow.tag = "arrow";
            }
            arrow.gameObject.SetActive(true);
        }
        // 先让箭在镜头前面不动
        arrow.GetComponent<Rigidbody>().isKinematic = true;
        // 箭跟随视角移动
        arrow.transform.parent = Camera.main.transform;
        arrow.transform.localPosition = new Vector3(0, 0, 1);
        Vector3 cameraDirection = Camera.main.transform.forward;
        arrow.transform.forward = cameraDirection.normalized;

        dirtyArrow.Add(arrow);
        return arrow;
    }


    public void FreeArrow(GameObject arrow)
    {
        foreach (GameObject a in dirtyArrow)
        {
            if (arrow.GetInstanceID() == a.GetInstanceID())
            {
                a.SetActive(false);
                a.GetComponent<Rigidbody>().isKinematic = true;
                dirtyArrow.Remove(a);
                freeArrow.Add(a);
                break;
            }
        }
    }
}

