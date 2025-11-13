using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AStarPathFinder 
{
    public float nodeSpacing = 1f;
    public float obstacleCheckRadius = 0.3f;
    public LayerMask obstacleLayer;
    public int maxIterations = 1000;

    public AStarPathFinder(float spacing, float checkRadius, LayerMask obstacles)
    {
        nodeSpacing = spacing;
        obstacleCheckRadius = checkRadius;
        obstacleLayer = obstacles;
    }

    // ฟังก์ชันหาเส้นทางหลัก
    public List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos, float stoppingDistance)
    {
        List<NodePath> openList = new List<NodePath>();
        HashSet<Vector2> closedList = new HashSet<Vector2>();

        NodePath startNode = new NodePath(startPos);
        NodePath targetNode = new NodePath(targetPos);

        openList.Add(startNode);

        int iterations = 0;

        while (openList.Count > 0 && iterations < maxIterations)
        {
            iterations++;

            NodePath currentNode = openList.OrderBy(n => n.fCost).ThenBy(n => n.hCost).First();

            openList.Remove(currentNode);
            closedList.Add(currentNode.position);

            // ถึงเป้าหมายแล้ว
            if (Vector2.Distance(currentNode.position, targetPos) <= stoppingDistance)
            {
                return SimplifyPath(RetracePath(startNode, currentNode));
            }

            // สำรวจโหนดข้างเคียง
            foreach (Vector2 neighborPos in GetNeighbors(currentNode.position))
            {
                if (closedList.Contains(neighborPos)) continue;
                if (IsObstacle(neighborPos)) continue;

                float newGCost = currentNode.gCost + Vector2.Distance(currentNode.position, neighborPos);
                NodePath neighbor = openList.FirstOrDefault(n => n.position == neighborPos);

                if (neighbor == null)
                {
                    neighbor = new NodePath(neighborPos);
                    neighbor.gCost = newGCost;
                    neighbor.hCost = Vector2.Distance(neighborPos, targetPos);
                    neighbor.parent = currentNode;
                    openList.Add(neighbor);
                }
                else if (newGCost < neighbor.gCost)
                {
                    neighbor.gCost = newGCost;
                    neighbor.parent = currentNode;
                }
            }
        }

        return null; // ไม่พบเส้นทาง
    }

    // สร้างโหนดข้างเคียง 8 ทิศทาง
    private List<Vector2> GetNeighbors(Vector2 position)
    {
        List<Vector2> neighbors = new List<Vector2>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                Vector2 neighborPos = position + new Vector2(x * nodeSpacing, y * nodeSpacing);
                neighbors.Add(neighborPos);
            }
        }

        return neighbors;
    }

    // ตรวจสอบสิ่งกีดขวาง
    private bool IsObstacle(Vector2 position)
    {
        Collider2D hit = Physics2D.OverlapCircle(position, obstacleCheckRadius, obstacleLayer);
        return hit != null;
    }

    // สร้างเส้นทางย้อนกลับ
    private List<Vector2> RetracePath(NodePath startNode, NodePath endNode)
    {
        List<Vector2> path = new List<Vector2>();
        NodePath currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    // ลดจำนวนจุดในเส้นทาง
    private List<Vector2> SimplifyPath(List<Vector2> path)
    {
        if (path.Count <= 2) return path;

        List<Vector2> simplified = new List<Vector2> { path[0] };

        for (int i = 1; i < path.Count - 1; i++)
        {
            Vector2 dirToNext = (path[i + 1] - path[i]).normalized;
            Vector2 dirFromPrev = (path[i] - path[i - 1]).normalized;

            if (Vector2.Dot(dirToNext, dirFromPrev) < 0.95f)
            {
                simplified.Add(path[i]);
            }
        }

        simplified.Add(path[path.Count - 1]);
        return simplified;
    }

    // คำนวณระยะทางทั้งหมดของเส้นทาง
    public static float CalculatePathDistance(List<Vector2> path)
    {
        if (path == null || path.Count < 2) return 0f;

        float totalDistance = 0f;
        for (int i = 0; i < path.Count - 1; i++)
        {
            totalDistance += Vector2.Distance(path[i], path[i + 1]);
        }
        return totalDistance;
    }
}
