using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeGameStatus
{
    BeforeStart,
    Playing,
    GameClear,
    TimeUp,
    Pause,
    GameOver,
}

public class GameController
	: MonoBehaviour
{
	public	MazeArray	MazeArray;
    public  UnityEngine.UI.Text scoreText;
    public  GameObject  cubeObject;
    public  GameObject  goalPlaneObject;
    public  GameObject  itemObject1;
    public  GameObject  itemObject2;
    public  GameObject  itemObject3;

    public  GameObject  playerModel1;
    public  GameObject  playerModel2;
    public  GameObject  playerModel3;

    public  GameObject  enemyModel1;
    public  GameObject  enemyModel2;
    public  GameObject  enemyModel3;

    public  GameObject  messagePanel;
    public  UnityEngine.UI.Text     messageText;

    public  UnityEngine.UI.Image    gameClearBoard;
    public  UnityEngine.UI.Image    timeUpBoard;
    public  UnityEngine.UI.Button   backButton;
    public  UnityEngine.UI.Text     timeText;

	public	AudioClip	pickUpItem;

    public  int m_nPoint;

    public  MazeGameStatus  GameStatus
    {
        get
        {
            return( this.m_eGameStatus );
        }
        set
        {
            this.m_eGameStatus = value;
            Debug.Log( System.String.Format( "Change game status : new status is '{0}'.", this.m_eGameStatus ) );
        }
    }

    private  MazeGameStatus  m_eGameStatus;

    private int m_nFrameCounter;
    private int m_nTotalFrame;

	// Use this for initialization
	void Start ()
	{
        GameStatus = MazeGameStatus.BeforeStart;
        this.timeText.gameObject.SetActive( false );

        //  使うキャラクターに
        switch( GameStaticParameters.PlayerModelNo )
        {
            case 0:
                this.playerModel1.SetActive( true );
                break;
            case 1:
                this.playerModel2.SetActive( true );
                break;
            case 2:
                this.playerModel3.SetActive( true );
                break;
            default:
                break;
        }
        //  対戦相手
        switch( GameStaticParameters.EnemyModelNo )
        {
            case 0:
                this.enemyModel1.SetActive( true );
                break;
            case 1:
                this.enemyModel2.SetActive( true );
                break;
            case 2:
                this.enemyModel3.SetActive( true );
                break;
            default:
                break;
        }

        this.messageText.text = GameStaticParameters.GetEnemyMessage( GameStaticParameters.EnemyModelNo );

        //  敵を配置


        this.m_nFrameCounter = 0;
        this.m_nTotalFrame = 60 * 5;

        this.m_nPoint = 0;
		this.MazeArray	= new MazeArray();

        List<Vector3> pPointList;
        pPointList = this.GetDeadEndPointList();

        for( int nCount = 0; nCount < 3; nCount++ )
        {
            Vector3 vecPos;
            int nIndex;
            nIndex = Random.Range( 0, pPointList.Count - 1 );
            vecPos  = pPointList[ nIndex ];
            switch( nCount )
            {
                case 0:
                    this.MazeArray.gameMaze[ (int)vecPos.x, (int)vecPos.z ] = MazeObjectType.Item1;
                    Debug.Log( System.String.Format( "Item1 Location x='{0}', y='{1}'.", vecPos.x, vecPos.z ) );
                    break;
                case 1:
                    this.MazeArray.gameMaze[ (int)vecPos.x, (int)vecPos.z ] = MazeObjectType.Item2;
                    Debug.Log( System.String.Format( "Item2 Location x='{0}', y='{1}'.", vecPos.x, vecPos.z ) );
                    break;
                case 2:
                    this.MazeArray.gameMaze[ (int)vecPos.x, (int)vecPos.z ] = MazeObjectType.Item3;
                    Debug.Log( System.String.Format( "Item3 Location x='{0}', y='{1}'.", vecPos.x, vecPos.z ) );
                    break;
                default:
                    break;
            }
            pPointList.RemoveAt( nIndex );
        }

        this.SetupMazeWalls();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if( null != this.scoreText )
        {
            this.scoreText.text = this.m_nPoint.ToString( "n0" );
        }

        switch( this.GameStatus )
        {
            case MazeGameStatus.BeforeStart:
                if( this.IsFrameCountOver() )
                {
                    this.messagePanel.SetActive( false );
                    this.SetGameStatus( MazeGameStatus.Playing, 60 * 60 );
                    this.timeText.gameObject.SetActive( true );
                }
                break;
            case MazeGameStatus.Playing:
                if( this.IsFrameCountOver() )
                {
                    this.GotoTimeup();
                }
                else
                {
                    this.timeText.text = ( ( this.m_nTotalFrame - this.m_nFrameCounter ) / 60 ).ToString();
                }
                break;
            case MazeGameStatus.GameClear:
                break;
            case MazeGameStatus.TimeUp:
                break;
            case MazeGameStatus.Pause:
                break;
            default:
                break;
        }

		//  アイテムを回転させる??

        //  カウントアップ
        if( 0 < this.m_nTotalFrame )
        {
            this.m_nFrameCounter++;
        }
	}

	public void ExtendTimer()
	{
		this.m_nTotalFrame = this.m_nTotalFrame + ( 60 * 60 );
		Debug.Log( "Extend play." );
	}

    public void GotoTimeup()
    {
        this.SetGameStatus( MazeGameStatus.TimeUp, 0 );
        this.timeUpBoard.gameObject.SetActive( true );
        this.timeText.gameObject.SetActive( false );
        this.backButton.gameObject.SetActive( true );
    }

    public void GotoGameClear()
    {
        this.SetGameStatus( MazeGameStatus.GameClear, 0 );
        this.gameClearBoard.gameObject.SetActive( true );
        this.timeText.gameObject.SetActive( false );
        this.backButton.gameObject.SetActive( true );
    }

    private bool IsFrameCountOver()
    {
        bool bRet;
        bRet = this.m_nTotalFrame <= this.m_nFrameCounter;
        return( bRet );
    }

    private void SetGameStatus( MazeGameStatus eStatus, int nMaxFrameCount )
    {
        this.GameStatus = eStatus;
        this.m_nFrameCounter = 0;
        this.m_nTotalFrame = nMaxFrameCount;
    }

    public void GotoGameover()
    {
    }

    public void DeleteItem( MazeObjectType eType )
    {
        switch( eType )
        {
            case MazeObjectType.Item1:
                if( null != this.itemObject1 )
                {
                    this.m_nPoint += 100;
                    Destroy( this.itemObject1 );
					GetComponent<AudioSource>().PlayOneShot( this.pickUpItem );
				}
                break;
            case MazeObjectType.Item2:
                if( null != this.itemObject2 )
                {
                    this.m_nPoint += 150;
                    Destroy( this.itemObject2 );
					GetComponent<AudioSource>().PlayOneShot( this.pickUpItem );
                }
                break;
            case MazeObjectType.Item3:
                if( null != this.itemObject3 )
                {
                    this.m_nPoint += 200;
                    Destroy( this.itemObject3 );
					GetComponent<AudioSource>().PlayOneShot( this.pickUpItem );
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 行き止まりの場所を調べる
    /// </summary>
    /// <returns>The dead end point list.</returns>
    List<Vector3>   GetDeadEndPointList()
    {
        List<Vector3> pRet;
        pRet = new List<Vector3>();
        for( int nX = 1; nX < ( MazeArray.WidthCount - 1 ); nX++ )
        {
            for( int nY = 1; nY < ( MazeArray.HeightCount - 1 ); nY++ )
            {
                int nCount;
                nCount = 0;
                if( MazeObjectType.None == this.MazeArray.gameMaze[ nX, nY ] )
                {
                    if( MazeObjectType.Wall == this.MazeArray.gameMaze[ nX - 1, nY ] )
                    {
                        nCount++;
                    }
                    if( MazeObjectType.Wall == this.MazeArray.gameMaze[ nX + 1, nY ] )
                    {
                        nCount++;
                    }
                    if( MazeObjectType.Wall == this.MazeArray.gameMaze[ nX, nY - 1] )
                    {
                        nCount++;
                    }
                    if( MazeObjectType.Wall == this.MazeArray.gameMaze[ nX, nY + 1] )
                    {
                        nCount++;
                    }

                    if( 3 <= nCount )
                    {
                        pRet.Add( new Vector3( nX, 0, nY ) );
                    }
                    else if( 4 <= nCount )
                    {
                        //   どうなっているんだ???
                        Debug.Log( "Wall count is 4!!" );
                    }
                }
            }
        }

        return( pRet );
    }

    /// <summary>
    /// 迷路オブジェクトを配置します。
    /// </summary>
    void    SetupMazeWalls()
    {
        for (int x = 0; x < MazeArray.WidthCount; x++)
        {
            for (int z = 0; z < MazeArray.HeightCount; z++)
            {
                //壁じゃない場合のみ倒して終了
                MazeObjectType eType;
                eType = this.MazeArray.gameMaze[ x, z ];
                Vector3 vecPos;
                vecPos = new Vector3( x * 1, 0, z * 1 );
                switch( eType )
                {
                    case    MazeObjectType.Wall:
                        GameObject obj = GameObject.Instantiate( cubeObject );
                        obj.transform.position = vecPos;
                        break;
                    case    MazeObjectType.Goal:
                        if( null != goalPlaneObject )
                        {
                            //  Z バッファーファイティングを避ける
                            vecPos.y = 0.0001f;
                            goalPlaneObject.transform.position = vecPos;
                            Debug.Log( "Goal is setup." );
                        }
                        break;
                    case MazeObjectType.Item1:
                        if( null != itemObject1 )
                        {
                            vecPos.y += 0.5f;
                            this.itemObject1.transform.position = vecPos;
                            Debug.Log( "Item1 is setup." );
                        }
                        break;
                    case MazeObjectType.Item2:
                        if( null != itemObject2 )
                        {
                            vecPos.y += 0.5f;
                            this.itemObject2.transform.position = vecPos;
                            Debug.Log( "Item2 is setup." );
                        }
                       break;
                    case MazeObjectType.Item3:
                        if( null != itemObject3 )
                        {
                            vecPos.y += 0.5f;
                            this.itemObject3.transform.position = vecPos;
                            Debug.Log( "Item3 is setup." );
                        }
                                              break;
                    default:
                        break;
                }
            }
        }
    }
}
