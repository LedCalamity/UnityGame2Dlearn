using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//check mana-> effect->audio->logic
public class SkillManager : MonoBehaviour
{
    static readonly Vector3Int[] connectedTileDirections =
    {
        Vector3Int.left,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.up
    };

    public static SkillManager Instance;
    public GameObject fire_bulletPrefab;
    public SkillData skillData = new SkillData();
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void CallSkill4DirFireTask(Vector3 pos)
    {
        StartCoroutine(Skill4DirFireTask(pos));
    }
    IEnumerator Skill4DirFireTask(Vector3 pos)
    {
        if (!PlayerMana.Instance.DeductMana(4)) yield break;
        //Effect
        EffectManager.Instance.Generate4DirFireEffect(pos);
        //Audio
        AudioManager.Instance.AudioPlay(2, "4DirFire_sef", false);
        //action(logic)
        Instantiate(fire_bulletPrefab, pos, Quaternion.Euler(0, 0, 45));
        Instantiate(fire_bulletPrefab, pos, Quaternion.Euler(0, 0, 135));
        Instantiate(fire_bulletPrefab, pos, Quaternion.Euler(0, 0, 225));
        Instantiate(fire_bulletPrefab, pos, Quaternion.Euler(0, 0, 315));

        yield return new WaitForSeconds(0.5f);
    }
    public void CallSkillFireAOETask(Vector3 pos)
    {
        StartCoroutine(SkillFireAOETask(pos));
    }
    IEnumerator SkillFireAOETask(Vector3 pos)
    {
        if(!PlayerMana.Instance.DeductMana(7)) yield break;
        var fire_aoe_object = EffectManager.Instance.GenerateFireAOEEffect(pos);
        
        AudioManager.Instance.AudioPlay(2, "FireAOE_sef", false);

        Collider2D[] hitrange = Physics2D.OverlapCircleAll(pos, fire_aoe_object.transform.localScale.x * fire_aoe_object.GetComponent<CircleCollider2D>().radius);
        //below is the place for logical manip. of aoe skill
        for (int i = 0; i < 5; i++) //hit 5 times consecutively
        {
            foreach(var hit in hitrange)
            {
                if(hit.CompareTag("Enemies"))
                {
                    hit.GetComponent<EnemyHP>().DeductHealth(1);
                }
            }
            yield return new WaitForSeconds(0.2f); //with interval 0.2s
        }
        
    }

    public void CallGroundPoundTask(PlayerControlGroundPound controller)
    {
        StartCoroutine(GroundPoundTask(controller));
    }

    IEnumerator GroundPoundTask(PlayerControlGroundPound controller)
    {
        GroundPoundSkillData data = skillData.groundPound;
        if (controller == null || !data.IsConfigured || !controller.CanStartGroundPound(data.minimumBreakHeight)) yield break;
        if (!PlayerMana.Instance.DeductMana(data.manaCost)) yield break;

        // Effect
        EffectManager.Instance?.GenerateGroundPoundEffect(controller.transform.position);
        // Audio
        AudioManager.Instance?.AudioPlay(2, "GroundPound_sef", false);
        // Logic
        controller.BeginGroundPound(data);

        HashSet<EnemyHP> hitEnemies = new HashSet<EnemyHP>();
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        int enemyMask = LayerMask.GetMask("EnemyLayer");

        while (controller != null && controller.IsGroundPounding)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(controller.HitCenter, controller.HitSize, 0f, enemyMask);
            foreach (Collider2D hit in hits)
            {
                EnemyHP enemy = hit.GetComponent<EnemyHP>();
                if (enemy == null || !hitEnemies.Add(enemy)) continue;

                // Effect
                EffectManager.Instance?.GenerateHitEffect(enemy.transform.position);
                // Audio
                AudioManager.Instance?.AudioPlay(3, "Hit_sef", false);
                // Logic
                enemy.DeductHealth(data.damage, false);
            }

            if (controller.TryGetBreakTarget(out Tilemap tilemap, out Vector3Int cell))
            {
                List<Vector3Int> breakCells = GetConnectedBreakCells(tilemap, cell, data.maxConnectedBreakCount);

                // Effect
                foreach (Vector3Int breakCell in breakCells)
                {
                    EffectManager.Instance?.GenerateGroundPoundEffect(tilemap.GetCellCenterWorld(breakCell));
                }
                // Audio
                AudioManager.Instance?.AudioPlay(2, "GroundPound_sef", false);
                // Logic
                foreach (Vector3Int breakCell in breakCells)
                {
                    tilemap.SetTile(breakCell, null);
                }

                TilemapCollider2D tilemapCollider = tilemap.GetComponent<TilemapCollider2D>();
                if (tilemapCollider != null && tilemapCollider.hasTilemapChanges)
                {
                    tilemapCollider.ProcessTilemapChanges();
                }

                controller.CompleteBreakTarget(tilemap, cell);
            }

            yield return waitForFixedUpdate;
        }
    }

    List<Vector3Int> GetConnectedBreakCells(Tilemap tilemap, Vector3Int startCell, int maxBreakCount)
    {
        List<Vector3Int> breakCells = new List<Vector3Int>();
        Queue<Vector3Int> cellsToCheck = new Queue<Vector3Int>();
        HashSet<Vector3Int> checkedCells = new HashSet<Vector3Int>();

        cellsToCheck.Enqueue(startCell);
        checkedCells.Add(startCell);

        while (cellsToCheck.Count > 0 && breakCells.Count < maxBreakCount)
        {
            Vector3Int currentCell = cellsToCheck.Dequeue();
            if (!(tilemap.GetTile(currentCell) is BreakableRuleTile))
            {
                continue;
            }

            breakCells.Add(currentCell);

            foreach (Vector3Int direction in connectedTileDirections)
            {
                Vector3Int nextCell = currentCell + direction;
                if (checkedCells.Add(nextCell))
                {
                    cellsToCheck.Enqueue(nextCell);
                }
            }
        }

        return breakCells;
    }
}
