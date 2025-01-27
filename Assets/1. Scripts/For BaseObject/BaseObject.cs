using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Pathfinding;
using UnityEngine;

public class BaseObject : PositionableObject, IStateGetable, IInteractable
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ObjectTag
    {
        None,
        Bed,
        Desk,
        Chair,
        Tree,
        Bench,
        KitchenDesk,
        River,
    }

    public int id;
    public ObjectTag objectTag;
    private ObjectType m_objectType;

    [SerializeField]
    private Transform position;

    private static readonly Dictionary<ObjectTag, ObjectType> ObjectTypeFactory = new()
    {
        { ObjectTag.None, null },
        { ObjectTag.Bed, new Bed() },
        { ObjectTag.Desk, null },
        { ObjectTag.Chair, null },
        { ObjectTag.Tree, null },
        { ObjectTag.Bench, null },
        { ObjectTag.KitchenDesk, null },
        { ObjectTag.River, null },
        // 추가 클래스들...
    };

    protected override void Awake()
    {
        Init();
        base.Awake();
    }

    private void Init()
    {
        if (ObjectTypeFactory.TryGetValue(objectTag, out var objectType))
        {
            m_objectType = objectType;
            //Debug.Log($"ObjectType 설정 성공: {objectTag}");
        }
        else
        {
            //Debug.LogError($"ObjectTag '{objectTag}'에 해당하는 ObjectType이 없습니다.");
        }
    }

    private void Start()
    {
        StartServerCall();
    }

    private async void StartServerCall()
    {
        // interact -> 서버에서 이 BaseObject의 상태를 어떻게 변경할 지 받아오기
        BaseObjectRequest request = new BaseObjectRequest
        {
            objectTag = this.objectTag,
            id = this.id,
        };

        // server에 request보내고 response받기
        var response = await NetworkManager.BaseObjectStartPost(request);

        // response로 온 new state로 setstate실행하기
        SetState(response.newFrontState, response.newBackState);

        // debug
        Debug.Log($"{objectTag} : {response.message}");
    }

    public string GetState()
    {
        return m_objectType?.GetState();
    }

    public void SetState(string newFrontState, string newBackState)
    {
        m_objectType?.SetState(newFrontState, newBackState);
    }

    public Transform GetTransform()
    {
        Debug.Log($"Print Transform : {transform.position}");
        if (position == null)
            return transform;
        return position;
    }

    public async void Interact(Agent agent, string content)
    {
        // interact -> 서버에서 이 BaseObject의 상태를 어떻게 변경할 지 받아오기
        BaseObjectRequest request = new BaseObjectRequest
        {
            objectTag = this.objectTag,
            id = this.id,
            agentName = agent.GetName(),
            interactContent = content,
        };

        // server에 request보내고 response받기
        var response = await NetworkManager.BaseObjectPost(request);

        // debug
        Debug.Log($"{objectTag} : {response.message}");

        // response로 온 new state로 setstate실행하기
        SetState(response.newFrontState, response.newBackState);
    }

    public void Interact(BaseObject.ObjectTag objectTag, string content)
    {
        throw new System.NotImplementedException();
    }
}

public abstract class ObjectType : IStateGetable
{
    public string frontState;
    public string backState;

    public virtual string GetState()
    {
        return frontState;
    }

    public virtual void SetState(string newFrontState, string newBackState)
    {
        frontState = newFrontState;
        backState = newBackState;
    }
}

public class Bed : ObjectType
{
    public override string GetState()
    {
        return base.GetState();
    }
}

public class Desk : ObjectType
{
    public override string GetState()
    {
        return base.GetState();
    }
}

public class Chair : ObjectType
{
    public override string GetState()
    {
        return base.GetState();
    }
}

public class Tree : ObjectType
{
    public override string GetState()
    {
        return base.GetState();
    }
}

public class Bench : ObjectType
{
    public override string GetState()
    {
        return base.GetState();
    }
}

public class KitchenDesk : ObjectType
{
    public override string GetState()
    {
        return base.GetState();
    }
}

public class River : ObjectType
{
    public override string GetState()
    {
        return base.GetState();
    }
}
