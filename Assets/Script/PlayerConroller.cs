using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConroller : MonoBehaviour
{
    private CharacterController characterController;
    private Animator myAnimator;
    private bool isWalk;
    private Vector3  direction;

    [Header("Config Player")]
    public float Velocidade = 3f;

    [Header("Config Attack")]
    public ParticleSystem fxAttack;
    public Transform HitBox;
    [Range(0.2f, 1)]
    public float HitRange = 0.5f;
    public LayerMask HitLayer;

    [SerializeField]
    private bool isAttack;


    public void Start()
    {
        characterController = GetComponent<CharacterController>();
        myAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        ApplyMovement();
        Attack();
    }

    public void isDoneAttack(){
        isAttack = false;
    }

    private void Attack(){
        if(Input.GetButtonDown("Fire1") && !isAttack){
            isAttack = true;
            myAnimator.SetTrigger("TriggerAttack");
            fxAttack.Emit(1);

            Collider[] colliders = Physics.OverlapSphere(HitBox.position, HitRange, HitLayer);
            foreach(Collider c in colliders){
                Debug.Log(c.gameObject.name);
                c.gameObject.SendMessage("GetHit", 1);
            }
        }
    }

    private void ApplyMovement()
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

    public void OnDrawGizmosSelected(){
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(HitBox.position, HitRange);
    }
}
