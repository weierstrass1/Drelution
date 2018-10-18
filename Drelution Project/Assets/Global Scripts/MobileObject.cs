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
    public const float PixelPerMeter = 9;
    public const float LimitSpeed = 900;
    public const float LimitDeltaTime = 1 / 60;
    public static readonly float LimitSpeedInMeters = LimitSpeed / PixelPerMeter;
    public static float DeltaTime
    {
        get
        {
            return Mathf.Min(LimitDeltaTime, Time.deltaTime);
        }
    }
    public int Mass { get; set; }
    public bool Solid { get; set; }
    public bool GuidedByTerrain { get; set; }
    public bool AffectedByTerrain { get; set; }
    public bool TerrainAngle { get; set; }
    public bool IgnoreBelowInteraction { get; set; }
    public bool BlockedAngleDetector { get; set; }
    public bool BlockedFromBelow { get; set; }
    public bool BlockedFromLeft { get; set; }
    public bool BlockedFromRight { get; set; }
    public bool BlockedFromAbove { get; set; }
    public Transform[] TopDetector { get; set; }
    public Transform[] BottomDetector { get; set; }
    public Transform[] LeftDetector { get; set; }
    public Transform[] RightDetector { get; set; }
    public Transform[] AngleDetector { get; set; }
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
    public float XSpeed { get; set; }
    public float MaxXSpeed { get; set; }
    public float MinXSpeed { get; set; }
    public float YSpeed { get; set; }
    public float MaxYSpeed { get; set; }
    public float MinYSpeed { get; set; }
    public float XAcceleration { get; set; }
    public float YAcceleration { get; set; }
    public float Angle { get; set; }
    public float AngularSpeed { get; set; }
    public float AngularAcceleration { get; set; }
    public float XFriction { get; set; }
    public float YFriction { get; set; }
    public float XGravity { get; set; }
    public float YGravity { get; set; }

    public void LayerInteractionX()
    {
        Layer[] layers = Level.Instance.Layers;

        float deltaXSp;

        for (int i = 0; i < layers.Length; i++)
        {
            deltaXSp = XSpeed - layers[i].XSpeed;

            if(deltaXSp > 0)
            {
                if (RightDetector != null && RightDetector.Length > 0)
                {
                    for (int j = 0; j < RightDetector.Length; j++)
                    {
                        float x = RightDetector[j].position.x;
                        float y = RightDetector[j].position.y;

                        int bid = layers[i].getBlock(x, y);

                        if (bid >= 0)
                        {
                            block b = Layer.AllBlocks[bid];

                            b.Left(this, RightDetector[j], x, y, layers[i]);
                        }
                    }
                }
            }
            else if(deltaXSp < 0)
            {
                if (LeftDetector != null && LeftDetector.Length > 0)
                {
                    for (int j = 0; j < LeftDetector.Length; j++)
                    {
                        float x = LeftDetector[j].position.x;
                        float y = LeftDetector[j].position.y;

                        int bid = layers[i].getBlock(x, y);

                        if (bid >= 0)
                        {
                            block b = Layer.AllBlocks[bid];

                            b.Right(this, LeftDetector[j], x, y, layers[i]);
                        }
                    }
                }
            }
        }
    }
    public void LayerInteractionY()
    {
        Layer[] layers = Level.Instance.Layers;

        float deltaYSp;

        for (int i = 0; i < layers.Length; i++)
        {
            deltaYSp = YSpeed - layers[i].YSpeed;

            if (deltaYSp > 0)
            {
                if (TopDetector != null && TopDetector.Length > 0) 
                {
                    for (int j = 0; j < TopDetector.Length; j++)
                    {
                        float x = TopDetector[j].position.x;
                        float y = TopDetector[j].position.y;

                        int bid = layers[i].getBlock(x, y);

                        if (bid >= 0)
                        {
                            block b = Layer.AllBlocks[bid];

                            b.Down(this, TopDetector[j], x, y, layers[i]);
                        }
                    }
                }
            }
            else if (deltaYSp < 0)
            {
                float deltaXSp = XSpeed - layers[i].XSpeed;
                int selAng = 0;
                IgnoreBelowInteraction = false;

                if (AngleDetector != null && AngleDetector.Length > 0)
                {
                    if (deltaXSp >= 0)
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

                    float x = AngleDetector[selAng].position.x;
                    float y = AngleDetector[selAng].position.y;

                    int bid = layers[i].getBlock(x, y);

                    if (bid >= 0)
                    {
                        block b = Layer.AllBlocks[bid];

                        b.AngleDetector(this, AngleDetector[selAng], x, y, layers[i]);
                    }
                }

                if (BottomDetector != null && BottomDetector.Length > 0) 
                {
                    for (int j = 0; j < BottomDetector.Length; j++)
                    {
                        float x = BottomDetector[j].position.x;
                        float y = BottomDetector[j].position.y;

                        int bid = layers[i].getBlock(x, y);

                        if (bid >= 0)
                        {
                            block b = Layer.AllBlocks[bid];

                            b.Up(this, BottomDetector[j], x, y, layers[i]);
                        }
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

        if (MinXSpeed >= 0)
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

        if (MinYSpeed >= 0)
        {
            if (YSpeed > 0 && YSpeed < MinYSpeed) YSpeed = MinYSpeed;
            else if (YSpeed < 0 && YSpeed > -MinYSpeed) YSpeed = -MinYSpeed;
        }

        if (YSpeed > LimitSpeedInMeters) YSpeed = LimitSpeedInMeters;
        else if (YSpeed < -LimitSpeedInMeters) YSpeed = -LimitSpeedInMeters;
    }
    public void ApplyXFriction()
    {
        if (XAcceleration != 0) return;

        float fr = -Mathf.Sign(XSpeed) * XFriction;

        XSpeed += fr * DeltaTime;

        if (Mathf.Sign(XSpeed) == Mathf.Sign(fr))
            XSpeed = 0;
    }
    public void ApplyYFriction()
    {
        if (YAcceleration != 0) return;

        float fr = -Mathf.Sign(YSpeed) * YFriction;

        YSpeed += fr * DeltaTime;

        if (Mathf.Sign(YSpeed) == Mathf.Sign(fr))
            YSpeed = 0;
    }
}