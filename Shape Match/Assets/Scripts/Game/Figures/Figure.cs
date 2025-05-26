using UnityEngine;

namespace Game.Figures
{
    public class Figure : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _innerSpriteTransform;
        [SerializeField] private SpriteRenderer _innerShape;
        [SerializeField] private SpriteRenderer _innerSprite;

        public Figure Prefab { get; private set; }
        public Sprite ShapeSprite => _innerShape.sprite;
        public Sprite InnerSprite => _innerSprite.sprite;
        public Color Color => _innerShape.color;
        public Vector2 InnerSpritePosition => _innerSpriteTransform.localPosition;
        public Vector2 InnerSpriteLocalScale => _innerSpriteTransform.localScale;
        public FigureContent Content => new FigureContent(Prefab, Color, InnerSprite);

        public void Initialize(FigureContent content, Vector2 position)
        {
            _transform.position = position;
            Initialize(content);
        }

        public void Initialize(FigureContent content)
        {
            Prefab = content.Prefab;
            _innerShape.color = content.Color;
            _innerSprite.sprite = content.Sprite;
        }
    }
}
