using UnityEngine;
using Zenject;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        [Inject] private GameManager m_GameManager;
        
        private Camera m_Camera;

        private void Awake()
        {
            m_Camera = GetComponent<Camera>();
            CenterCamera();
        }
        
        private void CenterCamera()
        {
            int width = m_GameManager.Width;
            int height = m_GameManager.Height;
            float cellSize = m_GameManager.CellSize;
            float x = width * cellSize * 0.5f;
            float y = height * cellSize * 0.5f;
            float size = CalculateSize(width, height);
            m_Camera.orthographicSize = size;
            transform.position = new Vector3(x, y, -10f);
        }

        private static float CalculateSize(int width, int height)
        {
            return width >= height ? width : (float)height / 2;
        }
    }
}