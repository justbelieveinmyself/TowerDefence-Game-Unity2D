using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
public class ArchShoot : MonoBehaviour
{
    public GameObject prefabArrow;
    private GameObject target;
    private GameObject[] targets;
    private Animator animator;
    private Vector3 direction;
    private float rotationZ;
    private int timeBtw = 1000;
    private bool isCoolDown = false;
    public bool needInShowRange = false;
    private float attackRange;
    private void Start()
    {
        animator = this.GetComponent<Animator>();   
    }

    private void Update() 
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        if(targets.Length > 0 && targets.Where(t => t.GetComponent<Enemy>().IsDead == false).ToArray().Length > 0)
        { //null exception
            target = targets.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).Where(t => t.GetComponent<Enemy>().IsDead == false).First();
            var dist = Vector3.Distance(transform.position - new Vector3(0, 0.216f, 0), target.transform.position);
            attackRange = this.gameObject.GetComponentInParent<Tower>().AttackRadius;
            if (dist <= attackRange && !isCoolDown)
            {
                StartShoot();
                SetCoolDown();
            }
            else if (dist <= attackRange)
            {
                animator.SetBool("Shooting", true);
            }
            else
            {
                target = null;
                animator.SetBool("Shooting", false);
            }
        }
        else
        {
            animator.SetBool("Shooting", false);
        }        
    }
    private void StartShoot()
    {
        direction = target.transform.position - transform.position;
        rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        animator.SetBool("Shooting", true);
        animator.SetFloat("Rotation", rotationZ);
        var towerDamage = gameObject.GetComponentInParent<Tower>().Damage;
        var arrow = GameObject.Instantiate(prefabArrow, this.transform.position, Quaternion.Euler(0,0, rotationZ));
        arrow.GetComponent<Arrow>().damage = towerDamage; 
    }

    private async void SetCoolDown()
    {
        isCoolDown = true;
        await Task.Delay(timeBtw);
        isCoolDown = false;
    }
}