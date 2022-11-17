using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpBar : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Image foreground;

    private void Start()
    {
        player.OnExpGained += OnExpGain;
        OnExpGain(0, 1);
    }

    public void OnExpGain(int currentXP, int XpToLvl)
    {
        float xpRatio = (float) currentXP/XpToLvl;
        foreground.transform.localScale = new Vector3(xpRatio, 1, 1);
    }
}
