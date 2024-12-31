using System;
using Pathfinding;
using UnityEngine;

public class MoveController
{
    private float m_speed = 4f;
    private FollowerEntity followerEntity;

    public MoveController(FollowerEntity follower)
    {
        this.followerEntity = follower;
    }

    public void SetTarget(Vector3 vector)
    {
        if (followerEntity != null)
        {
            followerEntity.SetDestination(vector);
        }
    }
}
