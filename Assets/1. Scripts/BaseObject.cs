using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class BaseObject : PositionableObject, IStateGetable, IStateSetable, IInteractable
{
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

    public string GetState()
    {
        return m_objectType?.GetState();
    }

    public void SetState(string new_state)
    {
        m_objectType?.SetState(new_state);
    }

    public Transform GetTransform()
    {
        Debug.Log($"Print Transform : {transform.position}");
        if (position == null)
            return transform;
        return position;
    }

    public void Interact(Agent agent, string content)
    {
        throw new NotImplementedException();
    }
}

public abstract class ObjectType : IStateGetable
{
    public string state;
    public virtual string GetState()
    {
        return state;
    }

    public virtual void SetState(string new_state)
    {
        state = new_state;
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