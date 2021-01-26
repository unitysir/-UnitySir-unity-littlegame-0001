using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSFramework
{
    public class SimpleMove : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;

        [Header("键盘输入")] [SerializeField] private KeyCode _keyForward = KeyCode.W;
        [SerializeField] private KeyCode _keyBackward = KeyCode.S;
        [SerializeField] private KeyCode _keyLeft = KeyCode.A;
        [SerializeField] private KeyCode _keyRight = KeyCode.D;

        [Header("角色方向控制变量")] [SerializeField] private Vector2 MoveDir = Vector2.zero; //移动方向
        [SerializeField] private Vector2 MoveDirNormalized = Vector2.zero;
        [SerializeField] private float TargetDir = 0f; //角色目标方向
        [SerializeField] private float walkSpeed = 2f; //行走速度
        private float SmoothTime = 0.1f;
        private float currentVelocity = 0;
        private float vMove = 0f;
        private float hMove = 0f;

        private void Awake()
        {
            _rigidbody = GameObject.Find("MainPlayer").GetComponent<Rigidbody>();
            _animator = GameObject.Find("MainPlayer/Player").GetComponent<Animator>();
        }

        private void Start()
        {
        }

        private void Update()
        {
            hMove = Input.GetAxis("Horizontal");
            vMove = Input.GetAxis("Vertical");
            SetMoveDir();
            SetMoveAni();
        }

        private void FixedUpdate()
        {
            PlayerMove();
        }

        /// <summary>
        /// 设置朝向
        /// </summary>
        void SetMoveDir()
        {
            MoveDir = new Vector2(hMove, vMove); //获取到输入的值
            MoveDirNormalized = MoveDir.normalized; //将值归一化
            TargetDir = Mathf.Atan2(MoveDirNormalized.x, MoveDirNormalized.y) * Mathf.Rad2Deg; //计算朝向目标的角度
            if (MoveDirNormalized != Vector2.zero) //当输入的值不为0时，转向目标角度
            {
                _rigidbody.transform.eulerAngles =
                    Vector3.up * Mathf.SmoothDampAngle(_rigidbody.transform.eulerAngles.y, TargetDir,
                        ref currentVelocity, SmoothTime);
            }
        }

        /// <summary>
        /// 设置移动动画
        /// </summary>
        void SetMoveAni()
        {
            _animator.SetFloat("forward", Mathf.Sqrt(hMove * hMove + vMove * vMove));
        }

        void PlayerMove()
        {
            _rigidbody.velocity = new Vector3(walkSpeed * hMove, 0, walkSpeed * vMove);
        }
    }
}