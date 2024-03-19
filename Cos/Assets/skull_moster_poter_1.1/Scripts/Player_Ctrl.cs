using UnityEngine;
using System.Collections;

public enum PlayerState
{
    Idle,
    Walk,
    Run,
    Attack,
    Dead,
}

public class Player_Ctrl : MonoBehaviour {

    public PlayerState PS;

    public Vector3 lookDirection;
    public float Speed = 0f;
    public float WalkSpeed = 3f;
    public float RunSpeed = 6f;

    Animation animation;
    public AnimationClip Idle_Ani;
    public AnimationClip Walk_Ani;
    public AnimationClip Run_Ani;

    void KeyboardInput()
    {
        float xx = Input.GetAxisRaw("Vertical");
        float ZZ = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            lookDirection = xx * Vector3.forward + ZZ * Vector3.right;
            Speed = WalkSpeed;
            PS = PlayerState.Walk;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                Speed = RunSpeed;
                PS = PlayerState.Run;
            }
        }

        if (xx == 0 && ZZ == 0 && PS != PlayerState.Idle)
        {
            PS = PlayerState.Idle;
            Speed = 0f;
        }
    }
    void LookUpdate()
    {
        Quaternion R = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, R, 10f);

        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    void Update()
    {
        KeyboardInput();
        LookUpdate();
        AnimationUpdate();
    }

    void Start()
    {
        animation = this.GetComponent<Animation>();
    }

    void AnimationUpdate()
    {
        if (PS == PlayerState.Idle)
        {
            animation.CrossFade(Idle_Ani.name, 0.2f);
        }
        else if (PS == PlayerState.Walk)
        {
            animation.CrossFade(Walk_Ani.name, 0.2f);
        }
        else if (PS == PlayerState.Run)
        {
            animation.CrossFade(Run_Ani.name, 0.2f);
        }
        else if (PS == PlayerState.Attack)
        {
            animation.CrossFade(Idle_Ani.name, 0.2f);
        }
        else if (PS == PlayerState.Dead)
        {
            animation.CrossFade(Idle_Ani.name, 0.2f);
        }
    }
}
