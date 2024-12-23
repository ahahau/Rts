using UnityEngine;

public class ANode 
{
    public bool isWalkAble;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;
    
    public int gCost; // 시작 노드에서 현재 노드까지의 실제 경로 비용
    public int hCost; // 현재 노드에서 목표 노드까지의 추정 비용
    public ANode parentNode;

    public ANode(bool nWalkAble, Vector3 nWorldPos,int nGridX,int nGridY)
    {
        isWalkAble = nWalkAble;
        worldPos = nWorldPos;
        gridX = nGridX;
        gridY = nGridY;
    }

    public int fCost //전체 비용
    {
        get{return gCost + hCost;}
    }
}
