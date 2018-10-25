using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidBlock : Block
{
    public SolidBlock() : base("Solid Block", new Color32(255, 0, 0, 128),
        new Vector3(-Layer.BlockSize / 2, -Layer.BlockSize / 2), 
        new Vector3(-Layer.BlockSize / 2, Layer.BlockSize / 2),
        new Vector3(Layer.BlockSize / 2, Layer.BlockSize / 2),
        new Vector3(Layer.BlockSize / 2, -Layer.BlockSize / 2))
    {
    }

    public override void AngleDetector(MobileObject target, Transform contactPoint, int x, int y, Layer l)
    {
        if (target.YSpeed - l.YSpeed > 0) return;

        float bx = GetX(x, l);
        float by = GetY(y, l);

        float cx = contactPoint.position.x;
        float cy = contactPoint.position.y;

        float dy = cy + 1 - (by + Layer.BlockSize);

        target.Y -= dy;

        target.BlockedAngleDetector = true;
        target.BlockedFromBelow = true;
        target.TerrainAngle = 0;

        target.YSpeed = 0;
    }

    public override void Down(MobileObject target, Transform contactPoint, int x, int y, Layer l)
    {
        if (target.YSpeed - l.YSpeed < 0) return;

        float bx = GetX(x, l);
        float by = GetY(y, l);

        float cx = contactPoint.position.x;
        float cy = contactPoint.position.y;

        float dy = by - cy + 1;

        target.BlockedFromAbove = true;

        target.Y += dy;
        target.YSpeed = 0;
    }

    public override void Left(MobileObject target, Transform contactPoint, int x, int y, Layer l)
    {
        if (target.XSpeed - l.XSpeed < 0) return;

        float bx = GetX(x, l);
        float by = GetY(y, l);

        float cx = contactPoint.position.x;
        float cy = contactPoint.position.y;

        float dx = cx - 1 - bx;

        target.X -= dx;

        target.BlockedFromRight = true;

        target.XSpeed = 0;
    }

    public override void Right(MobileObject target, Transform contactPoint, int x, int y, Layer l)
    {
        if (target.XSpeed - l.XSpeed > 0) return;

        float bx = GetX(x, l);
        float by = GetY(y, l);

        float cx = contactPoint.position.x;
        float cy = contactPoint.position.y;

        float dx = bx + Layer.BlockSize - cx - 1;

        target.X += dx;

        target.BlockedFromLeft = true;

        target.XSpeed = 0;
    }

    public override void Up(MobileObject target, Transform contactPoint, int x, int y, Layer l)
    {
        if (target.YSpeed - l.YSpeed > 0) return;

        float bx = GetX(x, l);
        float by = GetY(y, l);

        float cx = contactPoint.position.x;
        float cy = contactPoint.position.y;

        float dy = cy + 1 - (by + Layer.BlockSize);

        target.BlockedFromBelow = true;

        if (!target.BlockedAngleDetector) 
        {
            target.Y -= dy;
            target.YSpeed = 0;
        }
    }
}