using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class Agent : PositionableObject, IStateGetable
{
    [SerializeField]
    private AgentName agentName = AgentName.None;

    private int id;

    [SerializeField]
    private string state;
    private CommandManager m_commandManager;

    protected override void Awake()
    {
        m_commandManager = new CommandManager(this);

        // AgentManager에 에이전트 추가
        if (agentName != AgentName.None) // ID가 0인지 확인 (0은 기본값)
        {
            id = (int)agentName;
            AgentManager.AddAgent(id, this);
        }
        else
        {
            Debug.LogError("Agent ID is not set! Please assign a unique ID in the Inspector.");
        }
    }

    private void Start()
    {
        // TEST
        //m_commandManager.AddMoveToLocationCommand(TerrainManager.LocationTag.EveHouse);
        m_commandManager.AddMoveToObjectCommand(BaseObject.ObjectTag.Desk);

        m_commandManager.Execute();
    }

    public void SetState(string new_state)
    {
        this.state = new_state;
    }

    public string GetState()
    {
        return this.state;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    public void OnEventListener(Agent sender, string str)
    {
        Debug.Log($"{agentName}) {sender.agentName} : {str}");
    }

    void OnEnable() { }

    void OnDisable() { }

    public float Distance(Agent agent)
    {
        return Vector3.Distance(this.transform.position, agent.transform.position);
    }
}
