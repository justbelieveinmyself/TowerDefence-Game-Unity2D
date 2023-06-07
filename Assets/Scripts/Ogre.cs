using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ogre : Enemy
{
    public Ogre()
    {
        healPoints = 20;
        damage = 3;
        speed = 0.28f;
        bounty = 5;
    }

    public override void TakeDamage(int damageValue)
    {
        if(damageValue < 0)
            return;
        healPoints -= damageValue;
    }

    public override void Upgrade(int wave)
    {
        int strength = wave / 5;
        healPoints += strength * 5;
        damage += strength * 2;
        speed += strength * 0.01f;
        bounty += strength / 2;
    }
}
