using UnityEngine;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] private GameObject NextButton;
    public void PauseButtonClicked()
    {
        LeanTween.cancel(NextButton);
        HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.PauseMenu);
        Invoke(nameof(SelfDistroy), .1f);
    }

    public void SetActiveNextButton(bool state)
    {
        NextButton.SetActive(state);
    }

    public void NextButtonClicked()
    {
        LeanTween.cancelAll();
        SelfDistroy();
        if(Game_Manager.instance.currentGameType == GameType.MathAddition ||
            Game_Manager.instance.currentGameType == GameType.MathSubtraction)
        {
            /// The Index of MathAddition and MathSubtraction in Games <see cref="Game_Manager.Games)"/> Array is 3, 4
            Game_Manager.instance.StartNewGame((GameType)Random.Range(3, 5));
        }
        else
        {
            /// The Index of English Games in Games Games <see cref="Game_Manager.Games)"/> Array is 0, 1, 2
            Game_Manager.instance.StartNewGame((GameType)Random.Range(0, 3));
        }
        //StartCoroutine(HUD_Manager.instance.MoveCameraToNextLevelCO());
    }


    private void SelfDistroy()
    {
        HUD_Manager.instance.DestroyMenu(HUD_Manager.MenuNames.PlayMenu);
    }
}
