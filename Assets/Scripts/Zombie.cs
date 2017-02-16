using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zombie : Character {

	// Use this for initialization
	void Start () {
        State = CharacterState.Idle;

        if (!healthBarContainer)
            healthBarContainer = GameObject.Find("HealthBarContainer");
        healthBar = ObjectPool.instance.GetObjectForType("HealthBar", false);
        healthBar.transform.SetParent(healthBarContainer.transform, false);
        healthBar.GetComponent<HealthBarTracker>().TrackedCharacter = this;
        healthBar.SetActive(false);
    }

    private static List<Survivor> enemies;
    private Character target;
    [SerializeField]

    protected override void CombatBehaviour()
    {
        //Search for enemies
        if (enemies == null)
            enemies = new List<Survivor>(GameObject.FindObjectsOfType<Survivor>());

        //Establish target
        //Find a new target if no target
        if (!target)
        {
            target = BestTarget();
        }
        //Find a new target if current target is dead
        else if (target.State == CharacterState.Dead)
        {
            target = BestTarget();
        }

        //Melee
        //Decide if need to close distance or melee
        if (Vector2.Distance(transform.position, target.transform.position) > minimumMeleeDistance)
        {
            Move((target.transform.position - transform.position).normalized * moveSpeed);
            GetComponent<SpriteRenderer>().color = Color.white;

        }
        else
        {
            //Do Melee
            GetComponent<SpriteRenderer>().color = Color.yellow;
            Move(Vector2.zero);
        }

    }

    private Character BestTarget()
    {
        Character bestTarget = enemies[0];
        if (enemies != null)
        {
            foreach (Character c in enemies)
            {
                float cDistance = Vector2.Distance(transform.position, c.transform.position);
                float bestDistance = Vector2.Distance(transform.position, bestTarget.transform.position);

                if (cDistance < bestDistance)
                    bestTarget = c;
            }
        }
        else
            Debug.LogWarning("Need to establish enemies before finding target");

        return bestTarget;
    }
}
