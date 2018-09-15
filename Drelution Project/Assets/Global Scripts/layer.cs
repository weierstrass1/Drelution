using UnityEngine;

[System.Serializable]
public class layer : MobileObject
{
    public bool show = true;
    public int selectedX = 0, selectedY = 0;
    public int blockSize;
    public int width;
    public int height;
    public static block[] AllBlocks = { 
    };
    public int state = 0;

    public int initX = 0, initY = 0, endX = 0, endY = 0;
    public bool selectionStart = false;

    [SerializeField]
    [HideInInspector]
    public int[] map;

    public int getXPos(float X)
    {
        int xint = (int)x;
        int Xint = (int)X;

        return (Xint - xint) / blockSize;
    }

    public int getYPos(float Y)
    {
        int yint = (int)y;
        int Yint = (int)Y;

        return (Yint - height * blockSize + yint) / blockSize;
    }


    public int getBlock(float X, float Y)
    {
        int blockXPos = getXPos(X);

        int blockYPos = getYPos(Y);

        if (blockXPos >= width) return -1;
        if (blockXPos < 0) return -1;
        if (blockYPos >= height) return -1;
        if (blockYPos < 0) return -1;

        int index = map[blockXPos + (width * blockYPos)];

        if (index < 0) return -1;

        return index;
    }
}