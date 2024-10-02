using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator anim;

    public enum AnimationState {IDLE, RUN, DASH, JUMP, FALL};
    AnimationState currentAnim = AnimationState.IDLE;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeAnimation(AnimationState incomingAnim)
    {
        //Skip running the code if the animation hasn't changed
        if (incomingAnim == currentAnim)
        {
            return;
        }

        //Directly play the corresponding animation
        switch (incomingAnim)
        {
            case AnimationState.RUN:
                anim.Play("Player_Run");
                break;
            case AnimationState.DASH:
                anim.Play("Player_Dash");
                break;
            case AnimationState.JUMP:
                anim.Play("Player_Jump");
                break;
            case AnimationState.FALL:
                anim.Play("Player_Fall");
                break;
            default:
                anim.Play("Player_Idle");
                break;
        }

        currentAnim = incomingAnim;

    }
}
