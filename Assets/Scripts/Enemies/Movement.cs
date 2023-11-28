using UnityEngine;
using Pirates.Core;

namespace Pirates.AI.Movement
{
    public class Movement : MonoBehaviour,IPooledObject
    {
        [SerializeField] private float normalSpeed;
        [SerializeField] private float rotationSpeed;
        public float chaseSpeed;
        private Transform player;
        private Rigidbody2D rb;
        private GameplayObserver gameStatChecker;

        public void OnObjectSpawn()
        {
            gameStatChecker = FindObjectOfType<GameplayObserver>();
            rb = GetComponent<Rigidbody2D>();
            if(gameStatChecker.gameHasEnded == false) 
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
        public void Move( Vector2 direction)
        {
            Vector2 directionTo = direction - rb.position;
            directionTo.Normalize();
            rb.MovePosition( rb.position + directionTo * (normalSpeed * Time.fixedDeltaTime) );
        }

        public void Chase(float speed)
        {
            Vector2 directionToPlayer = (Vector2)player.position - rb.position;
            directionToPlayer.Normalize();

            rb.MovePosition(rb.position + directionToPlayer * speed * Time.fixedDeltaTime);
        }
        public void RotateToTarget(Vector2 target)
        {            
            Vector2 lookDirection = target - rb.position;

            float targetRotationAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

            float smoothedRotation = Mathf.LerpAngle(rb.rotation, targetRotationAngle, Time.deltaTime * rotationSpeed);

            rb.rotation = smoothedRotation;
        }
    }
}
