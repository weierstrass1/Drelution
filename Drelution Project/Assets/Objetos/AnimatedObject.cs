using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AnimatedObject : MobileObject
{
    public bool FlipX
    {
        get
        {
            return flipX;
        }
        set
        {
            if(value != flipX)
            {
                flipX = value;
                foreach (Transform t in LeftDetector)
                {
                    t.localPosition = 
                        new Vector3(-t.localPosition.x, t.localPosition.y, t.localPosition.z);
                }
                foreach (Transform t in RightDetector)
                {
                    t.localPosition =
                        new Vector3(-t.localPosition.x, t.localPosition.y, t.localPosition.z);
                }
                foreach (Transform t in AngleDetector)
                {
                    t.localPosition =
                        new Vector3(-t.localPosition.x, t.localPosition.y, t.localPosition.z);
                }
                foreach (Transform t in BottomDetector)
                {
                    t.localPosition =
                        new Vector3(-t.localPosition.x, t.localPosition.y, t.localPosition.z);
                }
                foreach (Transform t in TopDetector)
                {
                    t.localPosition =
                        new Vector3(-t.localPosition.x, t.localPosition.y, t.localPosition.z);
                }
                Transform[] trs = LeftDetector;
                LeftDetector = RightDetector;
                RightDetector = trs;
                foreach(graphicComponent gc in GraphicComponents)
                {
                    gc.mainFlipX = flipX;
                }
            }

        }
    }

    private bool flipX;
    public bool FlipY;
    private bool flipY;
    public animationComponent AnimationComponent;
    public graphicComponent[] GraphicComponents;
}
