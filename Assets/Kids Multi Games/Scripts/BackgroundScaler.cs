using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    [SerializeField] bool isAspectRatio = false;
    [SerializeField] bool isSpaceCraft = false;
    
    float worldSpaceWidth, worldSpaceHeight;
    
    SpriteRenderer Sprite;

    void Awake()
    {
        // World Coordinates of Place Visible at Top Right Corner Of the Screen
        Vector3 topRightCorner = Camera.main. ScreenToWorldPoint(new Vector3(Screen.width, Screen. height, Camera.main.transform.position.z));

        // TopLeftCorner to TopRightCorner = TopLeftCorner to Camera + Camera To TopRightCorner 
        worldSpaceWidth = topRightCorner.x * 2; 
        worldSpaceHeight = topRightCorner.y * 2;


        Vector2 spriteSize = GetComponent<SpriteRenderer>().size;

        float scaleFactorX = worldSpaceWidth / spriteSize.x;
        float scaleFactorY = worldSpaceHeight / spriteSize.y;

        if(isAspectRatio)

        if (scaleFactorX > scaleFactorY) 
            scaleFactorY = scaleFactorX;
        else
            scaleFactorX  = scaleFactorY;

        Sprite = GetComponent<SpriteRenderer>();
        Sprite.size = new Vector2(Sprite.size.x * scaleFactorX, Sprite.size.y * scaleFactorY);        
        

        int ChildCount = 0;
        foreach(Transform child in transform)
        {
            if(isSpaceCraft)
            { 
                child.position = new Vector3(child.position.x * scaleFactorX, child.position.y * scaleFactorY, 0);
                isSpaceCraft = false;
            }
            else
            {
                Sprite = child.gameObject.GetComponent<SpriteRenderer>();
                Sprite.size = new Vector2(Sprite.size.x * scaleFactorX, Sprite.size.y * scaleFactorY);
                if(ChildCount == 0)
                {
                    if(child.childCount > 0)
                    {
                        Transform spaceCraft = child.GetChild(0);
                        spaceCraft.localPosition = new Vector3(spaceCraft.position.x * scaleFactorX, spaceCraft.position.y * scaleFactorY, 0);
                    }
                    child.position = new Vector3(Sprite.size.x, 0,0);
                }
                else
                {
                    if(child.childCount > 0)
                    {
                        Transform spaceCraft = child.GetChild(0);
                        spaceCraft.localPosition = new Vector3(spaceCraft.position.x * scaleFactorX, spaceCraft.position.y * scaleFactorY, 0);
                    }
                    child.position = new Vector3(-Sprite.size.x, 0,0);
                }
                ChildCount++;
            }
        }
    }
}
