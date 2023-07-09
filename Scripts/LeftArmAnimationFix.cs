using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimationFix : MonoBehaviour
{
    private Animator animator;

    public Vector3 deltaRotation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (animator.GetBool("Defense") == false)
        {
            Transform leftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            leftLowerArm.localEulerAngles += deltaRotation;
            animator.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles));
        }
    }
}
