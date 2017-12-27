using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript
	: MonoBehaviour
{
	public void MoveSelectRivalSceneButton_Click()
	{
        GameStaticParameters.ResetAll();
		SceneManager.LoadScene( "SelectRival" );
	}

    public void GotoSelectRaivalSceneButton_Click()
    {
        SceneManager.LoadScene( "SelectRival" );
    }

	public void StartNantoButton_Click()
	{
        GameStaticParameters.EnemyModelNo = 0;
		//	for debug
		GameStaticParameters.PlayerModelNo = 0;
        GameStaticParameters.ResetGameMaze( GameStaticParameters.EnemyModelNo );
		SceneManager.LoadScene( "PlayMaze" );
	}

    public void StartKirarinButton_Click()
    {
        GameStaticParameters.EnemyModelNo = 1;
		//	for debug
		GameStaticParameters.PlayerModelNo = 1;
        GameStaticParameters.ResetGameMaze( GameStaticParameters.EnemyModelNo );
        SceneManager.LoadScene( "PlayMaze" );
    }

    public void StartJumballButton_Click()
    {
        GameStaticParameters.EnemyModelNo = 2;
		//	for debug
		GameStaticParameters.PlayerModelNo = 2;
        GameStaticParameters.ResetGameMaze( GameStaticParameters.EnemyModelNo );
        SceneManager.LoadScene( "PlayMaze" );
    }

    public void BackToTitleButton_Click()
    {
        GameStaticParameters.ResetGameMaze( GameStaticParameters.EnemyModelNo );
        SceneManager.LoadScene( "Title" );
    }
}
