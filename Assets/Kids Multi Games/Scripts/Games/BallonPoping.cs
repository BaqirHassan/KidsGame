using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallonPoping : Game
{
    /// <summary>
    /// The Image that display the alphabet that has been selected in Progress Bar
    /// </summary>
    [SerializeField] Image SelectedAlphabetInProgressBar;

    /// <summary>
    /// The Image used for showing Progress and having properties 
    /// Image Type as Filled,
    /// Fill Method as Radial 180,
    /// Fill Origin as Right and 
    /// ClockWise UnChecked
    /// </summary>
    [SerializeField] Image FillBar;

    /// <summary>
    /// An array of balloons sprites of all colors. Balloons are randomly selected for the Balloons GameObject.
    /// </summary>
    [SerializeField] Sprite[] AllBalloonsSprites;

    /// <summary>
    /// Max Number of Balloons that can be visible of scrren at a time.
    /// </summary>
    [SerializeField] int MaxBallonsVisibleAtOnce = 10;

    /// <summary>
    /// The vertical move speed of Ballons in Unit per second.
    /// </summary>
    [SerializeField] float VerticalMoveSpeed;


    /// <summary>
    /// In how much time in seconds interpluation (LERP) should take.
    /// </summary>
    [SerializeField] float TimeToLerp = 1.25f;

    /// <summary>
    /// The array that Contains all Balloons with letter A-Z in it.
    /// </summary>
    [SerializeField] GameObject[] AllAlphabteBalloons;

    /// <summary>
    /// The Layer of interactables item.
    /// </summary>
    [SerializeField] LayerMask interactableLayer;

    /// <summary>
    /// A variable of type WaitForSeconds. Used as cached variable for Memory Optimization.
    /// </summary>
    private WaitForSeconds WaitForThirdOfASecond = new WaitForSeconds(.33f);
    
    /// <summary>
    /// Index of Randomly Selected Alphabet from AllAlphabetBalloons Array. At start of the Game.
    /// </summary>
    private int IndexAlphabetForCurrentGame;

    /// <summary>
    /// The list containing All Instantiated alphabets as GameObject. Which are currently not visible on screen.
    /// It includes "nbrOfItemsToMatch" number of Selected Variable and 
    ///Ssame number of other alphabets Randomly selected from the Array "AllAlphabetsBalloons".
    /// </summary>
    private List<GameObject> BalloonsUnused = new List<GameObject>();

    /// <summary>
    /// The List Containing the Balloons that are active and visible on screen
    /// </summary>
    private List<GameObject> BalloonsOnScreen = new List<GameObject>();
     
    /// <summary>
    /// The Direction created at Run Time from VerticalMoveSpeed. ie Vector3(0, VerticalMoveSpeed, 0).
    /// </summary>
    private Vector3 VerticalMoveVector;

    /// <summary>
    /// How Many Times correct Balloon has been popped.
    /// </summary>
    private int SuccessfullBalloonPopped = 0;

    private List<GameObject> AlphabetGoingToProgressBar = new List<GameObject>();

    new private void Start()
    {
        base.Start();
        if(AllAlphabteBalloons.Length == 0)
        {
            Debug.LogError("No Alphabet Ballon in Poping Balloons Game", this);
            return;
        }

        if(GameObject.FindObjectsOfType<Camera>().Length > 1)
        {
            Debug.LogWarning(".isVisible Property check all Cameras in scene. There should not be More than One Camera in Scene for this to Work.", this);
        }

        VerticalMoveVector = new Vector3(0, VerticalMoveSpeed, 0);

        SelectAnAlphabetRandomly();
        PopulateBalloonsUnusedArray();
        StartCoroutine(CheckBalloonsOnScreenStatusCo());

        LineDrawing.OnItemClicked += itemClicked;

        StartCoroutine(checkFTF());
    }

    private IEnumerator checkFTF()
    {
        yield return new WaitForSeconds(1);

        if (!Game_Manager.IsPoppingFTF())
        {
            FTFHand.SetActive(true);

            foreach (var BalloonOnScreen in BalloonsOnScreen)
            {
                if (BalloonOnScreen.name.Equals(AllAlphabteBalloons[IndexAlphabetForCurrentGame].name + "(Clone)"))
                {
                    LeanTween.move(FTFHand, BalloonOnScreen.transform.position, 0);
                    LeanTween.rotate(FTFHand, new Vector3(-30, -10, 0), .5f).setLoopType(LeanTweenType.clamp).setEaseInOutSine().setIgnoreTimeScale(true);

                    Debug.Log("Verticle Move Speed = 0");
                    VerticalMoveVector = Vector3.zero;
                    break;
                }
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < BalloonsOnScreen.Count; i++)
        {
            BalloonsOnScreen[i].transform.position += VerticalMoveVector * Time.deltaTime;
        }
    }

    /// <summary>
    /// Check the Balloons for their visiblity on screen. Maintain "MaxBallonsVisibleAtOnce" number of ballons visible.
    /// </summary>
    /// <returns>None. This is Coroutine.</returns>
    IEnumerator CheckBalloonsOnScreenStatusCo()
    {
        while(!IsCompleted)
        {
            for(int i = 0; i < BalloonsOnScreen.Count; i++)
            {
                // .isVisible Property check all Cameras in scene.
                // There should not be More than One Camera in Scene for this to Work.
                // On Editor .isVisible property also consider scene Camera
                if (!BalloonsOnScreen[i].GetComponent<Renderer>().isVisible)
                {
                    RemoveUnvisibleBalloon(BalloonsOnScreen[i]);
                }
            }

            if (BalloonsOnScreen.Count < MaxBallonsVisibleAtOnce)
            {
                ShowAnUnusedBalloons();
            }

            yield return WaitForThirdOfASecond;
        }
    }

    /// <summary>
    /// Randomly Select an alphabet from the Array "AllAlphabetBalloons" as target alphabet. Also Assign its sprite on HUD. Should be called at start of game.
    /// </summary>
    private void SelectAnAlphabetRandomly()
    {
        IndexAlphabetForCurrentGame = Random.Range(0, AllAlphabteBalloons.Length);
        GameObject AlphabetForCurrentGame = AllAlphabteBalloons[IndexAlphabetForCurrentGame];
        SelectedAlphabetInProgressBar.sprite = AlphabetForCurrentGame.GetComponent<Balloon>().Alphabet.sprite;
    }

    /// <summary>
    /// Populate the list "BalloonsUnused" with "nbrOfItemsToMatch" number of Selected Alphabet ( the Alphabet that is selected for this game) and "nbrOfItemsToMatch" number of other alphabets.
    /// </summary>
    private void PopulateBalloonsUnusedArray()
    { 
        // Adding one selected Alphabet Balloon and one Randomly selected (Other than the Selected Balloon) Balloon 
        for (int i = 0; i < nbrOfItemsToMatch; i++)
        {
            AddSelectedAlphabetToBalloonsUnusedArray();

            AddRandomAlphabetToBalloonsUnusedArray();
        }
    }

    /// <summary>
    /// Adds an Alphabet that has been selected to The Array Named BalloonsUnused.
    /// </summary>
    private void AddSelectedAlphabetToBalloonsUnusedArray()
    {
        var CurrentGameObject = Instantiate(AllAlphabteBalloons[IndexAlphabetForCurrentGame]);
        var RandomSprite = SelectRandomSprite();
        if (RandomSprite)
        {
            CurrentGameObject.GetComponent<SpriteRenderer>().sprite = RandomSprite;
        }
        AddBalloonToBalloonUnused(CurrentGameObject);
    }

    /// <summary>
    /// Adds a random Alphabet other than selected alphabet to The Array Named BalloonsUnused.
    /// </summary>
    private void AddRandomAlphabetToBalloonsUnusedArray()
    {
        int randomSelection = Random.Range(0, AllAlphabteBalloons.Length);

        if (randomSelection == IndexAlphabetForCurrentGame)
        {
            randomSelection += Random.Range(1, AllAlphabteBalloons.Length - 1);
            randomSelection %= AllAlphabteBalloons.Length;
        }


        var CurrentGameObject = Instantiate(AllAlphabteBalloons[randomSelection]);
        var RandomSprite = SelectRandomSprite();
        if (RandomSprite)
        {
            CurrentGameObject.GetComponent<SpriteRenderer>().sprite = RandomSprite;
        }

        AddBalloonToBalloonUnused(CurrentGameObject);
    }

    private Sprite SelectRandomSprite()
    {
        if(AllBalloonsSprites.Length <= 0)
        {
            Debug.Log("the length of Sprite Array in Balloons Popping Game is zero.");
            return null;
        }

        return AllBalloonsSprites[Random.Range(0, AllBalloonsSprites.Length)];
    }

    /// <summary>
    /// Adds balloon to the array named BalloonsUnused.
    /// </summary>
    /// <param name="go">Balloon to be added.</param>
    private void AddBalloonToBalloonUnused(GameObject go)
    {
        if(go == null)
        {
            Debug.LogError("Balloon GameObject is Null. Can't add to Array BalloonUnused");
            return;
        }

        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        go.transform.SetParent(transform);
        go.SetActive(false);
        BalloonsUnused.Add(go);
    }

    /// <summary>
    /// Remove the Balloons from the list "BalloonsOnScreen".
    /// </summary>
    /// <param name="go">Balloons to be removed.</param>
    private void RemoveUnvisibleBalloon(GameObject go)
    {
        if(!BalloonsOnScreen.Contains(go))
        {
            Debug.LogError("Array BalloonsOnScreen don't contains gameObject names " + go + ". Can't Remove " + go, this);
            return;
        }
        go.SetActive(false);
        BalloonsOnScreen.Remove(go);
        BalloonsUnused.Add(go);
    }

    /// <summary>
    /// Spawn a Balloon from list "BalloonsUnused" at random Position on x axis, -2 on y axis and 10 distance on z axis. 
    /// </summary>
    private void ShowAnUnusedBalloons()
    {
        if (BalloonsUnused.Count == 0)
        {
            Debug.LogError("No Balloon to Spawn / show on screen", this);
            return;
        }
        var TempSelection = BalloonsUnused[Random.Range(0, BalloonsUnused.Count)];
        BalloonsUnused.Remove(TempSelection);
        BalloonsOnScreen.Add(TempSelection);
        TempSelection.SetActive(true);

        RandomlyPlaceBalloonsOnScreen(TempSelection);
    }

    /// <summary>
    /// Places the GameObject at bottom of screen. Horizantal position randomly selected from the 10% - 90% screen width. At distance of 10 on Z Axis.
    /// </summary>
    /// <param name="gObject"> The GameObject to be placed.</param>
    private void RandomlyPlaceBalloonsOnScreen(GameObject gObject)
    {
        if(gObject == null)
        {
            Debug.LogError("Null Referance Exception. The GameObject passed to the funcion \"RandomlyPlaceBalloonsOnScreen\" as paremeter is null.", this);
            return;
        }

        bool ShouldRepeat = true;
        Vector3 TempPosition = new Vector3(Random.Range(Screen.width * .1f, Screen.width * .9f), -2f, 10);

        while (ShouldRepeat)
        {
            TempPosition = new Vector3(Random.Range(Screen.width * .1f, Screen.width * .9f), -2f, 10);
            TempPosition = Camera.main.ScreenToWorldPoint(TempPosition);

            if (!Physics2D.OverlapCircle(TempPosition, 1.2f, interactableLayer))
            {
                ShouldRepeat = false;
            }
        }

        gObject.transform.position = TempPosition;
    }



    private void itemClicked(GameObject go)
    {
        if(go.name == AllAlphabteBalloons[IndexAlphabetForCurrentGame].name + "(Clone)")
        {
            StartCoroutine(popAlphabetCo(go, true));
            VerticalMoveVector = new Vector3(0, VerticalMoveSpeed, 0);

            if (!Game_Manager.IsMatchingFTF())
            {
                LeanTween.cancel(FTFHand);
                FTFHand.SetActive(false);
                Game_Manager.PoppingFTFDone();
            }
        }
        else
        {
            StartCoroutine(popAlphabetCo(go, false));
        }

        SoundManager.PlaySFX(SoundManager.SFXType.BalloonPopping);
        VFX_Manager.Instance.BalloonPop(go.transform.position);
    }

    private IEnumerator popAlphabetCo(GameObject go, bool Ismatched = false)
    {

        if (Ismatched)
        {
            AddBalloonToBalloonUnused(Instantiate(AllAlphabteBalloons[IndexAlphabetForCurrentGame]));
            Transform Alphabet = go.transform.GetChild(0);
            Alphabet.parent = null;
            AlphabetGoingToProgressBar.Add(Alphabet.gameObject);

            BalloonsOnScreen.Remove(go);
            Destroy(go);
            Debug.Log("Please Impliment Balloon Pop here.");

            float MoveSpeed = 20f;

            Vector3 distination = Camera.main.ScreenToWorldPoint(SelectedAlphabetInProgressBar.rectTransform.position);
            while ((Alphabet.position - distination).sqrMagnitude > .025f)
            {
                if (MoveSpeed < 40f)
                {
                    MoveSpeed += Time.deltaTime;
                }
                Alphabet.position = Vector3.MoveTowards(Alphabet.position, distination, MoveSpeed * Time.deltaTime);
                yield return null;
            }


            Destroy(Alphabet.gameObject);
            AlphabetGoingToProgressBar.Remove(Alphabet.gameObject);
            StartCoroutine(updateProgressBarCo());
            SuccessfullBalloonPopped++;

            if(SuccessfullBalloonPopped >= nbrOfItemsToMatch)
            {
                GameFinished();
            }
        }
        else
        {
            BalloonsOnScreen.Remove(go);
            Destroy(go);
            Debug.Log("Please Impliment Balloon Pop here.");
            AddRandomAlphabetToBalloonsUnusedArray();
        }
        yield return null;
    }


    new private void GameFinished()
    {
        base.GameFinished();

        StartCoroutine(PopAllOnScreenBalloons());
    }


    private IEnumerator PopAllOnScreenBalloons()
    {
        while(BalloonsOnScreen.Count > 0)
        {
            StartCoroutine(popAlphabetCo(BalloonsOnScreen[0], false));
            yield return WaitForThirdOfASecond;
        }
    }

    /// <summary>
    /// Gradually Update the ProgressFill
    /// </summary>
    /// <returns></returns>
    private IEnumerator updateProgressBarCo()
    {
        float starting = FillBar.fillAmount;
        float final = (float)SuccessfullBalloonPopped / (float)nbrOfItemsToMatch;

        float TimePassed = 0;
        float LerpSpeed = 1 / TimeToLerp;

        while (FillBar.fillAmount <= final)
        {
            FillBar.fillAmount = Mathf.Lerp(starting, final, TimePassed);

            TimePassed += Time.deltaTime * LerpSpeed;
            yield return null;
        }
    }

    void OnDestroy()
    {
        LineDrawing.OnItemClicked -= itemClicked;

        if(AlphabetGoingToProgressBar.Count > 0)
        {
            for (int i = AlphabetGoingToProgressBar.Count - 1; i >= 0; i--)
            {
                Destroy(AlphabetGoingToProgressBar[i].gameObject);
                AlphabetGoingToProgressBar.RemoveAt(i);
            }
        }

        Debug.Log("Please Impliment Remaning Balloon Poping");
    }

}
