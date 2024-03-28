using UnityEngine;

public class WelcomeMenu : MonoBehaviour
{
    //[SerializeField] GameObject nextButton;
    [SerializeField] GameObject QuitButton;
    [SerializeField] GameObject SettingButton;
    [SerializeField] GameObject PlayButton;

    private float PreviousValue = 0;
    private void Start()
    {
        //nextButton.SetActive(false);
        //ParentCanvas = GetComponent<Canvas>();

        LeanTween.cancelAll();

        PreviousValue = QuitButton.GetComponent<RectTransform>().localPosition.x;
        LeanTween.moveLocalX(QuitButton, -Screen.width, 0);
        LeanTween.moveLocalX(QuitButton, PreviousValue, 0.8f).setEase(LeanTweenType.easeOutBack);

        PreviousValue = SettingButton.GetComponent<RectTransform>().localPosition.x;
        LeanTween.moveLocalX(SettingButton, -Screen.width, 0);
        LeanTween.moveLocalX(SettingButton, PreviousValue, 0.8f).setEase(LeanTweenType.easeOutBack);

        PreviousValue = PlayButton.GetComponent<RectTransform>().localPosition.y;
        LeanTween.moveLocalY(PlayButton, -Screen.height, 0);
        LeanTween.moveLocalY(PlayButton, PreviousValue, 0.8f).setEase(LeanTweenType.easeOutBack);
    }

    public void playButtonClicked()
    {
        LeanTween.moveX(QuitButton, -Screen.width, 0.8f).setEase(LeanTweenType.easeInBack);
        LeanTween.moveX(SettingButton, -Screen.width, 0.8f).setEase(LeanTweenType.easeInBack);
        LeanTween.moveY(PlayButton, -Screen.height, 0.8f).setEase(LeanTweenType.easeInBack).setOnComplete(
            () =>
            {
                LeanTween.cancelAll();
                HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.ModeSelectionMenu);
                SoundManager.BlendMenuPlayMusic();
                Invoke(nameof(SelfDistroy), .1f);
            }
        );
    }

    private void SelfDistroy()
    {
        HUD_Manager.instance.DestroyMenu(HUD_Manager.MenuNames.WelcomeMenu);
    }

    public void SettingButtonClicked()
    {
        LeanTween.moveX(QuitButton, -Screen.width, 0.8f).setEase(LeanTweenType.easeInBack);
        LeanTween.moveX(SettingButton, -Screen.width, 0.8f).setEase(LeanTweenType.easeInBack);
        LeanTween.moveY(PlayButton, -Screen.height, 0.8f).setEase(LeanTweenType.easeInBack).setOnComplete(
            () =>
            {
                LeanTween.cancelAll();
                HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.SettingMenu);
                Invoke(nameof(SelfDistroy), .1f);
            }
        );
    }
        
    public void ExitButtonClicked()
    {
        LeanTween.moveX(QuitButton, -Screen.width, 0.8f).setEase(LeanTweenType.easeInBack);
        LeanTween.moveX(SettingButton, -Screen.width, 0.8f).setEase(LeanTweenType.easeInBack);
        LeanTween.moveY(PlayButton, -Screen.height, 0.8f).setEase(LeanTweenType.easeInBack).setOnComplete(
            () =>
            {
                LeanTween.cancelAll();
                HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.ExitMenu);
                Invoke(nameof(SelfDistroy), .1f);
            }
        );
    }

    //public void ShowNextButton(float timeToWait)
    //{
    //    Invoke(nameof(showNextButtonLater), timeToWait);
    //}

    //void showNextButtonLater()
    //{
    //    nextButton.SetActive(true);
    //    gameObject.SetActive(true);
    //}
}
