using UnityEngine;
using System.Collections.Generic;

public enum PlayerShooterState
{
    Idle,
    Reloading,
    Shooting
}

public class PlayerStateMachine : MonoBehaviour
{
    private Dictionary<PlayerShooterState, ShooterStateBase> m_stateByKey;
    private StateMachine m_stateMachine;
    private PlayerShooterState m_currentState;

    public PlayerShooterState CurrentState => m_currentState;

    private void Awake()
    {
        m_stateMachine = new StateMachine();
        m_stateByKey = new Dictionary<PlayerShooterState, ShooterStateBase>();

        m_stateByKey.Add(PlayerShooterState.Idle,
            new PlayerIdle(this));
        m_stateByKey.Add(PlayerShooterState.Reloading,
            new PlayerReloading(this));
        m_stateByKey.Add(PlayerShooterState.Shooting,
            new PlayerShooting(this));

        m_currentState = PlayerShooterState.Idle;
        m_stateMachine.Init(m_stateByKey[m_currentState]);
    }

    private void Update()
    {
        m_stateMachine.Update();
    }

    public void ChangeState(PlayerShooterState pState)
    {
        if (m_currentState == pState)
            return;

        m_currentState = pState;
        m_stateMachine.ChangeState(m_stateByKey[pState]);
    }
}