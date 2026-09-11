using System;
using UnityEngine.Serialization;

[Serializable]
public class SkillData
{
    public GroundPoundSkillData groundPound = new GroundPoundSkillData();
}

[Serializable]
public class GroundPoundSkillData
{
    public int manaCost;
    public int damage;
    public int maxConnectedBreakCount;
    public float invincibleDuration;
    [FormerlySerializedAs("speed")]
    public float acceleration;
    public float maxDuration;
    public float minimumBreakHeight;

    public bool IsConfigured =>
        manaCost > 0 &&
        damage > 0 &&
        maxConnectedBreakCount > 0 &&
        invincibleDuration > 0f &&
        acceleration > 0f &&
        maxDuration > 0f &&
        minimumBreakHeight > 0f;
}
