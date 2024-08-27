using MauiReactor;
using MauiWanted.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Components
{
    class TagMoaTypeItemComponentState
    {
        public List<ThemeZip> themeZipSet;
        public bool Loading;
    }
    partial class TagMoaTypeItemComponent : Component<TagMoaTypeItemComponentState>
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
        Task<List<ThemeZip>> propThemeZipSet;

        double temp = Helpers.Utilities.GetDeviceSize().Width;

        Microsoft.Maui.Controls.CollectionView _collectionView;
        protected override async void OnMounted()
        {
            //base.OnMounted();
            //// 로딩 상태를 먼저 처리
            //State.themeZipSet = null;
            //Invalidate(); // 로딩 중이라는 상태로 UI 업데이트

            //if (propThemeZipSet != null)
            //{
            //    // 비동기 작업으로 데이터 로드
            //    var dataList = await propThemeZipSet;
            //    State.themeZipSet = dataList; // 데이터가 준비된 후 상태 업데이트
            //    Invalidate(); // 데이터 로드 후 다시 UI 업데이트
            //}

            base.OnMounted();
            State.themeZipSet = null; // 초기화
            State.Loading = true;      // 로딩 상태 시작

            if (propThemeZipSet != null)
            {
                State.themeZipSet = await propThemeZipSet;
            }

            State.Loading = false;     // 로딩 상태 종료
        }
        public override VisualNode Render()
        {
            if (State.Loading || State.themeZipSet == null)
            {
                return Grid(
                    new ActivityIndicator() // 간단한 로딩 스피너나 텍스트로 로딩 중임을 표시
                ).VCenter().HCenter();
            }

            return VStack(
                new SectionBarComponent().HeaderText(headerText).RightButtonText(rightButtonText).IsRightButtonVisible(isRightButtonVisible),
                  CollectionView((sen)=>
                  {
                      //sen.ItemSizingStrategy = Microsoft.Maui.Controls.ItemSizingStrategy.MeasureFirstItem;
                      //_collectionView = sen;
                  }).ItemsSource(State.themeZipSet, RenderThemeZip).ItemsLayout(new HorizontalLinearItemsLayout().ItemSpacing(10)).HorizontalScrollBarVisibility(ScrollBarVisibility.Never)
            ).Padding(20, 0);
        }

        public VisualNode RenderThemeZip(ThemeZip zip)
        {
            var sizeW = temp / 2.5;
            var sizeH = temp / 2.7;
            return Border(
                Grid(
                        new CustomCachedImage().WebSourceSSL(zip.Image, TimeSpan.FromHours(24)).Aspect(Aspect.AspectFill).LoadingPlaceholder("Resources/Images/imageplaceholder.png").FadeAnimationEnabled(true).FadeAnimationDuration(200).DownsampleToViewSize(true).CacheDuration(TimeSpan.FromHours(7)),
                        Label().Text(zip.Title).LineBreakMode(LineBreakMode.TailTruncation).TextColor(Colors.White).FontFamily(Helpers.FontHelper.PretendardBold).FontSize(12).VEnd().Padding(10),
                        HStack(
                            HStack(
                                [.. zip.Logos.Select((item) => {
                                    return Border(
                                        new CustomCachedImage().WebSourceSSL(item, TimeSpan.FromHours(24)).Aspect(Aspect.AspectFill).LoadingPlaceholder("Resources/Images/imageplaceholder.png").FadeAnimationEnabled(true).FadeAnimationDuration(200).DownsampleToViewSize(false)
                                    ).WidthRequest(20).HeightRequest(20).Stroke(Color.FromArgb("#eceded")).StrokeCornerRadius(2);
                                })]
                            ).Spacing(2),
                            Label().Text(zip.MoreLogos).FontFamily(Helpers.FontHelper.PretendardMedium).TextColor(Color.FromArgb("#858588")).FontSize(10).VCenter().Margin(5, 0).IsVisible(String.IsNullOrEmpty(zip.MoreLogos) ? false : true)
                        ).Padding(8, 0).GridRow(1)
                ).Rows("8.5*,2.5*")
            ).StrokeThickness(1).Stroke(Color.FromArgb("#eceded")).WidthRequest(sizeW).HeightRequest(sizeH).StrokeCornerRadius(10).BackgroundColor(Colors.White);
        }
    }
}
