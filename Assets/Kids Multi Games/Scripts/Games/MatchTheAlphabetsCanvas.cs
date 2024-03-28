using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchTheAlphabetsCanvas : Game
{
    /// <summary>
    /// The pairs of all alphabets. From which we Choice "Game.nbrOfItemsToMatch" number of pairs at start of game
    /// </summary>
    [SerializeField] List<AlphabetsPair> AllPossiblePairs;

    /// <summary>
    /// The parent that Contain the Items of first (Left Hand side) Colunm
    /// </summary>
    [SerializeField] GameObject[] Column1Items;

    /// <summary>
    /// The parent that Contain the Items of Second (Right Hand side) Colunm
    /// </summary>
    [SerializeField] GameObject[] Column2Items;

    /// <summary>
    /// The Current Item Matched
    /// </summary>
    [SerializeField] int ItemsMatched = 0;

    void Start()
    {
        if(Column1Items.Length < nbrOfItemsToMatch || Column2Items.Length < nbrOfItemsToMatch)
        {
            Debug.LogError("Colunm Items Not Sufficient");
            return;
        }

        // Select Random Pair from allPossiblePairs 
        // Also Assign the selected pairs in Colunms
        // Also Remove the selected pair from "AllPossiblePairs List to avoid Dublicates
        for (int i = 0; i < nbrOfItemsToMatch; i++)
        {
            var RandomSelection = Random.Range(0, AllPossiblePairs.Count);

            Column1Items[i].GetComponent<Image>().sprite = AllPossiblePairs[RandomSelection].Alphabet;
            Column1Items[i].name = AllPossiblePairs[RandomSelection].Name;

            int RandomSelection2;

            // SHUFFLE Try again and again untill we find place in colunm 2 which hasn't been used 
            while(true)
            {
                RandomSelection2 = Random.Range(0, nbrOfItemsToMatch);
                if(Column2Items[RandomSelection2].GetComponent<Image>().sprite == null)
                {
                    break;
                }
            }
            Column2Items[RandomSelection2].GetComponent<Image>().sprite = AllPossiblePairs[RandomSelection].Alphabet;
            Column2Items[RandomSelection2].name = AllPossiblePairs[RandomSelection].Name;
            AllPossiblePairs.RemoveAt(RandomSelection);
        }

        //Game_Manager.instance.ScaleContainer(gameObject);

        LineDrawing.OnItemMatched += itemMatched;
        LineDrawing.OnItemMatched += itemMismatched;
    }

    public void itemMatched(GameObject ObjectMatched)
    {
        ItemsMatched++;

        if (ItemsMatched >= nbrOfItemsToMatch)
        {
            GameFinished();
        }
    }

    public void itemMismatched(GameObject ObjectMismatched)
    {

    }

    private void OnDisable()
    {
        LineDrawing.OnItemMatched -= itemMatched;
        LineDrawing.OnItemMatched -= itemMismatched;
        LineDrawing.Instance.ClearUsedLineRendrers();
    }
}
