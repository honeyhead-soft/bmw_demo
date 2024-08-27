using MauiReactor;
using MauiWanted.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Components
{
    partial class BottomTabComponent : Component
    {
        [Prop]
        int columnCount;

        [Prop]
        PageEnum currentTab;

        [Prop]
        Action<PageEnum> onTabChanged;

        [Prop]
        (PageEnum Page, string LabelText, string ImageSource, bool isNoChangeImage)[] tabItems;

        [Prop]
        Color selectedColor = Color.FromArgb("#0066FF");

        [Prop]
        Color unSelectedColor = Color.FromArgb("#AEB0B5");

        [Prop]
        double iconSize = 20;

        protected override void OnMounted()
        {
            base.OnMounted();
        }
        public override VisualNode Render()
        {
            var gridColumns = string.Join(",", Enumerable.Repeat("*", columnCount));

            return Grid(
                      [.. tabItems.Select((item, idx) => RenderTabBarButton(item.Page, item.LabelText, item.ImageSource, item.isNoChangeImage, idx))]
            )
            .Columns(gridColumns)
            .BackgroundColor(Colors.Transparent);
        }

        VisualNode RenderTabBarButton(PageEnum page, string labelText, string imageSource, bool isNoChangeImage, int idx)
        {
            bool isSelected = currentTab == page;

            return new Grid("Auto,Auto", "*")
            {
                (isNoChangeImage?
                    new CustomCachedImage().WebSource(imageSource).Transformations(new List<FFImageLoading.Work.ITransformation>{new FFImageLoading.Transformations.CircleTransformation()})
                    .HeightRequest(iconSize)
                    .WidthRequest(iconSize)
                    .Aspect(Aspect.AspectFit)
                    .Center()
                    :
                    Image(
                       new IconTintColorBehavior().TintColor(isSelected ? selectedColor :unSelectedColor)
                    ).Source(imageSource)
                    .HeightRequest(iconSize)
                    .WidthRequest(iconSize)
                    .Aspect(Aspect.AspectFit)
                    .Center()
                )
                ,
                Label(labelText)
                    .TextColor(isSelected ? selectedColor : unSelectedColor)
                    .FontFamily(Helpers.FontHelper.PretendardMedium)
                    .Center().GridRow(1)
            }
            .GridColumn(idx)
            .VerticalOptions(Microsoft.Maui.Controls.LayoutOptions.Center)
            .Padding(0, 5)
            .Margin(0, 10, 0, 0)
            .RowSpacing(5)
            .OnTapped(() =>
            {
                currentTab = page;
                onTabChanged?.Invoke(page); // Tab이 변경되면 콜백 실행
                this.Invalidate();
            });
        }
    }
}
