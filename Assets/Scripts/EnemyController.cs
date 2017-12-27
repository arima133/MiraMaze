using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController
    : MonoBehaviour
{

    public  GameController  gameController;
    public  GameObject      playerObject;

    private Quaternion  m_quaNextRotate;
    private Quaternion  m_quaCurRotate;
    private Vector3 m_vecNextPos;
    private Vector3 m_vecCurPos;
    private float   m_fStep;
    private int     m_nLeapCount;
    private int m_nNextActionDelay;
    private int m_nMaxNextActionDelay;
    private int m_nDir;
    private bool    m_bIsTracking;
    private int m_nPlayerX;
    private int m_nPlayerY;

    // Use this for initialization
	void Start ()
    {
        this.m_fStep = 1.0f;

        this.m_vecCurPos    = new Vector3( MazeArray.WidthCount - 2, 0, MazeArray.HeightCount - 2 );
        this.m_vecNextPos   = this.m_vecCurPos;
        this.m_nDir = 2;
        this.m_quaCurRotate = Quaternion.AngleAxis( this.m_nDir * 90, new Vector3( 0, 1, 0 ) );
        this.m_quaNextRotate = this.m_quaCurRotate;

        this.transform.SetPositionAndRotation( this.m_vecCurPos, this.m_quaCurRotate );
        this.m_nLeapCount   = 0;

        this.m_nNextActionDelay = 0;
        this.m_nMaxNextActionDelay = 60;

        this.m_bIsTracking = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if( this.gameController.GameStatus == MazeGameStatus.Playing )
        {
            if( 0 == this.m_nLeapCount )
            {
                if( this.m_nNextActionDelay <= 0 )
                {
                    //Debug.Log( "Enemy Action Start." );

                    //  行動開始
                    Vector3 vecNextOffset;
                    int nX;
                    int nY;
                    int nNewDir;

                    vecNextOffset = Vector3.zero;
                    nNewDir = this.m_nDir;
                    nX = (int)this.transform.position.x;
                    nY = (int)this.transform.position.z;
                    if( this.m_bIsTracking )
                    {
                        //Debug.Log( "Enemy Action tracking." );
                        //  新しい座標に更新
                        this.FindPlayer();
                        if( ( nX == this.m_nPlayerX ) && ( nY == this.m_nPlayerY ) )
                        {
                            //  見失った
                            this.m_bIsTracking = false;
                        }
                        else
                        {
                            //  追う - 向きは変わらない
                            if( nX < this.m_nPlayerX )
                            {
                                vecNextOffset = new Vector3( this.m_fStep, 0, 0 );
                            }
                            else
                            {
                                vecNextOffset = new Vector3( -this.m_fStep, 0, 0 );
                            }
                            if( nY < this.m_nPlayerY )
                            {
                                vecNextOffset = new Vector3( 0, 0, this.m_fStep );
                            }
                            else
                            {
                                vecNextOffset = new Vector3( 0, 0, -this.m_fStep );
                            }
                        }
                    }
                    else
                    {
                        //Debug.Log( "Enemy Action random." );
                        List<Vector3> pVecList;
                        pVecList = new List<Vector3>();

                        //Vector3 vecTemp;
                        int nAx;
                        int nAy;

                        //  前
                        this.GetOffsetFromDir( 0, out nAx, out nAy );
                        if( this.gameController.MazeArray.IsPath( nX + nAx, nY + nAy ) )
                        {
                            pVecList.Add( new Vector3( nAx, 0, nAy ) );
                        }
                        else
                        {
                        }

                        //  右
                        this.GetOffsetFromDir( 1, out nAx, out nAy );
                        if( this.gameController.MazeArray.IsPath( nX + nAx, nY + nAy ) )
                        {
                            pVecList.Add( new Vector3( nAx, 0, nAy ) );
                        }

                        //  左
                        this.GetOffsetFromDir( 3, out nAx, out nAy );
                        if( this.gameController.MazeArray.IsPath( nX + nAx, nY + nAy ) )
                        {
                            pVecList.Add( new Vector3( nAx, 0, nAy ) );
                        }

                        if( 0 < pVecList.Count )
                        {
                            int nListIndex;
                            if( 1 < pVecList.Count )
                            {
                                //  Random.Range で戻りが int のときは末尾の範囲は含まれない。
                                nListIndex = Random.Range( 0, pVecList.Count );
                                //Debug.Log( System.String.Format( "Enemy action vector list index = '{0}'", nListIndex ) );
                            }
                            else
                            {
                                nListIndex = 0;
                                //Debug.Log( System.String.Format( "Enemy action one way." ) );
                            }
                            vecNextOffset = pVecList[ nListIndex ];
                            if( 0 != vecNextOffset.x )
                            {
                                if( 0 < vecNextOffset.x )
                                {
                                    nNewDir = 1;
                                }
                                else
                                {
                                    nNewDir = 3;
                                }
                            }
                            else
                            {
                                if( 0 < vecNextOffset.z )
                                {
                                    nNewDir = 0;
                                }
                                else
                                {
                                    nNewDir = 2;
                                }
                            }
                            nNewDir = nNewDir % 4;
                        }
                        else
                        {
                            //  行き止まり
                            this.GetOffsetFromDir( 2, out nAx, out nAy );
                            vecNextOffset = new Vector3( nAx, 0, nAy );
                            nNewDir = ( this.m_nDir + 2 ) % 4;
                        }
                    }

                    if( ( 0 == vecNextOffset.x ) && ( 0 == vecNextOffset.z ) )
                    {
                        //  動いていない
                        Debug.Log( "Enemy Action next offset is zero." );
                    }
                    else
                    {
                        this.m_vecNextPos = this.transform.position + vecNextOffset;
                        this.m_quaNextRotate = Quaternion.AngleAxis( nNewDir * 90, new Vector3( 0, 1, 0 ) );
                        this.m_nLeapCount = 1;
                        this.m_nDir = nNewDir;
                    }
                }
                else
                {
                    //Debug.Log( "Enemy Action key wait." );
                    this.m_nNextActionDelay--;
                    this.FindPlayer();
                    if( this.m_bIsTracking )
                    {
                        this.m_nNextActionDelay = 0;
                    }
                }
            }
            else
            {
                if( GameStaticParameters.EnemyMaxLeapCount <= this.m_nLeapCount )
                {
                    this.m_nLeapCount = 0;
                    this.m_nNextActionDelay = this.m_nMaxNextActionDelay;
                    this.m_vecCurPos = this.m_vecNextPos;
                    this.m_quaCurRotate = this.m_quaNextRotate;

                    //  プレーヤーと重なった
                    if( ( (int)playerObject.transform.position.x == (int)this.transform.position.x ) &&
						( (int)playerObject.transform.position.z == (int)this.transform.position.z ) )
                    {
                        this.gameController.GotoTimeup();
                    }
                }
                else
                {
                    this.m_nLeapCount++;
                }
            }

            //  次の位置へ補完しながら動く
            float   fLeap;
            //fLeap = Mathf.Min( this.m_nLeapCount / (float)MaxLeapCount, (float)MaxLeapCount );
            fLeap   = Mathf.Sin( ( Mathf.PI / 2.0f ) * ( this.m_nLeapCount / (float)GameStaticParameters.EnemyMaxLeapCount ) );
            Vector3 vecNewPos   = Vector3.Lerp( this.m_vecCurPos, this.m_vecNextPos, fLeap );
            Quaternion  quaNewRotate    = Quaternion.Lerp( this.m_quaCurRotate, this.m_quaNextRotate, fLeap ); 
            this.transform.SetPositionAndRotation( vecNewPos, quaNewRotate );
        }
	}

    private void FindPlayer()
    {
        int nX;
        int nY;
        int nAx;
        int nAy;
        int nPx;
        int nPy;

        nX = (int)this.transform.position.x;
        nY = (int)this.transform.position.z;
        this.GetOffsetFromDir( 0, out nAx, out nAy );
        nPx = (int)playerObject.transform.position.x;
        nPy = (int)playerObject.transform.position.z;
        do
        {
            nX += nAx;
            nY += nAy;

            //  無効な座標は壁として戻ります
            if( gameController.MazeArray.IsWall( nX, nY ) )
            {
                break;
            }
            else
            {
                if( ( nX == nPx ) && ( nY == nPy ) )
                {
                    Debug.Log( System.String.Format( "プレーヤーを発見 x='{0}', y='{1}'", nX, nY ) );
                    //  発見
                    this.m_bIsTracking = true;
                    this.m_nPlayerX = nX;
                    this.m_nPlayerY = nY;
                }
            }
        } while( !this.m_bIsTracking );
    }

    /// <summary>
    /// Gets the offset from dir.
    /// </summary>
    /// <param name="nRelativDir">0:前,1:右,2:後ろ,3:左</param>
    /// <param name="outnAx">Outn ax.</param>
    /// <param name="outnAy">Outn ay.</param>
    private void GetOffsetFromDir( int nRelativDir, out int outnAx, out int outnAy )
    {
        outnAx = 0;
        outnAy = 0;
        if( ( 0 == nRelativDir ) || ( 2 == nRelativDir ) )
        {
            //  現在向いている方向の前へ進むオフセット
            switch( this.m_nDir )
            {
                case 0:
                    outnAy = 1;
                    break;
                case 1:
                    outnAx = 1;
                    break;
                case 2:
                    outnAy = -1;
                    break;
                case 3:
                    outnAx = -1;
                    break;
                default:
                    break;
            }

            if( 2 == nRelativDir )
            {
                outnAx *= -1;
                outnAy *= -1;
            }
        }
        else
        {
            //  現在向いている方向から右に進むオフセット
            switch( this.m_nDir )
            {
                case 0:
                    outnAx = 1;
                    break;
                case 1:
                    outnAy = -1;
                    break;
                case 2:
                    outnAx = -1;
                    break;
                case 3:
                    outnAy = 1;
                    break;
                default:
                    break;
            }

            if( 3 == nRelativDir )
            {
                outnAx *= -1;
                outnAy *= -1;
            }
                   }
    }
}
