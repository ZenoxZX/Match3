using System;
using Data;
using Extensions;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game
{
    [UsedImplicitly]
    public class InputHandler : IInitializable, ITickable
    {
        [Inject] private ProjectSettings m_ProjectSettings;
        [Inject] private GameEvents m_GameEvents;
        
        private Camera m_Camera;
        private Vector3 m_MousePosition;
        private bool m_IsOverUI;
        
        private Settings MySettings => m_ProjectSettings.InputHandlerSettings;

        void IInitializable.Initialize()
        {
            m_Camera = Camera.main;
            
            m_GameEvents.InputBegan += args =>
            {
                Debug.Log("Input began at " + args.MousePosition + " and world position " + args.WorldPosition);
            };
            
            m_GameEvents.InputEnded += args =>
            {
                Debug.Log("Input ended at " + args.MousePosition);
            };
            
            m_GameEvents.InputMoved += args =>
            {
                Debug.Log("Input moved to " + args.MousePosition + " with delta " + args.Delta + " and direction " + args.DirectionArgs.Direction);
            };
        }
        
        void ITickable.Tick()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (m_IsOverUI)
                {
                    m_IsOverUI = false;
                    return;
                }

                m_MousePosition = Input.mousePosition;
                
                m_GameEvents.InputEnded?.Invoke(new()
                {
                    MousePosition = m_MousePosition
                });
                
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                m_MousePosition = Input.mousePosition;
                m_IsOverUI = UIExtensions.IsOverUI;
                
                if (m_IsOverUI)
                    return;
                
                m_GameEvents.InputBegan?.Invoke(new()
                {
                    MousePosition = m_MousePosition,
                    WorldPosition = GetWorldPosition(m_MousePosition, -m_Camera.transform.position.z)
                });
                
                return;
            }
            
            if (Input.GetMouseButton(0))
            {
                if (m_IsOverUI)
                    return;
                
                Vector3 delta = Input.mousePosition - m_MousePosition;
                m_MousePosition = Input.mousePosition;
                
                if (delta.magnitude < MySettings.MinSwipeDistance)
                    return;
                
                m_GameEvents.InputMoved?.Invoke(new()
                {
                    MousePosition = m_MousePosition,
                    Delta = delta,
                    DirectionArgs = DirectionArgs.Calculate(delta)
                });
            }
        }
        
        private Vector3 GetWorldPosition(Vector3 mousePosition, float z = 0)
        {
            mousePosition.z = z;
            return m_Camera.ScreenToWorldPoint(mousePosition);
        }

        [Serializable, HideLabel, Title("Input Handler Settings")]
        public class Settings
        {
            [SerializeField] private float m_MinSwipeDistance = 100;
            
            public float MinSwipeDistance => m_MinSwipeDistance;
        }
        
        public struct DirectionArgs
        {
            public Direction Direction;
            public Vector2 Vector;
            
            public static DirectionArgs Calculate(Vector2 delta)
            {
                Direction direction;
                
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    direction = delta.x > 0 ? Direction.Right : Direction.Left;
                    return new()
                    {
                        Direction = direction,
                        Vector = GetVector(direction)
                    };
                }

                direction = delta.y > 0 ? Direction.Up : Direction.Down;
                return new()
                {
                    Direction = direction,
                    Vector = GetVector(direction)
                };
            }
            
            public static Vector2 GetVector(Direction direction)
            {
                return direction switch
                {
                    Direction.Left => Vector2.left,
                    Direction.Up => Vector2.up,
                    Direction.Right => Vector2.right,
                    Direction.Down => Vector2.down,
                    _ => Vector2.zero
                };
            }
        }
        
        public enum Direction
        {
            Left,
            Up,
            Right,
            Down
        }
    }
}