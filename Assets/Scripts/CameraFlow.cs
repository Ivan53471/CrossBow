using UnityEngine;
using System.Collections;

public class CameraFlow : MonoBehaviour
{
    public GameObject bow;
    private int gameState;
    public float moveSpeed;
    public LayerMask groundLayer;
    // 水平视角移动的敏感度
    public float sensitivityHor;
    // 垂直视角移动的敏感度
    public float sensitivityVer;
    // 视角向上移动的角度范围，该值越小范围越大
    public float upVer = -85;
    // 视角向下移动的角度范围，该值越大范围越大
    public float downVer = 85;
    // 垂直旋转角度
    private float rotVer;
    // 水平旋转角度
    private float rotHor;
    public float smoothness;

    private void Start()
    {
        rotVer = 0;
        rotHor = 0;
        sensitivityHor = 50f;
        sensitivityVer = 30f;
        smoothness = 10f;
        transform.localPosition = new Vector3(0, 3, -10);
        moveSpeed = 10f;
    }
    void Update()
    {
        // 更新gameState
        gameState = SSDirector.getInstance().currentController.GetGameState();
    }
    void FixedUpdate()
    {
        MoveCamera();
        RotateCameraTowardsMouse();
    }
    // 实现第一人称视角的移动
    private void MoveCamera()
    {
        if (gameState == 0)
            return;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        // 保持与地面的距离不变
        transform.Translate(new Vector3(movement.x, 0f, movement.z));
        transform.position = new Vector3(transform.position.x, 3f, transform.position.z);
    }
    // 实现视角转动
    private void RotateCameraTowardsMouse()
    {
        if (gameState == 0)
            return;
        // 获取鼠标上下的移动位置
        float mouseVer = Input.GetAxis("Mouse Y");
        // 获取鼠标左右的移动位置
        float mouseHor = Input.GetAxis("Mouse X");
        // 鼠标往上移动，视角其实是往下移，所以要想达到视角也往上移的话，就要减去它
        rotVer -= mouseVer * sensitivityVer;
        rotHor += mouseHor * sensitivityHor;
        // 限定上下移动的视角范围，即垂直方向不能360度旋转
        rotVer = Mathf.Clamp(rotVer, upVer, downVer);

        Quaternion targetRotation = Quaternion.Euler(rotVer, rotHor, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothness * Time.deltaTime);
    }
}
