using DSFramework;
using UnityEngine;

public class DSInput : DSingle<DSInput>
{
    private bool isStart = false;

    public DSInput()
    {
        DSEntity.Mono.AddUpdate("InputUpdate", InputUpdate);
    }

    public void StartOrEndCheck(bool isOpen)
    {
        isStart = isOpen;
    }

    //检测按键抬起或者按下
    public void CheckKeyCode(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            //DSEventCenter.DSInstance.EventTrigger($"某键按下",key);
        }

        if (Input.GetKeyUp(key))
        {
            //DSEventCenter.DSInstance.EventTrigger("某键抬起", key);
        }
    }

    private void InputUpdate()
    {
        if (!isStart) return;

        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.D);
    }
}