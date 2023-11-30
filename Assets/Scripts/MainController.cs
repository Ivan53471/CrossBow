using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour, ISceneController
{
    // ���Ķ���������
    public ArrowShootManager arrow_manager;
    // ���Ĺ���
    public ArrowFactory factory;
    // �����
    public Camera second_camera;
    //�����
    public Camera main_camera;
    // ��¼����
    public ScoreRecorder recorder;
    // ��
    public GameObject bow;
    // ��
    public GameObject arrow;
    // ���Ƿ�load
    private bool isLoad;
    private int arrow_num = 0;    //����ļ���
    // ���Ķ���
    private Queue<GameObject> arrows = new Queue<GameObject>();
    // 0����Ϸδ��ʼ��1����Ϸ����
    private int gameState = 0; 
    // ���
    public List<Material> skyboxMaterials;
    // ��ǰ����±�
    private int cnt;
    // ��ϰģʽ����
    private List<GameObject> prac_target;
    // �Ƿ���봳��ģʽ
    private int isStageMode; // 0��δ���룬1�����ڽ��У�2�����
    // ����ģʽ����
    private List<GameObject> stage_target;
    public void LoadResources()
    {
        SSDirector director = SSDirector.getInstance();
        director.currentController = this;

        bow = Instantiate(Resources.Load("Prefabs/Crossbow", typeof(GameObject))) as GameObject;

        LoadPracticeModeTarget();

        main_camera = Camera.main;
        second_camera = GameObject.Find("Second Camera").GetComponent<Camera>();
        second_camera.gameObject.SetActive(false);

        factory = gameObject.AddComponent<ArrowFactory>();

        recorder = gameObject.AddComponent<ScoreRecorder>();

        arrow_manager = gameObject.AddComponent<ArrowShootManager>();

        RenderSettings.skybox = skyboxMaterials[cnt];
    }
    // ������ϰģʽ����
    public void LoadPracticeModeTarget()
    {
        prac_target = new List<GameObject>();
        GameObject target1 = Instantiate(Resources.Load(
            "Prefabs/target", typeof(GameObject)), 
            new Vector3(-25, 0, 40), Quaternion.identity) as GameObject;
        GameObject target2 = Instantiate(Resources.Load(
            "Prefabs/target", typeof(GameObject)),
            new Vector3(-45, 0, 10), Quaternion.identity) as GameObject;
        GameObject target3 = Instantiate(Resources.Load(
            "Prefabs/target", typeof(GameObject)),
            new Vector3(-35, 0, 3), Quaternion.identity) as GameObject;
        GameObject target4 = Instantiate(Resources.Load(
            "Prefabs/slide target", typeof(GameObject))) as GameObject;
        prac_target.Add(target1);
        prac_target.Add(target2);
        prac_target.Add(target3);
        prac_target.Add(target4);
    }
    // ���ش���ģʽ����
    private void LoadStageTarget()
    {
        // �ͷ�֮ǰ��
        if (stage_target != null && stage_target.Count > 0)
        {
            foreach(GameObject obj in stage_target)
            {
                Destroy(obj); // ���������TargetFactory�Ż��������ظ�����ɾ��
            }
        }
        stage_target = new List<GameObject>();
        GameObject target1 = Instantiate(Resources.Load(
            "Prefabs/stage target", typeof(GameObject)),
            new Vector3(73, 0.3f, -6), Quaternion.Euler(0, 90, 0)) as GameObject;
        GameObject target2 = Instantiate(Resources.Load(
            "Prefabs/stage target", typeof(GameObject)),
            new Vector3(75, 0.3f, 18), Quaternion.identity) as GameObject;
        GameObject target3 = Instantiate(Resources.Load(
            "Prefabs/stage target", typeof(GameObject)),
            new Vector3(85, 0.3f, 70), Quaternion.identity) as GameObject;
        GameObject target4 = Instantiate(Resources.Load(
            "Prefabs/stage target", typeof(GameObject)),
            new Vector3(115, 0.3f, 30), Quaternion.Euler(0, 135, 0)) as GameObject;
        GameObject target5 = Instantiate(Resources.Load(
            "Prefabs/stage target", typeof(GameObject)),
            new Vector3(145, 0.3f, 80), Quaternion.identity) as GameObject;
        GameObject target6 = Instantiate(Resources.Load(
            "Prefabs/rotate target", typeof(GameObject)),
            new Vector3(210, 0, 80), Quaternion.Euler(0, 90, 0)) as GameObject;
        stage_target.Add(target1);
        stage_target.Add(target2);
        stage_target.Add(target3);
        stage_target.Add(target4);
        stage_target.Add(target5);
        stage_target.Add(target6);
    }
    // �ж��Ƿ������
    private bool canShoot()
    {
        // ��ȡ�������λ��
        Vector3 cameraPosition = Camera.main.transform.position;
        // Բ�ĺͰ뾶
        Vector3 circleCenter = new Vector3(-34f, 0f, -10f);
        float radius = 18f;
        // �������λ����Բ�ĵľ���
        float distance = Vector3.Distance(cameraPosition, circleCenter);
        // �ж��Ƿ���Բ��
        if (distance <= radius || isStageMode == 1)
            return true;
        return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadResources();
        isLoad = false;
        bow.GetComponent<BowController>().cam = main_camera;
        cnt = 0;
        isStageMode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // �������
        RenderSettings.skybox = skyboxMaterials[cnt];
        if (gameState == 1)
        {
            foreach (GameObject arrow in arrows)
            {
                // ����������棬����˶�ѧ�ü�����
                if (arrow.transform.position.z > 150)
                {
                    arrow.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
            // ����ֻ����3֧��
            if (arrows.Count > 3)
            {
                // ��ȡ���е�һ��
                GameObject tempArrow = arrows.Dequeue();
                factory.FreeArrow(tempArrow);
            }
            // ����stageMode״̬
            setStageMode();
        }
        
    }
    // �����Ƿ�װ��
    public bool GetisLoad()
    {
        return isLoad;
    }
    // װ��
    public void LoadArrow()
    {
        if (gameState == 1 && !isLoad && canShoot())
        {
            //�ӹ����õ���
            arrow = factory.GetArrow();
            arrows.Enqueue(arrow);
            isLoad = true;
            // ����arrow
            arrow.GetComponent<Renderer>().enabled = false;
            arrow.GetComponentInChildren<TrailRenderer>().enabled = false;
            bow.GetComponent<Animator>().SetTrigger("start");
        }
    }
    // �趨����
    public void SetForce(float scrollInput)
    {
        float force = bow.GetComponent<Animator>().GetFloat("holdPower") + scrollInput;
        force = Mathf.Clamp(force, 0.3f, 1);
        bow.GetComponent<Animator>().SetFloat("holdPower", force);
    }
    // ���
    public void ShootArrow()
    {
        if (gameState == 1 && isLoad && canShoot())
        {
            // ͨ�������෢�䶯��
            // ����arrow
            arrow.GetComponent<Renderer>().enabled = true;
            arrow.GetComponentInChildren<TrailRenderer>().enabled = true;
            float force = bow.GetComponent<Animator>().GetFloat("holdPower");
            arrow_manager.Shoot(arrow, force);
            bow.GetComponent<Animator>().SetTrigger("shoot");
            bow.GetComponent<Animator>().SetFloat("holdPower", 0.3f);

            //���������
            second_camera.GetComponent<SecondCamera>().ShowCamera(arrow);
            arrow_num += 1;
            isLoad = false;
        }
    }
    // ���ص�ǰ����
    public int GetScore()
    {
        return recorder.score;
    }
    // �õ�����ļ�������
    public int GetArrowNum()
    {
        return arrow_num;
    }
    // ��ʼ����ģʽ
    public void Restart()
    {
        arrow_num = 0;
        recorder.score = 0;
        main_camera.transform.position = new Vector3(50.69f, 0f, -10f);
        isStageMode = 1;
        LoadStageTarget();
        foreach (GameObject arrow in arrows)
        {
            factory.FreeArrow(arrow);
        }
        arrows.Clear();
    }
    // ��ʼ��Ϸ
    public void BeginGame()
    {
        gameState = 1;
        // �������
        Cursor.visible = false;
    }
    // �õ���Ϸ��״̬
    public int GetGameState()
    {
        return gameState;
    }
    // �������
    public void SwitchMaterial()
    {
        cnt = (cnt + 1) % skyboxMaterials.Count;
    }
    // ��ȡ��ǰ����ģʽ״̬
    public int GetIsStageMode()
    { return isStageMode; }
    // ���ڸ��´���ģʽ״̬
    private void setStageMode()
    {
        if (isStageMode == 1)
        {
            if (stage_target.Count == 0)
                return;
            // ���а��Ӷ������У����ִ�����Ϸ����
            foreach (GameObject tar in stage_target)
            {
                if (tar.GetComponent<StageTargetController>().isShot == false)
                {
                    Debug.Log(tar.GetComponent<StageTargetController>().isShot);
                    return;
                }
            }
            isStageMode = 2;
        }
    }
}

