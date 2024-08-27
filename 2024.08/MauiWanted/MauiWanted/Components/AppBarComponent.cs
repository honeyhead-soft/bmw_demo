using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MauiWanted.Components
{
    public enum RightButtonType
    {
        Text,
        Image
    }

    public class RightButtonItem
    {
        public RightButtonType ButtonType { get; set; } = RightButtonType.Text;
        public string Content { get; set; } = String.Empty;
        public Action OnTapped { get; set; }
        public string FontFamily { get; set; } = Helpers.FontHelper.PretendardRegular;
    }

    partial class AppBarComponent : Component
    {
        [Prop]
        protected string appBarText = string.Empty;

        [Prop]
        protected RightButtonItem[] rightButtons = Array.Empty<RightButtonItem>();
        protected override void OnMounted()
        {
            base.OnMounted();
        }
        public override VisualNode Render()
        {
            return Grid(
                Label().Text(appBarText).TextColor(Color.FromArgb("#3d3d3d")).FontFamily(Helpers.FontHelper.PretendardSemiBold).FontSize(20).VEnd().VerticalTextAlignment(TextAlignment.End),
                HStack(
                    [.. rightButtons.Select((btn, idx) => RenderRightButton(btn, idx))]
                )
                .HEnd()
                .Spacing(20)
            ).Padding(20).BackgroundColor(Colors.Transparent);
        }

        VisualNode RenderRightButton(RightButtonItem button, int idx)
        {
            return button.ButtonType switch
            {
                RightButtonType.Text => Label()
                    .Text(button.Content)
                    .IsVisible(!string.IsNullOrEmpty(button.Content))
                    .FontSize(20).VEnd().Padding(0, 0, 0, 1)
                    .FontFamily(button.FontFamily)
                    .TextColor(Color.FromArgb("#3d3d3d"))
                    .VerticalTextAlignment(TextAlignment.End)
                    .OnTapped(button.OnTapped),

                RightButtonType.Image => new CustomCachedImage().WebSource(button.Content)
                    .Transformations(new List<FFImageLoading.Work.ITransformation> { new FFImageLoading.Transformations.CircleTransformation() { BorderSize = 18, BorderHexColor = Color.FromArgb("#E4E6E6").ToHex() } })
                    .IsVisible(!string.IsNullOrEmpty(button.Content))
                    .WidthRequest(24)
                    .HeightRequest(24)
                    .VEnd()
                    .Aspect(Aspect.AspectFit)
                    .OnTapped(button.OnTapped),

                _ => null
            };
        }
    }
}
