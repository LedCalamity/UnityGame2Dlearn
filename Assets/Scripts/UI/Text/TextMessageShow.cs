using UnityEngine;
using TMPro;

public class TextMessageShow : MonoBehaviour
{
    public TMP_Text Mestext;
    float cur_time = 0f, allow_time = 2f;
    bool is_showing = false, is_consistent = false;
    void Start()
    {
        Mestext.text = "Press A/D to Move";
        is_showing = true;
    }
    void Update()
    {
        if(is_showing && !is_consistent)
        {
            cur_time += Time.deltaTime;
            if (cur_time >= allow_time)
            {
                cur_time = 0f;
                Mestext.text = "";
                is_showing = false;
            }
        }
    }
    public void UnlockDash()
    {
        Mestext.text = "Dash Unlocked";
        is_showing = true;
        is_consistent = false;
    }
    public void ResetText()  //total reset
    {
        Mestext.text = "";
        is_showing = false;
        is_consistent = false;
        cur_time = 0f;
    }
    public void EnterJumpHint()
    {
        Mestext.text = "Press <Space> to Jump";
        is_showing = true;
        is_consistent = true;
    }
    public void ExitJumpHint()
    {
        ResetText();
    }
    public void EnterDoubleJumpHint()
    {
        Mestext.text = "Press <Space> twice to Double Jump";
        is_showing = true;
        is_consistent = true;
    }
    public void ExitDoubleJumpHint()
    {
        ResetText();
    }
    public void EnterDashHint()
    {
        Mestext.text = "Press <Shift> to Dash";
        is_showing = true;
        is_consistent = true;
    }
    public void ExitDashHint()
    {
        ResetText();
    }
    public void EnterRedObjectHint()
    {
        Mestext.text = "Jump cannot reset on RED objects";
        is_showing = true;
        is_consistent = true;
    }
    public void ExitRedObjectHint()
    {
        ResetText();
    }
    public void EnterGemHint()
    {
        Mestext.text = "You can upgrade abilities by collecting GEMs";
        is_showing = true;
        is_consistent = true;
    }
    public void ExitGemHint()
    {
        ResetText();
    }
    public void EnterExitHint()
    {
        Mestext.text = "Left click-Shoot, Right click-Chaser, F-Skill";
        is_showing = true;
        is_consistent = true;
    }
    public void ExitExitHint()
    {
        ResetText();
    }
    public void EnterRegameHint()
    {
        Mestext.text = "When you got NOTHING u got nothing to lose";
        is_showing = true;
        is_consistent = true;
    }
    public void ExitRegameHint()
    {
        ResetText();
    }
}
