using System.Collections.Generic;

public static class AgentManager
{
    // Dictionary 초기화
    public static Dictionary<int, Agent> agentDictionary = new Dictionary<int, Agent>();

    // 에이전트 가져오기
    public static Agent GetAgent(int id)
    {
        if (agentDictionary.TryGetValue(id, out var agent))
        {
            return agent; // id에 해당하는 Agent 반환
        }

        // id가 없을 경우 null 반환 또는 예외 처리
        throw new KeyNotFoundException($"Agent with ID {id} does not exist in the dictionary.");
    }

    // 에이전트 추가
    public static void AddAgent(int id, Agent agent)
    {
        if (!agentDictionary.ContainsKey(id))
        {
            agentDictionary.Add(id, agent);
        }
        else
        {
            throw new System.Exception($"Agent with ID {id} already exists in the dictionary.");
        }
    }

    // 에이전트 제거
    public static bool RemoveAgent(int id)
    {
        return agentDictionary.Remove(id);
    }
}
