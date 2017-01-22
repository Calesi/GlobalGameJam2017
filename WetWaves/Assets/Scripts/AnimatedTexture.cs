using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTexture : MonoBehaviour
{
    public static AnimatedTexture instance;
    public int columns = 3;
    public int rows = 3;
    public int imagesPerRow = 3;
    public int rowToAnimate = 2;
    public float framesPerSecond = 10f;

    public bool forwards = true;

    //the current frame to display
    private int index = 0;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        StartCoroutine(updateTiling());

        //set the tile size of the texture (in UV units), based on the rows and columns
        Vector2 size = new Vector2(1f / columns, 1f / rows);
        GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", size);
    }

    //void Update() {
    //    rowToAnimate = 2;
    //}


    private IEnumerator updateTiling()
    {
        while (true)
        {
            //move to the next index
            if (forwards)
            {
                index++;
                if (index > rowToAnimate * columns + imagesPerRow)
                {
                    index = rowToAnimate * columns + imagesPerRow - 1;
                    forwards = false;
                }
            }
            else
            {
                index--;
                if (index < rowToAnimate * columns)
                {
                    index = rowToAnimate * columns + 1;
                    forwards = true;
                }
            }

            //split into x and y indexes
            Vector2 offset = new Vector2((float)index / columns - (index / columns), //x index
                                          (index / columns) / (float)rows);          //y index

            GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);

            yield return new WaitForSeconds(1f / framesPerSecond);
        }

    }
}
