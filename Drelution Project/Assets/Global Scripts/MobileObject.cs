using UnityEngine;

public class AvailablesLayers
{
    public const int Default = 0;
    public const int TransparentFX = 1;
    public const int IgnoreRayCast = 2;
    public const int Water = 4;
    public const int UI = 5;
    public const int Floor = 5;
    public const int Camera = 9;
    public const int CCPFromRight = 10;
    public const int CCPFromLeft = 11;
    public const int CCPFromTop = 12;
    public const int CCPFromBottom = 13;
    public const int Ally = 14;
    public const int Enemy = 15;
}

public abstract class MobileObject : MonoBehaviour
{
    public const float PixelPerMeter = 9f;
    public const float LimitSpeed = 900f;
    public const float LimitDeltaTime = 1f / 60f;
    public static readonly float LimitSpeedInMeters = LimitSpeed / PixelPerMeter;
    public static float DeltaTime
    {
        get
        {
            return Mathf.Min(LimitDeltaTime, Time.deltaTime);
        }
    }

    [HideInInspector]
    public int Mass;
    [HideInInspector]
    public bool Solid;
    [HideInInspector]
    public bool GuidedByTerrain;
    [HideInInspector]
    public bool AffectedByTerrain;
    [HideInInspector]
    public float TerrainAngle;
    [HideInInspector]
    public bool IgnoreBelowInteraction;
    [HideInInspector]
    public bool XDirection;
    [HideInInspector]
    public bool YDirection;
    [HideInInspector]
    public bool BlockedAngleDetector;
    [HideInInspector]
    public bool BlockedFromBelow;
    [HideInInspector]
    public bool BlockedFromLeft;
    [HideInInspector]
    public bool BlockedFromRight;
    [HideInInspector]
    public bool BlockedFromAbove;
    public Transform[] TopDetector;
    public Transform[] BottomDetector;
    public Transform[] LeftDetector;
    public Transform[] RightDetector;
    public Transform[] AngleDetector;
    public float X
    {
        get
        {
            return GetComponent<Transform>().position.x;
        }
        set
        {
            GetComponent<Transform>().position = new Vector3(
                value,
                GetComponent<Transform>().position.y,
                GetComponent<Transform>().position.z);
        }
    }
    public float Y
    {
        get
        {
            return GetComponent<Transform>().position.y;
        }
        set
        {
            GetComponent<Transform>().position = new Vector3(
                GetComponent<Transform>().position.x,
                value,
                GetComponent<Transform>().position.z);
        }
    }
    [HideInInspector]
    public float XSpeed;
    [HideInInspector]
    public float MaxXSpeed;
    [HideInInspector]
    public float MinXSpeed;
    [HideInInspector]
    public float YSpeed;
    [HideInInspector]
    public float MaxYSpeed;
    [HideInInspector]
    public float MinYSpeed;
    [HideInInspector]
    public float XAcceleration;
    [HideInInspector]
    public float YAcceleration;
    [HideInInspector]
    public float XFriction;
    [HideInInspector]
    public float YFriction;
    [HideInInspector]
    public float XGravity;
    [HideInInspector]
    public float YGravity;

