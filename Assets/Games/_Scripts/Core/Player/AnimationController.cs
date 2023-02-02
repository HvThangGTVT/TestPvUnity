using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private readonly int _moveAxisXHash = Animator.StringToHash("Horizontal");
    private readonly int _moveAxisYHash = Animator.StringToHash("Vertical");
    private readonly int _isMoveHash = Animator.StringToHash("IsMove");

    [SerializeField] private Animator animator;

    public void StartMoveAnimation(Vector2 direction)
    {
        if (direction.x != 0.0f || direction.y != 0.0f)
        {
            animator.SetBool(_isMoveHash, true);
        }
        else
        {
            animator.SetBool(_isMoveHash, false);
        }

        animator.SetFloat(_moveAxisXHash, direction.x);
        animator.SetFloat(_moveAxisYHash, direction.y);
    }


    private void ResetAllState()
    {
        animator.SetBool(_isMoveHash, false);
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }
}