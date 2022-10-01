using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Direction = GameLogic.Direction;

namespace Snake
{
    public class HeadMoving : MonoBehaviour
    {
        [SerializeField] private GameObject _bodyPrefab;

        private Direction _direction;
        private Queue<GameObject> _bodyCells;
        private Vector3 _previousHeadPosition;
        private bool _eating;

        private int _scores;

        private bool _borning => StarterLength > _bodyCells.Count;

        public int StarterLength { get; set; }
        public bool ManualControl { get; set; }

        public Action<GameObject> OnEating;
        public Action<int> OnScoreUp;

        /// <summary>
        /// Check collision vector3 with head and any body cells
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns>False if collisions not found</returns>
        public bool CheckCollision(Vector3 vector3)
        {
            return transform.position == vector3
                   || _bodyCells.Any(cell => cell.transform.position == vector3);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Food")
            {
                Eating();
                OnEating?.Invoke(col.gameObject);
            }
        }

        private void Start()
        {
            _direction = Direction.Up;
            _bodyCells = new Queue<GameObject>();
        }

        private void FixedUpdate()
        {
            Moving();
        }

        private void Update()
        {
            ReadInput();
        }

        private void Moving()
        {
            _previousHeadPosition = transform.position;
            if (_direction == Direction.Up)
            {
                transform.position += Vector3.up;
            }
            else if (_direction == Direction.Right)
            {
                transform.position += Vector3.right;
            }
            else if (_direction == Direction.Down)
            {
                transform.position += Vector3.down;
            }
            else if (_direction == Direction.Left)
            {
                transform.position += Vector3.left;
            }

            if (_eating)
            {
                GrowUp();
            }
            else if (_borning)
            {
                GrowUp();
            }
            else if (_bodyCells != null && _bodyCells.Count > 0)
            {
                var cell = _bodyCells.Dequeue();
                cell.transform.position = _previousHeadPosition;
                _bodyCells.Enqueue(cell);
            }
        }

        private void GrowUp()
        {
            var gameObject = Instantiate(_bodyPrefab, _previousHeadPosition, quaternion.identity);
            _bodyCells.Enqueue(gameObject);
            _eating = false;
        }

        public void Eating()
        {
            _scores++;
            OnScoreUp?.Invoke(_scores);
            _eating = true;
        }
        
        private void ReadInput()
        {
            if (!ManualControl)
            {
                return;
            }

            if (Input.GetKeyDown("up") && _direction != Direction.Down)
            {
                SetDirection(Direction.Up);
            }
            else if (Input.GetKeyDown("right") && _direction != Direction.Left)
            {
                SetDirection(Direction.Right);
            }
            else if (Input.GetKeyDown("down") && _direction != Direction.Up)
            {
                SetDirection(Direction.Down);
            }
            else if (Input.GetKeyDown("left") && _direction != Direction.Right)
            {
                SetDirection(Direction.Left);
            }
        }

        private void SetDirection(Direction direction)
        {
            _direction = direction;
        }
    }
}