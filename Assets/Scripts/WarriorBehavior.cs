using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;
public class WarriorBehavior : MonoBehaviour, IDamageable
{
    public Slider HealthBar;
    private GameObject[] targets;
    private GameObject target;
    private Enemy fightTarget;
    public Vector3 startPos;
    private float speed = 0.33f;
    private float attackRangeUnit;
    private float attackRangeTower;
    private Vector3 destinition;
    private int healPoints;
    private int startHealPoints;
    public int HealPoints => healPoints;
    public bool isHaveEnemy = false;
    public bool IsDead => healPoints <= 0? true:false;
    public bool MoveToEnemy = false;
    private Animator animatorForWarrior;
    private bool firstDest = true;
    private bool isNotHealing = true;
    void Start()
    {
        var warriorTower = this.GetComponentInParent<WarriorTower>();  
        healPoints = warriorTower.HealPoints;
        startHealPoints = healPoints;
        HealthBar.maxValue = healPoints;
        animatorForWarrior = GetComponent<Animator>();
        warriorTower.warriors++;
        float locationX = warriorTower.warriors * 0.3f;
        if(startPos == Vector3.zero){
            startPos = GameObject.FindGameObjectsWithTag("WayPoint(2)").OrderBy(wayP => 
            Vector3.Distance(warriorTower.gameObject.transform.position, wayP.transform.position)).First().transform.position + new Vector3(0.8f,0,0) - new Vector3(locationX, 0, 0);
        }
    }

    private void Update()
    {
        if(HealPoints < HealthBar.maxValue)
            HealthBar.gameObject.SetActive(true);
        else
            HealthBar.gameObject.SetActive(false);
        HealthBar.value = HealthBar.maxValue - HealPoints;
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        if(targets.Length > 0 && target == null && targets.Where(t => t.GetComponent<Enemy>().IsDead == false && t.GetComponent<Enemy>().isNeedToFight == false && t.GetComponent<Enemy>().isTarget == false).ToArray().Length > 0)
        {
            target = targets.OrderBy(t => Vector3.Distance(this.GetComponentInParent<Transform>().transform.position, t.transform.position)).Where(
            t => t.GetComponent<Enemy>().IsDead == false && t.GetComponent<Enemy>().isNeedToFight == false && t.GetComponent<Enemy>().isTarget == false).First();
        }
        if(target != null)
        {
            var distFromUnitToTarget = Vector3.Distance(transform.position, target.transform.position);
            var distFromTowerToTarget = Vector3.Distance(GetComponentInParent<Tower>().gameObject.transform.position, target.transform.position);
            attackRangeTower = GetComponentInParent<Tower>().AttackRadius;
            attackRangeUnit = 1.5f;
            if(distFromUnitToTarget <= attackRangeUnit && distFromTowerToTarget <= attackRangeTower)
            {
                if(target.GetComponent<Enemy>().isNeedToFight == false)
                {
                    target.GetComponent<Enemy>().isTarget = true;
                    if(firstDest)
                        destinition = target.transform.position - new Vector3(0, 0, 0);
                    this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, destinition
                    , speed * Time.deltaTime);
                }
                else if(destinition != null)
                {
                    this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, destinition
                    , speed * Time.deltaTime);
                }
                
            }
            else
            {
                this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, startPos, speed * Time.deltaTime);
                target = null;
            }
        }
        else 
        {
            this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, startPos, speed * Time.deltaTime);
        }
        if(MoveToEnemy && fightTarget != null)
        {
           this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, destinition, speed * Time.deltaTime);
        }
        if(Vector3.Distance(startPos,gameObject.transform.position) > 0.09)
        {
            animatorForWarrior.SetBool("isRun", true);
        }
        else
        {
            animatorForWarrior.SetBool("isRun", false);
            if(HealPoints < startHealPoints && isNotHealing)
            {
                StartHealing();
                isNotHealing = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy" && !isHaveEnemy && other.gameObject.GetComponent<Enemy>().isNeedToFight == false)
        {
            isHaveEnemy = true;
            firstDest = false;
            other.gameObject.GetComponent<Enemy>().isNeedToFight = true;
            target = other.gameObject;
            destinition = target.gameObject.transform.position - new Vector3(0.15f, 0, 0);
            other.gameObject.GetComponent<Enemy>().StartFight(this.gameObject);
            StartFight();
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Warrior")
        {
            Vector3 direction = (transform.position - other.transform.position).normalized;
            transform.position += direction * 0.001f;
        }
    }

    private async void StartFight()
    {
        try
        {
        fightTarget = target.GetComponent<Enemy>();
        Enemy enemy = fightTarget;
        int damage = this.gameObject.GetComponentInParent<Tower>().Damage;
        animatorForWarrior.SetBool("isFight", true);
        while(!enemy.IsDead && !IsDead && enemy.gameObject != null)
        {
            this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, destinition, speed * Time.deltaTime);
            MoveToEnemy = true;
            enemy.TakeDamage(damage);
            await Task.Delay(2000);
        }
        MoveToEnemy = false;    
        if(enemy != null && enemy.IsDead){
            var currentClipInfo = enemy.animatorForUnit.GetCurrentAnimatorClipInfo(0);
            var currentClipLength = currentClipInfo[0].clip.length;
            enemy.animatorForUnit.SetBool("isDead", true);
            destinition = startPos;
            Destroy(enemy.gameObject, currentClipLength);
        }
        animatorForWarrior.SetBool("isFight", false);
        isHaveEnemy = false;
        }
        catch(System.Exception){}
    }

    private async void StartHealing()
    {
        while(healPoints != startHealPoints)
        {
            healPoints+= 1;
            await Task.Delay(300);
        }
        isNotHealing = true;
    }

    public void TakeDamage(int damageValue)
    {
        if(damageValue < 0)
            return;
        healPoints -= damageValue;
    }
    
    public void OnDied()
    {
        GetComponentInParent<WarriorTower>().RespawnWarrior(startPos);
    }
}