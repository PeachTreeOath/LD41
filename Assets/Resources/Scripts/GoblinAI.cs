using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class GoblinAI : MonoBehaviour, IGlobalTimedObject {

    public List<AiAnimation> animations;
    
    private AiAnimation currAnim = null;
    private float animTimer = 0f;
    private bool playingAnim = false;

    void Start() {
        GlobalTimer.instance.RegisterObject(this);
        PlayAnimation(GetAnimationById("WarmUp"));
    }

    void PlayAnimation(AiAnimation anim) {
        currAnim = anim;
        animTimer = 0;
        playingAnim = true;
    }

    void StopAnimation() {
        playingAnim = false;
    }

    public void ManualUpdate(float deltaTime) {
        if(playingAnim && currAnim != null) {
            var nextTime = animTimer + deltaTime;
            var keyframes = currAnim.keyframes;

            // HACK evaluate events first
            foreach(var keyframe in keyframes) {
                if(keyframe.timestamp > animTimer && keyframe.timestamp <= nextTime && keyframe.type == KeyFrame.Type.Event) {
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
                        lastNonEventKeyframe = currAnim.keyframes[i];
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

            animTimer = nextTime;
        }
    }

    protected void OnAnimEvent(string eventName) {
        Debug.Log(eventName);
    }

    protected void UpdateAnimValue(float value) {
    }

    protected AiAnimation GetAnimationById(string id) {
        foreach(var anim in animations) {
            if(anim.id == id) {
                return anim;
            }
        }

        return null;
    }
}
