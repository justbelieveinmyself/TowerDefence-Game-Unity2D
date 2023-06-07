using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class WarriorTower : Tower
{
    public GameObject prefabWarrior;
    private int price = 50;
    private int upgradePrice = 100;
    private int sellPrice = 45;
    private int level = 1;
    private int damage = 3;
    private int healPoints = 25;
    public int warriors = 0;
    private float attackRadius = 2f;
    public override int Price => price;
    public override int SellPrice => sellPrice;
    public override int Level => level;
    public override int Damage => damage;
    public override float AttackRadius => attackRadius;
    public override int UpgradePrice => upgradePrice;
    public int HealPoints => healPoints;
    public override void DestroyTower(GameObject prefabSlot)
    {
        MenuPriceText.Gold += SellPrice;
        var newSlot = GameObject.Instantiate(prefabSlot);
        newSlot.transform.position = this.gameObject.transform.position - new Vector3(0, 0.3f, 0);
        Destroy(this.gameObject);
    }
    public override void UpgradeTower()
    {
        if(MenuPriceText.Gold < UpgradePrice)
        {
            return;
        }
        MenuPriceText.Gold -= UpgradePrice;
        level ++;
        switch (level)
        {
            case 2:
                upgradePrice += 100; // чтоб получить лвл 3;
                damage += 3;
                attackRadius += 0.1f;
                healPoints += 10;
                break;
            case 3:
                upgradePrice = 1000;
                damage += 6;
                attackRadius += 0.2f;
                healPoints += 15;
                break;
        }
        sellPrice = upgradePrice/2;
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = level == 2? spriteTowerLvl2 : spriteTowerLvl3;
    }
    public async void RespawnWarrior(Vector3 spawnPos)
    {
        await Task.Delay(10000);
        GameObject newWarrior = Instantiate(prefabWarrior, gameObject.transform);
        newWarrior.GetComponent<WarriorBehavior>().startPos = spawnPos;

    }
}
