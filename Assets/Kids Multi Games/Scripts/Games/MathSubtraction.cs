using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MathSubtraction : Game
{
    /// <summary>
    /// The parent that Contain the Items that remains Constant (Left Hand side)
    /// </summary>
    [SerializeField] GameObject[] Column1Constant;

    /// <summary>
    /// The parent that Contain the Items of first (Left Hand side) Colunm
    /// </summary>
    [SerializeField] GameObject[] Column1Variable;

    /// <summary>
    /// The parent that Contain the Items of Second (Right Hand side) Colunm
    /// </summary>
    [SerializeField] GameObject[] Column2Items;

    /// <summary>
    /// The Current Item Matched
    /// </summary>
    [SerializeField] int ItemsMatched = 0;

    /// <summary>
    /// The Text mesh used as heading/Tite for the this game.
    /// </summary>
    [SerializeField] TextMesh Heading;

    [SerializeField] List<int> Zero2Constant;

    new private void Start()
    {
        base.Start();
        if (Column1Constant.Length < nbrOfItemsToMatch || Column1Variable.Length < nbrOfItemsToMatch || Column2Items.Length < nbrOfItemsToMatch)
        {
            Debug.LogError("Colunm Items Not Sufficient");
            return;
        }

        string temp = string.Empty;

        foreach (var Char in discription)
        {
            temp += Char == ' ' ? "\n" : Char.ToString();
        }
        Heading.text = temp;

        // Select Random Pair from allPossiblePairs 
        // Also Assign the selected pairs in Colunms
        // Also Remove the selected pair from "AllPossiblePairs List to avoid Dublicates

        int ConstantSelection = Random.Range(5, 10);
        Zero2Constant = Enumerable.Range(0, ConstantSelection).ToList();
        
        for (int i = 0; i < nbrOfItemsToMatch; i++)
        {
            var RandomSelection = Random.Range(0, Zero2Constant.Count);

            Column1Constant[i].GetComponent<TextMesh>().text = ConstantSelection.ToString();
            Column1Variable[i].GetComponent<TextMesh>().text = Zero2Constant[RandomSelection].ToString();

            Column1Variable[i].name = (ConstantSelection - Zero2Constant[RandomSelection]).ToString(); 

            int RandomSelection2;

            // SHUFFLE Try again and again untill we find place in colunm 2 which hasn't been used 
            while (true)
            {
                RandomSelection2 = Random.Range(0, nbrOfItemsToMatch);
                if (Column2Items[RandomSelection2].GetComponent<TextMesh>().text == "")
                {
                    break;
                }
            }
            Column2Items[RandomSelection2].GetComponent<TextMesh>().text = (ConstantSelection - Zero2Constant[RandomSelection]).ToString();
            Column2Items[RandomSelection2].name = (ConstantSelection - Zero2Constant[RandomSelection]).ToString();

            Zero2Constant.Remove(Zero2Constant[RandomSelection]);
        }

        //Game_Manager.instance.ScaleContainer(gameObject);

        LineDrawing.OnItemMatched += itemMatched;
        LineDrawing.OnItemMatched += itemMismatched;

        checkFTF();
    }

    private void checkFTF()
    {
        if (!Game_Manager.IsMatchingFTF())
        {
            FTFHand.SetActive(true);
            string FirstItemName = Column1Variable[0].name;
            LeanTween.move(FTFHand, Column1Variable[0].transform.position, 0);

            foreach (var Column2Item in Column2Items)
            {
                if (FirstItemName.Equals(Column2Item.name))
                {
                    LeanTween.move(FTFHand, Column2Item.transform.position, 2).setLoopType(LeanTweenType.clamp).setEaseInOutSine();
                }
            }
        }
    }

    public void itemMatched(GameObject ObjectMatched)
    {
        if (!Game_Manager.IsMatchingFTF())
        {
            LeanTween.cancel(FTFHand);
            FTFHand.SetActive(false);
            Game_Manager.MatchingFTFDone();
        }

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
