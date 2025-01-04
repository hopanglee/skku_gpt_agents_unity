using Pathfinding;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField]
    private AgentName agentName = AgentName.None;

    private int id;
    public TerrainManager.LocationTag Area;

    [SerializeField]
    private string state;
    private CommandManager m_commandManager;

    void Awake()
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
        if (agentName == AgentName.Adam)
            m_commandManager.AddChaseCommand(AgentName.Eve);
        else
            m_commandManager.AddMoveToLocationCommand(TerrainManager.LocationTag.AdamHouse);
        m_commandManager.AddSpeakCommand("Hello", 10f);

        m_commandManager.Execute();
    }

    public void SetState(string new_state)
    {
        this.state = new_state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Area"))
        {
            var Area = other.GetComponent<LocationArea>();
            this.Area = Area.location;
            Area.Enter(this);
            Debug.Log($"Area Enter : {Area.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Area"))
        {
            var Area = other.GetComponent<LocationArea>();
            Area.Exit(this);
            Debug.Log($"Area Exit : {Area.name}");
        }
    }

    public void OnEventListener(Agent sender, string str)
    {
        Debug.Log($"{agentName}) {sender.name} : {str}");
    }

    void OnEnable() { }

    void OnDisable() { }

    public float Distance(Agent agent)
    {
        return Vector3.Distance(this.transform.position, agent.transform.position);
    }
}
