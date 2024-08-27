using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Components
{
    partial class IconBoxComponent : Component
    {
        [Prop]
        (string LabelText, string ImageSource, string path)[] iconItems;

        protected override void OnMounted()
        {
            base.OnMounted();
        }

        public override VisualNode Render()
        {
            var gridColumns = string.Join(",", Enumerable.Repeat("*", iconItems.Count()));
            return Grid(
                      [.. iconItems.Select((item, idx) => RenderIconButton(item.LabelText, item.ImageSource, item.path, idx))]
            )
            .Columns(gridColumns).Padding(0,0,0,20);
        }

        VisualNode RenderIconButton(string labelText, string imgPath, string appPath, int idx)
        {
            return VStack(
                new CustomCachedImage().Source(imgPath).WidthRequest(32).HeightRequest(32),
                Label().Text(labelText).TextColor(Color.FromArgb("#8c8c8c")).FontFamily(Helpers.FontHelper.PretendardRegular).Center().FontSize(12)
            ).Padding(10, 0).Spacing(10).GridColumn(idx).OnTapped(() =>
            {
                //Navigation 구현
            });
        }
    }
}
