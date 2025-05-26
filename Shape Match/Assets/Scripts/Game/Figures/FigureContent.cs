using UnityEngine;

namespace Game.Figures
{
    public struct FigureContent
    {
        public readonly Figure Prefab;
        public readonly Color Color;
        public readonly Sprite Sprite;

        public FigureContent(Figure prefab, Color color, Sprite sprite)
        {
            Prefab = prefab;
            Color = color;
            Sprite = sprite;
        }

        public bool Equals(FigureContent other)
        {
            return Prefab == other.Prefab && Color == other.Color && Sprite == other.Sprite;
        }
    }
}
