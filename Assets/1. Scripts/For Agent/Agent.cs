using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pathfinding;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Agent : PositionableObject, IStateGetable, IStateSetable //, IInteractable
{
    [SerializeField]
    private AgentName agentName = AgentName.None;

    private int id;

    [SerializeField]
    private string state;
    private CommandManager m_commandManager;

    private List<AINPCRequest.TalkInfo> TalkInfoList = new();

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
        // m_commandManager.AddMoveToObjectCommand(BaseObject.ObjectTag.Desk);

        // m_commandManager.Execute();
        DayStart();
    }

    public void SetState(string new_state)
    {
        this.state = new_state;
    }

    public string GetState()
    {
        return this.state;
    }

    public AgentName GetName()
    {
        return agentName;
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
        TalkInfoList.Add(new AINPCRequest.TalkInfo { agentName = sender.agentName, content = str });
    }

    void OnEnable() { }

    void OnDisable() { }

    public float Distance(Agent agent)
    {
        return Vector3.Distance(this.transform.position, agent.transform.position);
    }

    public async void DayStart()
    {
        var request = BuildRequest();
        var temp = await NetworkManager.AINPCStartPost(request);

        // debug
        Debug.Log($"{agentName} : {temp.message}");

        StartCoroutine(GetInformatrionCoroutine());
    }

    IEnumerator GetInformatrionCoroutine()
    {
        while (true)
        {
            // SendToInformation을 실행하고 완료될 때까지 대기
            yield return SendToInformationCoroutine();

            // 5초 대기
            yield return new WaitForSeconds(5f);
        }
    }

    // Coroutine 방식으로 Async 작업 처리
    private IEnumerator SendToInformationCoroutine()
    {
        var task = SendToInformation();
        while (!task.IsCompleted)
        {
            yield return null; // 한 프레임 대기
        }

        // Task에서 발생한 예외 처리
        if (task.Exception != null)
        {
            Debug.LogError(task.Exception);
        }
    }

    public async Task SendToInformation()
    {
        var request = BuildRequest();

        var responses = await NetworkManager.AINPCPost(request);

        if (responses == null || responses.commands == null || responses.commands.Length == 0)
        {
            Debug.LogWarning("No commands received from server.");
            return;
        }
        // debug
        Debug.Log($"{agentName} : {responses.message}");

        m_commandManager.Reset();

        foreach (var response in responses.commands)
        {
            m_commandManager.AddCommandFromResponseJson(response);
        }

        m_commandManager.Execute();
    }

    private AINPCRequest BuildRequest()
    {
        AINPCRequest request = new();

        request.myInformation = new AINPCRequest.AgentInformation
        {
            agentName = this.agentName,
            position = new SerializableVector3(this.transform.position),
        };
        request.areaName = Area;

        List<AINPCRequest.AgentInformation> agentList = new();

        foreach (var agent in TerrainManager.LocationToArea(Area).GetAgents())
        {
            var newData = new AINPCRequest.AgentInformation
            {
                agentName = agent.agentName,
                position = new SerializableVector3(agent.transform.position),
                state = agent.GetState(),
            };

            agentList.Add(newData);
        }

        request.otherAgents = agentList.ToArray();

        List<AINPCRequest.BaseObjectInformation> objectList = new();

        foreach (var baseObject in TerrainManager.LocationToArea(Area).GetBaseObjects())
        {
            var newData = new AINPCRequest.BaseObjectInformation
            {
                objectTag = baseObject.objectTag,
                position = new SerializableVector3(baseObject.transform.position),
                state = baseObject.GetState(),
            };

            objectList.Add(newData);
        }

        request.otherBaseObjects = objectList.ToArray();

        request.talkInfos = TalkInfoList.ToArray();
        TalkInfoList.Clear();

        return request;
    }

    public async void CommandIsComplete()
    {
        var request = BuildRequest();

        var temp = await NetworkManager.AINPCCommandCompletedPost(request);

        // debug
        Debug.Log($"{agentName} : {temp.message}");
    }
}
