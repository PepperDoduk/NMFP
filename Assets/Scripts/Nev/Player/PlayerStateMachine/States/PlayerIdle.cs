using UnityEngine;

class PlayerIdle : ShooterStateBase
{
    private PlayerWeapon m_playerWeapon;

    public PlayerIdle(PlayerStateMachine pStateMachine)
        : base(pStateMachine)
    {
        m_playerWeapon = pStateMachine.GetComponent<PlayerWeapon>();
    }

    public override void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            StateMachine.ChangeState(PlayerShooterState.Shooting);
            return;
        }
        if (Input.GetKeyDown(KeyCode.R) || m_playerWeapon.CurrentBullet <= 0)
        {
            StateMachine.ChangeState(PlayerShooterState.Reloading);
            return;
        }
    }
}