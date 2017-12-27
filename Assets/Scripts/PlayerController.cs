using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
	: MonoBehaviour {

	public	GameController	gameController;

	private	bool	m_bIsDown;
	private	bool	m_bIsUp;
	private	bool	m_bIsLeft;
	private	bool	m_bIsRight;
	private	Quaternion	m_quaNextRotate;
	private	Quaternion	m_quaCurRotate;
	private	Vector3 m_vecNextPos;
	private	Vector3	m_vecCurPos;
	private	float	m_fStep;
	private int		m_nLeapCount;
	private	int	m_nMaxLeapCount;

	// Use this for initialization
	void Start ()
	{
		this.m_fStep = 1.0f;

        this.m_vecCurPos	= new Vector3( 1, 0, 0 );
		this.m_vecNextPos	= this.m_vecCurPos;
		this.m_quaCurRotate	= Quaternion.identity;
		this.m_quaNextRotate = this.m_quaCurRotate;

		this.transform.SetPositionAndRotation( this.m_vecCurPos, this.m_quaCurRotate );
        this.m_nMaxLeapCount = GameStaticParameters.PlayerMaxLeapCount;
		this.m_nLeapCount	= 0;
	}

	void Update()
	{
		Vector3		vec;
		Vector3		vecRotate;
		int nDir;
		vec = new Vector3( 0, 0, 0 );
		vecRotate = new Vector3( 0, 1, 0 );
		nDir	= 0;

        if( gameController.GameStatus == MazeGameStatus.Playing )
        {
    		if( 0 == this.m_nLeapCount )
    		{
    			if( Input.GetKeyDown( KeyCode.LeftArrow ) )
    			{
    				vec.x = -this.m_fStep;
    				nDir = 270;
    			}
    			if( Input.GetKeyDown( KeyCode.RightArrow ) )
    			{
    				vec.x = this.m_fStep;
    				nDir = 90;
    			}

    			if( Input.GetKeyDown( KeyCode.UpArrow ) )
    			{
    				vec.z = this.m_fStep;
    				nDir = 0;
    			}
    			if( Input.GetKeyDown( KeyCode.DownArrow ) )
    			{
    				vec.z = -this.m_fStep;
    				nDir = 180;
    			}

    			//	vec で次の位置は壁ではない
    			if( ( vec.x != 0 ) || ( vec.z != 0 ) )
    			{
    				Vector3	vecNext;
    				vecNext = this.m_vecCurPos + vec;
    				if( !gameController.MazeArray.IsWall( (int)vecNext.x, (int)vecNext.z ) )
    				{
    					this.m_quaNextRotate	= Quaternion.AngleAxis( nDir, vecRotate );
    					this.m_vecNextPos += vec;
    					this.m_nLeapCount	= 1;
                        Debug.Log( System.String.Format( "Miratan new location x='{0}', y='{1}'.", this.m_vecNextPos.x, this.m_vecNextPos.z ) );
    				}
    				else
    				{
                        Debug.Log( System.String.Format( "{0:HH:mm:ss} : Next Is Wall.Can't move.", System.DateTime.Now ) );
    				}
    			}
    		}
    		else
    		{
                if( this.m_nLeapCount < this.m_nMaxLeapCount )
    			{
    				this.m_nLeapCount++;
    			}
    			else
    			{
    				this.m_vecCurPos	= this.m_vecNextPos;
    				this.m_quaCurRotate	= this.m_quaNextRotate;
    				this.m_nLeapCount = 0;
                    int nCurX;
                    int nCurY;
                    nCurX = (int)this.m_vecCurPos.x;
                    nCurY = (int)this.m_vecCurPos.z;
    				//	ゴールについた?
                    if( gameController.MazeArray.IsGoal( nCurX, nCurY ) )
                    {
                        gameController.GotoGameClear();
                    }
                    else
                    {
                        MazeObjectType eType;
                        eType = gameController.MazeArray.gameMaze[ nCurX, nCurY ];
                        switch( eType )
                        {
                            case MazeObjectType.Item1:
                                this.m_nMaxLeapCount = 15;
                                gameController.DeleteItem( eType );
                                break;
							case MazeObjectType.Item2:
								GameStaticParameters.EnemyMaxLeapCount += ( GameStaticParameters.EnemyMaxLeapCount / 2 );
                                gameController.DeleteItem( eType );
                                break;
							case MazeObjectType.Item3:
								gameController.ExtendTimer();
                                gameController.DeleteItem( eType );
                                break;
                            default:
                                break;
                        }
                    }
    			}
    		}

    		//	次の位置へ補完しながら動く
    		float	fLeap;
    		//fLeap	= Mathf.Min( this.m_nLeapCount / (float)MaxLeapCount, (float)MaxLeapCount );
            fLeap	= Mathf.Sin( ( Mathf.PI / 2.0f ) * ( this.m_nLeapCount / (float)this.m_nMaxLeapCount ) );
    		Vector3	vecNewPos	= Vector3.Lerp( this.m_vecCurPos, this.m_vecNextPos, fLeap );
    		Quaternion	quaNewRotate	= Quaternion.Lerp( this.m_quaCurRotate, this.m_quaNextRotate, fLeap ); 
    		this.transform.SetPositionAndRotation( vecNewPos, quaNewRotate );
        }
	}

	void FixedUpdate ()
	{
	}


}
