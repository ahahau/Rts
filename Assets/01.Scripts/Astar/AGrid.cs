using System;
using System.Collections.Generic;
using UnityEngine;

public class AGrid : MonoBehaviour
{
    public LayerMask unwalkableMask; // 이동 불가 
    public Vector2 gridWorldSize;  // 맵 전체 사이즈
    public float nodeRadius; // 반지름
    ANode[,] grid;
    
    float nodeDiameter; // 지름
    private int gridSizeX; //그리드에 X크기
    private int gridSizeY; //그리드에 Y크기 

    private void Start()
    {
        nodeDiameter = nodeRadius * 2; // 받은반지름을 지름으로 바꿈
        gridSizeX = Mathf.RoundToInt/*소수점 첫번째 자리에서 반올림, int로 반환*/(gridWorldSize.x / nodeDiameter); // 그리드의 가로 크기
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter); // 그리드의 세로 크기
        CreateGrid();
    }

    private void CreateGrid()//그리드 생성 함수
    {
        grid = new ANode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        Vector3 worldPoint;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x,y] = new ANode(walkable, worldPoint,x,y);
            }
        }
    }
    //자기 주변 노드를 찾는 함수
    public List<ANode> GetNeighbours(ANode node)
    {
        List<ANode> neighbours = new List<ANode>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0) continue; //자신일 경우 스킵
                
                int checkX = node.gridX + x;
                int checkY = node.gridY + y; 
                
                // X,Y의 값이 Grid범위안에 있을 경우 
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    //반환 리스트에 넣어주기
                    neighbours.Add(grid[checkX, checkY]); 
                }
            }
        }
        return neighbours;
    }
    //WorldPosition안에 그리드 상의 노드를 찾는 함수
    public ANode GetNodeFromWorldPoint(Vector3 worldPoint)
    {
        float percentX = (worldPoint.x / gridWorldSize.x) * nodeDiameter;
        float percentY = (worldPoint.y / gridWorldSize.y) * nodeDiameter;
        percentX = Mathf.Clamp01(percentX); // -일경우 -1로 +일경우 +1로
        percentY = Mathf.Clamp01(percentY);
        
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y)); 
        if (grid != null)
        {
            foreach (ANode n in grid) // 격자 기즈모 생성
            {
                Gizmos.color = (n.isWalkAble) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
