using UnityEngine;

public enum SSActionEventType : int { Started, Competed }
public interface ISSActionCallback
{
    // �ص�����
    void SSActionEvent(SSAction source, GameObject arrow = null);
}
