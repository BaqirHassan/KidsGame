using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject DialogueBox, Restart, Resume, Home;

    private float PreviousValue = 0;
    void Start()
    {
        LeanTween.cancel(Restart);
        LeanTween.cancel(Resume);
        LeanTween.cancel(Home);
        LeanTween.cancel(DialogueBox);
        PreviousValue = DialogueBox.GetComponent<RectTransform>().localPosition.y;
        LeanTween.moveLocalY(DialogueBox, -Screen.height, 0);
        LeanTween.moveLocalY(DialogueBox, PreviousValue, 0.8f).setEase(LeanTweenType.easeInSine);

        LeanTween.moveLocalX(Resume, -Screen.width, 0);
        LeanTween.moveLocalX(Resume, 0, 0.8f).setEase(LeanTweenType.easeOutBack).setDelay(0.8f);

        LeanTween.moveLocalX(Restart, -Screen.width, 0);
        LeanTween.moveLocalX(Restart, 0, 1f).setEase(LeanTweenType.easeOutBack).setDelay(0.8f);

        LeanTween.moveLocalX(Home, -Screen.width, 0);
        LeanTween.moveLocalX(Home, 0, 1.2f).setEase(LeanTweenType.easeOutBack).setDelay(0.8f).setOnComplete(
            () =>
            {
                Time.timeScale = 0;
            });
    }


    public void HomeButtonClicked()
    {
        LeanTween.cancel(Restart);
        LeanTween.cancel(Resume);
        LeanTween.cancel(Home);
        LeanTween.cancel(DialogueBox);
        Time.timeScale = 1;

        Resume.LeanMoveLocalX(Screen.width, 0.8f).setEase(LeanTweenType.easeInBack);
        Restart.LeanMoveLocalX(Screen.width, 1f).setEase(LeanTweenType.easeInBack);
        Home.LeanMoveLocalX(Screen.width, 1.2f).setEase(LeanTweenType.easeInBack).setOnComplete(
            () =>
            {
                HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.WelcomeMenu);
                Game_Manager.instance.EndCurrentGame();
                Invoke(nameof(SelfDistroy), .1f);
            });
    }

    public void RestartButtonClicked()
    {
        LeanTween.cancel(Restart);
        LeanTween.cancel(Resume);
        LeanTween.cancel(Home);
        LeanTween.cancel(DialogueBox);
        Time.timeScale = 1;

        Resume.LeanMoveLocalX(Screen.width, 0.8f).setEase(LeanTweenType.easeInBack);
        Home.LeanMoveLocalX(Screen.width, 1.2f).setEase(LeanTweenType.easeInBack);
        Restart.LeanMoveLocalX(Screen.width, 1f).setEase(LeanTweenType.easeInBack).setOnComplete(
            () =>
            {
                //HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.PlayMenu);
                Game_Manager.instance.StartNewGame(Game_Manager.instance.currentGameType);
                Invoke(nameof(SelfDistroy), .1f);
            });
    }

    public void ResumeButtonClicked()
    {
        LeanTween.cancel(Restart);
        LeanTween.cancel(Resume);
        LeanTween.cancel(Home);
        LeanTween.cancel(DialogueBox);
        Time.timeScale = 1;

        Restart.LeanMoveLocalX(Screen.width, 1f).setEase(LeanTweenType.easeInBack);
        Resume.LeanMoveLocalX(Screen.width, 0.8f).setEase(LeanTweenType.easeInBack).setOnComplete(
            () =>
            {
                HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.PlayMenu);
                Invoke(nameof(SelfDistroy), .1f);
            });
        Home.LeanMoveLocalX(Screen.width, 1.2f).setEase(LeanTweenType.easeInBack);
    }

    private void SelfDistroy()
    {
        HUD_Manager.instance.DestroyMenu(HUD_Manager.MenuNames.PauseMenu);
    }
}
