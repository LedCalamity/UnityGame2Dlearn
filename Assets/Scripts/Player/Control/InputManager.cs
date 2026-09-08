using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerActions inputActions;

    public event Action OnDash;
    public event Action OnFire;
    public event Action OnFireChaser;
    public event Action OnFireAOE;
    public event Action OnJump;
    public event Action<Vector2> OnMove;
    public event Action On4DirFire;
    public event Action OnGroundPound;
    public event Action OnGravityReverse;
    InputAction groundPoundAction;
    InputAction gravityReverseAction;
    void OnEnable()
    {
        // subscribe all event-subscribed functions to input, so input -> event -> function(logic)
        inputActions = new PlayerActions();
        inputActions.NormalPlayer.Enable();
        inputActions.NormalPlayer.Dash.performed += HandleDash;
        inputActions.NormalPlayer.Fire.performed += HandleFire;
        inputActions.NormalPlayer.FireChaser.performed += HandleFireChaser;
        inputActions.NormalPlayer.SkillFireAOE.performed += HandleFireAOE;
        inputActions.NormalPlayer.Jump.performed += HandleJump;
        inputActions.NormalPlayer.Move.performed += HandleMove;
        inputActions.NormalPlayer.Move.canceled += HandleMove;
        inputActions.NormalPlayer.Skill4DirFire.performed += Handle4DirFire;
        groundPoundAction = inputActions.NormalPlayer.Get().FindAction("GroundPound");
        if (groundPoundAction != null) groundPoundAction.performed += HandleGroundPound;
        gravityReverseAction = inputActions.NormalPlayer.Get().FindAction("GravityReverse");
        if (gravityReverseAction != null) gravityReverseAction.performed += HandleGravityReverse;
    }
    private void OnDisable()
    {
        inputActions.NormalPlayer.Disable();
        inputActions.NormalPlayer.Dash.performed -= HandleDash;
        inputActions.NormalPlayer.Fire.performed -= HandleFire;
        inputActions.NormalPlayer.FireChaser.performed -= HandleFireChaser;
        inputActions.NormalPlayer.SkillFireAOE.performed -= HandleFireAOE;
        inputActions.NormalPlayer.Jump.performed -= HandleJump;
        inputActions.NormalPlayer.Move.performed -= HandleMove;
        inputActions.NormalPlayer.Move.canceled -= HandleMove;
        inputActions.NormalPlayer.Skill4DirFire.performed -= Handle4DirFire;
        if (groundPoundAction != null) groundPoundAction.performed -= HandleGroundPound;
        if (gravityReverseAction != null) gravityReverseAction.performed -= HandleGravityReverse;
    }
    void HandleFire(InputAction.CallbackContext ctx)
    {
        OnFire?.Invoke();
    }
    void HandleFireChaser(InputAction.CallbackContext ctx)
    {
        OnFireChaser?.Invoke();
    }
    void HandleDash(InputAction.CallbackContext ctx)
    {
        OnDash?.Invoke();
    }
    void HandleMove(InputAction.CallbackContext ctx)
    {
        OnMove?.Invoke(ctx.ReadValue<Vector2>());
    }
    void HandleFireAOE(InputAction.CallbackContext ctx)
    {
        OnFireAOE?.Invoke();
    }
    void HandleJump(InputAction.CallbackContext ctx)
    {
        OnJump?.Invoke();
    }
    void Handle4DirFire(InputAction.CallbackContext ctx)
    {
        On4DirFire?.Invoke();
    }
    void HandleGroundPound(InputAction.CallbackContext ctx)
    {
        OnGroundPound?.Invoke();
    }
    void HandleGravityReverse(InputAction.CallbackContext ctx)
    {
        OnGravityReverse?.Invoke();
    }
}
