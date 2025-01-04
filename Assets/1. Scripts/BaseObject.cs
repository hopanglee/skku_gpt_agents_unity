using System;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class BaseObject : PositionableObject, IStateGetable
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

    public Transform GetTransform()
    {
        Debug.Log($"Print Transform : {transform.position}");
        if (position == null)
            return transform;
        return position;
    }
}

public abstract class ObjectType : IStateGetable
{
    public abstract string GetState();
}

public class Bed : ObjectType
{
    public override string GetState()
    {
        throw new System.NotImplementedException();
    }
}
