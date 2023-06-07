using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class BuildMenu : MonoBehaviour
{
    public GameObject buttonMenu;
    public GameObject buttonTowerManager;
    public static GameObject selectedSlot;
    private GameObject previosSelectedSlot;
    Animator animatorForSlot;
    Animator animatorForMenu;
    private GameObject previosSelectedTower;
    private void Start() 
    {
        buttonMenu.SetActive(false);
        buttonTowerManager.SetActive(false);
        animatorForMenu = buttonMenu.GetComponent<Animator>();
    }
    
    private void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);
            previosSelectedSlot = previosSelectedSlot == null? new GameObject("123") : previosSelectedSlot;
            
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                if(hit != false)
                {
                    selectedSlot = hit.transform.gameObject;
                    if (previosSelectedSlot?.transform?.position != selectedSlot?.transform?.position && animatorForSlot != null) 
                    {
                        animatorForSlot.SetBool("IsSelected", false);
                    }

                    animatorForSlot = selectedSlot.GetComponent<Animator>();
                    if(selectedSlot.tag == "Slot")
                    {
                        buttonTowerManager.SetActive(false);
                        buttonMenu.SetActive(true);
                        buttonMenu.transform.position = selectedSlot.transform.position;
                        animatorForSlot.SetBool("IsSelected", true);
                    }
                    else if(selectedSlot.tag == "Tower")
                    {
                        Tower tower = selectedSlot.gameObject.GetComponent<Tower>();
                        MenuPriceText.ChangeInfoUpgrade(tower);
                        buttonMenu.SetActive(false);
                        buttonTowerManager.SetActive(true);
                        tower.ShowAttackRange();
                        buttonTowerManager.transform.position = selectedSlot.transform.position;
                        if(previosSelectedTower != null && previosSelectedTower != selectedSlot)
                        {
                            previosSelectedTower.GetComponent<Tower>().HideAttackRange();
                        }
                        previosSelectedTower = selectedSlot;
                    }
                    
                    previosSelectedSlot = selectedSlot;
                }
                else
                {
                    if(animatorForSlot != null)
                    {
                        animatorForSlot.SetBool("IsSelected", false);
                    }
                    if(previosSelectedTower != null)
                        previosSelectedTower?.GetComponent<Tower>()?.HideAttackRange();
                    buttonMenu.SetActive(false);
                    buttonTowerManager.SetActive(false);
                }
            } 
        }
    }
}
