using MauiReactor;
using MauiReactor.Compatibility;
using MauiWanted.AppServices;
using MauiWanted.Components;
using MauiWanted.Models;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MauiWanted.Pages
{
    class HomePageState
    {
        //public Microsoft.Maui.Controls.ImageSource _imageSource;
    }
    partial class HomePage : Component<HomePageState>
    {
        //내가 관심 있을 만한 포지션 https://www.wanted.co.kr/api/chaos/recruit/v1/positions/recommended?1723995741082=
        //최근 본 포지션 https://www.wanted.co.kr/api/chaos/jobs/v1/history?1723995741082=&limit=20
        //업계 평균이상 https://www.wanted.co.kr/api/chaos/recruit/v2/companies/action?1723995741082=
        //요즘 뜨는 포지션 https://www.wanted.co.kr/api/chaos/recruit/v1/positions/week?1723995741082=

        [Inject]
        AppServices.DebugService debugService;

        [Inject]
        ComponentCacheService _componentCache;

        [Prop]
        bool isVisible = false;

        protected override async void OnMounted()
        {
            base.OnMounted();

            _componentCache.CacheComponents(debugService);
        }
        public override VisualNode Render()
        {
            //if (!isVisible)
            //{
            //    return Grid();
            //}
            return Grid(
                new AppBarComponent().AppBarText("채용").RightButtons(new RightButtonItem[]
                {
                                new RightButtonItem
                                {
                                    ButtonType = RightButtonType.Text,
                                    Content = Helpers.IconHelper.MagnifyingGlass,
                                    FontFamily = Helpers.FontHelper.FontAwsomeSolid,
                                    OnTapped = () => Console.WriteLine("Search tapped")
                                },
                                new RightButtonItem
                                {
                                    ButtonType = RightButtonType.Text,
                                    Content = Helpers.IconHelper.Bell,
                                    FontFamily = Helpers.FontHelper.FontAwsomeRegular,
                                    OnTapped = () => Console.WriteLine("Notifications tapped")
                                },
                                new RightButtonItem
                                {
                                    ButtonType = RightButtonType.Image,
                                    Content = "https://www.blockmedia.co.kr/wp-content/uploads/2024/03/%EB%B0%88%EC%BD%94%EC%9D%B8_%EB%8F%84%EA%B7%B8%EC%9C%84%ED%94%84%ED%96%87_%ED%8E%98%ED%8E%98.webp",
                                    OnTapped = () => Console.WriteLine("Notifications tapped")
                                }
                }).GridRow(0),
                ScrollView(
                     VStack(
                         new IconBoxComponent().IconItems(new (string LabelText, string ImageSource, string path)[]
                         {
                                         ("채용공고", "Resources/Images/appicon_01.png", "/tab1"),
                                         ("이력서 관리", "Resources/Images/appicon_02.png", "/tab2"),
                                         ("커리어 조회", "Resources/Images/appicon_03.png", "/tab3"),
                                         ("지원 현황", "Resources/Images/appicon_04.png", "/tab4")
                         }),

            #region 기본
                     //new NormalTypePositionItemComponent().HeaderText("최근 본 포지션").RightButtonText("전체보기").IsRightButtonVisible(true).PropJobList(debugService.LoadRecentPositionsAsync()),
                     //new MainCarouselComponent().PropCarouselSource(debugService.LoadBannerFeedsAsync()),
                     //new NormalTypePositionItemComponent().HeaderText("내가 관심 있을 만한 포지션").RightButtonText("전체보기").IsRightButtonVisible(true).PropJobList(debugService.LoadInterestPositionsAsync()),
                     //new TagCompanyTypeItemComponent().HeaderText("#유망산업").RightButtonText("포지션으로 더보기").IsRightButtonVisible(true).PropCompanySet(debugService.LoadCompanyCollectionAsync()),
                     //new TagSearchTypeItemComponent().HeaderText("#태그로 탐색해 보세요").IsRightButtonVisible(false).PropTagSearchSet(debugService.LoadTagSearchAsync()),
                     //new TagMoaTypeItemComponent().HeaderText("테마로 모아보기").IsRightButtonVisible(false).PropThemeZipSet(debugService.LoadThemeZipAsync()),
                     //new NormalTypePositionItemComponent().HeaderText("요즘 뜨는 포지션").IsRightButtonVisible(false).PropJobList(debugService.LoadRizingPositionsAsync())
            #endregion



            #region 개선 버전
                     _componentCache.GetComponent<NormalTypePositionItemComponent>("recentPositions"),
                     _componentCache.GetComponent<MainCarouselComponent>("mainCarousel"),
                     _componentCache.GetComponent<NormalTypePositionItemComponent>("interestPositions"),
                     _componentCache.GetComponent<TagCompanyTypeItemComponent>("promisingIndustry"),
                     _componentCache.GetComponent<TagSearchTypeItemComponent>("tagSearch"),
                     _componentCache.GetComponent<TagMoaTypeItemComponent>("themeCollection"),
                     _componentCache.GetComponent<NormalTypePositionItemComponent>("risingPositions")
            #endregion


                     ).Padding(0, 20)
                ).GridRow(1)
            ).Rows("Auto,*").IsVisible(isVisible);//.Set(Microsoft.Maui.Controls.CompressedLayout.IsHeadlessProperty, true);
        }
    }
}
