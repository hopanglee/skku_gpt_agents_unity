using UnityEngine;

[RequireComponent(typeof(MoveController))]
public class Agent : MonoBehaviour
{
    public TerrianManager.Location Area;
    public string state;

    [SerializeField]
    private int id;
    private MoveController m_moveController;
    private CommandManager m_commandManager;

    void Awake()
    {
        m_moveController = new MoveController();
        m_commandManager = new CommandManager();

        // AgentManager에 에이전트 추가
        if (id != 0) // ID가 0인지 확인 (0은 기본값)
        {
            AgentManager.AddAgent(id, this);
        }
        else
        {
            Debug.LogError("Agent ID is not set! Please assign a unique ID in the Inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Area")
        {
            var Area = other.GetComponent<LocationArea>();
            this.Area = Area.location;
            Area.OnEventInvoked += OnEventListener;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Area")
        {
            var Area = other.GetComponent<LocationArea>();
            Area.OnEventInvoked -= OnEventListener;
        }
    }

    private void OnEventListener() { }

    void OnEnable()
    {
        //EventManager.
    }

    void OnDisable() { }
}
