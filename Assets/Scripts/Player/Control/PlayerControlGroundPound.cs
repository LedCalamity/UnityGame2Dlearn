using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerControlGroundPound : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D playerCollider;
    PlayerControlMove moveControl;
    InputManager inputManager;

    GroundPoundSkillData activeData;
    float startTime;
    float invincibleEndTime;
    bool canBreakTiles;
    bool isLandingInvincible;
    bool isIgnoringEnemyCollision;
    bool isWaitingForTileBreak;
    int enemyLayer;

    Tilemap breakTilemap;
    Vector3Int breakCell;

    public bool is_unlocked = false;
    public bool IsGroundPounding { get; private set; }
    public bool IsInvincible => IsGroundPounding || isLandingInvincible;
    public bool CanBreakTiles => canBreakTiles;
    public Vector2 HitCenter => playerCollider.bounds.center;
    public Vector2 HitSize => playerCollider.bounds.size;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        moveControl = GetComponent<PlayerControlMove>();
        inputManager = GetComponent<InputManager>();
        enemyLayer = LayerMask.NameToLayer("EnemyLayer");
        inputManager.OnGroundPound += TryStartGroundPound;
    }

    void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnGroundPound -= TryStartGroundPound;
        }

        CancelGroundPound();
    }

    void TryStartGroundPound()
    {
        if (!is_unlocked || IsGroundPounding || IsGrounded() || SkillManager.Instance == null)
        {
            return;
        }

        SkillManager.Instance.CallGroundPoundTask(this);
    }

    public bool CanStartGroundPound(float minimumBreakHeight)
    {
        if (IsGroundPounding || IsGrounded())
        {
            return false;
        }

        canBreakTiles = GetDistanceToGround() >= minimumBreakHeight;
        return true;
    }

    public void BeginGroundPound(GroundPoundSkillData data)
    {
        activeData = data;
        startTime = Time.time;
        isLandingInvincible = false;
        breakTilemap = null;
        isWaitingForTileBreak = false;
        IsGroundPounding = true;

        if (moveControl != null)
        {
            moveControl.enabled = false; //nice addition
        }

        if (enemyLayer >= 0)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer, true);
            isIgnoringEnemyCollision = true;
        }
    }

    void FixedUpdate()
    {
        UpdateLandingInvincibility();

        if (!IsGroundPounding)
        {
            return;
        }

        rb.linearVelocity = new Vector2(0f, -activeData.speed);

        if (Time.time - startTime >= activeData.maxDuration)
        {
            EndGroundPound(false);
            return;
        }

        if (!isWaitingForTileBreak)
        {
            CheckGroundImpact();
        }
    }

    void CheckGroundImpact()
    {
        Bounds bounds = playerCollider.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y + 0.02f);
        float distance = activeData.speed * Time.fixedDeltaTime + 0.05f;
        RaycastHit2D hit = Physics2D.BoxCast(origin, new Vector2(bounds.size.x * 0.8f, 0.02f), 0f, Vector2.down, distance, LayerMask.GetMask("Ground"));

        if (!hit.collider) //if not hit ground no need analyze ground
        {
            return;
        }

        Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
        if (canBreakTiles && tilemap != null)
        {
            Vector3Int cell = tilemap.WorldToCell(hit.point + Vector2.down * 0.01f);
            if (tilemap.GetTile(cell) is BreakableRuleTile)
            {
                breakTilemap = tilemap;
                breakCell = cell;
                isWaitingForTileBreak = true;
                return;
            }
        }

        EndGroundPound(true);
    }

    public bool TryGetBreakTarget(out Tilemap tilemap, out Vector3Int cell)
    {
        tilemap = breakTilemap;
        cell = breakCell;
        return isWaitingForTileBreak && tilemap != null;
    }

    public void CompleteBreakTarget(Tilemap tilemap, Vector3Int cell)
    {
        if (tilemap != breakTilemap || cell != breakCell)
        {
            return;
        }

        breakTilemap = null;
        isWaitingForTileBreak = false;
    }

    void EndGroundPound(bool startLandingInvincibility)
    {
        if (!IsGroundPounding)
        {
            return;
        }

        IsGroundPounding = false;
        breakTilemap = null;
        isWaitingForTileBreak = false;

        if (moveControl != null)
        {
            moveControl.enabled = true;
        }

        if (startLandingInvincibility && activeData.invincibleDuration > 0f)
        {
            isLandingInvincible = true;
            invincibleEndTime = Time.time + activeData.invincibleDuration;
            return;
        }

        isLandingInvincible = false;
        RestoreEnemyCollision();
    }

    void UpdateLandingInvincibility()
    {
        if (!isLandingInvincible || Time.time < invincibleEndTime)
        {
            return;
        }

        isLandingInvincible = false;
        RestoreEnemyCollision();
    }

    void CancelGroundPound()
    {
        IsGroundPounding = false;
        isLandingInvincible = false;
        breakTilemap = null;
        isWaitingForTileBreak = false;

        if (moveControl != null)
        {
            moveControl.enabled = true;
        }

        RestoreEnemyCollision();
    }

    void RestoreEnemyCollision()
    {
        if (enemyLayer >= 0 && isIgnoringEnemyCollision)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer, false);
            isIgnoringEnemyCollision = false;
        }
    }

    bool IsGrounded()
    {
        return GetDistanceToGround() <= 0.05f;
    }

    float GetDistanceToGround()
    {
        Bounds bounds = playerCollider.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y + 0.02f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
        return hit.collider ? hit.distance : 0f;
    }
}
