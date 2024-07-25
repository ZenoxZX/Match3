using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.EventArgs;
using UnityEngine;

namespace CommandSystem
{
    public readonly struct SwapCommand : ICommand
    {
        private const float k_DurationThreshold = 0.05f;
        
        private readonly int m_X1;
        private readonly int m_Y1;
        private readonly int m_X2;
        private readonly int m_Y2;
        private readonly Transform m_A;
        private readonly Transform m_B;
        private readonly float m_Duration;
        private readonly Func<int, int, Vector3> m_GetCenteredWorldPosition;
        private readonly Action<int, int, int, int> m_SwapCallback;
        private readonly Action<GridSwapArgs> m_GridSwapped;

        public SwapCommand(int x1, int y1, int x2, int y2, 
            Transform a, Transform b, float duration, 
            Func<int, int, Vector3> getCenteredWorldPosition,
            Action<int, int, int, int> swapCallback,
            Action<GridSwapArgs> gridSwapped)
        {
            m_X1 = x1;
            m_Y1 = y1;
            m_X2 = x2;
            m_Y2 = y2;
            m_A = a;
            m_B = b;
            m_Duration = duration;
            m_GetCenteredWorldPosition = getCenteredWorldPosition;
            m_SwapCallback = swapCallback;
            m_GridSwapped = gridSwapped;
        }

        public void Execute()
        {
            m_A.DOMove(m_GetCenteredWorldPosition(m_X2, m_Y2), m_Duration);
            m_B.DOMove(m_GetCenteredWorldPosition(m_X1, m_Y1), m_Duration);
            m_SwapCallback?.Invoke(m_X1, m_Y1, m_X2, m_Y2);
            m_GridSwapped?.Invoke(new GridSwapArgs(m_X1, m_Y1, m_X2, m_Y2));
        }

        public void Undo()
        {
            m_A.DOMove(m_GetCenteredWorldPosition(m_X1, m_Y1), m_Duration);
            m_B.DOMove(m_GetCenteredWorldPosition(m_X2, m_Y2), m_Duration);
            m_SwapCallback?.Invoke(m_X2, m_Y2, m_X1, m_Y1);
            m_GridSwapped?.Invoke(new GridSwapArgs(m_X2, m_Y2, m_X1, m_Y1));
        }

        public UniTask WaitForCompletion()
        {
            return UniTask.Delay(TimeSpan.FromSeconds(m_Duration + k_DurationThreshold));
        }
    }
}