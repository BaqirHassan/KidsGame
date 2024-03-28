using System;
using UnityEngine;

public class ModeSelectionMenu : MonoBehaviour
{
    [SerializeField] GameObject Heading, MathsButton, EnglihButton;
    //[SerializeField] CanvasGroup ClearBackground, BlurBackground;
    //[SerializeField] GameObject GameButton1, GameButton2, GameButton3, BackButton;

    float PreviousValue = 0;
    private void Start()
    {
        PreviousValue = Heading.GetComponent<RectTransform>().localPosition.y;
        LeanTween.scale(Heading, Vector3.zero, 0);
        LeanTween.scale(Heading, HUD_Manager.VectorOne, 0.8f).setEaseOutBack();
        LeanTween.moveLocalY(Heading, 0, 0);        
        LeanTween.moveLocalY(Heading, PreviousValue, 0.8f).setEaseOutBack().setDelay(0.8f);


        PreviousValue = MathsButton.GetComponent<RectTransform>().localPosition.x;
        LeanTween.moveLocalX(MathsButton, -Screen.width, 0);
        LeanTween.moveLocalX(MathsButton, PreviousValue, 0.8f).setEaseOutBack().setDelay(0.8f);


        PreviousValue = EnglihButton.GetComponent<RectTransform>().localPosition.x;
        LeanTween.moveLocalX(EnglihButton, Screen.width, 0);
        LeanTween.moveLocalX(EnglihButton, PreviousValue, 0.8f).setEaseOutBack().setDelay(0.8f);
    }

    /// The Index of MathAddition and MathSubtraction in Games <see cref="Game_Manager.Games)"/> Array is 3, 4
    public void StartMatchGame()
    {
        StartNewGame(UnityEngine.Random.Range(3, 5));
    }

    /// The Index of English Games in Games Games <see cref="Game_Manager.Games)"/> Array is 0, 1, 2
    public void StartEnglishGame()
    {
        StartNewGame(UnityEngine.Random.Range(0, 3));
    }


    public void StartNewGame(int game)
    {
        LeanTween.cancelAll();

        LeanTween.scale(Heading, Vector3.zero, 0.8f).setEaseInBack();

        LeanTween.moveLocalX(MathsButton, -Screen.width, 0.8f).setEaseInBack();
        LeanTween.moveLocalX(EnglihButton, Screen.width, 0.8f).setEaseInBack().setOnComplete(
            () =>
            {
                LeanTween.cancel(gameObject);
                if (game < -1 || game >= Enum.GetNames(typeof(GameType)).Length)
                {
                    Debug.LogError("The index is out of range of GameType Enum.");
                    return;
                }

                //This Menu is being instantiated in Game.cs
                //HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.PlayMenu);
                Game_Manager.instance.StartNewGame((GameType)game);
                Invoke(nameof(SelfDistroy), .1f);
            }
        );
    }

    public void BackButtonClicked()
    {
        LeanTween.cancelAll();

        LeanTween.scale(Heading, Vector3.zero, 0.8f).setEaseInBack();

        LeanTween.moveLocalX(MathsButton, -Screen.width, 0.8f).setEaseInBack();
        LeanTween.moveLocalX(EnglihButton, Screen.width, 0.8f).setEaseInBack().setOnComplete(
            () =>
            {
                LeanTween.cancel(gameObject);
                HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.WelcomeMenu);
                Invoke(nameof(SelfDistroy), .1f);
            }
        );
    }

    private void SelfDistroy()
    {
        HUD_Manager.instance.DestroyMenu(HUD_Manager.MenuNames.ModeSelectionMenu);
    }
}
