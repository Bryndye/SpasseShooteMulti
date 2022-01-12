using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomPlayer : MonoBehaviour
{
    [SerializeField] Image spriteColor;
    [SerializeField] Slider sR, sG, sB;

    public void OnValueChangeSetColor()
    {
        spriteColor.color = new Color(sR.value, sG.value, sB.value);
    }

}
