using MauiReactor;
using MauiWanted.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Components
{
    public class MainCarouselState
    {
        public int CarouselIndex { get; set; }

        public List<BannerFeed> carouselSource { get; set; }

        public bool Loading;
    }
    partial class MainCarouselComponent : Component<MainCarouselState>
    {
        [Prop]
        Task<List<BannerFeed>> propCarouselSource;

        Microsoft.Maui.Controls.CarouselView carouselView;

        protected override async void OnMounted()
        {
            //base.OnMounted();
            //// 로딩 상태를 먼저 처리
            //State.carouselSource = null;
            //Invalidate(); // 로딩 중이라는 상태로 UI 업데이트

            //if (propCarouselSource != null)
            //{
            //    // 비동기 작업으로 데이터 로드
            //    var jobList = await propCarouselSource;
            //    State.carouselSource = jobList; // 데이터가 준비된 후 상태 업데이트
            //    Invalidate(); // 데이터 로드 후 다시 UI 업데이트
            //}

            base.OnMounted();
            State.carouselSource = null; // 초기화
            State.Loading = true;      // 로딩 상태 시작

            if (propCarouselSource != null)
            {
                State.carouselSource = await propCarouselSource;
            }

            State.Loading = false;     // 로딩 상태 종료
        }

        public override VisualNode Render()
        {
            return State.carouselSource == null ?
                 Grid() //로딩 중
                 :
                 CarouselView((sen) => carouselView = sen).ItemsSource(State.carouselSource, _ =>
                 {
                     return Grid(
                             Border(
                                 new CustomCachedImage().Source(_.background_image).Aspect(Aspect.AspectFit).CacheType(FFImageLoading.Cache.CacheType.All).CacheDuration(TimeSpan.FromHours(24)).LoadingPlaceholder("Resource/Images/imageplaceholder.png").FadeAnimationEnabled(true).FadeAnimationDuration(200)
                             ).Stroke(Colors.Transparent).StrokeCornerRadius(10),
                             VStack(
                               Label().Text(_.title).TextColor(_.is_darkText ? Colors.Black : Colors.White).FontSize(12).FontFamily(Helpers.FontHelper.PretendardSemiBold),
                               Label().Text(_.subtitle).TextColor(_.is_darkText ? Colors.Black : Colors.White).FontSize(10).FontFamily(Helpers.FontHelper.PretendardRegular)
                             ).Padding(15, 20).VStart()
                         ).BackgroundColor(Colors.Transparent);
                 }).HorizontalScrollBarVisibility(ScrollBarVisibility.Never).OnPositionChanged((s, args) =>
                 {
                     int newIndex = args.CurrentPosition;
                     SetState(_ => _.CarouselIndex = newIndex + 1);
                 }).OnSizeChanged(() =>
                 {
                     this.Invalidate();
                 }).Margin(20, 20);
                 //.OnPanUpdated((s, e) =>
                 //{
                 //    switch (e.StatusType)
                 //    {
                 //        case GestureStatus.Running:
                 //            if (Math.Abs(e.TotalX) > Math.Abs(e.TotalY))
                 //            {
                 //                // 가로 스크롤 중
                 //                carouselView.IsSwipeEnabled = true;
                 //                //((CarouselView)sender).IsScrollEnabled = true;
                 //            }
                 //            else
                 //            {
                 //                // 세로 스크롤 중
                 //                carouselView.IsSwipeEnabled = false;
                 //                //((CarouselView)sender).IsScrollEnabled = false;
                 //            }
                 //            break;

                 //        case GestureStatus.Completed:
                 //        case GestureStatus.Canceled:
                 //            carouselView.IsSwipeEnabled = true;
                 //            //((CarouselView)sender).IsScrollEnabled = true;
                 //            break;
                 //    }
                 //});
        }
    }
}
