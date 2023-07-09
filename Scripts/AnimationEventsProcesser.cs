using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsProcesser : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void ResetTrigger(string trigger)
    {
        animator.ResetTrigger(trigger);
    }
}
