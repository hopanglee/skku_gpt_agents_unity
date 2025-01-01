using Pathfinding;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField]
    private AgentName agentName = AgentName.None;

    private int id;
    public TerrainManager.Location Area;

    [SerializeField]
    private string state;
    private MoveController m_moveController;
    private CommandInvoker m_commandInvoker;

    void Awake()
    {
        m_moveController = GetComponent<MoveController>();
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
                m_moveController,
                TerrainManager.Location.River,
                null,
                () => Debug.Log("Movecommand OnEND()")
            )
        );
        m_commandInvoker.ExcuteNextCommand();
    }

    private void SetAreaInit()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2);

        foreach (var other in colliders)
        {
            if (other.CompareTag("Area"))
            {
                Debug.Log($"Already in Area: {other.name}");
                var Area = other.GetComponent<LocationArea>();
                this.Area = Area.location;
                Area.OnEventInvoked += OnEventListener;
            }
        }
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
            Area.OnEventInvoked += OnEventListener;
            Debug.Log($"Area Enter : {Area.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Area"))
        {
            var Area = other.GetComponent<LocationArea>();
            Area.OnEventInvoked -= OnEventListener;
            Debug.Log($"Area Exit : {Area.name}");
        }
    }

    private void OnEventListener() { }

    void OnEnable() { }

    void OnDisable() { }
}
