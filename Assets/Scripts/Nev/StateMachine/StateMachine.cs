
public class StateMachine
{
    private StateBase m_currentState;

    public void Init(StateBase pState)
    {
        ChangeState(pState);
    }

    public void ChangeState(StateBase pState)
    {
        m_currentState?.Exit(); // == if(m_currentState != null) m_currentState.Exit();
        m_currentState = pState;
        m_currentState.Enter();
    }

    public void Update()
    {
        m_currentState?.Update();
    }
}