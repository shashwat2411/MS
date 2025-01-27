using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static EnemyBase;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.UI.GridLayoutGroup;

public class BossEnemy : EnemyBase
{
    public enum BOSSENEMY_STATE
    {
        IDLE = 0,
        SCREAM,
        SLAP,
        SMASH,
        SMASHFAR,
        SUMMONLIGHTNING,
        LASERBEAM,

        MAX
    }

    [Header("References")]
    public HealthBar bossHealthBar;
    public Animator canvasAnimator;
    public Animator mainCameraAnimator;
    public GameObject eyeballBone;
    public bool laserBeam;
    private bool lookAt = true;
    [Range(0, 1)] public float lookingSpeed = 0.2f;

    [Header("Attack")]
    public float slapAttackPower;
    public float smashAttackPower;
    public float lightningAttackPower;
    public float laserBeamAttackPower;
    public bool disableColliders;
    public float speed = 1f;
    public float maxSpeed = 1.5f;

    [Header("State Time")]
    public float screamTime;
    public float slapTime;
    public float smashTime;
    public float lightningTime;
    public float laserBeamTime;
    public float phaseChangeThreshold = 0.25f;
    public BOSSENEMY_STATE currentState;
    public BOSSENEMY_STATE previousState;
    public BOSSENEMY_STATE nextState;

    private float nextPhaseTime;
    private float countDown = 0f;

    private bool currentPhaseOver;
    private bool countDownStart = false;


    [Header("Material")]
    public EnemyMaterial eyeball;
    public EnemyMaterial gums;
    public EnemyMaterial teeth;
    public EnemyMaterial hands;
    public EnemyMaterial tongue;
    public Animator visualAimer;

    //Hash Map
    protected static int _Scream = Animator.StringToHash("_Scream");
    protected static int _Slap = Animator.StringToHash("_Slap");
    protected static int _Smash = Animator.StringToHash("_Smash");
    protected static int _SmashFar = Animator.StringToHash("_SmashFar");
    protected static int _SummonLightning = Animator.StringToHash("_SummonLightning");
    protected static int _Death = Animator.StringToHash("_Death");
    protected static int _LaserBeam = Animator.StringToHash("_LaserBeam");
    protected static int _Speed = Animator.StringToHash("_Speed");

    protected override void Start()
    {
        player = FindFirstObjectByType<PlayerManager>().gameObject;

        healthBar = bossHealthBar;

        disableColliders = false;
        countDownStart = false;
        currentPhaseOver = false;
        laserBeam = false;
        lookAt = true;

        countDown = 0f;

        ReturnToIdle();
        StartCoroutine(ChangeState(BOSSENEMY_STATE.SCREAM));
        StartCoroutine(BossEnterCutScene(3f, 0.33f));

        healthBar.Stimulate();
        healthBar.disappear = false;

        eyeball.InstantiateMaterial();
        gums.InstantiateMaterial();
        teeth.InstantiateMaterial();
        hands.InstantiateMaterial();
        tongue.InstantiateMaterial();
    }

    protected override void FixedUpdate()
    {
        if (lookAt == true) 
        {
            Quaternion rotation = eyeballBone.transform.rotation;
            Quaternion targetRotation = LookAt(eyeballBone.transform, player.transform.position, eyeballBone.transform.up);
            eyeballBone.transform.rotation = Quaternion.Lerp(rotation, targetRotation, lookingSpeed);
        }

        float percentage = 1f - (bossHealthBar.health / (bossHealthBar.maxHealth * 0.7f));
        speed = Mathf.Lerp(1f, maxSpeed, percentage);

        animator.SetFloat(_Speed, speed);
        visualAimer.SetFloat(_Speed, speed);

        if (dead == false)
        {
            if (countDownStart == true && currentPhaseOver == false)
            {
                if (countDown < nextPhaseTime) { countDown += Time.deltaTime * speed; }
                else if (IsCurrentAnimationOver())
                {
                    countDown = 0f;
                    countDownStart = false;
                    currentPhaseOver = true;
                }
            }

            if (currentPhaseOver == true)
            {
                StartCoroutine(ChangeState(nextState));
            }

            switch (currentState)
            {
                case BOSSENEMY_STATE.IDLE:
                    Idle();
                    break;

                case BOSSENEMY_STATE.SCREAM:
                    Scream();
                    break;

                case BOSSENEMY_STATE.SLAP:
                    Slap();
                    break;

                case BOSSENEMY_STATE.SMASH:
                    Smash();
                    break;

                case BOSSENEMY_STATE.SMASHFAR:
                    SmashFar();
                    break;

                case BOSSENEMY_STATE.SUMMONLIGHTNING:
                    SummonLightning();
                    break;

                case BOSSENEMY_STATE.LASERBEAM:
                    LaserBeam();
                    break;
            }
        }
    }

