using UnityEngine;

public class PrivacyPolicyManager : MonoBehaviour
{
    [SerializeField] private GameObject Parent;
    private void Start()
    {
        if (PlayerPrefs.GetInt("IsPrivacyPolicyDone") != 0)
        {
            Parent.SetActive(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    public void AgreeButtonClicked()
    {
        PlayerPrefs.SetInt("IsPrivacyPolicyDone", 1);
        {
            Parent.SetActive(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}
