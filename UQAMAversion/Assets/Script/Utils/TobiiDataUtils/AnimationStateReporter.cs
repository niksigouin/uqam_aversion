using System;
using System.Collections;
using UnityEngine;

namespace Script.Utils
{
    public class AnimationStateReporter : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => TobiiDataWriterController.Instance != null);
        }

        public void InformDataWriter(int info)
        {
            AnimatorClipInfo[] current_clip_info = animator.GetCurrentAnimatorClipInfo(0);
            TobiiDataWriterController.Instance.AnimationEvent?.Invoke(current_clip_info[0].clip.name, (CharacterAnimState)info);
        }
    }
}

public enum CharacterAnimState
{
    None = 0,
    AnimStart = 1,
    AnimStop = 2,
    Touch = 3,
    StopTouch = 4,
}