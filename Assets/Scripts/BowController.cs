using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

public class BowController : MonoBehaviour
{
    public Camera cam;

    void Start()
    {
        // 跟随主相机移动
        transform.SetParent(cam.transform);
        transform.localPosition = new Vector3(1, -0.5f, 1);
    }
}
