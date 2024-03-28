using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    ///// <summary>
    ///// The Type of game that needed to be played. It's different then GameType defined in Game Script.
    ///// </summary>
    //enum GameType
    //{
    //    Match
    //}
    [SerializeField] Game[] Games;
    [SerializeField] bool doKeepAspectRatio = true;

    public static Game_Manager instance;

    private Game currentGame;
    public GameType currentGameType
    {
        get
        {
            return currentGame.m_GameType;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    void Start()
    {
        SoundManager.PlayMenuMusic();
        HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.WelcomeMenu);
    }

    // This function needs to be Serialized in unityEvent.onClick()
    public void StartNewGame(GameType gameType)
    {
        if(Games.Length == 0)
        {
#if SHOW_IMPORTANT_LOGS
            Debug.LogError("[Game_Manager] There is No Game Assigned in Game Manager", this);
#endif
            return;
        }

        int GameIndex = gameType == GameType.Random? Random.Range(0, Games.Length) : (int)gameType;

        EndCurrentGame();

        currentGame = Instantiate(
            Games[GameIndex], 
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10)),
            Quaternion.identity
            );
    }

    public void EndCurrentGame()
    {
        if (currentGame != null)
        {
            Destroy(currentGame.gameObject);
            HUD_Manager.instance.DestroyMenu(HUD_Manager.MenuNames.PlayMenu);
        }
#if SHOW_DETAIL_LOGS
        else Debug.Log("No Game is currently being Played", this);
#endif
    }

    #region MISC
    public void ScaleContainer( GameObject itemTOScale)
    {
        Vector3 NewScale = new Vector3(Screen.width/1280f, Screen.height/800f, 1f);
        
        if(doKeepAspectRatio)
        {
            float xScale = Screen.width/1280f;
            float yScale = Screen.height/800f;
            if(xScale > yScale)
            {
                yScale = xScale;
            }
            else
            {
                xScale = yScale;
            }
            NewScale = new Vector3(xScale, yScale, 1f);
        }
        else
        {
            if(NewScale.x > 1.75f)      NewScale.x = 1.75f;
            else if(NewScale.x < .5f)   NewScale.x = .5f;

            if(NewScale.y > 1.75f)      NewScale.y = 1.75f;
            else if(NewScale.y < .5f)   NewScale.y = .5f;


            if(Mathf.Abs(NewScale.x - NewScale.y) > 0.5f)
            {

                
                if(NewScale.x > 1.75f)      NewScale.x = 1.75f;
                else if(NewScale.x < .5f)   NewScale.x = .5f;

                if(NewScale.y > 1.75f)      NewScale.y = 1.75f;
                else if(NewScale.y < .5f)   NewScale.y = .5f;


                if(NewScale.x > NewScale.y)
                {
                    NewScale.x -= (NewScale.x - NewScale.y)/2;
                    NewScale.y += (NewScale.x - NewScale.y)/2;
                }
                else
                {
                    NewScale.x += (NewScale.y - NewScale.x)/2;
                    NewScale.y -= (NewScale.y - NewScale.x)/2;
                }
            }
        }

        itemTOScale.transform.localScale = NewScale;
    }

    public static bool IsMatchingFTF()
    {
        return PlayerPrefs.GetInt("MatchingFirstTimeFlow", 0) == 1;
    }

    public static bool IsPoppingFTF()
    {
        return PlayerPrefs.GetInt("PoppingFirstTimeFlow", 0) == 1;
    }

    public static void MatchingFTFDone()
    {
        PlayerPrefs.SetInt("MatchingFirstTimeFlow", 1);
    }

    public static void PoppingFTFDone()
    {
        PlayerPrefs.SetInt("PoppingFirstTimeFlow", 1);
    }

    #endregion
}
