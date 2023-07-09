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
    // ƽ���ƶ�����
    private Vector3 planarVec;
    private bool lockPlanar = false;
    private bool isOnGround = false;
    // �����ƶ�����
    private Vector3 thrustVec;
    // ����ʱֹͣ�ƶ�
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


        // ��ֹʱforward����Ϊ������
        if (playerInput.Dmag > 0.1f)
        {
            Vector3 targetForward = Vector3.Slerp(model.transform.forward, playerInput.Dvec, 0.1f);
            model.transform.forward = targetForward;
        }
        // �뿪����󣬱��ֵ�ǰˮƽ�ٶȣ�lockPlanar == true
        // planarVec������Զ��forward���򱣳�һ��
        if (lockPlanar == false)
        {
            planarVec = playerInput.Dmag * model.transform.forward;
        }
        // ����ʱֹͣ�ƶ�
        if (clearPlanar)
        {
            planarVec = new Vector3(0f, 0f, 0f);
        }
    }

    private void FixedUpdate()
    {
        // ƽ��λ��
        rigid.position += planarVec * Time.fixedDeltaTime * walkSpeed * (playerInput.run ? runMultiplier : 1.0f);

        // ����λ��
        rigid.velocity += thrustVec;
        thrustVec = Vector3.zero; // �ٶȵ��δ�������Ҫ����������
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
        // �ṩ�����ٶ�
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
        // һ���뿪���棬�Ͳ�������ҿ��ƽ�ɫ
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
