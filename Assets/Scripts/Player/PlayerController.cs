using Pirates.Core;
using Pirates.Control;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float cannonBulletForce = 20f;
    [SerializeField] private Transform frontFirePoint;
    [SerializeField] private Transform[] sideFirePoints;
    [SerializeField] private GameObject cannonBallBullet;    
    private Camera mainCamera;
    private PlayerControls playerControls;
    private Rigidbody2D rb;
    private AudioSource shootSound;
    private HealthPlayer healthControl;
    private GameplayObserver gameStatChecker;
    private Vector2 movementReader;
    private Vector2 mousePosition;

    void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        shootSound = GetComponent<AudioSource>();
        healthControl = GetComponent<HealthPlayer>();
        gameStatChecker = FindObjectOfType<GameplayObserver>();

        mainCamera = Camera.main;
    }
    void Update()
    {
        if(gameStatChecker.gameHasEnded) { return; }

        if (healthControl.IsDead()) { return; }
        PlayerMovementInput();
        PlayerMousePosition();
        PlayerFrontShoot();
        PlayerSideShoot();
    }

    private void FixedUpdate() 
    {
        if(gameStatChecker.gameHasEnded) { return; }
        
        if (healthControl.IsDead()) { return; }
        Move();
        Rotate();
    }

    private void PlayerMovementInput()
    {
        movementReader = playerControls.Gameplay.Move.ReadValue<Vector2>();
    }
    private void PlayerMousePosition()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void PlayerFrontShoot()
    {
        playerControls.Gameplay.Fire.performed += FrontShoot;
    }
    private void PlayerSideShoot()
    {
        playerControls.Gameplay.SideFire.performed += SideShoot;  
    }
    private void Move()
    {
        rb.MovePosition( rb.position + movementReader * (moveSpeed *Time.fixedDeltaTime) );
    }

    private void Rotate()
    {
        Vector2 lookDirection = mousePosition - rb.position;

        float targetRotationAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

        float smoothedRotation = Mathf.LerpAngle(rb.rotation, targetRotationAngle, Time.deltaTime * rotationSpeed);

        rb.rotation = smoothedRotation;
    } 

    private void FrontShoot(InputAction.CallbackContext context)
    {
        shootSound.Play();
        GameObject shotableBullet =  Instantiate(cannonBallBullet, frontFirePoint.position, frontFirePoint.rotation);
        Rigidbody2D bulletRb = shotableBullet.GetComponent<Rigidbody2D>();

        bulletRb.AddForce( frontFirePoint.up * cannonBulletForce, ForceMode2D.Impulse);
    }

    private void SideShoot(InputAction.CallbackContext context)
    {
        shootSound.Play();
        for( int i = 0; i < 3;  i++  )
        {
            GameObject shotableBullet =  Instantiate(cannonBallBullet, sideFirePoints[i].position, frontFirePoint.rotation);
            Rigidbody2D bulletRb = shotableBullet.GetComponent<Rigidbody2D>();

            bulletRb.AddForce( sideFirePoints[i].right * cannonBulletForce, ForceMode2D.Impulse);
        }
    }

    private void OnEnable() 
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
