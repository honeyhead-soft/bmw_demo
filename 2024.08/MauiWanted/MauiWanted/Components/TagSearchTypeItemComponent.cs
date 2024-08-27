using MauiReactor;
using MauiWanted.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Components
{
    class TagSearchTypeItemComponentState
    {
        public List<TagSearch> tagSearchSet;

        public bool Loading;
    }

    partial class TagSearchTypeItemComponent : Component<TagSearchTypeItemComponentState>
    {
        [Inject]
        AppServices.DebugService debugService;

        [Prop]
        string headerText = String.Empty;

        [Prop]
        string rightButtonText = String.Empty;

        [Prop]
        bool isRightButtonVisible;

        [Prop]
        Task<List<TagSearch>> propTagSearchSet;

        double sizeW = Helpers.Utilities.GetDeviceSize().Width / 2.5;
        double sizeH = Helpers.Utilities.GetDeviceSize().Width / 2.7;
        Microsoft.Maui.Controls.CollectionView collectionView;

        protected override async void OnMounted()
        {
            base.OnMounted();
            //if (propTagSearchSet != null)
            //{
            //    State.tagSearchSet = await propTagSearchSet;
            //}
            State.tagSearchSet = null; // 초기화
            State.Loading = true;      // 로딩 상태 시작

            if (propTagSearchSet != null)
            {
                State.tagSearchSet = await propTagSearchSet;
            }

            State.Loading = false;     // 로딩 상태 종료
        }

        public override VisualNode Render()
        {
            if (State.Loading || State.tagSearchSet == null)
            {
                return Grid(
                    new ActivityIndicator() // 간단한 로딩 스피너나 텍스트로 로딩 중임을 표시
                ).VCenter().HCenter();
            }

            #region ScrollView로 구현 시
            //return VStack(
            //    new SectionBarComponent().HeaderText(headerText).IsRightButtonVisible(false),
            //    ScrollView(
            //        HStack(
            //            [.. State.tagSearchSet.Select((item, idx) => RenderSearchTagBox(item))]
            //        ).Spacing(10)
            //    ).Orientation(ScrollOrientation.Horizontal).HorizontalScrollBarVisibility(ScrollBarVisibility.Never)
            //).Padding(20, 20); 
            #endregion

            #region CollectionView로 구현시 (빠름)
            return VStack(
            new SectionBarComponent().HeaderText(headerText).IsRightButtonVisible(false),
                CollectionView((sen) =>
                {
                    //sen.ItemSizingStrategy = Microsoft.Maui.Controls.ItemSizingStrategy.MeasureFirstItem;
                    //collectionView = sen;
                }).ItemsSource(State.tagSearchSet, RenderSearchTagBox).ItemsLayout(new HorizontalLinearItemsLayout().ItemSpacing(10)).HorizontalScrollBarVisibility(ScrollBarVisibility.Never)
            ).Padding(20, 20);
            #endregion
        }

        public VisualNode RenderSearchTagBox(TagSearch tagItem)
        {
            return Border(
                Grid(
                    new CustomCachedImage().WebSourceSSL(tagItem.Image, TimeSpan.FromHours(24)).Aspect(Aspect.AspectFill).LoadingPlaceholder("Resources/Images/imageplaceholder.png")
                        .FadeAnimationEnabled(true).FadeAnimationDuration(200).DownsampleToViewSize(true),
                    VStack(
                        Label().Text(tagItem.Title).TextColor(Colors.White).FontFamily(Helpers.FontHelper.PretendardBold).FontSize(12),
                        Label().Text($"관련 포지션 {tagItem.RelatedPositions.ToString("N0")}").TextColor(Color.FromArgb("#d6d6d4")).FontFamily(Helpers.FontHelper.PretendardMedium).FontSize(10).Margin(0, 2, 0, 0),
                        Label().Text($"기업 {tagItem.Companies.ToString("N0")}").TextColor(Color.FromArgb("#d6d6d4")).FontFamily(Helpers.FontHelper.PretendardMedium).FontSize(10)
                    ).GridRow(1).VCenter().Padding(10, 0)
                ).Rows("5*,5*")
            ).StrokeThickness(0).WidthRequest(sizeW).HeightRequest(sizeH).StrokeCornerRadius(10).BackgroundColor(Color.FromArgb(tagItem.CardHexColor)).WithKey(tagItem.Title);            
        }
    }
}
