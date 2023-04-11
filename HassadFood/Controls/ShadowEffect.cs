using System;
using Xamarin.Forms;

namespace HassadFood
{
    public class ShadowEffect : RoutingEffect
    {
        public float Radius { get; set; }

        public Color Color { get; set; }

        public Color BGColor { get; set; }

        public float DistanceX { get; set; }

        public float DistanceY { get; set; }

        public ShadowEffect() : base("Sobha.LayoutShadowEffect")
        {
        }
    }
}