    public override void Death()
    {
        dead = true;


        speed = 1f;

        animator.SetBool(_Scream, true);
        animator.SetBool(_Slap, false);
        animator.SetBool(_Smash, false);
        animator.SetBool(_SmashFar, false);
        animator.SetBool(_SummonLightning, false);
        animator.SetBool(_LaserBeam, false);
        animator.SetBool(_Death, true);

        animator.Play("Death2");
        animator.speed = 0;
        visualAimer.speed = 0f;
        //animator.StopPlayback();

        //StartCoroutine(DissolveOut(4f, 2f));

        //animator.speed = 0f;

        SoundManager.Instance.StopAllSE();
        StartCoroutine(BossDeathCutScene(2f));
        //Destroy(gameObject, 2.2f);
    }

    private IEnumerator BossEnterCutScene(float delay, float delay2)
    {
        SoundManager.Instance.StopAllSE();
        mainCameraAnimator.GetComponent<CameraBrain>().target = gameObject.transform;
        player.GetComponent<PlayerInput>().enabled = false;

        yield return new WaitForSeconds(delay);

        canvasAnimator.Play("BlackBarsOut");

        yield return new WaitForSeconds(delay2);

        
        //mainCameraAnimator.GetComponent<CameraBrain>().target = player.transform;
        StartCoroutine(mainCameraAnimator.GetComponent<CameraBrain>().ShiftTarget(player.transform, 1f));
        player.GetComponent<PlayerInput>().enabled = true;
    }

    private IEnumerator BossDeathCutScene(float delay)
    {
        canvasAnimator.SetBool("in", true);
        canvasAnimator.SetBool("out", false);

        mainCameraAnimator.GetComponent<CameraBrain>().target = gameObject.transform;

        //mainCamera.Play("ZoomOut2");

        yield return new WaitForSeconds(delay);

        animator.speed = 1f;
        SoundManager.Instance.PlaySE("BossDeath");
        FindFirstObjectByType<BossAnimationEvents>().StartDeathSound();

    }

    private IEnumerator DissolveOut(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(eyeball.DissolveOut(duration));
        StartCoroutine(gums.DissolveOut(duration));
        StartCoroutine(teeth.DissolveOut(duration));
        StartCoroutine(hands.DissolveOut(duration));
        StartCoroutine(tongue.DissolveOut(duration));
    }

    //----------------------------------------------------

