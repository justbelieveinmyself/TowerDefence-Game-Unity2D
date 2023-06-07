using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
public abstract class Enemy : MonoBehaviour, IDamageable
{
    protected int healPoints;
    protected int damage;
    protected float speed;
    protected int bounty;
    public Slider HealthBar;
    public int HealPoints
    {
      get => healPoints;
      set
      {
        if (value > 0)
            healPoints = value;
      }   
    } 
    public int Damage
    {
      get => damage;
      set
      {
        if (value > 0)
            damage = value;
      }   
    } 
    public float Speed
    {
      get => speed;
      set
      {
        if (value > 0)
            speed = value;
      }   
    } 
    public int Bounty
    {
      get => bounty;
      set
      {
        if (value > 0)
            bounty = value;
      }   
    } 
    public bool IsDead
    {
        get => healPoints <= 0? true:false;
    }

    private GameObject[] way1Points1;
    private GameObject[] way1Points2;
    private Vector2 destination;
    private int currentWayPoint;
    private int randomWay;
    public bool isNeedToFight = false;
    public bool isTarget = false;
    public Animator animatorForUnit;
    public abstract void TakeDamage(int damageValue);
    public abstract void Upgrade(int wave);
    public void SpawnTo(Vector2 spawnPoint)
    {
        Vector2 randomPoint = Random.insideUnitCircle * 0.7f;
        transform.position = spawnPoint + randomPoint;
    }
    private void Start() 
    {
        HealthBar.maxValue = HealPoints;
        animatorForUnit = GetComponent<Animator>();
        way1Points1 = GameObject.FindGameObjectsWithTag("WayPoint");
        way1Points2 = GameObject.FindGameObjectsWithTag("WayPoint(1)");
        randomWay = Random.Range(0,2);
        destination = randomWay == 0? way1Points1[currentWayPoint].transform.position : way1Points2[currentWayPoint].transform.position; 
    }
    private void OnDestroy()
    {
        MenuPriceText.Gold += Bounty;
        UnitFactory.currentCountOfUnits -= 1;
    }
    private void Update() 
    {
        if(HealPoints < HealthBar.maxValue)
            HealthBar.gameObject.SetActive(true);
        HealthBar.value = HealthBar.maxValue - HealPoints;
        if(IsDead || isNeedToFight)
        {
            destination = transform.position;
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, destination, Speed * Time.deltaTime);
        if(Vector2.Distance(transform.position, destination) < 0.01f && currentWayPoint < way1Points1.Length-1)
        {
            randomWay = Random.Range(0,2);
            currentWayPoint ++;
            destination = randomWay == 0? way1Points1[currentWayPoint].transform.position : way1Points2[currentWayPoint].transform.position;
        }
        if(currentWayPoint == way1Points1.Length-1)
        {
            GameController.OnEnemyPassed();
            Destroy(this.gameObject);
        }
    }

    public async void StartFight(GameObject target)
    {
        try
        {
            animatorForUnit.SetBool("isFight", true);
            isNeedToFight = true;
            WarriorBehavior warrior = target.GetComponent<WarriorBehavior>();
            while(!warrior.IsDead && !IsDead && warrior.gameObject != null)
            {
                warrior.TakeDamage(Damage);
                await Task.Delay(2000);
            }
            if(warrior != null && warrior.IsDead)
            {
                warrior.OnDied();
                Destroy(warrior.gameObject);
            }
            isNeedToFight = false;
            if(animatorForUnit != null) 
                animatorForUnit.SetBool("isFight", false);
        }
        catch(System.Exception)
        {
            isNeedToFight = false;
            if(animatorForUnit != null) 
            animatorForUnit.SetBool("isFight", false);
        }
    }
}