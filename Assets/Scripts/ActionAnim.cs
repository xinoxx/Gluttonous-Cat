using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAnim : MonoBehaviour
{
    public Animator animtor;

    // ��������������
    public void HideActionTip()
    {
        if (gameObject.activeSelf)
        {
            animtor.SetBool("show", false);
        }
    }
}
