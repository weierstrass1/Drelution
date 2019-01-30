using UnityEngine;
using System;

[Serializable]
public delegate void action();

[Serializable]
public class animation
{
    public static float FPS = 60;
    public float AnimationReduction= 1;
    public string name;
    public frameList[] frames;
    public int[] times;
    public int[] nexts;
    public bool[] xFlips;
    public bool[] yFlips;
    public bool canChangeFlipX = false, canChangeFlipY = false;
    public Action[] startAction = null, endAction = null;
    public Action[] StartAction
    {
        get
        {
            if (startAction == null)
            {
                startAction = new Action[frames.Length];
            }
            return startAction;
        }
        set
        {
            startAction = value;
        }
    }
    public Action[] EndAction
    {
        get
        {
            if (endAction == null)
            {
                endAction = new Action[frames.Length];
            }
            return endAction;
        }
        set
        {
            endAction = value;
        }
    }
    public int index;
    float currentTime = 0;
    public int currentIndex = 0;
    public AnimatedObject owner;
    public bool affectedByReduction = true;
    public bool fold;
    public bool[] foldFrames;
    bool starting = false;

    public animation(AnimatedObject Owner,int Index)
    {
        owner = Owner;
        index = Index;
    }

    public void startAnimation(int index)
    {
        starting = true;
        UseFrame(index);
    }

    public void startAnimationFromAnotherAnimation(animation animation)
    {
        currentIndex = animation.currentIndex;
        UseFrame(currentIndex);
        currentTime = animation.currentTime;
    }

    public void UseFrame(int index)
    {
        if (!starting && EndAction != null && EndAction[currentIndex] != default(Action))
        {
            try
            {
                EndAction[currentIndex]();
            }
            catch
            {
                EndAction[currentIndex] = default(Action);
            }
        }
        starting = false;
        currentIndex = index;
        currentTime = 0;

        if (xFlips != null && canChangeFlipX)
            owner.FlipX = owner.FlipX ^ xFlips[currentIndex];
        if (yFlips != null && canChangeFlipY)
            owner.FlipY = owner.FlipY ^ yFlips[currentIndex];

        if(frames!=null)
        {
            if (currentIndex >= frames.Length || currentIndex < 0)
            {
                Debug.Log(name + ": " + currentIndex);
            }
            foreach (frame f in frames[currentIndex].frames)
            {
                if (f != null)
                {
                    f.Use();
                }
            }  
        }


        if (StartAction != null && StartAction[currentIndex] != default(Action))
        {
            try
            {
                StartAction[currentIndex]();
            }
            catch
            {
                StartAction[currentIndex] = default(Action);
            }
        }
    }

    public bool FrameIsFinished(int frame)
    {
        if (currentIndex != frame) return false;
        return isFinished();
    }

    public bool FrameIsFinished(int frame,int FPS)
    {
        if (currentIndex != frame) return false;
        return isFinished(FPS);
    }

    // Update is called once per frame
    public void excecuteAnimation()
    {
        float maxTime = FPS / times[currentIndex];

        float checkTime = AnimationReduction / maxTime;
        if (maxTime == 0) checkTime = float.MaxValue;

        if(currentTime >= checkTime)
        {
            UseFrame(nexts[currentIndex]);
            currentTime = 0;
        }
        else
        {
            currentTime += MobileObject.DeltaTime;
        }
    }

    public void excecuteAnimation(int FPS)
    {
        if (affectedByReduction)
        {
            float maxTime = FPS / (float)times[currentIndex];
            float checkTime = 1 / maxTime;
            if (maxTime == 0) checkTime = float.MaxValue;

            currentTime += Time.deltaTime;
            if (currentTime >= checkTime)
            {
                UseFrame(nexts[currentIndex]);
            }

        }
    }

    public void excecuteAnimationWithoutAnimReduction(int FPS)
    {
        float maxTime = FPS / (float)times[currentIndex];
        float checkTime = 1 / maxTime;
        if (maxTime == 0) checkTime = float.MaxValue;

        currentTime += Time.deltaTime;
        if (currentTime >= checkTime)
        {
            UseFrame(nexts[currentIndex]);
        }
    }

    public bool isFinished()
    {
        if (affectedByReduction)
        {
            float maxTime = FPS / (float)times[currentIndex];
            float checkTime = 1 / maxTime;
            if (maxTime == 0) checkTime = float.MaxValue;

            if (currentTime + Time.deltaTime >= checkTime)
            {
                return true;
            }

        }
        return false;
    }

    public bool isFinished(int FPS)
    {
        if (affectedByReduction)
        {
            float maxTime = FPS / (float)times[currentIndex];
            float checkTime = 1 / maxTime;
            if (maxTime == 0) checkTime = float.MaxValue;

            if (currentTime + Time.deltaTime >= checkTime)
            {
                return true;
            }

        }
        return false;
    }
}