    public void ReturnToIdle()
    {
        if (laserBeam == false)
        {
            previousState = currentState;
            currentState = BOSSENEMY_STATE.IDLE;

            currentPhaseOver = false;
            CalculateNextAttack();
            countDownStart = true;
            disableColliders = false;

            animator.SetBool(_Scream, false);
            animator.SetBool(_Slap, false);
            animator.SetBool(_Smash, false);
            animator.SetBool(_SmashFar, false);
            animator.SetBool(_SummonLightning, false);
            animator.SetBool(_LaserBeam, false);
        }
        else
        {
            previousState = currentState;
            currentState = BOSSENEMY_STATE.LASERBEAM;
            animator.SetBool(_LaserBeam, true);

            laserBeam = false;
            lookAt = true;

            nextPhaseTime = laserBeamTime;
        }
    }
    private bool IsCurrentAnimationOver()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) { return true; }

        return false;
    }

    private void CalculateNextAttack()
    {
        int current = (int)currentState;
        int previous = (int)previousState;
        int rand = current;

        int laserBeam = (int)BOSSENEMY_STATE.LASERBEAM;

        bool phaseTwo = bossHealthBar.health / bossHealthBar.maxHealth < 0.7f;

        if (phaseTwo == true)
        {
            while ((rand == current || rand == previous) || rand == laserBeam)
            {
                rand = Random.Range(1, (int)BOSSENEMY_STATE.MAX);
            }
        }
        else
        {
            int lightningSummon = (int)BOSSENEMY_STATE.SUMMONLIGHTNING;

            while ((rand == current || rand == previous) || rand == laserBeam)
            {
                rand = Random.Range(1, (int)BOSSENEMY_STATE.MAX);
                if (rand == lightningSummon) { rand = current; }
            }
        }

        nextState = (BOSSENEMY_STATE)rand;

        if (nextState == BOSSENEMY_STATE.SCREAM) { nextPhaseTime = screamTime; }
        else if (nextState == BOSSENEMY_STATE.SLAP) { nextPhaseTime = slapTime; }
        else if (nextState == BOSSENEMY_STATE.SMASH) { nextPhaseTime = smashTime; }
        else if (nextState == BOSSENEMY_STATE.SMASHFAR) { nextPhaseTime = smashTime; }
        else if (nextState == BOSSENEMY_STATE.SUMMONLIGHTNING) { nextPhaseTime = lightningTime; }
        else { nextPhaseTime = 0f; }
    }

    //____ステート________________________________________________________________________________________________________________________
    void Idle() { }
    void Scream() { animator.SetBool(_Scream, true); }
    void Slap() { animator.SetBool(_Slap, true); }
    void Smash() { animator.SetBool(_Smash, true); }
    void SmashFar() { animator.SetBool(_SmashFar, true); }
    void SummonLightning() { animator.SetBool(_SummonLightning, true); }
    void LaserBeam() {
        animator.SetBool(_LaserBeam, true);
    }
    //____________________________________________________________________________________________________________________________

    //___Coroutines_________________________________________________________________________________________________________________________
    IEnumerator ChangeState(BOSSENEMY_STATE value)
    {
        yield return new WaitForSeconds(0f);
        previousState = currentState;
        currentState = value;

        currentPhaseOver = false;
        //CalculateNextAttack();
        countDownStart = true;
    }
    //____________________________________________________________________________________________________________________________

    protected override void OnCollisionEnter(Collision collision)
    {
        PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();
        if(player)
        {
            if (disableColliders == false)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slap"))
                {
                    player.playerHP.Damage(slapAttackPower);
                    disableColliders = true;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Smash") || animator.GetCurrentAnimatorStateInfo(0).IsName("SmashFar"))
                {
                    player.playerHP.Damage(smashAttackPower);
                    disableColliders = true;
                }
            }
        }
    }

    public void BossOnCollision(GameObject collision)
    {
        PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();
        if (player)
        {
            if (disableColliders == false)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slap"))
                {
                    player.playerHP.Damage(slapAttackPower);
                    disableColliders = true;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Smash") || animator.GetCurrentAnimatorStateInfo(0).IsName("SmashFar"))
                {
                    player.playerHP.Damage(smashAttackPower);
                    disableColliders = true;
                }
            }
        }
    }

    public void SetNextPhaseToLaserBeam()
    {
        lookAt = false;
        laserBeam = true;
    }

    Quaternion LookAt(Transform owner, Vector3 targetPosition, Vector3 up)
    {
        // Calculate the forward direction
        Vector3 forward = (targetPosition - owner.transform.position).normalized;

        // Ensure the up direction is valid
        Vector3 right = Vector3.Cross(up, forward).normalized;
        Vector3 trueUp = Vector3.Cross(forward, right).normalized;

        // Create a rotation matrix
        Matrix4x4 rotationMatrix = new Matrix4x4();
        rotationMatrix.SetColumn(0, right);    // X-axis
        rotationMatrix.SetColumn(1, trueUp);  // Y-axis
        rotationMatrix.SetColumn(2, forward); // Z-axis
        rotationMatrix.SetColumn(3, new Vector4(0, 0, 0, 1));

        // Apply the rotation to the transform
        return rotationMatrix.rotation;
    }
}
