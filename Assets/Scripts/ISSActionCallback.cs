using UnityEngine;

public enum SSActionEventType : int { Started, Competed }
public interface ISSActionCallback
{
    // »Øµ÷º¯Êý
    void SSActionEvent(SSAction source, GameObject arrow = null);
}
