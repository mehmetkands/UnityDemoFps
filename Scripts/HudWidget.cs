using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HudWidget : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentAmmoText;

    [SerializeField]
    private TextMeshProUGUI currentAmmunitionText;

    [SerializeField]
    private TextMeshProUGUI weaponNameText;

    [SerializeField]
    private TextMeshProUGUI killMessageText;

    [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField]
    private TextMeshProUGUI taskText;

    [SerializeField]
    public Character playerActor;

    [SerializeField]
    public bool useDirection;

    [SerializeField]
    public Image indicatorImage;

    [SerializeField]
    public RectTransform indicator;

    public Transform otherTransform;

    public Transform player;

    public WeaponAk47 weaponObject;

    float angle = 0;

    float lastTime;

    float timeRate = 0.4f;

    bool beginPlay = false;

    protected float value = 0f;


    // Start is called before the first frame update
    void Start()
    {
        indicatorImage.color = new Color(255f, 255f, 255f, value); //Set Alpha
    }

    // Update is called once per frame
    void Update()
    {
        weaponObject = playerActor.weaponAK47GO;

        currentAmmoText.text = weaponObject.GetAmmo().ToString();

        currentAmmunitionText.text = weaponObject.GetHave().ToString();

        healthText.text = playerActor.GetHealth().ToString();

        if (otherTransform && player)
        {
            angle = GetHitAngle(player, (player.position - otherTransform.position).normalized);

            indicator.rotation = Quaternion.Euler(0, 0, -angle);

            Timer();
        }

        else
        {
            value = 0f;
            Timer();
        }

    }

    public float GetHitAngle(Transform player, Vector3 incomingDir)
    {
        var otherDir = new Vector3(-incomingDir.x, 0f, -incomingDir.z);

        var playerFwd = Vector3.ProjectOnPlane(player.forward, Vector3.up);

        var angle = Vector3.SignedAngle(playerFwd, otherDir, Vector3.up);

        return angle;
    }

    //Start fade indicator image
    public void BeginFade()
    {
        beginPlay = true;

        value = 1f;

        indicatorImage.color = new Color(255f, 255f, 255f, value);
    }

    //Fade indicator image smoothly
    public void FadeIndicator()
    {
        if (value > 0)
            value -= 0.1f;

        indicatorImage.color = new Color(255f, 255f, 255f, value);
        if (value <= 0f)
        {
            beginPlay = false;
        }
    }

    void Timer()
    {
        if (Time.time > lastTime + timeRate && beginPlay)
        {
            lastTime = Time.time;
            FadeIndicator();
        }
    }

    public bool SetVisibility(bool active)
    {
        gameObject.SetActive(active);

        return active;
    }
}


