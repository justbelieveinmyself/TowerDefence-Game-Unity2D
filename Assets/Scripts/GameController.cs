using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameController : MonoBehaviour
{
    private static int lifes = 20;
    public GameObject LifesInfoParent;
    public GameObject GameOverParent;    
    private static TMP_Text lifesInfo;
    private static TMP_Text GameOver;
    public static int Lifes
    {
        get => lifes;
        private set
        {
            lifes = value;
            ChangeInfoLifes();
        }
    } 
    private void Start() {
        lifesInfo = LifesInfoParent.GetComponentInChildren<TMP_Text>();
        GameOver = GameOverParent.GetComponentInChildren<TMP_Text>();
    }
    public static void OnEnemyPassed()
    {
        Lifes -= 1;
    }
    private static void ChangeInfoLifes()
    {
        lifesInfo.text = lifes.ToString(); 
        if(lifes <= 0)
        {
            GameOver.gameObject.SetActive(true);
        }
    }
}
