using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelebrationManager : MonoBehaviour
{
    [SerializeField] GameObject Character;

    Animator CharacterAnim;

    public static CelebrationManager Instance;
    void Awake()
    {
        if(Instance != null)
            Destroy(Instance);
        Instance = this;
    }

    public enum CharacterAnimationStates
    {
        Idle, Moving, Celebrating, Stoping 
    }

    // Start is called before the first frame update
    void Start()
    {
        #region Character Placement at Bottom of the Screen
        Camera cam = Camera.main;

        Vector3 min = Character.GetComponentInChildren<Renderer>().bounds.min;
        Vector3 max = Character.GetComponentInChildren<Renderer>().bounds.max;

        //  --------(Max)
        //  |         |
        //  |         |
        //  |         |
        //  |         |
        //(Min)--------

        Vector3 screenMin = cam.WorldToScreenPoint(min);                
        Vector3 screenMax = cam.WorldToScreenPoint(max);

        float screenWidth = screenMax.x - screenMin.x;

        Vector3 ScreenCordinates = new Vector3(0 + screenWidth/ 2 + screenWidth * .1f, Screen.height * .05f, 5);

        Character.transform.position = Camera.main.ScreenToWorldPoint(ScreenCordinates);

        #endregion

        CharacterAnim = Character.GetComponent<Animator>();
        ChangeState(CharacterAnimationStates.Idle);
    }

    private void ChangeState(CharacterAnimationStates State)
    {
        CharacterAnim.SetFloat("SubState", 0);
        if(State == CharacterAnimationStates.Idle)
        {
            CharacterAnim.SetBool("Celebrate", false);
            CharacterAnim.SetBool("Move", false);
            CharacterAnim.SetFloat("SubState", 0);

            //SoundManager.Instance.play
        }
        else if (State == CharacterAnimationStates.Moving)
        {
            CharacterAnim.SetBool("Celebrate", false);
            CharacterAnim.SetBool("Move", true);
            CharacterAnim.SetFloat("SubState", 0);
        }
        else if (State == CharacterAnimationStates.Celebrating)
        {
            CharacterAnim.SetBool("Celebrate", false);
            CharacterAnim.SetBool("Celebrate", true);
            CharacterAnim.SetFloat("SubState", 0);
        }
        else if( State == CharacterAnimationStates.Stoping)
        {

        }
    }

    public void CharacterCelebrating()
    {
        ChangeState(CharacterAnimationStates.Celebrating);
        Invoke(nameof(CharacterIdle), 6.567f);
    }

    public void CharacterIdle()
    {
        ChangeState(CharacterAnimationStates.Idle);
    }

    public void MoveCharacter(bool State)
    {
        if (State)
        {
            ChangeState(CharacterAnimationStates.Moving);
        }
        else
        {
            ChangeState(CharacterAnimationStates.Idle);
        }
    }
}
