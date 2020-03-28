using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour
{
    private IMovementState mState;
    public IMovementState stIdle;
    public IMovementState stRun;
    public IMovementState stClimb;
    public IMovementState stFall;
    public IMovementState stJump;
    public IMovementState stWallJump;
    public IMovementState stClimbJump;
    public IMovementState stWallSlide;
    public IMovementState stDash;
    public IMovementState stBoostJump;
    public IMovementState stBoostWallJump;

    public Vector2 footCenterOffset;
    public Vector2 onGroundBoxSize;
    public Vector2 grabCenterOffset;
    public Vector2 onWallBoxSize;

    public float MaxRun = 12f;
    public float AirShrink = 0.6f;
    public float RunReduce = 20f;
    public float RunAccel = 50f;
    public float LiftXCap = 60f;
    public float LiftYCap = 40f;
    public float ClimbSpeed = 4f;
    public float MaxFall = -30f;
    public float JumpTime = 0.2f;
    public float JumpSpeed = 30f;
    public float ClimbJumpTime = 0.1f;
    public float ClimbJumpSpeed = 14f;
    public float WallJumpTime = 0.2f;
    public float WallJumpSpeed = 20f;
    [Range(0f, 1.5f)]
    public float WallJumpDir = 1;
    public float WallJumpBonusDst = 0.2f;
    public float WallSlideSpeed = 1f;
    public float OnWallTime = 2f;
    public float DashTime = 0.15f; // You can't dash twice in one DashTime period
    public float DashLockTime = 0.08f; // In DashLockTime, state will not change, out of DashLockTime, player can jump
    public float DashStartSpeed = 64f;
    public float DashEndSpeed = 48f;
    public float BoostLifeTime = 0.3f;

    public TextMeshProUGUI stateText;

    public LayerMask groundLayer;

    private Animator anim;

    private float airRunReduce;
    private float airRunAccel;
    private Vector2 footCenter;
    private Vector2 grabCenter;
    private Rigidbody2D rb;
    private Collider2D hitWall;
    private Collider2D hitGround;
    private Collider2D hitWallJump;
    private bool onWall;
    private bool onGround;
    private bool wasOnGround;
    private Vector2 playerSize;
    private Vector2 liftVelocity;
    private Vector2 wallVelocity;

    private Booster liftBooster;
    private Booster wallBooster;

    public Controller() {
        stIdle = new IdleState(this);
        stRun = new RunState(this);
        stClimb = new ClimbState(this);
        stFall = new FallState(this);
        stJump = new JumpState(this);
        stWallJump = new WallJumpState(this);
        stClimbJump = new ClimbJumpState(this);
        stWallSlide = new WallSlideState(this);
        stDash = new DashState(this);
        stBoostJump = new BoostJumpState(this);
        stBoostWallJump = new BoostWallJumpState(this);
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        airRunReduce = RunReduce * AirShrink;
        airRunAccel = RunAccel * AirShrink;

        liftBooster = new Booster(BoostLifeTime, Time.fixedDeltaTime);
        StartCoroutine(liftBooster.Run());
        wallBooster = new Booster(BoostLifeTime, Time.fixedDeltaTime);
        StartCoroutine(wallBooster.Run());

        SetState(stFall);
    }

    private void FixedUpdate() {
        UpdateOnGround();
        UpdateOnWall();
        mState.Update();
    }

    public void SetState(IMovementState state) {
        if (mState == state) {
            return;
        }

        Debug.LogFormat("{0} -> {1}", mState, state);

        stateText.text = state.ToString();

        if (mState != null) mState.Exit();
        mState = state;
        mState.Enter();

        anim.SetInteger("StateID", mState.ID);
    }

    public Vector2 Speed {
        get {
            return rb.velocity;
        }
        set {
            rb.velocity = value;
        }
    }

    public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force) {
        rb.AddForce(force, mode);
    }

    public void RelativeMove(Vector2 pos) {
        rb.MovePosition((Vector2)transform.position + pos);
    }

    public bool Grab { get; set; }
    public bool Jump { get; set; }
    public Vector2 Movement { get; set; }
    public bool Dash { get; set; }


    public Vector2 LiftBoost {
        get {
            return new Vector2(
                Mathf.Round(Mathf.Clamp(liftBooster.X, -LiftXCap, LiftXCap)),
                Mathf.Clamp(liftBooster.Y, 0, LiftYCap));
        }
    }

    public Vector2 LiftVelocity {
        get {
            return liftVelocity;
        }
    }

    public Vector2 WallBoost {
        get {
            return new Vector2(
                Mathf.Round(Mathf.Clamp(wallBooster.X, -LiftXCap, LiftXCap)),
                Mathf.Clamp(wallBooster.Y, 0, LiftYCap));
        }
    }

    public Vector2 WallVelocity {
        get {
            return wallVelocity;
        }
    }

    public float OnWallTimer { get; set; }

    public bool CanOnWall() {
        return OnWallTimer > 0;
    }

    private void UpdateOnGround() {
        footCenter = new Vector2(transform.position.x + footCenterOffset.x, transform.position.y + footCenterOffset.y);
        hitGround = Physics2D.OverlapBox(footCenter, onGroundBoxSize, 0, groundLayer);

        wasOnGround = onGround;
        onGround = hitGround != null; 

        if (onGround && hitGround.attachedRigidbody != null) {
            liftVelocity = hitGround.attachedRigidbody.velocity;
        } else {
            liftVelocity = Vector2.zero;
        }
        liftBooster.Update(liftVelocity);
    }

    private void UpdateOnWall() {
        grabCenter = new Vector2(transform.position.x + grabCenterOffset.x, transform.position.y + grabCenterOffset.y);
        hitWall = Physics2D.OverlapBox(grabCenter, onWallBoxSize, 0, groundLayer);

        onWall = hitWall != null;
        if (onWall && hitWall.attachedRigidbody != null) {
            wallVelocity = hitWall.attachedRigidbody.velocity;
        } else {
            wallVelocity = Vector2.zero;
        }
        wallBooster.Update(wallVelocity);
    }

    public bool OnWall {
        get {
            return onWall;
        }
    }

    public bool OnGround {
        get {
            return onGround;
        }
    }

    public bool IsDashing { get; set; }
    public bool Dashable { get; set; }

    public void Flip() {
        Vector3 ls = transform.localScale;
        transform.localScale = new Vector3(ls.x * -1, ls.y, ls.z);
        footCenterOffset = new Vector2(footCenterOffset.x * -1, footCenterOffset.y);
        grabCenterOffset = new Vector2(grabCenterOffset.x * -1, grabCenterOffset.y);
    }

    public float Facing {
        get {
            return transform.localScale.x;
        }
    }

    public bool IsBoostJump() {
        return Jump && !Mathf.Approximately(LiftBoost.sqrMagnitude, 0);
    }

    public bool ClimbCheck() {
        return OnWall && Grab;
    }

    public bool WallJumpCheck() {
        if (!Jump) return false;

        if (OnWall) return true;

        Vector2 wjCenter = new Vector2(transform.position.x + grabCenterOffset.x + WallJumpBonusDst, transform.position.y + grabCenterOffset.y);
        hitWallJump = Physics2D.OverlapBox(wjCenter, onWallBoxSize, 0, groundLayer);
        return hitWallJump != null;
    }

    public float Gravity {
        get {
            return rb.gravityScale;
        }
         set {
            rb.gravityScale = value;
        }
    }

    public void InAirMovement() {
        if (Movement.x == 0) {
            Speed = new Vector2(0, Speed.y);
        }
        else {
            float vx;
            if (Movement.x * Facing < 0) {
                Flip();
                vx = 0;
            } else if (Mathf.Abs(Speed.x) > MaxRun) {
                vx = Mathf.Lerp(Speed.x, Mathf.Sign(Movement.x) * MaxRun, airRunReduce * Time.fixedDeltaTime);
            } else {
                vx = Mathf.Lerp(Speed.x, Mathf.Sign(Movement.x) * MaxRun, airRunAccel * Time.fixedDeltaTime);
            }
            Speed = new Vector2(vx, Speed.y);
        }
    }

    public Vector2 OctoDir(float radius) {
        if (-Mathf.PI * 7f / 8f <= radius && radius < -Mathf.PI * 5f / 8f) {
            return new Vector2(-1, -1).normalized;
        }
        else if (-Mathf.PI * 5f / 8f <= radius && radius < -Mathf.PI * 3f / 8f) {
            return new Vector2(0, -1);
        }
        else if (-Mathf.PI * 3f / 8f <= radius && radius < -Mathf.PI / 8f) {
            return new Vector2(1, -1).normalized;
        }
        else if (-Mathf.PI / 8f <= radius && radius < Mathf.PI / 8f) {
            return new Vector2(1, 0);
        }
        else if (Mathf.PI / 8f <= radius && radius < Mathf.PI * 3f / 8f) {
            return new Vector2(1, 1).normalized;
        }
        else if (Mathf.PI * 3f / 8f <= radius && radius < Mathf.PI * 5f / 8f) {
            return new Vector2(0, 1);
        }
        else if (Mathf.PI * 5f / 8f <= radius && radius < Mathf.PI * 7f / 8f) {
            return new Vector2(-1, 1).normalized;
        }
        else {
            return new Vector2(-1, 0);
        }
    }

    public void OnDrawGizmos() {
        if (OnGround) {
            Gizmos.color = Color.green;
        } else {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireCube(footCenter, onGroundBoxSize);
        if (OnWall) {
            Gizmos.color = Color.green;
        } else {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireCube(grabCenter, onWallBoxSize);
    }
}
