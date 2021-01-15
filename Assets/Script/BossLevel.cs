using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BossLevel : MonoBehaviour
{
    public NewBoss boss;

    private void Update()
    {
        if (boss == null || boss.isDead)
        {
            Singleton.instance.level.status = Level.LevelStatus.Won;
        }
    }
}