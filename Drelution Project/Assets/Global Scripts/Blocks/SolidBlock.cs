using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidBlock : Block
{
    public SolidBlock() : base("Solid Block", new Color32(255, 0, 0, 128),
        new Vector3(-BlockLayer.BlockSize / 2, -BlockLayer.BlockSize / 2), 
        new Vector3(-BlockLayer.BlockSize / 2, BlockLayer.BlockSize / 2),
        new Vector3(BlockLayer.BlockSize / 2, BlockLayer.BlockSize / 2),
        new Vector3(BlockLayer.BlockSize / 2, -BlockLayer.BlockSize / 2))
    {
    }

    public override void AngleDetector(MobileObject target, Transform contactPoint, int x, int y, BlockLayer l)
    {
        if (target.YSpeed - l.YSpeed >= 0) return;

        float by = GetY(y, l);

        float cy = contactPoint.position.y;

        float dy = cy - (by + BlockLayer.BlockSize);

        target.Y -= dy;

        target.BlockedAngleDetector = true;
        target.BlockedFromBelow = true;
        target.TerrainAngle = 0;
    }

    public override void Down(MobileObject target, Transform contactPoint, int x, int y, BlockLayer l)
    {
        if (target.YSpeed - l.YSpeed <= 0) return;

        float by = GetY(y, l);

        float cy = contactPoint.position.y;

        float dy = by - cy;

        target.BlockedFromAbove = true;

        target.Y += dy;
        target.YSpeed = 0;
    }

    public override void Left(MobileObject target, Transform contactPoint, int x, int y, BlockLayer l)
    {
        if (target.XSpeed - l.XSpeed <= 0) return;

        float bx = GetX(x, l);

        float cx = contactPoint.position.x;

        float dx = cx - bx;

        target.X -= dx;

        target.BlockedFromRight = true;

        target.XSpeed = 0;
    }

    public override void Right(MobileObject target, Transform contactPoint, int x, int y, BlockLayer l)
    {
        if (target.XSpeed - l.XSpeed >= 0) return;

        float bx = GetX(x, l);

        float cx = contactPoint.position.x;

        float dx = bx + BlockLayer.BlockSize - cx;

        target.X += dx;

        target.BlockedFromLeft = true;

        target.XSpeed = 0;
    }

    public override void Up(MobileObject target, Transform contactPoint, int x, int y, BlockLayer l)
    {
        if (target.YSpeed - l.YSpeed >= 0) return;

        float by = GetY(y, l);

        float cy = contactPoint.position.y;

        float dy = cy - (by + BlockLayer.BlockSize);

        target.BlockedFromBelow = true;

        if (!target.BlockedAngleDetector) 
        {
            target.Y -= dy;
        }
    }
}