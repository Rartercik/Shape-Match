using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Configurations;

namespace Game.Figures
{
    public class FiguresGenerator : MonoBehaviour
    {
        [SerializeField] private FigureConfigurations _configurations;
        [SerializeField] private uint _groupsCount;
        [SerializeField] private float _leftBorder;
        [SerializeField] private float _rightBorder;
        [SerializeField] private float _lowestHeight;
        [SerializeField] private float _maxShift;
        [SerializeField] private float _figuresRadius;
        [SerializeField] private float _space;

        public event Action<int> OnGenerated;

        private FiguresBuffer _buffer;
        private Figure[] _figures;
        private Figure[] _figurePrefabs;
        private Color[] _colors;
        private Sprite[] _sprites;
        private uint _groupsSize;

        private int InitialFiguresCount => (int)(_groupsCount * _groupsSize);

        public void Initialize(FiguresBuffer buffer, uint groupsSize)
        {
            _buffer = buffer;
            _figurePrefabs = _configurations.Figures;
            _colors = _configurations.Colors;
            _sprites = _configurations.Sprites;
            _groupsSize = groupsSize;
        }

        public void Generate()
        {
            var groupFigures = GenerateGroups(_groupsCount, _figurePrefabs, _colors, _sprites);
            _figures = GenerateFigures(groupFigures, _groupsSize, _leftBorder, _rightBorder, _lowestHeight, _maxShift, _figuresRadius, _space);
            OnGenerated?.Invoke(_figures.Length);
        }

        public void Regenerate()
        {
            var remainingContents = GetRemainingContents(_buffer.NotDefaultContents, _groupsSize);
            var groupsCount = (_figures.Where(figure => figure != null).Count() - remainingContents.Length) / _groupsSize;
            ClearObjects(_figures);
            var figuresCount = (int)(remainingContents.Length + groupsCount * _groupsSize);
            _figures = new Figure[figuresCount];
            var positions = GenerateFigurePositions(InitialFiguresCount, _leftBorder, _rightBorder, _lowestHeight, _maxShift, _figuresRadius, _space);
            Shuffle(positions);
            for (int i = 0; i < remainingContents.Length; i++)
            {
                _figures[i] = CreateFigure(remainingContents[i], positions[i]);
            }

            var newGroups = GenerateGroups((uint)groupsCount, _figurePrefabs, _colors, _sprites);
            var newFigures = CreateFigures(newGroups, positions[remainingContents.Length..], _groupsSize);
            for (int i = remainingContents.Length; i < figuresCount; i++)
            {
                _figures[i] = newFigures[i - remainingContents.Length];
            }
        }

        public void ResetState()
        {
            ClearObjects(_figures);
        }

        private void ClearObjects(Figure[] figures)
        {
            for (int i = 0; i < figures.Length; i++)
            {
                if (figures[i] == null) continue;

                Destroy(figures[i].gameObject);
                figures[i] = null;
            }
        }

        private FigureContent[] GenerateGroups(uint groupsCount, Figure[] prefab, Color[] colors, Sprite[] sprites)
        {
            var groupFigures = new FigureContent[groupsCount];
            for (int i = 0; i < groupsCount; i++)
            {
                var figure = GetRandomElement(prefab);
                var color = GetRandomElement(colors);
                var sprite = GetRandomElement(sprites);
                groupFigures[i] = new FigureContent(figure, color, sprite);
            }
            return groupFigures;
        }

        private Figure[] GenerateFigures(FigureContent[] figures, uint groupsSize, float leftBorder, float rightBorder, float lowestHeight,
            float maxShift, float figuresRadius, float space)
        {
            var figuresCount = figures.Length * (int)groupsSize;
            var positions = GenerateFigurePositions(figures.Length * (int)groupsSize, leftBorder, rightBorder, lowestHeight, maxShift, figuresRadius, space);
            Shuffle(positions);
            return CreateFigures(figures, positions, groupsSize);
        }

        private FigureContent[] GetRemainingContents(FigureContent[] bufferContents, uint groupsSize)
        {
            var identicalContents = GetIdenticalContents(bufferContents);
            return ConvertToArrayContents(identicalContents, groupsSize);
        }

        private List<Tuple<FigureContent, int>> GetIdenticalContents(FigureContent[] bufferContents)
        {
            var identicalContents = new List<Tuple<FigureContent, int>>();
            foreach (var content in bufferContents)
            {
                var foundIdentical = false;
                for (int i = 0; i < identicalContents.Count; i++)
                {
                    if (content.Equals(identicalContents[i].Item1))
                    {
                        identicalContents[i] = Tuple.Create(identicalContents[i].Item1, identicalContents[i].Item2 + 1);
                        foundIdentical = true;
                        break;
                    }
                }

                if (foundIdentical == false)
                {
                    identicalContents.Add(Tuple.Create(content, 1));
                }
            }
            return identicalContents;
        }

        private FigureContent[] ConvertToArrayContents(List<Tuple<FigureContent, int>> identicalContents, uint groupsSize)
        {
            var result = new List<FigureContent>();
            foreach (var content in identicalContents)
            {
                for (int i = 0; i < groupsSize - content.Item2; i++)
                {
                    result.Add(content.Item1);
                }
            }
            return result.ToArray();
        }

        private T GetRandomElement<T>(T[] array)
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        private Vector2[] GenerateFigurePositions(int figuresCount, float leftBorder, float rightBorder, float lowestHeight,
            float maxShift, float figuresRadius, float space)
        {
            var positions = new Vector2[figuresCount];
            var rightestPosition = rightBorder - figuresRadius;
            var positionX = GetRandomShift(leftBorder, rightBorder, figuresRadius, maxShift);
            var positionY = lowestHeight + figuresRadius;
            var position = new Vector2(positionX, positionY);
            for (int i = 0; i < figuresCount; i++)
            {
                positions[i] = position;
                position.x += 2 * figuresRadius + space;
                if (position.x > rightestPosition)
                {
                    position.x = GetRandomShift(leftBorder, rightBorder, figuresRadius, maxShift);
                    position.y += 2 * figuresRadius + space;
                }
            }
            return positions;
        }

        private Figure[] CreateFigures(FigureContent[] figures, Vector2[] positions, uint groupsSize)
        {
            var figuresCount = figures.Length * (int)groupsSize;
            var result = new Figure[figuresCount];
            for (int i = 0; i < figures.Length; i++)
            {
                for (int j = 0; j < groupsSize; j++)
                {
                    var index = i * groupsSize + j;
                    result[index] = CreateFigure(figures[i], positions[index]);
                }
            }
            return result;
        }

        private Figure CreateFigure(FigureContent figure, Vector2 position)
        {
            var result = Instantiate(figure.Prefab, position, Quaternion.identity);
            result.Initialize(figure);
            return result;
        }

        private float GetRandomShift(float minBorder, float maxBorder, float figuresRadius, float maxShift)
        {
            var result = minBorder + figuresRadius;
            var farestPosition = maxBorder - figuresRadius;
            var remainingSpace = farestPosition - result;
            if (remainingSpace < 0)
            {
                remainingSpace = 0;
            }
            maxShift = Mathf.Clamp(maxShift, 0, remainingSpace);
            result += UnityEngine.Random.Range(0, maxShift);
            return result;
        }

        private void Shuffle<T>(T[] array)
        {
            for (int t = 0; t < array.Length; t++)
            {
                var tmp = array[t];
                int r = UnityEngine.Random.Range(t, array.Length);
                array[t] = array[r];
                array[r] = tmp;
            }
        }
    }
}
