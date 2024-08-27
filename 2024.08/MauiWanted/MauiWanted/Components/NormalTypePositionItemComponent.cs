//using Google.Android.Material.Tabs;
using MauiReactor;
using MauiWanted.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Components
{
    class NormalTypePositionItemComponentState
    {
        public List<Models.JobPosition> jobList;
        public bool Loading;

    }
    partial class NormalTypePositionItemComponent : Component<NormalTypePositionItemComponentState>
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
        Task<List<Models.JobPosition>> propJobList;

        Microsoft.Maui.Controls.LinearGradientBrush gradientBrush = new Microsoft.Maui.Controls.LinearGradientBrush
        {
            StartPoint = new Point(0, 0), // 시작 위치 (좌측 상단)
            EndPoint = new Point(0, 1),   // 끝 위치 (아래로)
            GradientStops = new Microsoft.Maui.Controls.GradientStopCollection
            {
                new Microsoft.Maui.Controls.GradientStop { Color = Colors.Black.WithAlpha(0.6f), Offset = 0.0f }, // 처음 색 (투명도 0.8)
                new Microsoft.Maui.Controls.GradientStop { Color = Colors.Black.WithAlpha(0.0f), Offset = 1.0f }  // 끝 색 (투명도 0)
            }
        };

        double itemSize = Helpers.Utilities.GetDeviceSize().Width / 3.5;
        double positionItemSize = Helpers.Utilities.GetDeviceSize().Width / 2.6;

        Microsoft.Maui.Controls.CollectionView collectionView;

        protected override void OnWillUnmount()
        {
            base.OnWillUnmount();
        }

        protected override async void OnMounted()
        {
            //base.OnMounted();
            //// 로딩 상태를 먼저 처리
            //State.jobList = null;
            //Invalidate(); // 로딩 중이라는 상태로 UI 업데이트

            //if (propJobList != null)
            //{
            //    // 비동기 작업으로 데이터 로드
            //    var jobList = await propJobList;
            //    State.jobList = jobList.Take(10).ToList(); // 데이터가 준비된 후 상태 업데이트
            //    Invalidate(); // 데이터 로드 후 다시 UI 업데이트
            //}

            base.OnMounted();
            State.jobList = null; // 초기화
            State.Loading = true;      // 로딩 상태 시작

            if (propJobList != null)
            {
                State.jobList = await propJobList;
            }

            State.Loading = false;     // 로딩 상태 종료
        }

        public override VisualNode Render()
        {
            if (State.Loading || State.jobList == null)
            {
                return Grid(
                    new ActivityIndicator() // 간단한 로딩 스피너나 텍스트로 로딩 중임을 표시
                ).VCenter().HCenter();
            }

            //return VStack(
            //    new SectionBarComponent().HeaderText(headerText).RightButtonText(rightButtonText).IsRightButtonVisible(isRightButtonVisible),
            //     State.jobList == null ?
            //     Grid() //로딩 중
            //     :
            //      ScrollView(
            //         HStack(
            //            [.. State.jobList.Select((item) => RenderPostionItem(item))]
            //         ).Spacing(10)
            //      ).Orientation(ScrollOrientation.Horizontal).HorizontalScrollBarVisibility(ScrollBarVisibility.Never)
            //).Padding(20, 10);

            return VStack(
                new SectionBarComponent().HeaderText(headerText).RightButtonText(rightButtonText).IsRightButtonVisible(isRightButtonVisible),
                  CollectionView((_)=>
                  {
                  }).ItemsSource(State.jobList, RenderPostionItem)
                  .ItemsLayout(new HorizontalLinearItemsLayout().ItemSpacing(10))
                  .HorizontalScrollBarVisibility(ScrollBarVisibility.Never)                 
            ).Padding(20, 10);
        }

        public VisualNode RenderPostionItem(JobPosition item)
        {
            return VStack(
                Grid(
                    Border(
                        new CustomCachedImage().WebSourceSSL(item.TitleImg.Thumb, TimeSpan.FromHours(24)).HeightRequest(itemSize).Aspect(Aspect.AspectFill)
                        .LoadingPlaceholder("Resources/Images/imageplaceholder.png").FadeAnimationEnabled(true).FadeAnimationDuration(200).DownsampleToViewSize(true)
                    ).StrokeCornerRadius(10).StrokeThickness(1).Stroke(Color.FromArgb("#eceded")),
                    Border(
                        Grid(
                            VStack(
                              Label().Text("합격보상금").TextColor(Colors.White).FontFamily(Helpers.FontHelper.PretendardMedium).FontSize(12),
                              Label().Text(item.Reward != null ? item.Reward.FormattedTotal : "").TextColor(Colors.White).FontFamily(Helpers.FontHelper.PretendardMedium).FontSize(12)
                            ).HStart(),

                            item.IsBookmark == true ?
                             Label().TextColor(Color.FromArgb("#3366ff")).FontFamily(Helpers.FontHelper.FontAwsomeSolid).Text(Helpers.IconHelper.Bookmark).HEnd().VStart().Margin(2)
                            :
                             Label().TextColor(Colors.White).FontFamily(Helpers.FontHelper.FontAwsomeRegular).Text(Helpers.IconHelper.Bookmark).HEnd().VStart().Margin(2)
                        )
                    ).Background(gradientBrush).StrokeCornerRadius(10).StrokeThickness(0).VStart().Padding(10)
                ).Padding(0, 5),

                Label().TextColor(Color.FromArgb("#333333")).FontFamily(Helpers.FontHelper.PretendardSemiBold).Text(item.Position).FontSize(12).HStart().LineBreakMode(LineBreakMode.TailTruncation).Padding(0, 0, 5, 0),
                Label().TextColor(Color.FromArgb("#b0b0b2")).FontFamily(Helpers.FontHelper.PretendardSemiBold).Text(item.Company.Name).FontSize(10),
                Border(
                 Label().TextColor(Color.FromArgb("#663d3d3d")).FontSize(10).FontFamily(Helpers.FontHelper.PretendardSemiBold).Text(item.Score.HasValue ? "AI 예측 " + (item.Score.Value * 100).ToString("F0") + "%" : "").FontSize(10)
                ).IsVisible(item.Score.HasValue).HStart().StrokeCornerRadius(5).BackgroundColor(Color.FromArgb("#e4e6e6")).Padding(5, 3).StrokeThickness(0).Margin(0, 5)
            ).Spacing(3).WidthRequest(positionItemSize).WithKey(item.Id);
        }
    }
}
