using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngearStates
{
    public const int Idle = 0;
    public const int Break = 1;
    public const int Walk = 2;
    public const int Jump = 3;
    public const int Fall = 4;
}

public class EngearAnimations
{
    public const int Idle = 0;
    public const int Walk = 1;
    public const int Run = 2;
    public const int Jump = 3;
    public const int JumpSide = 4;
    public const int Fall = 5;
    public const int FallSide = 6;
    public const int IdleFallGround = 7;
    public const int WalkFallGround = 8;
    public const int RunFallGround = 9;
}

public class Engear : StateMachineObject
{
    int selectedAnimation = 0;
    float noRunTimer = 0;
    public float NoRunMaxTime = 0.1f;
    public float WalkSpeed = 10;
    public float RunSpeed = 20;
    public float Accel = 10;
    public float JumpSpeed = 30;
    public float JumpAccel = 5;
    public float JumpXSpeedMin = 5;
    public float NormalFriction = 40;
    public float GravityReduce = 0.5f;

    // Use this for initialization
    public void Start()
    {
        StatesStarts = new Action[10];
        StatesStarts[EngearStates.Idle] = idleStart;
        StatesStarts[EngearStates.Break] = breakStart;
        StatesStarts[EngearStates.Walk] = walkStart;
        StatesStarts[EngearStates.Jump] = jumpStart;
        StatesStarts[EngearStates.Fall] = fallStart;

        States = new Action[10];
        States[EngearStates.Idle] = idle;
        States[EngearStates.Break] = breakst;
        States[EngearStates.Walk] = walk;
        States[EngearStates.Jump] = jump;
        States[EngearStates.Fall] = fall;

        YGravity = DefaultGravity;
    }
    //###############################################################################
    //###############################################################################
    //###############################################################################
    //###############################################################################
    #region States Starts
    void idleStart()
    {
        if (selectedAnimation != EngearAnimations.Idle)
        {
            AnimationComponent.animations[EngearAnimations.Idle].startAnimation(AnimationComponent.animations[0].currentIndex);
            selectedAnimation = EngearAnimations.Idle;
        }
        GuidedByTerrain = true;
    }
    void breakStart()
    {
        if (selectedAnimation != EngearAnimations.Idle)
        {
            AnimationComponent.animations[EngearAnimations.Idle].startAnimation(AnimationComponent.animations[0].currentIndex);
            selectedAnimation = EngearAnimations.Idle;
        }
        XAcceleration = 0;
        GuidedByTerrain = true;
    }
    void walkStart()
    {
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            if (selectedAnimation != EngearAnimations.Walk)
            {
                FlipX = true;
                selectedAnimation = EngearAnimations.Walk;
                AnimationComponent.animations[EngearAnimations.Walk].startAnimation(0);
            }
            XSpeed = -WalkSpeed;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (selectedAnimation != EngearAnimations.Walk)
            {
                FlipX = false;
                selectedAnimation = EngearAnimations.Walk;
                AnimationComponent.animations[EngearAnimations.Walk].startAnimation(0);
            }
            XSpeed = WalkSpeed;
        }
        GuidedByTerrain = true;
    }
    void jumpStart()
    {
        if ((FlipX && XSpeed < 0 && XSpeed <= -JumpXSpeedMin) ||
            (!FlipX && XSpeed > 0 && XSpeed >= JumpXSpeedMin))
            selectedAnimation = EngearAnimations.JumpSide;
        else
            selectedAnimation = EngearAnimations.Jump;

        AnimationComponent.animations[selectedAnimation].startAnimation(0);
        YSpeed = JumpSpeed;
        XAcceleration = 0;
        GuidedByTerrain = false;
    }
    void fallStart()
    {
        if ((FlipX && XSpeed < 0 && XSpeed <= -JumpXSpeedMin) ||
            (!FlipX && XSpeed > 0 && XSpeed >= JumpXSpeedMin))
            selectedAnimation = EngearAnimations.FallSide;
        else
            selectedAnimation = EngearAnimations.Fall;

        AnimationComponent.animations[selectedAnimation].startAnimation(0);
        XAcceleration = 0;
        GuidedByTerrain = false;
    }
    #endregion
    //###############################################################################
    //###############################################################################
    //###############################################################################
    //###############################################################################
    #region States
    void idle()
    {
        if(selectedAnimation == EngearAnimations.IdleFallGround &&
            AnimationComponent.animations[selectedAnimation].FrameIsFinished(2))
        {
            selectedAnimation = EngearAnimations.Idle;
            AnimationComponent.animations[selectedAnimation].startAnimation(0);
        }
        if (Input.GetAxisRaw("Horizontal") != 0)
            StartState(EngearStates.Walk);
        if (Input.GetButtonDown("Button2") && BlockedFromBelow)
            StartState(EngearStates.Jump);
        if (BlockedFromBelow)
            XFriction = NormalFriction;
        else
            XFriction = 0;

        ApplyMovement();
        ApplyLayerInteraction();

        AnimationComponent.animations[selectedAnimation].excecuteAnimation();
    }

    void breakst()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 &&
            XSpeed <= WalkSpeed && XSpeed >= -WalkSpeed)
            StartState(EngearStates.Walk);
        if (Input.GetButtonDown("Button2") && BlockedFromBelow)
            StartState(EngearStates.Jump);
        if (BlockedFromBelow)
            XFriction = NormalFriction;
        else
            XFriction = 0;

        ApplyMovement();
        ApplyLayerInteraction();

        if (XSpeed == 0)
            StartState(EngearStates.Idle);

        AnimationComponent.animations[selectedAnimation].excecuteAnimation();
    }

    void walk()
    {
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            noRunTimer = 0;
            if (XSpeed > WalkSpeed)
                XSpeed = -XSpeed;
            if (XSpeed > -WalkSpeed)
                XSpeed = -WalkSpeed;

            if (Input.GetButton("Button1"))
            {
                XAcceleration = -Accel;
                if(selectedAnimation == EngearAnimations.WalkFallGround ||
                    selectedAnimation == EngearAnimations.RunFallGround)
                {
                    if (AnimationComponent.animations[selectedAnimation].FrameIsFinished(2))
                    {
                        if (XSpeed < -(WalkSpeed + RunSpeed) / 2)
                        {
                            selectedAnimation = EngearAnimations.Run;
                            AnimationComponent.animations[selectedAnimation].startAnimation(3);
                        }
                        else
                        {
                            selectedAnimation = EngearAnimations.Walk;
                            AnimationComponent.animations[selectedAnimation].startAnimation(3);
                        }
                    }
                    else
                    {
                        if (selectedAnimation != EngearAnimations.RunFallGround && 
                            XSpeed < -(WalkSpeed + RunSpeed) / 2)
                        {
                            AnimationComponent.animations[EngearAnimations.RunFallGround].
                                startAnimationFromAnotherAnimation(AnimationComponent.animations[selectedAnimation]);
                            selectedAnimation = EngearAnimations.RunFallGround;
                        }
                        else if (selectedAnimation != EngearAnimations.WalkFallGround &&
                            XSpeed > -(WalkSpeed + RunSpeed) / 2)
                        {
                            AnimationComponent.animations[EngearAnimations.WalkFallGround].
                                startAnimationFromAnotherAnimation(AnimationComponent.animations[selectedAnimation]);
                            selectedAnimation = EngearAnimations.WalkFallGround;
                        }
                    }
                }
                else if (selectedAnimation != EngearAnimations.Run && XSpeed < -(WalkSpeed + RunSpeed) / 2)
                {
                    selectedAnimation = EngearAnimations.Run;
                    AnimationComponent.animations[selectedAnimation].startAnimation(0);
                }
            }
            else
            {
                XAcceleration = 0;
                if (selectedAnimation == EngearAnimations.WalkFallGround ||
                    selectedAnimation == EngearAnimations.RunFallGround)
                {
                    if (AnimationComponent.animations[selectedAnimation].FrameIsFinished(2))
                    {
                        if (XSpeed < -(WalkSpeed + RunSpeed) / 2)
                        {
                            selectedAnimation = EngearAnimations.Run;
                            AnimationComponent.animations[selectedAnimation].startAnimation(3);
                        }
                        else
                        {
                            selectedAnimation = EngearAnimations.Walk;
                            AnimationComponent.animations[selectedAnimation].startAnimation(3);
                        }
                    }
                    else
                    {
                        if (selectedAnimation != EngearAnimations.RunFallGround &&
                            XSpeed < -(WalkSpeed + RunSpeed) / 2)
                        {
                            AnimationComponent.animations[EngearAnimations.RunFallGround].
                                startAnimationFromAnotherAnimation(AnimationComponent.animations[selectedAnimation]);
                            selectedAnimation = EngearAnimations.RunFallGround;
                        }
                        else if (selectedAnimation != EngearAnimations.WalkFallGround &&
                            XSpeed > -(WalkSpeed + RunSpeed) / 2)
                        {
                            AnimationComponent.animations[EngearAnimations.WalkFallGround].
                                startAnimationFromAnotherAnimation(AnimationComponent.animations[selectedAnimation]);
                            selectedAnimation = EngearAnimations.WalkFallGround;
                        }
                    }
                }
                else if (selectedAnimation != EngearAnimations.Walk && XSpeed > -(WalkSpeed + RunSpeed) / 2)
                {
                    selectedAnimation = EngearAnimations.Walk;
                    AnimationComponent.animations[selectedAnimation].startAnimation(0);
                }
            }
            FlipX = true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            noRunTimer = 0;
            if (XSpeed < -WalkSpeed)
                XSpeed = -XSpeed;
            if (XSpeed < WalkSpeed)
                XSpeed = WalkSpeed;
            if (Input.GetButton("Button1"))
            {
                XAcceleration = Accel;
                if (selectedAnimation == EngearAnimations.WalkFallGround ||
                    selectedAnimation == EngearAnimations.RunFallGround)
                {
                    if (AnimationComponent.animations[selectedAnimation].FrameIsFinished(2))
                    {
                        if (XSpeed > (WalkSpeed + RunSpeed) / 2)
                        {
                            selectedAnimation = EngearAnimations.Run;
                            AnimationComponent.animations[selectedAnimation].startAnimation(3);
                        }
                        else
                        {
                            selectedAnimation = EngearAnimations.Walk;
                            AnimationComponent.animations[selectedAnimation].startAnimation(3);
                        }
                    }
                    else
                    {
                        if (selectedAnimation != EngearAnimations.RunFallGround &&
                            XSpeed > (WalkSpeed + RunSpeed) / 2)
                        {
                            AnimationComponent.animations[EngearAnimations.RunFallGround].
                                startAnimationFromAnotherAnimation(AnimationComponent.animations[selectedAnimation]);
                            selectedAnimation = EngearAnimations.RunFallGround;
                        }
                        else if (selectedAnimation != EngearAnimations.WalkFallGround &&
                            XSpeed < (WalkSpeed + RunSpeed) / 2)
                        {
                            AnimationComponent.animations[EngearAnimations.WalkFallGround].
                                startAnimationFromAnotherAnimation(AnimationComponent.animations[selectedAnimation]);
                            selectedAnimation = EngearAnimations.WalkFallGround;
                        }
                    }
                }
                else if (selectedAnimation != EngearAnimations.Run && XSpeed > (WalkSpeed + RunSpeed) / 2)
                {
                    selectedAnimation = EngearAnimations.Run;
                    AnimationComponent.animations[selectedAnimation].startAnimation(0);
                }
            }
            else
            {
                XAcceleration = 0;
                if (selectedAnimation == EngearAnimations.WalkFallGround ||
                    selectedAnimation == EngearAnimations.RunFallGround)
                {
                    if (AnimationComponent.animations[selectedAnimation].FrameIsFinished(2))
                    {
                        if (XSpeed > (WalkSpeed + RunSpeed) / 2)
                        {
                            selectedAnimation = EngearAnimations.Run;
                            AnimationComponent.animations[selectedAnimation].startAnimation(3);
                        }
                        else
                        {
                            selectedAnimation = EngearAnimations.Walk;
                            AnimationComponent.animations[selectedAnimation].startAnimation(3);
                        }
                    }
                    else
                    {
                        if (selectedAnimation != EngearAnimations.RunFallGround &&
                            XSpeed > (WalkSpeed + RunSpeed) / 2)
                        {
                            AnimationComponent.animations[EngearAnimations.RunFallGround].
                                startAnimationFromAnotherAnimation(AnimationComponent.animations[selectedAnimation]);
                            selectedAnimation = EngearAnimations.RunFallGround;
                        }
                        else if (selectedAnimation != EngearAnimations.WalkFallGround &&
                            XSpeed < (WalkSpeed + RunSpeed) / 2)
                        {
                            AnimationComponent.animations[EngearAnimations.WalkFallGround].
                                startAnimationFromAnotherAnimation(AnimationComponent.animations[selectedAnimation]);
                            selectedAnimation = EngearAnimations.WalkFallGround;
                        }
                    }
                }
                else if (selectedAnimation != EngearAnimations.Walk && XSpeed < (WalkSpeed + RunSpeed) / 2)
                {
                    selectedAnimation = EngearAnimations.Walk;
                    AnimationComponent.animations[selectedAnimation].startAnimation(0);
                }
            }
            FlipX = false;
        }
        else
        {
            if (Input.GetButton("Button1"))
            {
                if (noRunTimer >= NoRunMaxTime) StartState(EngearStates.Break);
                else noRunTimer += DeltaTime;
            }
            else
                StartState(EngearStates.Break);
        }
        if (Input.GetButtonDown("Button2") && BlockedFromBelow)
            StartState(EngearStates.Jump);
        if (BlockedFromBelow)
            XFriction = NormalFriction;
        else
            XFriction = 0;

        float lxspeed = XSpeed;
        ApplyMovement();
        ApplyLayerInteraction();

        if (lxspeed < 0 && lxspeed >= -RunSpeed && XSpeed < -RunSpeed) XSpeed = -RunSpeed;
        if (lxspeed > 0 && lxspeed <= RunSpeed && XSpeed > RunSpeed) XSpeed = RunSpeed;

        AnimationComponent.animations[selectedAnimation].excecuteAnimation();
    }
    void jump()
    {
        if ((FlipX && XSpeed < 0 && XSpeed <= -JumpXSpeedMin) ||
            (!FlipX && XSpeed > 0 && XSpeed >= JumpXSpeedMin))
        {
            if(selectedAnimation != EngearAnimations.JumpSide)
            {
                selectedAnimation = EngearAnimations.JumpSide;
                AnimationComponent.animations[selectedAnimation].
                    startAnimationFromAnotherAnimation(
                    AnimationComponent.animations[EngearAnimations.Jump]);
            }
        }
        else
        {
            if (selectedAnimation != EngearAnimations.Jump)
            {
                selectedAnimation = EngearAnimations.Jump;
                AnimationComponent.animations[selectedAnimation].
                    startAnimationFromAnotherAnimation(
                    AnimationComponent.animations[EngearAnimations.JumpSide]);
            }
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
            XAcceleration = -JumpAccel;
        else if (Input.GetAxisRaw("Horizontal") > 0)
            XAcceleration = JumpAccel;
        else
            XAcceleration = 0;

        YGravity = DefaultGravity;

        if (Input.GetButton("Button2"))
            YGravity = YGravity * GravityReduce;

        if (BlockedFromBelow)
            XFriction = NormalFriction;
        else
            XFriction = 0;
        

        float lxspeed = XSpeed;
        ApplyMovement();
        ApplyLayerInteraction();
        if (lxspeed < 0 && lxspeed >= -JumpXSpeedMin && XSpeed < -JumpXSpeedMin) XSpeed = -JumpXSpeedMin;
        if (lxspeed > 0 && lxspeed <= JumpXSpeedMin && XSpeed > JumpXSpeedMin) XSpeed = JumpXSpeedMin;

        if (YSpeed < 0)
            StartState(EngearStates.Fall);
        if (Input.GetButton("Button1") && XSpeed > RunSpeed) XSpeed = RunSpeed;
        else if (Input.GetButton("Button1") && XSpeed < -RunSpeed) XSpeed = -RunSpeed;
        if (!Input.GetButton("Button1") && XSpeed > WalkSpeed) XSpeed = WalkSpeed;
        else if (!Input.GetButton("Button1") && XSpeed < -WalkSpeed) XSpeed = -WalkSpeed;

        AnimationComponent.animations[selectedAnimation].excecuteAnimation();
    }
    void fall()
    {
        if ((FlipX && XSpeed < 0 && XSpeed <= -JumpXSpeedMin) ||
            (!FlipX && XSpeed > 0 && XSpeed >= JumpXSpeedMin))
        {
            if (selectedAnimation != EngearAnimations.FallSide)
            {
                selectedAnimation = EngearAnimations.FallSide;
                AnimationComponent.animations[selectedAnimation].
                    startAnimationFromAnotherAnimation(
                    AnimationComponent.animations[EngearAnimations.Fall]);
            }
        }
        else
        {
            if (selectedAnimation != EngearAnimations.Fall)
            {
                selectedAnimation = EngearAnimations.Fall;
                AnimationComponent.animations[selectedAnimation].
                    startAnimationFromAnotherAnimation(
                    AnimationComponent.animations[EngearAnimations.FallSide]);
            }
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
            XAcceleration = -JumpAccel;
        else if (Input.GetAxisRaw("Horizontal") > 0)
            XAcceleration = JumpAccel;
        else
            XAcceleration = 0;

        YGravity = DefaultGravity;

        if (Input.GetButton("Button2"))
            YGravity = YGravity * GravityReduce;

        if (BlockedFromBelow)
        {
            XFriction = NormalFriction;
            if (XSpeed <= JumpXSpeedMin && XSpeed >= -JumpXSpeedMin)
            {
                StartState(EngearStates.Idle);
                AnimationComponent.animations[EngearAnimations.IdleFallGround].startAnimation(0);
                selectedAnimation = EngearAnimations.IdleFallGround;
            }
            else if (XSpeed <= WalkSpeed && XSpeed >= -WalkSpeed)
            {
                StartState(EngearStates.Walk);
                AnimationComponent.animations[EngearAnimations.WalkFallGround].startAnimation(0);
                selectedAnimation = EngearAnimations.WalkFallGround;
            }
            else
            {
                StartState(EngearStates.Walk);
                AnimationComponent.animations[EngearAnimations.RunFallGround].startAnimation(0);
                selectedAnimation = EngearAnimations.RunFallGround;
            }

        }
            else
                XFriction = 0;

        float lxspeed = XSpeed;
        ApplyMovement();
        ApplyLayerInteraction();
        if (lxspeed < 0 && lxspeed >= -JumpXSpeedMin && XSpeed < -JumpXSpeedMin) XSpeed = -JumpXSpeedMin;
        if (lxspeed > 0 && lxspeed <= JumpXSpeedMin && XSpeed > JumpXSpeedMin) XSpeed = JumpXSpeedMin;

        if (Input.GetButton("Button1") && XSpeed > RunSpeed) XSpeed = RunSpeed;
        else if (Input.GetButton("Button1") && XSpeed < -RunSpeed) XSpeed = -RunSpeed;
        if (!Input.GetButton("Button1") && XSpeed > WalkSpeed) XSpeed = WalkSpeed;
        else if (!Input.GetButton("Button1") && XSpeed < -WalkSpeed) XSpeed = -WalkSpeed;

        AnimationComponent.animations[selectedAnimation].excecuteAnimation();
    }
    #endregion
}
