using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    [SerializeField] GameObject DialogueBox, YesButton, NoButton;

    private float PreviousValue = 0;
    void Start()
    {
        Debug.LogWarning("Please Impliment Jerry Sad Reaction");
        PreviousValue = DialogueBox.GetComponent<RectTransform>().localPosition.y;
        LeanTween.moveLocalY(DialogueBox, -Screen.height, 0);
        LeanTween.moveLocalY(DialogueBox, PreviousValue, 0.8f).setEaseOutBack();


        PreviousValue = NoButton.GetComponent<RectTransform>().localPosition.x;
        LeanTween.moveLocalX(NoButton, 0, 0);
        LeanTween.moveLocalX(NoButton, PreviousValue, 1.2f).setEaseOutBack().setDelay(1.2f);


        PreviousValue = YesButton.GetComponent<RectTransform>().localPosition.x;
        LeanTween.scale(YesButton, Vector3.zero, 0);
        LeanTween.scale(YesButton, HUD_Manager.VectorOne, 0.8f).setEaseOutBack().setDelay(2f);
    }

    public void YesButtonClicked()
    {
        LeanTween.cancelAll();
        LeanTween.moveLocalY(DialogueBox, -Screen.height, 0.8f).setEaseInBack().setOnComplete(
            () =>
            {
                Application.Quit();

#if UNITY_EDITOR
                Debug.Log("Game Exit", this);
#endif
            });
    }

    public void NoButtonClicked()
    {
        LeanTween.cancelAll();
        LeanTween.moveLocalY(DialogueBox, -Screen.height, 0.8f).setEaseInBack().setOnComplete(
            () =>
            {
                HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.WelcomeMenu);
                Invoke(nameof(SelfDistroy), .1f);
            });
    }

    private void SelfDistroy()
    {
        HUD_Manager.instance.DestroyMenu(HUD_Manager.MenuNames.ExitMenu);
    }
}
