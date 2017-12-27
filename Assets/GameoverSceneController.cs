using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverSceneController : MonoBehaviour {
	public 	GameObject	playerObject;
	public	GameObject	enemyObject;

	public	GameObject	playerModel1;
	public	GameObject	playerModel2;
	public	GameObject	playerModel3;
	public	GameObject	enemyModel1;
	public	GameObject	enemyModel2;
	public	GameObject	enemyModel3;

	private	int	m_nFrame;
	private	int	m_nScene;

	void Start()
	{
		switch( GameStaticParameters.PlayerModelNo )
		{
			case 0:
				playerModel1.SetActive( true );
				break;
			case 1:
				playerModel2.SetActive( true );
				break;
			case 2:
				playerModel3.SetActive( true );
				break;
			default:
				break;
		}
		switch( GameStaticParameters.EnemyModelNo )
		{
			case 0:
				enemyModel1.SetActive( true );
				break;
			case 1:
				enemyModel2.SetActive( true );
				break;
			case 2:
				enemyModel3.SetActive( true );
				break;
			default:
				break;
		}

		this.m_nScene = 0;
		this.m_nFrame = 0;
	}

	void Update()
	{
		GameObject	pCamera	= GameObject.FindGameObjectWithTag( "MainCamera" );

		if( 0 == this.m_nScene )
		{
			Vector3	vecCameraBegin	= new Vector3( -0.5f, 0.1f, -3.0f );
			Vector3	vecCameraEnd	= new Vector3( -0.3f, 0.1f, -3.0f );

			//	カメラは 120
			pCamera.transform.position = Vector3.Lerp( vecCameraBegin, vecCameraEnd, this.m_nFrame / 120.0f );
			pCamera.transform.rotation = Quaternion.Euler( new Vector3( -10, 0, 0 ) );

			this.CheckNextScene( 120 );
		}
		else
		{
		}

		this.m_nFrame++;
	}

	private	void	NextScene()
	{
		this.m_nScene++;
		this.m_nFrame = -1;
	}
				
	private	void	CheckNextScene( int nFrame )
	{
		if( nFrame <= this.m_nFrame )
		{
			this.NextScene();
		}
	}
}
