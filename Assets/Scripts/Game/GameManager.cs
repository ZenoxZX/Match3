using System;
using CommandSystem;
using Cysharp.Threading.Tasks;
using GridSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int m_Seed;
        [SerializeField] private int m_Width = 8;
        [SerializeField] private int m_Height = 8;
        [SerializeField] private float m_CellSize = 1f;

        [Inject] private Shape.Factory m_ShapeFactory;
        [Inject] private GameEvents m_GameEvents;

        private GridTile<Shape> m_GridTile;
        private IGridTile m_Tile;
        private Transform m_ShapeParent;
        private Shape m_CurrentShape;
        private ICommand m_SwapCommand;
        private bool m_IsSwapping;
        
        public GridTile<Shape> GridTile => m_GridTile;
        public int Width => m_Width;
        public int Height => m_Height;
        public float CellSize => m_CellSize;
        public IGridObject this[int x, int y] => m_GridTile[x, y];

        #region Lifecycle

        private void Awake()
        {
            m_ShapeParent = new GameObject("Shapes").transform;
            m_GridTile = new GridTile<Shape>(m_Width, m_Height);
            m_Tile = m_GridTile;
            FillRandom();
        }

        private void Start()
        {
            m_GameEvents.InputBegan += OnInputBegan;
            m_GameEvents.InputMoved += OnInputMoved;
            m_GameEvents.InputEnded += OnInputEnded;
        }

        
        #endregion
        
        public void Swap(int x1, int y1, int x2, int y2)
        {
            const float duration = 0.2f;
            m_SwapCommand = new SwapCommand
            (
                x1,
                y1,
                x2,
                y2,
                ((Shape)m_GridTile[x1, y1]).transform,
                ((Shape)m_GridTile[x2, y2]).transform,
                duration,
                GetCenteredWorldPosition,
                m_GridTile.Swap,
                m_GameEvents.GridSwapped
            );
            
            m_SwapCommand.Execute();
        }
        
        public Vector3 GetWorldPosition(int x, int y) 
            => new Vector3(x, y) * m_CellSize;

        public Vector3 GetCenteredWorldPosition(int x, int y) 
            => GetWorldPosition(x, y) + new Vector3(m_CellSize, m_CellSize) * 0.5f;

        public (int x, int y) GetGridPosition(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt(worldPosition.x / m_CellSize);
            int y = Mathf.FloorToInt(worldPosition.y / m_CellSize);
            return (x, y);
        }
        
        public bool TryGetShape(Vector3 worldPosition, out Shape shape)
        {
            (int x, int y) = GetGridPosition(worldPosition);
            bool isInside = m_Tile.IsInside(x, y);
            
            if (isInside)
            {
                shape = (Shape)m_Tile.Get(x, y);
                return shape != null;
            }

            shape = null;
            return false;
        }

        private Shape CreateShape(Shape.Type type, int x, int y)
        {
            Shape shape = m_ShapeFactory.Create();
            shape.Set(x, y);
            shape.SetType(type);
            shape.transform.position = GetCenteredWorldPosition(x, y);
            shape.transform.SetParent(m_ShapeParent);
            return shape;
        }
        
        private void FillRandom()
        {
            Random.State state = Random.state;
            Random.InitState(m_Seed);
            
            IGridTile tile = m_GridTile;

            for (int x = 0; x < m_Width; x++)
            {
                for (int y = 0; y < m_Height; y++)
                {
                    int shapeCount = Enum.GetValues(typeof(Shape.Type)).Length;
                    int randomNumber = Random.Range(0, shapeCount);
                    Shape.Type type = (Shape.Type)randomNumber;
                    m_GridTile.Set(x, y, CreateShape(type, x, y));
                }
            }

            Random.state = state;
        }
        
        private void OnInputBegan(InputArgs args)
        {
            if (m_IsSwapping)
                return;
            
            Vector3 worldPosition = args.WorldPosition;
            bool hasShape = TryGetShape(worldPosition, out Shape shape);
            
            if (hasShape)
                m_CurrentShape = shape;
        }

        private void OnInputMoved(InputArgs args)
        {
            if (m_IsSwapping)
                return;
            
            if (m_CurrentShape == null)
                return;

            Vector2 vector2 = InputHandler.DirectionArgs.GetVector(args.DirectionArgs.Direction);
            vector2.x += m_CurrentShape.X;
            vector2.y += m_CurrentShape.Y;
            
            if (m_Tile.IsInside((int)vector2.x, (int)vector2.y))
            {
                Swap(m_CurrentShape.X, m_CurrentShape.Y, (int)vector2.x, (int)vector2.y);
                m_IsSwapping = true;
                bool isMatch = false;

                if (!isMatch)
                {
                    UniTask.RunOnThreadPool
                    (
                        async () =>
                        {
                            await m_SwapCommand.WaitForCompletion();
                            m_SwapCommand.Undo();
                            await m_SwapCommand.WaitForCompletion();
                            m_IsSwapping = false;
                        }
                    ).Forget();
                }
                
                else
                {
                    // TODO: Implement match logic
                }

                m_CurrentShape = null;
            }
        }

        private void OnInputEnded(InputArgs args)
        {
            m_CurrentShape = null;
        }
    }
}