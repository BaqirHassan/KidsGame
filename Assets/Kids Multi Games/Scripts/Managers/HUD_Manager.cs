using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Manager : MonoBehaviour
{

    //HUD_Manager.instance.InstantiateMenu(HUD_Manager.MenuNames.ModeSelectionMenu, 2.5f);
    public enum MenuNames
    {
        None,
        WelcomeMenu,
        ModeSelectionMenu,
        SettingMenu,
        PauseMenu,
        PlayMenu,
        ExitMenu
    }
    public static Dictionary<MenuNames, string> m_MenuNames = new Dictionary<MenuNames, string>
    {
        [MenuNames.WelcomeMenu] = "Welcome Menu",
        [MenuNames.ModeSelectionMenu] = "Mode Selection Menu",
        [MenuNames.SettingMenu] = "Setting Menu",
        [MenuNames.PauseMenu] = "Pause Menu",
        [MenuNames.PlayMenu] = "Play Menu",
        [MenuNames.ExitMenu] = "Exit Menu",
        [MenuNames.ExitMenu] = "Exit Menu",
        [MenuNames.ExitMenu] = "Exit Menu",
        [MenuNames.ExitMenu] = "Exit Menu",
        [MenuNames.ExitMenu] = "Exit Menu",
        [MenuNames.ExitMenu] = "Exit Menu",
    };

    public static Vector3 VectorOne = new Vector3(1, 1, 1);

    public static MenuNames PreviousMenu{   get;    private set;    }

    public static MenuNames CurrentMenu{    get;    private set;    }


    private Transform m_Camera;

    public  Dictionary<MenuNames, GameObject> CurrentlyInstantiatedMenus = new Dictionary<MenuNames, GameObject>();

    public static HUD_Manager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    void Start()
    {
        m_Camera = Camera.main.transform;
    }

    public IEnumerator MoveCameraToNextLevelCO()
    {
        //nextButton.SetActive(false);
        CelebrationManager.Instance.MoveCharacter(true);

        Vector3 StartPosition = m_Camera.position;
        Vector3 EndPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + Screen.width / 2, Screen.height / 2f, 0));
        float TimePassed = 0;


        while (TimePassed <= 1)
        {
            m_Camera.position = Vector3.Lerp(StartPosition, EndPosition, TimePassed);
            TimePassed += Time.deltaTime / 4f;
            yield return null;
        }
        CelebrationManager.Instance.MoveCharacter(false);
        Game_Manager.instance.StartNewGame(GameType.Random);
    }

    public void InstantiateMenu(MenuNames MenuName, float delay = 0)
    {
        StartCoroutine(InstantiateMenuWithDelay(MenuName, delay));
    }
        
    private IEnumerator InstantiateMenuWithDelay(MenuNames MenuName, float delay)
    {
        if(!CurrentlyInstantiatedMenus.ContainsKey(MenuName))
        {
            yield return new WaitForSeconds(delay);
            GameObject NewlyInstantiatedMenu = Instantiate(Resources.Load(m_MenuNames[MenuName])) as GameObject;
            CurrentlyInstantiatedMenus.Add(MenuName, NewlyInstantiatedMenu);

            PreviousMenu = CurrentMenu;
            CurrentMenu = MenuName;
#if SHOW_DETAIL_LOGS
            Debug.Log($"[Menu_Manager] ========== {MenuName} is Instantiated ==========", this);
#endif
        }
        else
        {
#if SHOW_DETAIL_LOGS
            Debug.LogError($"{MenuName} Already exists in Scene.");
#endif
        }
    }

    /// <summary>
    /// Destroy a menu if it is present in Scene.
    /// </summary>
    /// <param name="MenuName">The name of the Menu you want to destroy. i.e. MenuNames.PlayMenu</param>
    public void DestroyMenu(MenuNames MenuName)
    {
        if (!CurrentlyInstantiatedMenus.ContainsKey(MenuName))
        {
#if SHOW_DETAIL_LOGS
            Debug.LogError($"[Menu_Manager] ========== No {MenuName} is Currently in Scene ==========", this);
#endif
            return;
        }
            Destroy(CurrentlyInstantiatedMenus[MenuName]);
            CurrentlyInstantiatedMenus.Remove(MenuName);
#if SHOW_DETAIL_LOGS
            Debug.Log($"[Menun Manager] ========== {MenuName} is Destroyed.");
#endif
    }
}
