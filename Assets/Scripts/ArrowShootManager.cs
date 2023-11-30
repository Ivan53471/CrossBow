using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShootManager : SSActionManager, ISSActionCallback
{
    // 箭飞行的动作
    private ArrowShoot arrowShoot;
    public MainController mainController;

    void Start()
    {
        // 设置MainController的arrow_manager
        mainController = (MainController)SSDirector.getInstance().currentController;
        mainController.arrow_manager = this;

    }

    //箭飞行
    public void Shoot(GameObject arrow, float force)
    {
        // 实例化一个飞行动作。
        arrowShoot = ArrowShoot.GetSSAction(force);
        // 调用SSActionmanager的方法运行动作。
        this.RunAction(arrow, arrowShoot, this);
    }
    public void SSActionEvent(SSAction source, GameObject arrow = null)
    {
        ArrowTremble tremble = ArrowTremble.GetSSAction();
        RunAction(arrow, tremble, this);
    }
}

