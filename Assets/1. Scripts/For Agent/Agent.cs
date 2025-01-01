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
    private CommandInvoker m_commandInvoker;

    void Awake()
    {
        m_commandInvoker = new CommandInvoker();

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

        //SetAreaInit();
    }

    private void Start()
    {
        // TEST
        m_commandInvoker.EnqueueCommand(
            new MoveCommand(
                this,
                TerrainManager.LocationTag.River,
                null,
                m_commandInvoker.ExcuteNextCommand
            )
        );
        m_commandInvoker.EnqueueCommand(new SpeakCommand(this, "I Speak!"));
        m_commandInvoker.ExcuteNextCommand();
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

    public void OnEventListener(string str)
    {
        Debug.Log($"{agentName} hear {str}");
    }

    void OnEnable() { }

    void OnDisable() { }
}
