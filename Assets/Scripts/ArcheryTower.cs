using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcheryTower : Tower
{

    private int price = 75;
    private int upgradePrice = 200;
    private int sellPrice = 50;
    private int level = 1;
    private int damage = 8;
    private float attackRadius = 1.5f;
    public override int Price => price;
    public override int SellPrice => sellPrice;
    public override int Level => level;
    public override int Damage => damage;
    public override float AttackRadius => attackRadius;
    public override int UpgradePrice => upgradePrice;

    public override void UpgradeTower()
    {
        if(MenuPriceText.Gold >= UpgradePrice)
        {
            MenuPriceText.Gold -= UpgradePrice;
        }
        else
        {
            return;
        }
        level ++;
        switch (level)
        {
            case 2:
                upgradePrice += 190; // чтоб получить лвл 3;
                damage += 9;
                attackRadius += 0.25f;
                break;
            case 3:
                upgradePrice = 1000;
                damage += 19;
                attackRadius += 0.5f;
                break;
        }
        sellPrice = upgradePrice/2;
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = level == 2? spriteTowerLvl2 : spriteTowerLvl3;
    }

    public override void DestroyTower(GameObject prefabSlot)
    {
        MenuPriceText.Gold += SellPrice;
        var newSlot = GameObject.Instantiate(prefabSlot);
        newSlot.transform.position = this.gameObject.transform.position - new Vector3(0, 0.3f, 0);
        Destroy(this.gameObject);
    }
}
