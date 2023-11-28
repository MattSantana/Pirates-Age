using Pirates.Control;
using Pirates.AI.Movement;
using Pirates.Core;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ChaserAiController : MonoBehaviour, IPooledObject
{
    [SerializeField] private float chaseRange;
    [SerializeField] private float damageValue;
    [SerializeField] private float waypointDwellTime;
    [SerializeField] private float volume = 0.5f; 
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject floatText;
    [SerializeField] private Vector3 floatTextOffset;
    private Transform player;
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
        if(gameStatChecker.gameHasEnded == false) 
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        moveScript = GetComponent<Movement>();

        healthControl = GetComponent<HealthEnemies>();
        
        patrolPoints = GameObject.FindGameObjectWithTag("PatrolPath").GetComponentsInChildren<Transform>();
        initialWaypointDwellTime = waypointDwellTime;   
    }
    void FixedUpdate()
    {
        if(gameStatChecker.gameHasEnded) { return; }
        
        if (healthControl.IsDead()) { return; }

        if(player == null ) { return; }

        if(IsInChaseRange())
        {
            moveScript.Chase(moveScript.chaseSpeed);
            moveScript.RotateToTarget(player.transform.position);    
        }
        else 
        {
            PatrolBehaviour();
        }     
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
        float distanceToTarget =  Vector2.Distance(transform.position, player.position);

        return distanceToTarget < chaseRange ;
    }
    private void PlayExplosionAudio()
    {
        AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, volume);
    }
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);   
    }
    
    private void IntanciatingEffects()
    {
      GameObject impactIntance = Instantiate(impactEffect, transform.position, Quaternion.identity);

      Destroy(impactIntance, 1f);

      GameObject textInstance = Instantiate(floatText, transform.position + floatTextOffset, Quaternion.identity);

      textInstance.GetComponentInChildren<TextMeshProUGUI>().text = damageValue.ToString();

      Destroy(textInstance, 0.8f);
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.transform.tag != "Player")
        {
            waypointDwellTime = initialWaypointDwellTime;
            changePosition= false;
        }else
        {
            other.gameObject.GetComponent<HealthPlayer>().ApplyDamage(damageValue);

            PlayExplosionAudio();
            IntanciatingEffects();
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = healthControl.sprites[0]; 
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            healthControl.healthPoints= 100;
            gameObject.SetActive(false);
        }

    }
}
