using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject SoundTick, SoundCross;
    [SerializeField] GameObject MusicTick, MusicCross;


    [SerializeField] GameObject Heading, DialogueBox, preferanceSaved, BackButton;

    private float PreviousValue = 0;
    void Start()
    {
        LeanTween.cancelAll();

        SoundTick.SetActive(GameConstantsAndData.SoundPreferance);
        SoundCross.SetActive(!GameConstantsAndData.SoundPreferance);

        MusicTick.SetActive(GameConstantsAndData.MusicPreferance);
        MusicCross.SetActive(!GameConstantsAndData.MusicPreferance);

        LeanTween.scale(Heading, Vector3.zero, 0);
        LeanTween.scale(Heading, HUD_Manager.VectorOne, 0.8f).setEase(LeanTweenType.easeOutBack);

        PreviousValue = Heading.GetComponent<RectTransform>().localPosition.y;
        LeanTween.moveLocalY(Heading, 0, 0);
        LeanTween.moveLocalY(Heading, PreviousValue, 0.8f).setEase(LeanTweenType.easeInSine).setDelay(0.8f);

        PreviousValue = DialogueBox.GetComponent<RectTransform>().localPosition.y;
        LeanTween.moveLocalY(DialogueBox, -Screen.height, 0);
        LeanTween.moveLocalY(DialogueBox, PreviousValue, 0.8f).setEase(LeanTweenType.easeInSine).setDelay(0.8f);

        PreviousValue = BackButton.GetComponent<RectTransform>().localPosition.x;
        LeanTween.moveLocalX(BackButton, -Screen.width, 0);
        LeanTween.moveLocalX(BackButton, PreviousValue, 0.8f).setEase(LeanTweenType.easeOutBack);
        
        preferanceSaved.LeanMoveLocalX(-Screen.width, 0);
    }

    public void ToggleSoundPreferance()
    {
        GameConstantsAndData.SoundPreferance = !GameConstantsAndData.SoundPreferance;

        SoundTick.SetActive(GameConstantsAndData.SoundPreferance);
        SoundCross.SetActive(!GameConstantsAndData.SoundPreferance);
        PreferanceSavedAcrossScreen();
    }

    public void ToggleMusicPreferance()
    {
        GameConstantsAndData.MusicPreferance = !GameConstantsAndData.MusicPreferance;

        MusicTick.SetActive(GameConstantsAndData.MusicPreferance);
        MusicCross.SetActive(!GameConstantsAndData.MusicPreferance);
        PreferanceSavedAcrossScreen();
    }

    private void PreferanceSavedAcrossScreen()
    {
        LeanTween.cancelAll();
        preferanceSaved.LeanMoveLocalX(-Screen.width, 0);
        
        preferanceSaved.LeanMoveLocalX(0, 0.8f).setEase(LeanTweenType.easeOutBack).setOnComplete(
            () =>
            {
                preferanceSaved.LeanMoveLocalX(Screen.width, 0.8f).setEase(LeanTweenType.easeInBack).setDelay(0.5f);
            });
    }

    public void BackButtonClicked()
    {
        LeanTween.cancelAll();

        Heading.LeanScale(HUD_Manager.VectorOne, 0);
        DialogueBox.LeanMoveLocalY(0, 0);
        preferanceSaved.LeanMoveLocalX(Screen.width, 0);

        Heading.LeanScale(Vector3.zero, 0.8f).setEase(LeanTweenType.easeInBack);
        DialogueBox.LeanMoveLocalY(-Screen.height, 0.8f).setEase(LeanTweenType.easeInBack).setOnComplete
            ( () =>
            {
                Heading.LeanCancel();
                DialogueBox.LeanCancel();
                preferanceSaved.LeanCancel();
                HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.WelcomeMenu);
                Invoke(nameof(SelfDistroy), .1f);
            });
        BackButton.LeanMoveLocalX(-Screen.width, 0.8f).setEase(LeanTweenType.easeInBack);
    }

    private void SelfDistroy()
    {
        HUD_Manager.instance.DestroyMenu(HUD_Manager.MenuNames.SettingMenu);
    }
}
