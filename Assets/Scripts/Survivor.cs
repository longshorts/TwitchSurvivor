using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Survivor : Character
{

    public enum CombatType
    {
        Melee,
        Gun,
        Pyro
    }

    [SerializeField]
    private Collider2D wanderBounds;

    
    private Vector2 _wanderDirection;
    private Collider2D _myCollider;
    private CombatType _myCombatType;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(SetWanderDirection());
        _myCollider = GetComponent<Collider2D>();

        if (!healthBarContainer)
            healthBarContainer = GameObject.Find("HealthBarContainer");
        healthBar = ObjectPool.instance.GetObjectForType("HealthBar", false);
        healthBar.transform.SetParent(healthBarContainer.transform, false);
        healthBar.GetComponent<HealthBarTracker>().TrackedCharacter = this;
        healthBar.GetComponent<Slider>().value = (float)health / (float)maxHealth;
    }

    

    private static List<Zombie> enemies;
    private Character target;
    public float targetDistance;

    protected override void CombatBehaviour()
    {
        //Search for enemies
        if(enemies == null)
            enemies = new List<Zombie>(GameObject.FindObjectsOfType<Zombie>());


        //Establish target
        //Find a new target if no target
        if (!target)
        {
            target = BestTarget();
        }
        //Find a new target if current target is dead
        else if(target.State == CharacterState.Dead)
        {
            target = BestTarget();
        }

        if (!target)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            return;
        }

        //Determine combat method
        switch (_myCombatType)
        {
            default:
                //Melee
                //Decide if need to close distance or melee
                targetDistance = Vector2.Distance(transform.position, target.transform.position);
                if (targetDistance > minimumMeleeDistance)
                {
                    Move((target.transform.position - transform.position).normalized * moveSpeed);
                    GetComponent<SpriteRenderer>().color = Color.white;

                }
                else
                {
                    //Do Melee
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                    Move(Vector2.zero);

                    if (attackRdy)
                    {
                        target.ModifyHealth(-attackPower);
                        attackRdy = false;
                        StartCoroutine(ProcessAttackCD());
                    }
                }
                break;
        }

    }


    private Character BestTarget()
    {
        Character bestTarget = enemies[0];
        int aliveEnemiesSeen = 0;
        if(enemies != null)
        {
            foreach(Character c in enemies)
            {
                if (c.State != CharacterState.Dead)
                {
                    float cDistance = Vector2.Distance(transform.position, c.transform.position);
                    float bestDistance = Vector2.Distance(transform.position, bestTarget.transform.position);
                    aliveEnemiesSeen++;
                    //gameObject.GetComponentInChildren<TextMesh>().text = aliveEnemiesSeen.ToString();

                    if (cDistance < bestDistance || bestTarget.State == CharacterState.Dead)
                        bestTarget = c;
                }
            }
        } else
            Debug.LogWarning("Need to establish enemies before finding target");

        if (bestTarget.State != CharacterState.Dead)
            return bestTarget;
        else
            return null;
    }

    protected override void WanderBehaviour()
    {
        Move(_wanderDirection * moveSpeed);

        //Constrain movement to the survivorBounds
        if (Mathf.Abs(wanderBounds.bounds.center.x - transform.position.x)
                + _myCollider.bounds.extents.x > wanderBounds.bounds.extents.x)
        {
            _wanderDirection = Vector3.Reflect(_wanderDirection, Vector3.right);
            Move(_wanderDirection * moveSpeed);

        }
        if (Mathf.Abs(wanderBounds.bounds.center.y - transform.position.y)
            + _myCollider.bounds.extents.y > wanderBounds.bounds.extents.y)
        {
            _wanderDirection = Vector3.Reflect(_wanderDirection, Vector3.up);
            Move(_wanderDirection * moveSpeed);

        }

    }

    

    IEnumerator SetWanderDirection()
    {
        while (gameObject.activeSelf)
        {
            _wanderDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            _wanderDirection = _wanderDirection.normalized;
            yield return new WaitForSeconds(Random.Range(1f, 5f));
        }
    }


}
