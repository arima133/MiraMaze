using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meiro : MonoBehaviour
{
    public GameObject cubeObject;

   private int[,] gameMaze = null;

    // Use this for initialization
    void Start()
    {

        GenerateMaze(21, 21);

        gameMaze = GenerateMaze(21, 21);
        DebugPrint(gameMaze);

        for (int x = 0; x < 21; x++)
        {
            for (int z = 0; z < 21; z++)
            {
                //壁じゃない場合のみ倒して終了
                if (gameMaze[x, z] == Wall)
                {
                    GameObject obj = GameObject.Instantiate(cubeObject);

                    obj.transform.position = new Vector3(x * 1, 0, z * 1);

                }
            }
        }

    }
    //通路・壁
    const int Path = 0;
    const int Wall = 1;

    //棒倒し法による迷路生成
    public int[,] GenerateMaze(int width, int height)
    {

        //5未満のサイズでは生成できない
        ///if (height < 5 || width <5) throw new ArgumentOutOfRangeException();
        if (width % 2 == 0) width++;
        if (height % 2 == 0) height++;

        //指定サイズで生成し外周を壁にする
        var maze = new int[width, height];
        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
                if (x == 0 || z == 0 || x == width - 1 || z == height - 1)
                    maze[x, z] = Wall;//外周はすべて壁
                else
                    maze[x, z] = Path;//外周以外は通路

        //棒を立て、倒す
        var rnd = new Random();
        for (int x = 2; x < width - 1; x += 2)
        {
            for (int z = 2; z < height - 1; z += 2)
            {
                maze[x, z] = Wall;//棒を立てる

                //倒せるまで繰り返す
                while (true)
                {

                    //1行目のみ上に倒せる
                    int direction;
                    if (z == 2)
                        direction = Mathf.RoundToInt(Random.Range(1, 4)); //rnd.Next(4);
                    else
                        direction = Mathf.RoundToInt(Random.Range(1, 3));// rnd.Next(3);

                    //棒を倒す方向を決める
                    int wallX = x;
                    int wallZ = z;
                    switch (direction)
                    {
                        case 0://右
                            wallX++;
                            break;
                        case 1://下
                            wallZ++;
                            break;
                        case 2://左
                            wallX--;
                            break;

                    }
                    //壁じゃない場合のみ倒して終了
                    if (maze[wallX, wallZ] != Wall)
                    {
                        maze[wallX, wallZ] = Wall;
                        break;
                    }
                }
            }
        }
        return maze;
    }

    //デバック用メソッド
    public static void DebugPrint(int[,] maze)
    {
        Debug.Log("Width:" + maze.GetLength(0));
        Debug.Log("Height:" + maze.GetLength(1));



        for (int z = 0; z < maze.GetLength(1); z++)
        {
            string tempString = "";
            for (int x = 0; x < maze.GetLength(0); x++)
            {
                tempString += (maze[x, z] == Wall ? "*" : " ");
            }
            Debug.Log(tempString);
            //Console.WriteLine ();
        }
    }



    // Update is called once per frame
    void Update()
    {

    }
}
