using UnityEngine;

public  enum MazeObjectType
{
    None,
    Path = None,
    Wall,
    Goal,
    Item1,
    Item2,
    Item3,
}

public class MazeArray
{
    public 	const	int	WidthCount	= 21;
    public	const	int	HeightCount	= 21;

    private	MazeObjectType[,]	m_pKabe;

    public MazeObjectType[,] gameMaze
    {
        get
        {
            return( this.m_pKabe );
        }
    }

	public MazeArray()
	{
        this.m_pKabe = this.GenerateMaze( WidthCount, HeightCount );
		//	for で初期化?
	}

    private bool IsValidLocation( int x, int y )
    {
        bool bRet;
        bRet = ( 0 <= x ) && ( x < WidthCount ) && ( 0 <= y ) && ( y < HeightCount );
        if( !bRet )
        {
            Debug.Log( System.String.Format( "Invalid Location x='{0}', Y='{1}'.", x, y ) );
        }

        return( bRet );
    }

    public bool IsPath( int x, int y )
    {
        bool bRet;
        bRet = false;

        if( this.IsValidLocation( x, y ) )
        {
            bRet = ( MazeObjectType.Path == m_pKabe[ x, y ] );
        }

        return( bRet );
    }

	public bool	IsWall( int x, int y )
	{
        bool bRet;

        if( this.IsValidLocation( x, y ) )
        {
            bRet = ( MazeObjectType.Wall == m_pKabe[ x, y ] );
        }
        else
        {
            bRet = true;
        }

        return( bRet );
	}

	public void SetWall( int x, int	y, bool isWall )
	{
        if( this.IsValidLocation( x, y ) )
        {
            this.m_pKabe[ x, y ] = ( isWall ? MazeObjectType.Wall : MazeObjectType.None );
        }
	}

    public void SetGoal( int x, int y, bool isWall )
    {
        if( this.IsValidLocation( x, y ) )
        {
            this.m_pKabe[ x, y ] = ( isWall ? MazeObjectType.Goal : MazeObjectType.None );
        }
    }

	public bool	IsGoal( int x, int y )
	{
        bool bRet;
        bRet = false;

        if( this.IsValidLocation( x, y ) )
        {
            bRet = ( MazeObjectType.Goal == m_pKabe[ x, y ] );
        }

        return( bRet );
	}

    //棒倒し法による迷路生成
    public MazeObjectType[,] GenerateMaze(int width, int height)
    {

        //5未満のサイズでは生成できない
        ///if (height < 5 || width <5) throw new ArgumentOutOfRangeException();
        if (width % 2 == 0) width++;
        if (height % 2 == 0) height++;

        //指定サイズで生成し外周を壁にする
        var maze = new MazeObjectType[width, height];
        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
                if (x == 0 || z == 0 || x == width - 1 || z == height - 1)
                    maze[x, z] = MazeObjectType.Wall;//外周はすべて壁
                else
                    maze[x, z] = MazeObjectType.Path;//外周以外は通路

        //棒を立て、倒す
        //var rnd = GameStaticParameters.Random;
        for (int x = 2; x < width - 1; x += 2)
        {
            for (int z = 2; z < height - 1; z += 2)
            {
                maze[x, z] = MazeObjectType.Wall;//棒を立てる

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
                    if (maze[wallX, wallZ] != MazeObjectType.Wall)
                    {
                        maze[wallX, wallZ] = MazeObjectType.Wall;
                        break;
                    }
                }
            }
        }

        //  スタート位置
        maze[ 1, 0 ]    = MazeObjectType.Path;

        //  ゴール
        maze[ 1, height - 1 ] = MazeObjectType.Goal;

        return maze;
    }
}


