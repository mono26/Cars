using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float health;
    private float specialMeter;

    [SerializeField]
    private bool drivingState;
    [SerializeField]
    private bool shootingState;

    [SerializeField]
    private Animator pAnimator;

    private MachineGun machineGun;

    public GameObject targetImg;

    // Use this for initialization
    void Start ()
    {
        targetImg.SetActive(false);
        pAnimator = GetComponent<Animator>();
        machineGun = GetComponentInChildren<MachineGun>();
        health = 100;
        specialMeter = 10;
        drivingState = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
       /* if (Input.GetKey(KeyCode.LeftShift))
        {
            targetImg.SetActive(true);
            pAnimator.SetBool("isShooting", true);
            pAnimator.SetBool("isDriving", false);
            shootingState = true;
            drivingState = false;
        } else if (Input.GetKeyUp(KeyCode .LeftShift))
        {
            targetImg.SetActive(false);
            pAnimator.SetBool("isShooting", false);
            pAnimator.SetBool("isDriving", true);
            shootingState = false;
            drivingState = true;
        }*/
       if (Input.GetAxis("Aim") != 0)
        {
            targetImg.SetActive(true);
            pAnimator.SetBool("isShooting", true);
            pAnimator.SetBool("isDriving", false);
            shootingState = true;
            drivingState = false;
        }
        else if (Input.GetAxis("Aim") == 0)
        {
           targetImg.SetActive(false);
            pAnimator.SetBool("isShooting", false);
            pAnimator.SetBool("isDriving", true);
            shootingState = false;
            drivingState = true;
        }


    }

    public void ReceiveAmmo(int ammo)
    {
        //machineGun.ReceiveAmmo(ammo);
    }

    public void ReceiveDamage(float damage)
    {
        health = -damage;
    }
}
