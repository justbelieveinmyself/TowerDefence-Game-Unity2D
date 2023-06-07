using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public abstract int Damage{get;}
    public abstract int Price{get;}
    public abstract int UpgradePrice{get;}
    public abstract int SellPrice{get;}
    public abstract int Level{get;}
    public abstract float AttackRadius {get;}
    public Sprite spriteTowerLvl2;
    public Sprite spriteTowerLvl3;
    public GameObject radiusAttackRenderer;
    public void SpawnTo(GameObject slot){
        Destroy(slot);
        transform.position = slot.transform.position + new Vector3(0, 0.3f, 0);
    }
    public abstract void UpgradeTower();
    public void ShowAttackRange()
    {
        float scaler = AttackRadius/1.5f * 0.3f;
        radiusAttackRenderer.transform.localScale = new Vector3(scaler, scaler, 0);
        radiusAttackRenderer.SetActive(true);
    }
    public void HideAttackRange()
    {
        radiusAttackRenderer.SetActive(false);
    }
    public abstract void DestroyTower(GameObject prefabSlot);
}
