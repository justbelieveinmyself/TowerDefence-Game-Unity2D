using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    private Tower tower;
    public GameObject prefabSlot;
    public void CreateTower(GameObject Building)
    {
        int towerPrice = Building.GetComponent<Tower>().Price;
        if(MenuPriceText.Gold >= towerPrice)
        {
            MenuPriceText.Gold -= towerPrice;
            tower = Instantiate(Building.GetComponent<Tower>());
            tower.SpawnTo(BuildMenu.selectedSlot);
        }
    }

    public void UpgradeTower()
    {
        tower = BuildMenu.selectedSlot.GetComponent<Tower>();
        tower.UpgradeTower();
        tower.HideAttackRange();    
    }

    public void SellTower()
    {
        tower = BuildMenu.selectedSlot.GetComponent<Tower>();
        tower.DestroyTower(prefabSlot);
    }
}
