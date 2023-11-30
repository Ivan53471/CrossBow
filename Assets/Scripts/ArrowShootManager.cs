using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShootManager : SSActionManager, ISSActionCallback
{
    // �����еĶ���
    private ArrowShoot arrowShoot;
    public MainController mainController;

    void Start()
    {
        // ����MainController��arrow_manager
        mainController = (MainController)SSDirector.getInstance().currentController;
        mainController.arrow_manager = this;

    }

    //������
    public void Shoot(GameObject arrow, float force)
    {
        // ʵ����һ�����ж�����
        arrowShoot = ArrowShoot.GetSSAction(force);
        // ����SSActionmanager�ķ������ж�����
        this.RunAction(arrow, arrowShoot, this);
    }
    public void SSActionEvent(SSAction source, GameObject arrow = null)
    {
        ArrowTremble tremble = ArrowTremble.GetSSAction();
        RunAction(arrow, tremble, this);
    }
}

