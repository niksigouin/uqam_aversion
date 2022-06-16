using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIKController : MonoBehaviour
{
    // Start is called before the first frame update
    protected Animator _animator;
    
    public bool ikActive = false;

    [SerializeField] private Transform lookObj = null;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float weight;
    // void Start()
    // {
    //     _animator = GetComponent<Animator>();
    // }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => SceneChangeController.Instance != null);
        _animator = GetComponent<Animator>();
        lookObj = SceneChangeController.Instance.ikTarget;
    }
    
    private void OnAnimatorIK(int layerIndex)
    {
        if (_animator)
        {
            if (ikActive)
            {
                if (lookObj != null)
                {
                    _animator.SetLookAtWeight(weight);
                    _animator.SetLookAtPosition(lookObj.position);
                }
            }
            else
            {
                _animator.SetLookAtWeight(0);
            }
        }
    }
}
