using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class paddingcontroll : MonoBehaviour
{
    Animator animator=null;
    Vector3 move;
    float horizontal;
    float vertical;
    Quaternion rotation = Quaternion.identity;
    Rigidbody rb;
    AnimatorStateInfo anim;
    float speed;
    public float turnspeed;
    private bool isrunning;
    float runspeed;
    bool isjumping;

    Vector3 v2;
    private bool isfight;
    //float attack = 1;
    float attacktime;
    float attacktimes = 2.0f;
    float force = 1.5f;
    float force1 = 2.1f;
    float time;
    bool ishit;
    bool attacks;
    float lenquetime = 3f;
    int mHitCount;
    private const string IdleState = "idleandwalk";
    private const string Attack1State = "Attack1";
    private const string Attack2State = "Attack2";
    private const string Attack3State = "Attack3";




    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isfight = false;

    }

    // Update is called once per frame
    void Update()
    {
       
        /*for (int i = 0; i < animator.layerCount; i++)
        {
            if (animator.GetLayerName(i) == "attack")
            {
                anim = animator.GetCurrentAnimatorStateInfo(i);
            }
        }*/
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            isrunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.Mouse1))
        {
            isrunning = false;

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {

            isjumping = true;
            animator.SetTrigger("jump");

            rb.velocity = new Vector3(horizontal, 4.0f, vertical);

        }
        else { isjumping = false; }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("fight");
            isfight = !isfight;
        }
        // animator.SetFloat("v_h", horizontal);
        /// animator.SetFloat("v_v", vertical);
        //if (!anim.IsName("Attack")) { attack = 1; }
        anim = animator.GetCurrentAnimatorStateInfo(0);
        if (anim.IsName("Attack1State") || anim.IsName("Attack2State") || anim.IsName("Attack3State") && anim.normalizedTime > 1.0f)
        {
            mHitCount = 0;
            animator.SetInteger("ActionID", mHitCount);
            attacks = false;

        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            attack();
        }


    }

    private void FixedUpdate()
    { 
        move.Set(horizontal, 0.0f, vertical);
        bool ishorizontal = !Mathf.Approximately(horizontal, 0.0f);
        bool isvertical = !Mathf.Approximately(vertical, 0.0f);  
        bool iswalking=ishorizontal || isvertical;
        //animator.SetBool("walk", iswalking);

        if (iswalking)
        {

            if (anim.IsName("Attack"))
            {
                speed = 0f;
                runspeed = 0f;
                Debug.Log("ÒÑ¾­ÕÒµ½");
            }
            else
            {
                if (isrunning)
                {
                    if (!anim.IsName("run ")) {
                        animator.Play("run ");
                    }
                    runspeed = 5;
                    if (isfight)
                    {
                        v2 = move * runspeed;
                        animator.SetFloat("v_h", v2.x);
                        animator.SetFloat("v_v", v2.z);
                    }
                    animator.SetFloat("speed", runspeed);
                    rb.MovePosition(rb.position + move * runspeed * Time.deltaTime);

                }
                else
                {
                    if (!anim.IsName("walkforword"))
                    {
                        animator.Play("walkforword ");
                    }
                    speed = 1.8f;
                    if (isfight)
                    {
                        v2 = move * speed;
                        animator.SetFloat("v_h", v2.x);
                        animator.SetFloat("v_v", v2.z);
                    }
                    animator.SetFloat("speed", speed);
                    rb.MovePosition(rb.position + move * speed * Time.deltaTime);

                }
            }

        }
        else {
            animator.Play("idle");
            speed = 0;
            animator.SetFloat("v_h", speed);
            animator.SetFloat("v_v", speed);
            animator.SetFloat("speed", speed);
        }
       
        
        Vector3 rr = Vector3.RotateTowards(transform.forward,move,turnspeed*Time.deltaTime,0.0f);
        rotation = Quaternion.LookRotation(rr);
        
        
    }
   public void OnAnimatorMove()
    {
        if (!isfight) { rb.MoveRotation(rotation); }
        if (ishit) { rb.MovePosition(rb.position + move * animator.deltaPosition.magnitude); }
    }
    void attack()
    {
        
        //¼ÙÉèÍæ¼Ò´¦ÓÚIdle×´Ì¬ÇÒ¹¥»÷´ÎÊýÎª0£¬ÔòÍæ¼ÒÒÀÕÕ¹¥»÷ÕÐÊ½1¹¥»÷£¬·ñÔòÒÀÕÕ¹¥»÷ÕÐÊ½2¹¥»÷£¬·ñÔòÒÀÕÕ¹¥»÷ÕÐÊ½3¹¥»÷
        if ( anim.IsName(IdleState) &&mHitCount == 0)
        {
            mHitCount = 1;
            animator.SetInteger("ActionID", mHitCount);
            
        }
        else if (anim.IsName(Attack1State) && mHitCount == 1 && anim.normalizedTime >0.7F)
        {
           
            mHitCount = 2;
            animator.SetInteger("ActionID", mHitCount);
        }
        else if (anim.IsName(Attack2State) && mHitCount == 2 && anim.normalizedTime <0.8F)
        {
             mHitCount = 3;
            animator.SetInteger("ActionID", mHitCount);
        }


    }
    void gotonextattackaction() {

        animator.SetInteger("ActionID", mHitCount);
    
    }
    
    
  

    
}
