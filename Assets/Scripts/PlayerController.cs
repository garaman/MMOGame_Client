using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class PlayerController : MonoBehaviour
{
    public Grid _grid;
    [SerializeField]
    float _speed = 5.0f;

    Vector3Int _cellPos = Vector3Int.zero;
    bool _isMoving = false;

    Animator _animator;
    SpriteRenderer _spriteRenderer;

    MoveDir _dir = MoveDir.None;
    public MoveDir Dir 
    { 
        get { return _dir; } 
        set 
        { 
            if(_dir == value) { return; }

            switch (value)
            {
                case MoveDir.Up:
                    _animator.Play("Walk_Back");
                    _spriteRenderer.flipX = false;
                    break;
                case MoveDir.Down:
                    _animator.Play("Walk_Front");
                    _spriteRenderer.flipX = false;
                    break;
                case MoveDir.Left:
                    _animator.Play("Walk_Side");
                    _spriteRenderer.flipX = true;
                    break;
                case MoveDir.Right:
                    _animator.Play("Walk_Side");
                    _spriteRenderer.flipX = false;
                    break;
                case MoveDir.None:
                    if(_dir == MoveDir.Up)
                    {
                        _animator.Play("Idle_Back");
                        _spriteRenderer.flipX = false;
                    }
                    else if (_dir == MoveDir.Down)
                    {
                        _animator.Play("Idle_Front");
                        _spriteRenderer.flipX = false;
                    } 
                    else if (_dir == MoveDir.Left)
                    {
                        _animator.Play("Idle_Side");
                        _spriteRenderer.flipX = true;
                    }
                    else
                    {
                        _animator.Play("Idle_Side");
                        _spriteRenderer.flipX = false;
                    }

                    break;
            }
            
            _dir = value;
        } 
    }

    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Vector3 pos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0f);
        transform.position = pos;
    }

    
    void Update()
    {
        GetDirInput();
        UpdatePosition();
        UpdateIsMoving();
    }

    void GetDirInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {            
            Dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Dir = MoveDir.Down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Dir = MoveDir.Right;
        }
        else
        {
            Dir = MoveDir.None;
        }
    }

    void UpdatePosition()
    {
        if (_isMoving == false) { return; }

        Vector3 desPos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0f);
        Vector3 moveDir = desPos - transform.position;

        // 도착 여부 체크
        float dist = moveDir.magnitude;
        if (dist < _speed * Time.deltaTime)
        {
            transform.position = desPos;
            _isMoving = false;
        }
        else
        {
            transform.position += moveDir.normalized * Time.deltaTime * _speed;
        }
    }

    void UpdateIsMoving()
    {
        if (_isMoving == false)
        {
            switch (Dir)
            {
                case MoveDir.Up:
                    _cellPos += Vector3Int.up;
                    _isMoving = true;
                    break;
                case MoveDir.Down:
                    _cellPos += Vector3Int.down;
                    _isMoving = true;
                    break;
                case MoveDir.Left:
                    _cellPos += Vector3Int.left;
                    _isMoving = true;
                    break;
                case MoveDir.Right:
                    _cellPos += Vector3Int.right;
                    _isMoving = true;
                    break;
                case MoveDir.None:
                    break;
            }
        }
    }
}
