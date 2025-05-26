using UnityEngine;
using UnityEngine.UI;

namespace Game.Figures
{
    public class FiguresBufferCell : MonoBehaviour
    {
        [SerializeField] private RectTransform _innerSpriteTransform;
        [SerializeField] private Image _outerShape;
        [SerializeField] private Image _innerShape;
        [SerializeField] private Image _innerSprite;

        private readonly Color _normalOuterShapeColor = Color.white;

        private Sprite _defaultShape;
        private Color _defaultOuterShapeColor;

        public bool IsDefault { get; private set; } = true;
        public FigureContent FigureContent { get; private set; }

        public void Initialize()
        {
            _defaultShape = _outerShape.sprite;
            _defaultOuterShapeColor = _outerShape.color;
        }

        public void SetContent(FigureContent content)
        {
            _innerShape.gameObject.SetActive(true);
            _outerShape.sprite = content.Prefab.ShapeSprite;
            _innerShape.sprite = content.Prefab.ShapeSprite;
            _outerShape.color = _normalOuterShapeColor;
            _innerShape.color = content.Color;
            _innerSprite.sprite = content.Sprite;
            var prefab = content.Prefab;
            var positionX = prefab.InnerSpritePosition.x * _innerSpriteTransform.rect.width / prefab.InnerSpriteLocalScale.x;
            var positionY = prefab.InnerSpritePosition.y * _innerSpriteTransform.rect.height / prefab.InnerSpriteLocalScale.y;
            _innerSpriteTransform.localPosition = new Vector2(positionX, positionY);
            FigureContent = content;
            IsDefault = false;
        }

        public void SetDefault()
        {
            _outerShape.sprite = _defaultShape;
            _outerShape.color = _defaultOuterShapeColor;
            _innerShape.gameObject.SetActive(false);
            IsDefault = true;
        }
    }
}