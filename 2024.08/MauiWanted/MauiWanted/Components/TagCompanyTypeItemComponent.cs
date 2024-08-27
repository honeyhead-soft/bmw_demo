using MauiReactor;
using MauiWanted.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Components
{
    class TagCompanyTypeItemComponentState
    {
        public CompanyCollection companySet;
        public bool Loading;
    }
    partial class TagCompanyTypeItemComponent : Component<TagCompanyTypeItemComponentState>
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
        Task<Models.CompanyCollection> propCompanySet;

        double itemSize = Helpers.Utilities.GetDeviceSize().Width / 2.4;
        Microsoft.Maui.Controls.CollectionView collectionView;
        Microsoft.Maui.Controls.LinearGradientBrush gradientBrush = new Microsoft.Maui.Controls.LinearGradientBrush
        {
            StartPoint = new Point(0, 0), // 시작 위치 (좌측 상단)
            EndPoint = new Point(0, 1),   // 끝 위치 (아래로)
            GradientStops = new Microsoft.Maui.Controls.GradientStopCollection
            {
                new Microsoft.Maui.Controls.GradientStop { Color = Colors.Black.WithAlpha(0.0f), Offset = 0.4f }, // 처음 색 (투명도 0.8)
                new Microsoft.Maui.Controls.GradientStop { Color = Colors.Black.WithAlpha(0.4f), Offset = 1.0f }  // 끝 색 (투명도 0)
            }
        };
        protected override async void OnMounted()
        {
            //base.OnMounted();
            //// 로딩 상태를 먼저 처리
            //State.companySet = null;
            //Invalidate(); // 로딩 중이라는 상태로 UI 업데이트

            //if (propCompanySet != null)
            //{
            //    // 비동기 작업으로 데이터 로드
            //    var dataList = await propCompanySet;
            //    State.companySet = dataList; // 데이터가 준비된 후 상태 업데이트
            //    Invalidate(); // 데이터 로드 후 다시 UI 업데이트
            //}

            base.OnMounted();
            State.companySet = null; // 초기화
            State.Loading = true;      // 로딩 상태 시작

            if (propCompanySet != null)
            {
                State.companySet = await propCompanySet;
            }

            State.Loading = false;     // 로딩 상태 종료
        }

        public override VisualNode Render()
        {
            if (State.Loading || State.companySet == null)
            {
                return Grid(
                    new ActivityIndicator() // 간단한 로딩 스피너나 텍스트로 로딩 중임을 표시
                ).VCenter().HCenter();
            }


            //return VStack(
            //    new SectionBarComponent().HeaderText(headerText).RightButtonText(rightButtonText).IsRightButtonVisible(isRightButtonVisible),
            //     ScrollView(
            //         HStack(
            //                [.. State.companySet.Companies.Select((item, idx) => RenderTagCompany(item, idx))]
            //            ).Spacing(10)
            //     ).Orientation(ScrollOrientation.Horizontal).HorizontalScrollBarVisibility(ScrollBarVisibility.Never).Set(Microsoft.Maui.Controls.CompressedLayout.IsHeadlessProperty, true)
            //).Padding(20, 0);

            return VStack(
                new SectionBarComponent().HeaderText(headerText).RightButtonText(rightButtonText).IsRightButtonVisible(isRightButtonVisible),
                  CollectionView((sen) =>
                  {
                      //sen.ItemSizingStrategy = Microsoft.Maui.Controls.ItemSizingStrategy.MeasureFirstItem;
                      //collectionView = sen;
                  }).ItemsSource(State.companySet.Companies, RenderTagCompany).ItemsLayout(new HorizontalLinearItemsLayout().ItemSpacing(10)).HorizontalScrollBarVisibility(ScrollBarVisibility.Never)
            ).Padding(20, 0);
        }
               
        public VisualNode RenderTagCompany(Company item)
        {
            return Grid(
                    Border(
                        new CustomCachedImage() 
                            .WebSourceSSL(item.TitleImg.Thumb, TimeSpan.FromHours(24))
                            .LoadingPlaceholder("Resources/Images/imageplaceholder.png")
                            .HeightRequest(itemSize)
                            .Aspect(Aspect.AspectFill)
                            .FadeAnimationEnabled(true).FadeAnimationDuration(200).DownsampleToViewSize(true) // 큰 이미지에만 적용
                    ).StrokeCornerRadius(10).StrokeThickness(1).Stroke(Color.FromArgb("#eceded")),

                    Border().StrokeCornerRadius(10).StrokeThickness(0).Background(gradientBrush),

                    Grid(
                        VStack(
                            Label()
                                .Text(item.Name)
                                .TextColor(Colors.White)
                                .FontFamily(Helpers.FontHelper.PretendardSemiBold)
                                .FontSize(12),

                            Label()
                                .Text(item.IndustryTag.Text)
                                .TextColor(Colors.White)
                                .FontFamily(Helpers.FontHelper.PretendardRegular)
                                .FontSize(10)
                        ).GridColumn(1),

                        new CustomCachedImage()
                            .WebSourceSSL(item.LogoImg.Thumb, TimeSpan.FromHours(24))
                            .LoadingPlaceholder("Resources/Images/imageplaceholder.png")
                            .WidthRequest(28)
                            .HeightRequest(28)
                            .VCenter()
                            .FadeAnimationEnabled(true).FadeAnimationDuration(200).DownsampleToViewSize(false), // 로고 이미지 다운샘플링 비활성화

                        Border(
                            Label()
                                .Text("팔로우")
                                .TextColor(Colors.White)
                                .FontFamily(Helpers.FontHelper.PretendardMedium)
                                .FontSize(12)
                        ).HEnd().Stroke(Colors.White).StrokeThickness(1).GridColumn(2).VCenter().StrokeCornerRadius(5).Padding(8, 5)
                    ).Columns("Auto,*,Auto").VEnd().Padding(10, 15).ColumnSpacing(8).WithKey(item.Id)
            );
        }
    }
}
