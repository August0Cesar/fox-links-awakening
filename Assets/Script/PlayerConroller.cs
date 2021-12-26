using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConroller : MonoBehaviour
{
    private CharacterController characterController;
    private Animator myAnimator;
    private bool isWalk;

    [Header("Config Player")]
    public float Velocidade = 3f;

    private float timeChangeDirection = 15;
    private float timeCurrent = 0;
    private Vector3  direction;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        ApplyMovement();
        Attack();
    }

    void Attack(){
        if(Input.GetButtonDown("Fire1")){
            myAnimator.SetTrigger("TriggerAttack");
        }
    }

    void ApplyMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3  direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude > 0.1f){
            isWalk = true;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }else{
            isWalk = false;
        }

        myAnimator.SetBool("isWalk", isWalk);
        characterController.Move(direction * Velocidade * Time.deltaTime);
    }
}
