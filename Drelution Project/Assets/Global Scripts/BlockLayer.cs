﻿using UnityEngine;

[System.Serializable]
public class BlockLayer : MobileObject
{
    public bool show = true;
    public int selectedX = 0, selectedY = 0;
    public static int BlockSize = 16;
    public int width;
    public int height;
    public static Block[] AllBlocks = { 
        new DelegateBlock(),
        new SolidBlock(),
        new SolidBlock20_56Part1(),
        new SolidBlock20_56Part2(),
        new SolidBlock20_56Part3(),
        new SolidSlope45()
    };
    public int state = 0;

    public int initX = 0, initY = 0, endX = 0, endY = 0;
    public bool selectionStart = false;

    [SerializeField]
    [HideInInspector]
    public int[] map;

    public int getXPos(float X)
    {
        int xint = (int)base.X;
        int Xint = (int)X;

        return (Xint - xint) / BlockSize;
    }

    public int getYPos(float Y)
    {
        int yint = (int)base.Y;
        int Yint = (int)Y;

        return (Yint - height * BlockSize + yint) / BlockSize;
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