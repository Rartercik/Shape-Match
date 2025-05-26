using UnityEngine;
using Game.Figures;

namespace Game.Configurations
{
    [CreateAssetMenu(fileName = "FigureConfigurations", menuName = "Configurations/FigureConfigurations")]
    public class FigureConfigurations : ScriptableObject
    {
        [SerializeField] private Figure[] _figures;
        [SerializeField] private Color[] _colors;
        [SerializeField] private Sprite[] _sprites;

        public Figure[] Figures => (Figure[])_figures.Clone();
        public Color[] Colors => (Color[])_colors.Clone();
        public Sprite[] Sprites => (Sprite[])_sprites.Clone();
    }
}
