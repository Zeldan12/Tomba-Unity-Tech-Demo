using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Tomba _player;
    [SerializeField]
    private float _offset = 1, _movespeed = 0.1f, _heigthOffset = 1;
    [SerializeField]
    private float _wallTopLimit = 1, _wallBotLimit = 1, _wallOffset = 2;
    [SerializeField]
    private float _wallRotationDeggre;

    public float Offset { get => _offset; }
    public float Movespeed { get => _movespeed; }
    public float HeigthOffset { get => _heigthOffset; }
    public float WallTopLimit { get => _wallTopLimit; }
    public float WallBotLimit { get => _wallBotLimit; }
    public float WallOffSet { get => _wallOffset; }
    public float WallRotationDeggre { get => _wallRotationDeggre; }
    public Camera Camera { get => _camera; }
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        
    }

    private void FixedUpdate() {
        _player.CurrentState.CameraBehaviour(this);
    }

}
