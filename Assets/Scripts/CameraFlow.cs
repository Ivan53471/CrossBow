using UnityEngine;
using System.Collections;

public class CameraFlow : MonoBehaviour
{
    public GameObject bow;
    private int gameState;
    public float moveSpeed;
    public LayerMask groundLayer;
    // ˮƽ�ӽ��ƶ������ж�
    public float sensitivityHor;
    // ��ֱ�ӽ��ƶ������ж�
    public float sensitivityVer;
    // �ӽ������ƶ��ĽǶȷ�Χ����ֵԽС��ΧԽ��
    public float upVer = -85;
    // �ӽ������ƶ��ĽǶȷ�Χ����ֵԽ��ΧԽ��
    public float downVer = 85;
    // ��ֱ��ת�Ƕ�
    private float rotVer;
    // ˮƽ��ת�Ƕ�
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
        // ����gameState
        gameState = SSDirector.getInstance().currentController.GetGameState();
    }
    void FixedUpdate()
    {
        MoveCamera();
        RotateCameraTowardsMouse();
    }
    // ʵ�ֵ�һ�˳��ӽǵ��ƶ�
    private void MoveCamera()
    {
        if (gameState == 0)
            return;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        // ���������ľ��벻��
        transform.Translate(new Vector3(movement.x, 0f, movement.z));
        transform.position = new Vector3(transform.position.x, 3f, transform.position.z);
    }
    // ʵ���ӽ�ת��
    private void RotateCameraTowardsMouse()
    {
        if (gameState == 0)
            return;
        // ��ȡ������µ��ƶ�λ��
        float mouseVer = Input.GetAxis("Mouse Y");
        // ��ȡ������ҵ��ƶ�λ��
        float mouseHor = Input.GetAxis("Mouse X");
        // ��������ƶ����ӽ���ʵ�������ƣ�����Ҫ��ﵽ�ӽ�Ҳ�����ƵĻ�����Ҫ��ȥ��
        rotVer -= mouseVer * sensitivityVer;
        rotHor += mouseHor * sensitivityHor;
        // �޶������ƶ����ӽǷ�Χ������ֱ������360����ת
        rotVer = Mathf.Clamp(rotVer, upVer, downVer);

        Quaternion targetRotation = Quaternion.Euler(rotVer, rotHor, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothness * Time.deltaTime);
    }
}
