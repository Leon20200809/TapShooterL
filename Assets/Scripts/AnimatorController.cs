using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    public enum  ActionType
    {
        attack,
        getHit,
        die,
    }

    /// <summary>
    /// アニメーション再生
    /// </summary>
    /// <param name="actionType"></param>
    public void PlayAnimaition(string actionType)
    {
        if (actionType == ActionType.die.ToString())
        {
            animator.ResetTrigger(ActionType.attack.ToString());
            animator.ResetTrigger(ActionType.getHit.ToString());
            animator.SetBool(ActionType.die.ToString(), true);
        }
        else
        {
            animator.ResetTrigger(ActionType.attack.ToString());
            animator.SetTrigger(actionType);
        }
    }

}