    public void ApplyLayerInteraction()
    {
        //float lastY = Y;
        //float lastX = X;
        LayerInteractionY();
        LayerInteractionX();

        /*if (!BlockedAngleDetector && BlockedFromBelow && (BlockedFromLeft || BlockedFromRight))
        {
            BlockedFromBelow = false;
            Y = lastY;
        }*/
    }
    public void LayerInteractionX()
    {
        Layer[] layers = Level.Instance.Layers;

        float deltaXSp;
        BlockedFromLeft = false;
        BlockedFromRight = false;

        for (int i = 0; i < layers.Length; i++)
        {
            deltaXSp = XSpeed - layers[i].XSpeed;

            if (RightDetector != null && RightDetector.Length > 0)
            {
                for (int j = 0; j < RightDetector.Length; j++)
                {
                    int x = layers[i].getXPos(RightDetector[j].position.x);
                    int y = layers[i].getYPos(RightDetector[j].position.y);

                    int bid = layers[i].getBlock(RightDetector[j].position.x,
                        RightDetector[j].position.y);

                    if (bid >= 0)
                    {
                        Block b = Layer.AllBlocks[bid];

                        b.Left(this, RightDetector[j], x, y, layers[i]);
                    }
                }
            }
            if (LeftDetector != null && LeftDetector.Length > 0)
            {
                for (int j = 0; j < LeftDetector.Length; j++)
                {
                    int x = layers[i].getXPos(LeftDetector[j].position.x);
                    int y = layers[i].getYPos(LeftDetector[j].position.y);

                    int bid = layers[i].getBlock(LeftDetector[j].position.x,
                        LeftDetector[j].position.y);

                    if (bid >= 0)
                    {
                        Block b = Layer.AllBlocks[bid];

                        b.Right(this, LeftDetector[j], x, y, layers[i]);
                    }
                }
            }
        }
    }
    public void LayerInteractionY()
    {
        Layer[] layers = Level.Instance.Layers;

        for (int i = 0; i < layers.Length; i++)
        {
            BlockedFromAbove = false;
            if (TopDetector != null && TopDetector.Length > 0)
            {
                for (int j = 0; j < TopDetector.Length; j++)
                {
                    int x = layers[i].getXPos(TopDetector[j].position.x);
                    int y = layers[i].getYPos(TopDetector[j].position.y);

                    int bid = layers[i].getBlock(TopDetector[j].position.x,
                        TopDetector[j].position.y);

                    if (bid >= 0)
                    {
                        Block b = Layer.AllBlocks[bid];

                        b.Down(this, TopDetector[j], x, y, layers[i]);
                    }
                }
            }
            int selAng = 0;

            BlockedAngleDetector = false;
            BlockedFromBelow = false;

            if (AngleDetector != null && AngleDetector.Length > 0)
            {
                if (XDirection)
                {
                    for (int j = 0; j < AngleDetector.Length; j++)
                    {
                        if (AngleDetector[selAng].position.x < AngleDetector[j].position.x)
                        {
                            selAng = j;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < AngleDetector.Length; j++)
                    {
                        if (AngleDetector[selAng].position.x > AngleDetector[j].position.x)
                        {
                            selAng = j;
                        }
                    }
                }

                int x = layers[i].getXPos(AngleDetector[selAng].position.x);
                int y = layers[i].getYPos(AngleDetector[selAng].position.y);

                int bid = layers[i].getBlock(AngleDetector[selAng].position.x,
                    AngleDetector[selAng].position.y);

                if (bid >= 0)
                {
                    Block b = Layer.AllBlocks[bid];

                    b.AngleDetector(this, AngleDetector[selAng], x, y, layers[i]);
                }
            }

            if (BottomDetector != null && BottomDetector.Length > 0)
            {
                for (int j = 0; j < BottomDetector.Length; j++)
                {
                    int x = layers[i].getXPos(BottomDetector[j].position.x);
                    int y = layers[i].getYPos(BottomDetector[j].position.y);

                    int bid = layers[i].getBlock(BottomDetector[j].position.x,
                        BottomDetector[j].position.y);

                    if (bid >= 0)
                    {
                        Block b = Layer.AllBlocks[bid];

                        b.Up(this, BottomDetector[j], x, y, layers[i]);
                    }
                }
            }
        }
    }
    public void ApplyMovement()
    {
        ApplyXSpeed();
        ApplyXGravity();
        ApplyXAcceleration();
        ApplyXFriction();
        ApplyMinMaxX();
        ApplyYSpeed();
        ApplyYGravity();
        ApplyYAcceleration();
        ApplyYFriction();
        ApplyMinMaxY();
    }
    public void ApplyXSpeed()
    {
        X += XSpeed * DeltaTime * PixelPerMeter;
    }
    public void ApplyYSpeed()
    {
        Y += YSpeed * DeltaTime * PixelPerMeter;
    }
    public void ApplyXGravity()
    {
        XSpeed -= XGravity * DeltaTime;
    }
    public void ApplyYGravity()
    {
        YSpeed -= YGravity * DeltaTime;
    }
    public void ApplyXAcceleration()
    {
        XSpeed += XAcceleration * DeltaTime;
    }
    public void ApplyYAcceleration()
    {
        YSpeed += YAcceleration * DeltaTime;
    }
    public void ApplyMinMaxX()
    {
        if (MaxXSpeed >= 0)
        {
            if (XSpeed > 0 && XSpeed > MaxXSpeed) XSpeed = MaxXSpeed;
            else if (XSpeed < 0 && XSpeed < -MaxXSpeed) XSpeed = -MaxXSpeed;
        }

        if (MinXSpeed > 0)
        {
            if (XSpeed > 0 && XSpeed < MinXSpeed) XSpeed = MinXSpeed;
            else if (XSpeed < 0 && XSpeed > -MinXSpeed) XSpeed = -MinXSpeed;
        }

        if (XSpeed > LimitSpeedInMeters) XSpeed = LimitSpeedInMeters;
        else if (XSpeed < -LimitSpeedInMeters) XSpeed = -LimitSpeedInMeters;
    }
    public void ApplyMinMaxY()
    {
        if (MaxYSpeed >= 0)
        {
            if (YSpeed > 0 && YSpeed > MaxYSpeed) YSpeed = MaxYSpeed;
            else if (YSpeed < 0 && YSpeed < -MaxYSpeed) YSpeed = -MaxYSpeed;
        }

        if (MinYSpeed > 0)
        {
            if (YSpeed > 0 && YSpeed < MinYSpeed) YSpeed = MinYSpeed;
            else if (YSpeed < 0 && YSpeed > -MinYSpeed) YSpeed = -MinYSpeed;
        }

        if (YSpeed > LimitSpeedInMeters) YSpeed = LimitSpeedInMeters;
        else if (YSpeed < -LimitSpeedInMeters) YSpeed = -LimitSpeedInMeters;
    }
    public void ApplyXFriction()
    {
        if (XAcceleration != 0 || XFriction <= 0) return;

        float fr = -Mathf.Sign(XSpeed) * XFriction;

        XSpeed += fr * DeltaTime;

        if (Mathf.Sign(XSpeed) == Mathf.Sign(fr))
            XSpeed = 0;
    }
    public void ApplyYFriction()
    {
        if (YAcceleration != 0 || YFriction <= 0) return;

        float fr = -Mathf.Sign(YSpeed) * YFriction;

        YSpeed += fr * DeltaTime;

        if (Mathf.Sign(YSpeed) == Mathf.Sign(fr))
            YSpeed = 0;
    }
}