using Pirates.AI.Movement;
using Pirates.Control;
using Pirates.Core;
using UnityEngine;

public class ShooterAiController : MonoBehaviour, IPooledObject
{
    [SerializeField] private float attackRange = 10;
    [SerializeField] private float fireRate = 1.0f;
    [SerializeField] private float chaseRange;
    [SerializeField] private float cannonBulletForce = 20f;
    [SerializeField] private float waypointDwellTime;
    [SerializeField] private GameObject cannonBallBullet;
    [SerializeField] private Transform frontFirePoint; 
    [SerializeField] private Transform[] patrolPoints;
    private Transform player;
    private AudioSource shootSound;
    private Movement moveScript;
    private HealthEnemies healthControl;
    private GameplayObserver gameStatChecker;
    private int randomPatrolPoint;
    private float initialWaypointDwellTime;
    private float nextAttackTime = 0f;
    private bool changePosition= false ;
    public void OnObjectSpawn()
    {
        gameStatChecker = FindObjectOfType<GameplayObserver>();
        shootSound = GetComponent<AudioSource>();
        if(gameStatChecker.gameHasEnded == false) 
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        moveScript = GetComponent<Movement>();   
        healthControl = GetComponent<HealthEnemies>();   

        patrolPoints = GameObject.FindGameObjectWithTag("PatrolPath").GetComponentsInChildren<Transform>();
        initialWaypointDwellTime = waypointDwellTime;
    }

    
    void Update()
    {
        if(gameStatChecker.gameHasEnded) { return; }

        if (healthControl.IsDead()) { return; }

        if(player == null ) { return; }

        if(IsInAttackRange())
        {
            moveScript.RotateToTarget(player.transform.position);   
            moveScript.Chase(1.5f);
            if(Time.time >= nextAttackTime)
            {
                Shoot();
                nextAttackTime = Time.time + 1f / fireRate;
            }
        }
        else if(IsInChaseRange())
        {
            moveScript.Chase(moveScript.chaseSpeed);
            moveScript.RotateToTarget(player.transform.position);    
        }
        else 
        {
            PatrolBehaviour();
        }

    }

    private void Shoot()
    {
        shootSound.Play();
        GameObject shotableBullet =  Instantiate(cannonBallBullet, frontFirePoint.position, frontFirePoint.rotation);
        Rigidbody2D bulletRb = shotableBullet.GetComponent<Rigidbody2D>();

        bulletRb.AddForce( frontFirePoint.up * cannonBulletForce, ForceMode2D.Impulse);
    }
    private void PatrolBehaviour()
    {
        waypointDwellTime-=Time.deltaTime;

        if(waypointDwellTime <= 0)
        {
            if(!changePosition)
            {
                randomPatrolPoint = Random.Range(0, patrolPoints.Length);

                changePosition = true;
            }
            
            moveScript.Move( patrolPoints[randomPatrolPoint].position );
            moveScript.RotateToTarget(patrolPoints[randomPatrolPoint].position );

            if(IsAtDestinyPoint())
            {
                waypointDwellTime = initialWaypointDwellTime;
                changePosition= false;
            }
        }
    }

    private bool IsAtDestinyPoint()
    {
        float distanceToDestiny = Vector2.Distance(transform.position, patrolPoints[randomPatrolPoint].position);
        return distanceToDestiny < 1F ;
    }
    private bool IsInChaseRange()
    { 
        if(!IsInAttackRange())
        {
            return false;
        }
        float distanceToTarget =  Vector2.Distance(transform.position, player.position);

        return distanceToTarget < chaseRange ;
    }
    private bool IsInAttackRange()
    {
        float distanceToTarget =  Vector2.Distance(transform.position, player.position);
        return distanceToTarget < attackRange ;
    }
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);   

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);   
    }

    private void OnCollisionEnter2D(Collision2D other) {
        waypointDwellTime = initialWaypointDwellTime;
        changePosition= false;
    }
}
