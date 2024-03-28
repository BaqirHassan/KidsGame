using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Type of a Game. 
/// Note Random Should be assigned -1.
/// </summary>

[SerializeField]
public enum GameType
{
    Random = -1,
    MatchTheAlphabets = 0,
    BallonPoping = 1,
    MatchTheItems = 2,
    MathAddition = 3,
    MathSubtraction = 4,
}

[System.Serializable]
public class AlphabetsPair
{
    public string Name;
    public Sprite Alphabet;
}

[System.Serializable]
public class AlphabetsItemsPairs
{
    public string Name;
    public Sprite Alphabet;
    public List<Sprite> AllMatchingItems;
}

public class Game : MonoBehaviour
{
    /// <summary>
    /// The Game Title for better understanding in Unity's Inspector Windows
    /// </summary>
    public string discription;

    /// <summary>
    /// The Number of Items To match or clicked in order to clear the level
    /// </summary>
    public int nbrOfItemsToMatch;

    /// <summary>
    /// The Game Type of this game
    /// </summary>
    public GameType m_GameType;

    /// <summary>
    /// Is the current game Completed or not.
    /// </summary>
    public bool IsCompleted = false;


    public GameObject FTFHand;

    public void Start()
    {
        HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.PlayMenu);
    }

    /// <summary>
    /// This function is called when game is won. It does necessary task upon game winning. Like Celebration etc.
    /// </summary>
    public void GameFinished()
    {
        IsCompleted = true;
        Celebrate();

        PlayMenu playMenu;
        if(HUD_Manager.instance.CurrentlyInstantiatedMenus[HUD_Manager.MenuNames.PlayMenu]
            .TryGetComponent<PlayMenu>(out playMenu))
        {
            playMenu.SetActiveNextButton(true);
        }

        //HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.ModeSelectionMenu, 2.5f);
        //HUD_Manager.instance.DestroyMenu(HUD_Manager.MenuNames.PlayMenu);

        //Invoke(nameof(DestroyWithDelay), 2.4f);
    }

    private void DestroyWithDelay()
    {
        Destroy(gameObject);
    }

    private void Celebrate()
    {
        VFX_Manager.Instance.ConfittiAtRandomPosition();
        CelebrationManager.Instance.CharacterCelebrating();
        SoundManager.PlaySFX(SoundManager.SFXType.Celebration);
    }
}
