using UnityEngine;

public class GameStaticParameters
{
    public  const   int DefaultEnemy1MaxLeapCount = 40;
    public  const   int DefaultEnemy2MaxLeapCount = 30;
    public  const   int DefaultEnemy3MaxLeapCount = 20;

    static  public  Random Random = new Random();

    static  public  int PlayerModelNo   = 0;
    static  public  int EnemyModelNo    = 0;

    static  public  int PlayerMaxLeapCount  = 30;
    static  public  int EnemyMaxLeapCount   = 30;

    static public string GetEnemyMessage( int nNo )
    {
        System.Text.StringBuilder psbText;

        psbText = new System.Text.StringBuilder( 1024 );
        switch( nNo )
        {
            case 0:
                psbText.AppendLine( "'NANTOくん' からのおたより" );
                psbText.AppendLine( "「一緒に遊ぼう!」" );
                psbText.AppendLine();
                psbText.AppendLine( "Message from 'NANTO'" );
                psbText.AppendLine( "\"You can't go any further.\"" );
                               break;
            case 1:
                psbText.AppendLine( "'キラリン' からのおたより" );
                psbText.AppendLine( "「仲良くしましょう」" );
                psbText.AppendLine();
                psbText.AppendLine( "Message from 'Kirarin'" );
                psbText.AppendLine( "\"I'll bash you down.\"" );
                break;
            case 2:
                psbText.AppendLine( "'ジャンボ～ル三世' からのおたより" );
                psbText.AppendLine( "「頑張りましょう」" );
                psbText.AppendLine();
                psbText.AppendLine( "Message from 'Jumball 3rd'" );
                psbText.AppendLine( "\"You shall never return alive.\"" );
                break;
        }

        return( psbText.ToString() );
    }

    static public void ResetAll()
    {
        PlayerModelNo = 0;
        EnemyModelNo = 0;
    }

    static public void ResetGameMaze( int nNo )
    {
        PlayerMaxLeapCount = 30;
        switch( nNo )
        {
            case 0:
                EnemyMaxLeapCount = DefaultEnemy1MaxLeapCount;
                break;
            case 1:
                EnemyMaxLeapCount = DefaultEnemy2MaxLeapCount;
                break;
            case 2:
                EnemyMaxLeapCount = DefaultEnemy3MaxLeapCount;
                break;
            default:
                break;
        }
    }
}

