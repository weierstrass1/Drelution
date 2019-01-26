using UnityEngine;

public abstract class SolidSlopeBlock : SolidBlock
{
    protected float x1,x2;
    protected float y1,y2;
    protected float m;
    protected float angle;

    public SolidSlopeBlock(float X1, float X2,
                            float Y1, float Y2)
    {
        x1 = X1;
        x2 = X2;
        y1 = Y1;
        y2 = Y2;
        m = (y2 - y1) / (x2 - x1);
        angle = (180 * Mathf.Atan(m)) / (Mathf.PI);
        vertices = new Vector3[6];

        vertices[0] = new Vector3(-Layer.BlockSize / 2, Layer.BlockSize / 2);
        vertices[1] = new Vector3(x1 - (Layer.BlockSize / 2), y1 - (Layer.BlockSize / 2));
        vertices[2] = new Vector3(x2 - (Layer.BlockSize / 2), y2 - (Layer.BlockSize / 2));
        vertices[3] = new Vector3(Layer.BlockSize / 2, Layer.BlockSize / 2);
        vertices[4] = new Vector3(Layer.BlockSize / 2, -Layer.BlockSize / 2);
        vertices[5] = new Vector3(-Layer.BlockSize / 2, -Layer.BlockSize / 2);
    }

    public override void AngleDetector(MobileObject target, Transform contactPoint, int x, int y, Layer l)
    {
        float contactX = contactPoint.position.x;
        float contactY = contactPoint.position.y;
        float bx = GetX(x, l);
        float by = GetY(y, l);

        if (contactX < bx + x1)
        {
            if (contactY <= by + y1)
                base.AngleDetector(target, contactPoint, x, y, l);
        }
        else if (contactX > bx + x2)
        {
            if (contactY <= by + y2)
                base.AngleDetector(target, contactPoint, x, y, l);
        }
        else 
        {
            float dy = m * (contactX - (bx + x1)) + (by + y1) - contactY;

            if (dy > 0 || target.GuidedByTerrain) 
            {
                target.Y += dy;
                target.BlockedAngleDetector = true;
                target.TerrainAngle = angle;
                target.BlockedFromBelow = true;
            }
        }
    }

    public override void Left(MobileObject target, Transform contactPoint, int x, int y, Layer l)
    {
        float by = GetY(y, l);
        float contactY = contactPoint.position.y;

        if(contactY <= by + y1)
            base.Left(target, contactPoint, x, y, l);
    }

    public override void Right(MobileObject target, Transform contactPoint, int x, int y, Layer l)
    {
        float by = GetY(y, l);
        float contactY = contactPoint.position.y;

        if (contactY <= by + y2)
            base.Right(target, contactPoint, x, y, l);
    }

    public override void Up(MobileObject target, Transform contactPoint, int x, int y, Layer l)
    {
        float contactX = contactPoint.position.x;
        float contactY = contactPoint.position.y;
        float bx = GetX(x, l);
        float by = GetY(y, l);

        if (contactX < bx + x1)
        {
            if (contactY <= by + y1)
                base.Up(target, contactPoint, x, y, l);
        }
        else if (contactX > bx + x2)
        {
            if (contactY <= by + y2)
                base.AngleDetector(target, contactPoint, x, y, l);
        }
    }
}
