using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MenuPriceText : MonoBehaviour
{
    public static TMP_Text goldInfo;
    public TMP_Text textPriceArchery;
    public TMP_Text textPriceWarrior;
    public TMP_Text textPriceMage;
    public TMP_Text textPriceRocket;
    public GameObject UpgradeMenu;
    public GameObject GoldInfoParent;
    public static TMP_Text upgradePriceTower;
    public static TMP_Text sellPriceTower;
    private static int gold;
    public static int Gold
    {
        get => gold;
        set
        {
            gold = value;
            ChangeInfoGold();
        }
    }

    public static void ChangeInfoGold()
    {
        goldInfo.text = gold.ToString();     
    }

    void Start()
    {
        gold = 130;
        goldInfo = GoldInfoParent.GetComponentInChildren<TMP_Text>();
        goldInfo.text = gold.ToString();
        upgradePriceTower = UpgradeMenu.GetComponentInChildren<TMP_Text>();
        textPriceArchery.text = "75";
        textPriceWarrior.text = "50";
        textPriceMage.text = "?";
        textPriceRocket.text = "?";
    }

    public static void ChangeInfoUpgrade(Tower tower)
    {
        upgradePriceTower.text = tower.UpgradePrice.ToString();
    }
}
