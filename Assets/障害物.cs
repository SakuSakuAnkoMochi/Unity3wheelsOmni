using UnityEngine;

public class 障害物 : MonoBehaviour
{
    public int[,] map = new int[500, 500];
    public GameObject robot;
    public GameObject cube;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int j = 0; j < map.GetLength(0); j++)
        {
            for (int i = 0; i < map.GetLength(1); i++)
            {
                if (i == 0 || j == 0 || i == map.GetLength(1) - 1 || j == map.GetLength(0) - 1)
                    map[j, i] = 1;
                else
                    map[j, i] = 0;
            }
        }
        makeWall((50,50),(100,300));
        makeWall((50,50),(300,100));
        makeStage();
    }
    private void makeWall((int x, int y) leftUp, (int x, int y) rightDown)
    {
        if (leftUp.x < 0 || leftUp.x >= map.GetLength(1) || leftUp.y < 0 || leftUp.y >= map.GetLength(0)) return;
        if (rightDown.x < 0 || rightDown.x >= map.GetLength(1) || rightDown.y < 0 || rightDown.y >= map.GetLength(0)) return;
        if (leftUp.x > rightDown.x || leftUp.y > rightDown.y) return;
        for (int j = leftUp.y; j <= rightDown.y; j++)
        {
            for (int i = leftUp.x; i <= rightDown.x; i++)
            {
                map[j,i]=1;
            }
        }
    }
    private void makeSG((int x, int y) Start, (int x, int y) Goal)
    {
        if (Start.x < 0 || Start.x >= map.GetLength(1) || Start.y < 0 || Start.y >= map.GetLength(0)) return;
        if (Goal.x < 0 || Goal.x >= map.GetLength(1) || Goal.y < 0 || Goal.y >= map.GetLength(0)) return;
        map[Start.y,Start.x]=2;
        map[Goal.y,Goal.x]=3;
    }
    private void makeStage()
    {
        for (int j = 0; j < map.GetLength(0); j++)
        {
            for (int i = 0; i < map.GetLength(1); i++)
            {
                switch(map[j,i])
                {
                    case 1:
                        Instantiate(cube,new Vector3(i-250,1,j-250),Quaternion.identity);
                        break;
                    case 2:
                        //Instantiate(cube,new Vector3(i-250,1,j-250),Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(cube,new Vector3(i-250,1,j-250),Quaternion.identity);
                        break;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            makeSG(((int)robot.transform.position.x+250,(int)robot.transform.position.z+250),(400,300));
        }
    }
}
