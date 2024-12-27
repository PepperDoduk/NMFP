
public class ShooterStateBase : StateBase
{
    private PlayerStateMachine m_stateMachine;
    protected PlayerStateMachine StateMachine
        => m_stateMachine;


    public ShooterStateBase(PlayerStateMachine pStateMachine)
    {
        m_stateMachine = pStateMachine;
    }

}