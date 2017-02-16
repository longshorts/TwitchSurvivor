using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Character : MonoBehaviour {

    public enum CharacterState
    {
        Wander,
        Idle,
        Combat,
        Dead
    }

    public float moveSpeed;
    public CharacterState State { get; protected set; }
    protected Rigidbody2D rigidbody;
    protected GameObject healthBar;

    protected int maxHealth = 5;
    protected int health = 5;
    protected float attackCD = 2f;
    protected int attackPower = 1;
    protected bool attackRdy = true;

    protected static GameObject healthBarContainer;
    [SerializeField] protected float minimumMeleeDistance;


    void Awake()
    {
        GameManager.Instance.ChangeGameStateEvent.AddListener(ChangeState);
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        switch (State)
        {
            case CharacterState.Wander:
                WanderBehaviour();
                break;
            case CharacterState.Combat:
                CombatBehaviour();
                break;
            case CharacterState.Idle:
                IdleBehaviour();
                break;
            default:
                break;
        }
    }

    void ChangeState()
    {
        Debug.Log(name + ": Changing state! " + GameManager.Instance.GameState);
        switch (GameManager.Instance.GameState)
        {
            case GameManager.GameStateType.Combat:
                State = CharacterState.Combat;
                break;
            case GameManager.GameStateType.Journey:
                State = CharacterState.Wander;
                break;
            default:
                State = CharacterState.Idle;
                break;
        }
    }

    protected IEnumerator ProcessAttackCD()
    {
        if (attackRdy)
            yield break;
        else
            yield return new WaitForSeconds(attackCD);

        attackRdy = true;
    }

    public int ModifyHealth(int modifier)
    {
        if (healthBar) healthBar.SetActive(true);
        health += modifier;
        if (health > maxHealth) health = maxHealth;
        else if(health <= 0)
        {
            health = 0;
            State = CharacterState.Dead;
            GetComponent<SpriteRenderer>().color = Color.gray;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Animator>().enabled = false;
            if (healthBar) ObjectPool.instance.PoolObject(healthBar);
            healthBar = null;
        }

        if(healthBar)healthBar.GetComponent<Slider>().value = (float)health / (float)maxHealth;
        return health;
    }

    protected virtual void CombatBehaviour()
    {

    }

    protected virtual void WanderBehaviour()
    {

    }

    protected virtual void IdleBehaviour()
    {

    }

    protected void Move(Vector2 moveVector)
    {
        if (!rigidbody)
            transform.Translate(moveVector);
        else
            GetComponent<Rigidbody2D>().velocity = (moveVector);
    }
}
