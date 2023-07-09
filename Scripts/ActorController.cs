using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public KeyboardInput playerInput;
    public CameraController cameraController;

    [SerializeField]
    private Animator animator;
    private Rigidbody rigid;
    // 平面移动向量
    private Vector3 planarVec;
    private bool lockPlanar = false;
    private bool isOnGround = false;
    // 向上移动向量
    private Vector3 thrustVec;
    // 攻击时停止移动
    private bool clearPlanar = false;

    [SerializeField]
    private float walkSpeed = 1.5f;
    [SerializeField]
    private float runMultiplier = 2.5f;
    [SerializeField]
    private float jumpSpeed = 5.0f;

    [Space(10)]
    [Header("==== Friction Material ====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    private CapsuleCollider capsuleCollider;

    private void Awake()
    {
        playerInput = GetComponent<KeyboardInput>();
        animator = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // OnGround
        float targetRunMulti = playerInput.run ? 2.0f : 1.0f;
        animator.SetFloat("Forward",  playerInput.Dmag * Mathf.Lerp(animator.GetFloat("Forward"), targetRunMulti, 0.1f));
        // Jump
        if (playerInput.jump)
        {
            animator.SetTrigger("Jump");
        }
        // Roll
        if (rigid.velocity.magnitude > 5.0f)
        {
            animator.SetTrigger("Roll");
        }
        // Lockon
        if (playerInput.lockonPressed)
        {
            cameraController.SwitchLockOnMode();
        }

        // Attack
        if (playerInput.attack && isOnGround)
        {
            animator.SetTrigger("Attack");
            animator.SetBool("IsMirrored", playerInput.mirrored);
        }
        // Defense
        animator.SetBool("Defense", playerInput.defense);


        // 静止时forward不能为空向量
        if (playerInput.Dmag > 0.1f)
        {
            Vector3 targetForward = Vector3.Slerp(model.transform.forward, playerInput.Dvec, 0.1f);
            model.transform.forward = targetForward;
        }
        // 离开地面后，保持当前水平速度，lockPlanar == true
        // planarVec方向永远和forward方向保持一致
        if (lockPlanar == false)
        {
            planarVec = playerInput.Dmag * model.transform.forward;
        }
        // 攻击时停止移动
        if (clearPlanar)
        {
            planarVec = new Vector3(0f, 0f, 0f);
        }
    }

    private void FixedUpdate()
    {
        // 平面位移
        rigid.position += planarVec * Time.fixedDeltaTime * walkSpeed * (playerInput.run ? runMultiplier : 1.0f);

        // 向上位移
        rigid.velocity += thrustVec;
        thrustVec = Vector3.zero; // 速度单次触发，需要触发后清零
        //rigid.velocity = movingVec;
    }

    // Helper Function
    private bool CheckState(string stateName, string layerName = "Base Layer")
    {
        return animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex(layerName)).IsName(stateName);
    }

    private void LerpLayerWeight(string layerName, float targetWeight, float t = 0.2f)
    {
        int layerIndex = animator.GetLayerIndex(layerName);
        float currentWeight = animator.GetLayerWeight(layerIndex);
        currentWeight = Mathf.Lerp(currentWeight, targetWeight, t);
        animator.SetLayerWeight(layerIndex, currentWeight);
    }


    // Message Processing Blocks
    public void OnJumpEnter()
    {
        print("On Jump Enter");
        // 提供向上速度
        thrustVec = jumpSpeed * Vector3.up;    
    }

    public void OnJumpExit()
    {
        print("On Jump Exit");
    }

    public void OnGroundEnter()
    {
        isOnGround = true;
        animator.SetBool("IsOnGround", true);
        playerInput.inputEnabled = true;
        lockPlanar = false;

        capsuleCollider.material = frictionOne;
    }

    public void OnGroundExit()
    {
        isOnGround = false;
        animator.SetBool("IsOnGround", false);
        // 一旦离开地面，就不允许玩家控制角色
        playerInput.inputEnabled = false;
        lockPlanar = true;

        capsuleCollider.material = frictionZero;
    }

    public void OnAttack1HandAEnter()
    {
        clearPlanar = true;
    }

    public void OnAttack1HandAUpdate()
    {
        //LerpLayerWeight("Attack", 1.0f);
    }

    public void OnAttackIdleEnter()
    {
        clearPlanar = false;
        //animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 0f);
    }

    public void OnAttackIdleUpdate()
    {
        //LerpLayerWeight("Attack", 0.0f);
    }
}
