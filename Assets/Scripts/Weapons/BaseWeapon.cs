using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    protected int level = 0;
    protected float size;    
    protected float numToSpawn; 
    protected float damageModifier;

    public float DamageModifier { get => damageModifier; set => damageModifier = value; }
    public float Size { get => size; set => size = value; }
    public float NumToSpawn { get => numToSpawn; set => numToSpawn = value; }

    internal void LevelUp()
    {
        if (++level == 1)
        {
            gameObject.SetActive(true);
        }
    }
}
