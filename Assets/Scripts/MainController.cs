using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour, ISceneController
{
    // 箭的动作管理者
    public ArrowShootManager arrow_manager;
    // 箭的工厂
    public ArrowFactory factory;
    // 副相机
    public Camera second_camera;
    //主相机
    public Camera main_camera;
    // 记录分数
    public ScoreRecorder recorder;
    // 弓
    public GameObject bow;
    // 箭
    public GameObject arrow;
    // 箭是否load
    private bool isLoad;
    private int arrow_num = 0;    //射出的箭数
    // 箭的队列
    private Queue<GameObject> arrows = new Queue<GameObject>();
    // 0是游戏未开始，1是游戏进行
    private int gameState = 0; 
    // 天空
    public List<Material> skyboxMaterials;
    // 当前天空下标
    private int cnt;
    // 练习模式靶子
    private List<GameObject> prac_target;
    // 是否进入闯关模式
    private int isStageMode; // 0是未进入，1是正在进行，2是完成
    // 闯关模式靶子
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
    // 加载练习模式靶子
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
    // 加载闯关模式靶子
    private void LoadStageTarget()
    {
        // 释放之前的
        if (stage_target != null && stage_target.Count > 0)
        {
            foreach(GameObject obj in stage_target)
            {
                Destroy(obj); // 这里可以用TargetFactory优化，避免重复生成删除
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
    // 判断是否能射箭
    private bool canShoot()
    {
        // 获取主相机的位置
        Vector3 cameraPosition = Camera.main.transform.position;
        // 圆心和半径
        Vector3 circleCenter = new Vector3(-34f, 0f, -10f);
        float radius = 18f;
        // 计算相机位置与圆心的距离
        float distance = Vector3.Distance(cameraPosition, circleCenter);
        // 判断是否在圆内
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
        // 更新天空
        RenderSettings.skybox = skyboxMaterials[cnt];
        if (gameState == 1)
        {
            foreach (GameObject arrow in arrows)
            {
                // 如果超出画面，设成运动学让箭不动
                if (arrow.transform.position.z > 150)
                {
                    arrow.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
            // 场景只能有3支箭
            if (arrows.Count > 3)
            {
                // 获取队列第一个
                GameObject tempArrow = arrows.Dequeue();
                factory.FreeArrow(tempArrow);
            }
            // 更新stageMode状态
            setStageMode();
        }
        
    }
    // 返回是否装箭
    public bool GetisLoad()
    {
        return isLoad;
    }
    // 装箭
    public void LoadArrow()
    {
        if (gameState == 1 && !isLoad && canShoot())
        {
            //从工厂得到箭
            arrow = factory.GetArrow();
            arrows.Enqueue(arrow);
            isLoad = true;
            // 隐藏arrow
            arrow.GetComponent<Renderer>().enabled = false;
            arrow.GetComponentInChildren<TrailRenderer>().enabled = false;
            bow.GetComponent<Animator>().SetTrigger("start");
        }
    }
    // 设定力量
    public void SetForce(float scrollInput)
    {
        float force = bow.GetComponent<Animator>().GetFloat("holdPower") + scrollInput;
        force = Mathf.Clamp(force, 0.3f, 1);
        bow.GetComponent<Animator>().SetFloat("holdPower", force);
    }
    // 射箭
    public void ShootArrow()
    {
        if (gameState == 1 && isLoad && canShoot())
        {
            // 通过管理类发射动作
            // 显现arrow
            arrow.GetComponent<Renderer>().enabled = true;
            arrow.GetComponentInChildren<TrailRenderer>().enabled = true;
            float force = bow.GetComponent<Animator>().GetFloat("holdPower");
            arrow_manager.Shoot(arrow, force);
            bow.GetComponent<Animator>().SetTrigger("shoot");
            bow.GetComponent<Animator>().SetFloat("holdPower", 0.3f);

            //开启副相机
            second_camera.GetComponent<SecondCamera>().ShowCamera(arrow);
            arrow_num += 1;
            isLoad = false;
        }
    }
    // 返回当前分数
    public int GetScore()
    {
        return recorder.score;
    }
    // 得到射出的箭的数量
    public int GetArrowNum()
    {
        return arrow_num;
    }
    // 开始闯关模式
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
    // 开始游戏
    public void BeginGame()
    {
        gameState = 1;
        // 隐藏鼠标
        Cursor.visible = false;
    }
    // 得到游戏的状态
    public int GetGameState()
    {
        return gameState;
    }
    // 更换天空
    public void SwitchMaterial()
    {
        cnt = (cnt + 1) % skyboxMaterials.Count;
    }
    // 获取当前闯关模式状态
    public int GetIsStageMode()
    { return isStageMode; }
    // 用于更新闯关模式状态
    private void setStageMode()
    {
        if (isStageMode == 1)
        {
            if (stage_target.Count == 0)
                return;
            // 所有靶子都被击中，该轮闯关游戏结束
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

