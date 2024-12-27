using UnityEngine;

class PlayerReloading : ShooterStateBase
{
    private PlayerWeapon m_playerWeapon;

    public PlayerReloading(PlayerStateMachine pStateMachine)
        : base(pStateMachine)
    {
        m_playerWeapon = pStateMachine.GetComponent<PlayerWeapon>();
    }

    public override void Update()
    {
        OnReloadFinished();
    }

    private void OnReloadFinished()
    {
        m_playerWeapon.ReloadBullet();

        StateMachine.ChangeState(PlayerShooterState.Idle);
    }
}