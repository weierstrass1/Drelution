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
    public const float pixelPerMeter = 9;

    [HideInInspector]
    public int mass;
    public int Mass
    {
        get { return mass; }
        set { mass = value; }
    }

    [HideInInspector]
    public bool solid = true;
    public bool Solid
    {
        get { return solid; }
        set { solid = value; }
    }

    public bool GuidedByTerrain = true;

    [HideInInspector]
    public bool blockedAngleDetector = true;
    public bool BlockedAngleDetector
    {
        get { return blockedAngleDetector; }
        set { blockedAngleDetector = value; }
    }

    public bool blockedFromBelow = true;
    public bool BlockedFromBelow
    {
        get { return blockedFromBelow; }
        set { blockedFromBelow = value; }
    }

    public bool IgnoreBelowInteraction = false;

    [HideInInspector]
    public bool blockedFromLeft;
    public bool BlockedFromLeft
    {
        get { return blockedFromLeft; }
        set
        {
            blockedFromLeft = value;
        }
    }

    [HideInInspector]
    public bool blockedFromRight;
    public bool BlockedFromRight
    {
        get { return blockedFromRight; }
        set { blockedFromRight = value; }
    }

    [HideInInspector]
    public bool blockedFromAbove;
    public bool BlockedFromAbove
    {
        get { return blockedFromAbove; }
        set { blockedFromAbove = value; }
    }

    public float x
    {
        get { return GetComponent<Transform>().position.x; }
        set
        {
            GetComponent<Transform>().position = new Vector3(
                value,
                GetComponent<Transform>().position.y,
                GetComponent<Transform>().position.z);
        }
    }
    public float y
    {
        get { return GetComponent<Transform>().position.y; }
        set
        {
            GetComponent<Transform>().position = new Vector3(
                GetComponent<Transform>().position.x,
                value,
                GetComponent<Transform>().position.z);
        }
    }
}