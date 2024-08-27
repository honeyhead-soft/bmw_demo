using MauiReactor;
using System;

namespace MauiWanted.Components
{
    partial class SectionBarComponent : Component
    {
        [Prop]
        protected string headerText = String.Empty;

        [Prop]
        protected string rightButtonText = String.Empty;

        [Prop]
        protected bool isRightButtonVisible;

        protected override void OnMounted()
        {
            base.OnMounted();
        }

        public override VisualNode Render()
        {
            return Grid(
                Label().Text(headerText).TextColor(Colors.Black).HStart().VEnd().FontFamily(Helpers.FontHelper.PretendardSemiBold).FontSize(14),
                Label().Text(rightButtonText).TextColor(Color.FromArgb("#868689")).IsVisible(isRightButtonVisible).HEnd().VEnd().FontFamily(Helpers.FontHelper.PretendardMedium).FontSize(12)
            ).Padding(0, 10);
        }
    }
}
