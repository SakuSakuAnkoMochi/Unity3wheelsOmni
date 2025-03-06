  using System;
using System.Collections.Generic;

class AStar
{
    public static Node SolveAStar(int[,] map)
    {
        Node[,] nodes = new Node[map.GetLength(0), map.GetLength(1)];
        Node start = null;
        Node goal = null;
        for (int j = 0; j < map.GetLength(0); j++)
        {
            for (int i = 0; i < map.GetLength(1); i++)
            {
                switch (map[j, i])
                {
                    case 0:
                        nodes[j, i] = new Node()
                        {
                            kind = NodeKind.empty,
                            position = (j, i)
                        };
                        break;
                    case 1:
                        nodes[j, i] = new Node()
                        {
                            kind = NodeKind.wall,
                            position = (j, i)
                        };
                        break;
                    case 2:
                        start = new Node()
                        {
                            kind = NodeKind.start,
                            position = (j, i)
                        };
                        nodes[j, i] = start;
                        break;
                    case 3:
                        goal = new Node()
                        {
                            kind = NodeKind.goal,
                            position = (j, i)
                        };
                        nodes[j, i] = goal;
                        break;
                }
            }
        }
        if (start == null || goal == null) return null;
        Node nowNode = start;
        List<Node> OpenedNodes = new();
        List<Node> ClosedNodes = new();
        OpenedNodes.Add(nowNode);
        while (OpenedNodes.Count > 0)
        {
            nowNode = OpenedNodes[0];
            for (int i = 0; i < OpenedNodes.Count; i++)
            {
                if (OpenedNodes[i].distanceSum < nowNode.distanceSum)
                {
                    nowNode = OpenedNodes[i];
                }
            }
            for (int i = 0; i < OpenedNodes.Count; i++)
            {
                if (OpenedNodes[i].distanceSum == nowNode.distanceSum)
                {
                    if (OpenedNodes[i].distanceFromStart < nowNode.distanceFromStart)
                    {
                        nowNode = OpenedNodes[i];
                    }
                }
            }
            OpenedNodes.Remove(nowNode);
            ClosedNodes.Add(nowNode);
            if (nowNode == goal) break;
            foreach (var (dx, dy) in new (int, int)[] { (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1), (1, -1) })
            {
                if (nowNode.position.j + dx < 0 || nowNode.position.j + dx >= nodes.GetLength(0) || nowNode.position.i + dy < 0 || nowNode.position.i + dy >= nodes.GetLength(0)) continue;
                var 候補 = nodes[nowNode.position.j + dx, nowNode.position.i + dy];
                if (候補.kind == NodeKind.wall) continue;
                if (OpenedNodes.Contains(候補)) continue;
                if (ClosedNodes.Contains(候補)) continue;
                候補.parent = nowNode;
                候補.distanceFromStart = nowNode.distanceFromStart + dx*dx+dy*dy;//MathF.Sqrt(dx*dx+dy*dy);
                候補.distanceFromGoal = (goal.position.j - 候補.position.j) * (goal.position.j - 候補.position.j) + (goal.position.i - 候補.position.i) * (goal.position.i - 候補.position.i);//MathF.Sqrt() sqrtすると3秒ぐらい待たされる(しないと止まったかわからない)
                候補.distanceSum = 候補.distanceFromStart + 候補.distanceFromGoal;
                OpenedNodes.Add(候補);
            }

        }
        return goal;
    }
}

class Node
{
    public Node parent;
    public NodeKind kind;
    public (int j, int i) position;
    public float distanceFromStart;
    public float distanceFromGoal;
    public float distanceSum;
}
enum NodeKind
{
    empty,
    wall,
    start,
    goal,
}