using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;
using System.Linq;

public class AiAnimationController : MonoBehaviour, IGlobalTimedObject {

    public List<AiAnimation> animations;
    public AnimEventEvent animEventEvent;
    public AnimValueEvent animValueEvent;
    
    private AiAnimation currAnim = null;
    private float animTimer = 0f;
    private bool playingAnim = false;
    private bool changingAnimationFromEvent = false;

    void Start() {
        GlobalTimer.instance.RegisterObject(this);
        if (animEventEvent == null) animEventEvent = new AnimEventEvent();
        if (animValueEvent == null) animValueEvent = new AnimValueEvent();

        foreach(var anim in animations) {
            anim.keyframes.Sort();
        }
    }

    public void PlayAnimation(string id) {
        currAnim = GetAnimationById(id);
        animTimer = 0;
        playingAnim = true;
    }

    public void StopAnimation() {
        playingAnim = false;
    }

    public bool InterpretCommand(string command, string argument) {
        if(command.ToLower() == "play") {
            PlayAnimation(argument.Trim());
            changingAnimationFromEvent = true;
            return true;
        }

        return false;
    }

    public void ManualUpdate(float deltaTime) {
        if(playingAnim && currAnim != null) {
            var nextTime = animTimer + deltaTime;
            var keyframes = currAnim.keyframes;

            // HACK evaluate events first
            foreach(var keyframe in keyframes) {
                if((animTimer == 0 || keyframe.timestamp > animTimer) && keyframe.timestamp <= nextTime && keyframe.type == KeyFrame.Type.Event) {
                    OnAnimEvent(keyframe.eventName);
                }
            }

            KeyFrame nextKeyframe = null;
            KeyFrame lastNonEventKeyframe = null;

            // find first, non-event keyframe after nextTime
            var i = keyframes.Count - 1;
            for(; i >= 0; i--) {
                if(keyframes[i].type != KeyFrame.Type.Event) {
                    if(lastNonEventKeyframe == null) {
                        lastNonEventKeyframe = keyframes[i];
                    }

                    if(keyframes[i].timestamp < nextTime) {
                        break;
                    } else {
                        nextKeyframe = keyframes[i];
                    }
                }
            }

            // look back to prev non-event keyframe
            var prevKeyFrame = nextKeyframe;
            for(; i >= 0; i--) {
                if(keyframes[i].type != KeyFrame.Type.Event) {
                    if(lastNonEventKeyframe == null) {
                        lastNonEventKeyframe = keyframes[i];
                    }

                    prevKeyFrame = keyframes[i];
                    break;
                }
            }

            // ensure we have a pair of keyframes
            if(nextKeyframe == null) {
                nextKeyframe = lastNonEventKeyframe;
            }

            if (prevKeyFrame == null) {
                prevKeyFrame = nextKeyframe; 
            }

            // determine value
            if(nextKeyframe.type == KeyFrame.Type.Lerp) {
                var denom = nextKeyframe.timestamp - prevKeyFrame.timestamp;
                if(denom == 0) {
                    UpdateAnimValue(nextKeyframe.value);
                } else {
                    var t = (nextTime - prevKeyFrame.timestamp) / denom;
                    var value = Mathf.Lerp(prevKeyFrame.value, nextKeyframe.value, t);

                    UpdateAnimValue(value);
                }
            } else {
                // TODO figure out steps 
            }

            if(changingAnimationFromEvent) {
                changingAnimationFromEvent = false;
            } else {
                animTimer = nextTime;
            }
        }
    }

    protected void OnAnimEvent(string eventName) {
        animEventEvent.Invoke(eventName);
    }

    protected void UpdateAnimValue(float value) {
        animValueEvent.Invoke(value);
    }

    protected AiAnimation GetAnimationById(string id) {
        foreach(var anim in animations) {
            if(anim.id == id) {
                return anim;
            }
        }

        Debug.LogErrorFormat("Could not find animation with id: '{0}'", id);
        return null;
    }
}
