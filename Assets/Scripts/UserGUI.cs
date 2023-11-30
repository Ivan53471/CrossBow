using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private MainController action;
    GUIStyle tip_style = new GUIStyle(); 
    GUIStyle title_style = new GUIStyle();
    GUIStyle button_style = new GUIStyle();
    GUIStyle state_style = new GUIStyle();
    GUIStyle time_style = new GUIStyle();
    public Texture texture;
    private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        action = this.transform.gameObject.GetComponent<MainController>();
        tip_style.fontSize = 30;
        tip_style.normal.textColor = Color.blue;

        title_style.normal.textColor = Color.red;
        title_style.fontSize = 40;

        button_style.normal.textColor = Color.red;
        button_style.fontSize = 20;

        state_style.normal.textColor = Color.red;
        state_style.fontSize = 30;

        time_style.normal.textColor = Color.red;
        time_style.fontSize = 20;

    }

    // Update is called once per frame
    void Update()
    {
        if (action.GetGameState() == 1)
        {
            // 实现射击
            // 点击鼠标右键拉弓
            if (Input.GetButtonDown("Fire2"))
            {
                action.LoadArrow();
            }
            if (action.GetisLoad())
            {
                // 调整射箭力量
                float scrollInput = Input.GetAxis("Mouse ScrollWheel") / 2; //向上滑+1，反之-1
                action.SetForce(scrollInput);
                // 左键射击
                if (Input.GetButtonDown("Fire1"))
                {
                    action.ShootArrow();
                }   
            }
            // 实现闯关模式计时
            if (action.GetIsStageMode() == 1)
                time += Time.deltaTime;
            // 进入/重启闯关模式
            if (Input.GetKeyDown(KeyCode.R))
            {
                time = 0;
                action.Restart();
            }
        }
        // 切换天空
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            action.SwitchMaterial();
        }
    }
    //
    private void OnGUI()
    {
        if (action.GetGameState() == 1)
        {
            // 显示准星
            int width = texture.width / 20;
            int height = texture.height / 20;

            Rect rect = new Rect( (Screen.width - width) / 2, 
                (Screen.height - height) / 2, width, height);
            GUI.DrawTexture(rect, texture);

            //显示各种信息
            GUI.Label(new Rect(20, 10, 150, 50), "Score: ", button_style);
            GUI.Label(new Rect(100, 10, 100, 50), action.GetScore().ToString(), state_style);

            GUI.Label(new Rect(20, 50, 150, 50), "ArrowNum: ", button_style);
            GUI.Label(new Rect(120, 50, 100, 50), action.GetArrowNum().ToString(), button_style);
            if(action.GetIsStageMode() != 0)
                GUI.Label(new Rect(20, 80, 150, 50), "Time: " + time + " s", time_style);
            if(action.GetIsStageMode() == 2)
                GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height / 2, 400, 100),
                "闯关模式结束，请按R重新开始", tip_style);
        }
        //游戏未开始，检测开始按键
        else
        {
            GUI.Label(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 320, 100, 100), "  Counter Strike:\nPrimitive Campaign", title_style);
            //开始按键如果按下，游戏开始
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 100, 100, 50), "游戏开始"))
            {
                action.BeginGame();
            }
            GUI.Label(new Rect(Screen.width / 2 - 350, Screen.height / 2, 400, 100), 
                "使用方向键控制弓箭移动，鼠标右键拉弓，左键射击\n按r进入闯关模式，按tab切换天空", tip_style);
        }
    }
}

