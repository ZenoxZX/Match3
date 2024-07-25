using Game;
using Game.EventArgs;
using TMPro;
using UnityEngine;
using Zenject;

namespace GridSystem
{
    public class GridDisplay : MonoBehaviour
    {
        [SerializeField] private float m_DebugDuration = 120f;
        [SerializeField] private Color m_DebugColor = Color.white;
        
        [Inject] private GameManager m_GameManager;
        [Inject] private GameEvents m_GameEvents;
        
        private TextMeshPro[,] m_DebugTextArray;
        
        private void Start()
        {
            m_GameEvents.GridSwapped += OnSwap;
            
            DrawLines();
            CreateDebugGrid();
        }

        private void DrawLines()
        {
            int width = m_GameManager.Width;
            int height = m_GameManager.Height;
            
            for (int x = 0; x <= width; x++)
                Debug.DrawLine(m_GameManager.GetWorldPosition(x, 0), m_GameManager.GetWorldPosition(x, height), m_DebugColor, m_DebugDuration);

            for (int y = 0; y <= height; y++)
                Debug.DrawLine(m_GameManager.GetWorldPosition(0, y), m_GameManager.GetWorldPosition(width, y), m_DebugColor, m_DebugDuration);
        }
        
        private void CreateDebugGrid()
        {
            int width = m_GameManager.Width;
            int height = m_GameManager.Height;
            m_DebugTextArray = new TextMeshPro[width, height];
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    CreateText(m_GameManager[x, y].ToString(), x, y);
                }
            }
        }

        private void CreateText(string text, int x, int y)
        {
            float cellSize = m_GameManager.CellSize;
            TextMeshPro textMesh = new GameObject("Text").AddComponent<TextMeshPro>();
            RectTransform rectTransform = textMesh.rectTransform;
            rectTransform.SetParent(transform);
            rectTransform.sizeDelta = new Vector2(cellSize, cellSize);
            textMesh.transform.position = m_GameManager.GetCenteredWorldPosition(x, y);
            textMesh.text = text;
            textMesh.fontSize = 2;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.color = m_DebugColor;
            
            m_DebugTextArray[x, y] = textMesh;
        }
        
        public void OnSwap(GridSwapArgs args)
        {
            int x1 = args.X1;
            int y1 = args.Y1;
            int x2 = args.X2;
            int y2 = args.Y2;
            
            m_DebugTextArray[x1, y1].text = m_GameManager[x1, y1].ToString();
            m_DebugTextArray[x2, y2].text = m_GameManager[x2, y2].ToString();
        }
    }
}