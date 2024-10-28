using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ScripPlayerController : MonoBehaviour
{
    private Rigidbody myRB;
    Camera playercam;

    Transform cameraHolder;
    
    Vector2 camRotation;

    public Transform WeaponSlot;

    public GameManager gm;

    public AudioSource pickupSpeaker;

    public int damageRecieved = 1;

    [Header("Player Stats")]
    public int maxHealth = 25;
    public int health = 25;
    public int healthRestore = 5;
    public int healthAdd = 10;
    public bool inAir = true;

    [Header("Weapon Stats")]
    public AudioSource weaponSpeaker;
    public GameObject shot;
    public float shotSpeed = 0;
    public int weaponID = 0;
    public int fireMode = 0;
    public float fireRate = 0;
    public float currentClip = 0;
    public float clipSize = 0;
    public float maxAmmo = 0;
    public float currentAmmo = 0;
    public float reloadAmt = 0;
    public float shotLifeSpan = 0;
    public bool canFire = true;
   

    [Header("Ammo Counts")]
    public float CurrentAmmo = 0;
    public float CurrentAmmo1 = 0;
    public float CurrentAmmo2 = 0;
    public float CurrentChip = 0;
    public float CurrentChlip1 = 0;
    public float CurrentClip2 = 0;

    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float jumpHeight = 5.0f;
    public float SprintMultplyer = 2.5f;
    public float groundDetectDistance = 4.25f;
    public bool sprintMode = false;

    [Header("User Settings")]
    public bool sprintToggleOption = false;
    public float mouseSensitivity = 2.0f;
    public float Xsensitivity = 2.0f;
    public float Ysensitivity = 2.0f;
    public float camRotationLimit = 90f;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        myRB = GetComponent<Rigidbody>();
        playercam = Camera.main;
        cameraHolder = transform.GetChild(0);

        camRotation = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.isPaused)
        {
            playercam.transform.position = cameraHolder.position;

            camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

            camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);

            playercam.transform.rotation = Quaternion.Euler(-camRotation.y, camRotation.x, 0);
            transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);

            if (Input.GetMouseButtonDown(0) && canFire && currentClip > 0 && weaponID >= 0)
            {
                weaponSpeaker.Play();
                GameObject s = Instantiate(shot, WeaponSlot.position, WeaponSlot.rotation);
                s.GetComponent<Rigidbody>().AddForce(playercam.transform.forward * shotSpeed);
                Destroy(s, shotLifeSpan);

                canFire = false;
                currentClip--;
                StartCoroutine("cooldownFire");
            }

            if (Input.GetKeyDown(KeyCode.R))
                reloadClip();

            Vector3 temp = myRB.velocity;

            float verticalMove = Input.GetAxisRaw("Vertical");
            float horizonralMove = Input.GetAxisRaw("Horizontal");

            if (!sprintToggleOption)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    sprintMode = true;

                if (Input.GetKeyUp(KeyCode.LeftShift))
                    sprintMode = false;
            }

            if (sprintToggleOption)
            {
                if (Input.GetKey(KeyCode.LeftShift) && verticalMove > 0)
                    sprintMode = true;

                if (verticalMove <= 0)
                    sprintMode = false;
            }

            if (!sprintMode)
                temp.x = verticalMove * speed;

            if (sprintMode)
                temp.x = verticalMove * speed * SprintMultplyer;

            temp.z = horizonralMove * speed;


            if (Input.GetKeyDown(KeyCode.Space) && !inAir) 
            {
                temp.y = jumpHeight;
            }
            
            if (Physics.Raycast(transform.position, -transform.up, groundDetectDistance))
            {
                inAir = false;
            }
            else
            {
                inAir = true;
            }

            myRB.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);

            if (health <= 0)
                gm.Loadlevel(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            other.gameObject.transform.position = WeaponSlot.position;

            other.gameObject.transform.SetParent(WeaponSlot);

            weaponSpeaker = other.gameObject.GetComponent<AudioSource>();

            switch (other.gameObject.name)
            {
                case "Weapon 1":

                   

                    weaponID = 0;
                    shotSpeed = 15000;
                    fireMode = 0;
                    fireRate = 0.25f;
                    currentClip = 20;
                    clipSize = 20;
                    maxAmmo = 200;
                    currentAmmo = 200;
                    reloadAmt = 20;
                    shotLifeSpan = 1;
                    break;

                default:

                case "weapon 2":
                   
                    
                    weaponID = 1;
                    shotSpeed = 15000;
                    fireMode = 0;
                    fireRate = 0.5f;
                    currentClip = 10;
                    clipSize = 10;
                    maxAmmo = 150;
                    currentAmmo = 150;
                    reloadAmt = 10;
                    shotLifeSpan = 1;
                    break;

            }
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((health < maxHealth) && collision.gameObject.tag == "Health Pickup")
        {
            health += healthRestore;
           
            if (health > maxHealth)
                health = maxHealth;

            Destroy(collision.gameObject);
        }

       if ((health <= maxHealth) && collision.gameObject.tag == "Health buff")
        {
            maxHealth += healthAdd;
            health += healthAdd;

            Destroy(collision.gameObject);
        }

        if ((currentAmmo < maxAmmo) && collision.gameObject.tag == "Ammo Pickup")
        {
            currentAmmo += reloadAmt;

            if (currentAmmo < maxAmmo)
                currentAmmo = maxAmmo;

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "basicEnamy")
        {
            health -= 1;
        }

        if (collision.gameObject.tag == "Boss")
        {
            health -= 5;
        }

        if (collision.gameObject.tag == "door")
        {
            Destroy(collision.gameObject);

        }

        if (collision.gameObject.tag == "Next level")
        {
            gm.Loadlevel(2);
        }
    }

    public void reloadClip()
    {
        if (currentClip >= clipSize)
            return;

        else
        {
            float reloadCount = clipSize - currentClip;

            if (currentAmmo < reloadCount)
            {
                currentClip += currentAmmo;

                currentAmmo = 0;
                return;
            }

            else
            {
                currentClip += reloadCount;

                currentAmmo -= reloadCount;
                return;
            }
        }
    }

    private void onCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "basicEnamy")
        {
            health -= damageRecieved;
            Destroy(collision.gameObject);
        }
    }

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;

    }
}
