using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ScripPlayerController : MonoBehaviour
{
    private Rigidbody myRB;
    Camera playercam;

    Vector2 camRotation;

    public Transform WeaponSlot;

    [Header("Player Stats")]
    public int maxHealth = 5;
    public int health = 5;
    public int healthRestore = 1;

    [Header("Weapon Stats")]
    public int weaponID = 0;
    public int fireMode = 0;
    public float fireRate = 0;
    public float currentClip = 0;
    public float clipSize = 0;
    public float maxAmmo = 0;
    public float currentAmmo = 0;
    public float reloadAmt = 0;
    public bool canFire = true;


    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float jumpHeight = 5.0f;
    public float SprintMultplyer = 2.5f;
    public float groundDetectDistance = 1.0f;
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
        myRB = GetComponent<Rigidbody>();
        playercam = transform.GetChild(0).GetComponent<Camera>();

        camRotation = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);

        playercam.transform.localRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);
        transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);

        if (Input.GetMouseButtonDown(0) && canFire && currentClip > 0)
        {
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
        
        if(sprintToggleOption)
        {
            if (Input.GetKey(KeyCode.LeftShift) && verticalMove > 0)
                sprintMode = true;

            if (verticalMove <= 0)
                sprintMode = false;
        }

        if(!sprintMode)
            temp.x = verticalMove * speed;

        if (sprintMode)
            temp.x = verticalMove * speed * SprintMultplyer;

        temp.z = horizonralMove * speed;


        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, groundDetectDistance))
            temp.y = jumpHeight;

        myRB.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            other.gameObject.transform.position = WeaponSlot.position;

            other.gameObject.transform.SetParent(WeaponSlot);

            switch (other.gameObject.name)
            {
                case "Weapon 1":
                    weaponID = 0;
                    fireMode = 0;
                    fireRate = 0.25f;
                    currentClip = 20;
                    clipSize = 20;
                    maxAmmo = 400;
                    currentAmmo = 200;
                    reloadAmt = 20;
                    break;

                default:
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

       

        if ((currentAmmo < maxAmmo) && collision.gameObject.tag == "AmmoPickup")
        {
            currentAmmo += reloadAmt;

            if (currentAmmo > maxAmmo)
                currentAmmo = maxAmmo;

            Destroy(collision.gameObject);
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

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;

    }
}
