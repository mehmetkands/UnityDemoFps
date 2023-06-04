using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private CharacterController playerController;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    public WeaponAk47 weaponAK47GO;

    [SerializeField]
    private float playerHealth = 100.0f;

    [SerializeField]
    private float characterMoveSpeed;

    [SerializeField]
    private Spectator spectatorActor;

    [SerializeField]
    private HudWidget hudWidget;

    [SerializeField]
    private DeadWidget deadMenuWidget;

    [SerializeField]
    private MessageWidgetScript messageWidget;

    [SerializeField]
    bool IsMouseHide = true;

    [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;

    [Range(0.0f, 0.5f)] float LookSmoothTime = 0.03f;

    Vector2 currentDirection = Vector2.zero;

    Vector2 currentDirectionVelocity = Vector2.zero;

    Vector2 currentLookDelta = Vector2.zero;
    Vector2 currentLookDeltaVelocity = Vector2.zero;

    float Speed = 6f;

    float TurnSmoothAmmount = 1.1f;

    float TurnSmoothVelocity;

    float MouseSensivity = 3.0f;

    float cameraPitch = 0.0f;

    float gravity = -13f;

    float velocityY = 0f;


    // Start is called before the first frame update
    void Start()
    {
        HideMouseCursor(IsMouseHide);
    }

    // Update is called once per frame
    void Update()
    {
        MouseLook();

        Move();

        Run();

        SlowWalk();

        GeneralInputs();

        Killer();
    }

    void HideMouseCursor(bool hideCursor)
    {
        if (hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Killer()
    {
        if (playerHealth <= 0)
        {
            spectatorActor.EnableActor(true);

            Destroy(gameObject, 0);
        }
    }


    void MouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentLookDelta = Vector2.SmoothDamp(currentLookDelta, targetMouseDelta, ref currentLookDeltaVelocity, LookSmoothTime);

        transform.Rotate(Vector3.up * currentLookDelta.x * MouseSensivity);

        cameraPitch -= currentLookDelta.y * MouseSensivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        cameraTransform.localEulerAngles = Vector3.right * cameraPitch;
    }

    void Move()
    {
        Vector2 targetDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        targetDirection.Normalize();

        currentDirection = Vector2.SmoothDamp(currentDirection, targetDirection, ref currentDirectionVelocity, moveSmoothTime);

        //gravity
        if (playerController.isGrounded)
        {
            velocityY = 0.0f;
        }

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * targetDirection.y + transform.right * targetDirection.x) * Speed + Vector3.up * velocityY;

        playerController.Move(velocity * Time.deltaTime);

        characterMoveSpeed = velocity.magnitude;

        WeaponMoveAnimation();
    }


    void Run()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Speed = 12f;

            weaponAK47GO.PlayRunAnim(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Speed = 6f;
            weaponAK47GO.PlayRunAnim(false);
        }

    }

    void SlowWalk()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Speed = 3f;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            Speed = 6f;
        }
    }

    void GeneralInputs()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            weaponAK47GO.AutoFire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponAK47GO.ReloadGun();
        }
    }

    void WeaponMoveAnimation()
    {
        if (characterMoveSpeed > 3.0f)
        {
            weaponAK47GO.PlayWalkAnim(true);
        }

        else
        {
            weaponAK47GO.PlayWalkAnim(false);
        }
    }

    public float TakeDamage(float damage, GameObject damagingObject)
    {
        if (playerHealth > 0)
            playerHealth -= damage;

        hudWidget.BeginFade();

        hudWidget.player = this.transform;

        hudWidget.otherTransform = damagingObject.transform;

        if (playerHealth < 0)
        {
            playerHealth = 0;
        }

        Killer();

        return playerHealth;
    }

    public float SetHealth(float Heal)
    {
        playerHealth += Heal;

        if (playerHealth > 100)
        {
            playerHealth = 100f;
        }

        if (Heal == 0)
        {
            playerHealth = 0;
        }

        return playerHealth;
    }

    public float GetHealth()
    {
        return playerHealth;
    }

    public float SetAmmo(float Value)
    {
        weaponAK47GO.SetAmmo(Value);

        return Value;
    }
}