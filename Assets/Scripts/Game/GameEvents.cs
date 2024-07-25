using System;
using Game.EventArgs;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    [UsedImplicitly]
    public class GameEvents
    {
        // GridTile events
        public Action<GridSwapArgs> GridSwapped;
        
        // Input events
        public Action<InputArgs> InputBegan;
        public Action<InputArgs> InputEnded;
        public Action<InputArgs> InputMoved;
    }

    public struct InputArgs
    {
        public Vector3 MousePosition;
        public Vector3 WorldPosition;
        public Vector2 Delta;
        public InputHandler.DirectionArgs DirectionArgs;
    }
}