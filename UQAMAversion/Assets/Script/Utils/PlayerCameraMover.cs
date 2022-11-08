using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMover : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _inSpeed;
    [SerializeField] private float _outSpeed;
    [SerializeField] private float _waitTime;

    private Vector3 _initialPosition;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => SceneChangeController.Instance != null);
        _player = SceneChangeController.Instance.PlayerCamera;
    }

    //Move the camera to the target
    public void MoveCameraToTarget()
    {
        _initialPosition = _player.transform.position;
        LeanTween.move(_player, _target.transform.position, _inSpeed);
        StartCoroutine(ResetCameraToOrigin());
    }

    private IEnumerator ResetCameraToOrigin()
    {
        yield return new WaitForSeconds(_waitTime);
        LeanTween.move(_player, _initialPosition, _outSpeed);
    }
}
