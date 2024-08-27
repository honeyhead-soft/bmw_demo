using MauiWanted.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.AppServices
{
    public class ComponentCacheService
    {
        //private Dictionary<string, object> _cache = new Dictionary<string, object>();
        private ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();  //스레드 간 충돌 방지


        public void CacheComponents(AppServices.DebugService _debugService)
        {
            AppServices.DebugService debugService = _debugService;

            _cache["recentPositions"] = new NormalTypePositionItemComponent()
                .HeaderText("최근 본 포지션")
                .RightButtonText("전체보기")
                .IsRightButtonVisible(true)
                .PropJobList(debugService.LoadRecentPositionsAsync());

            _cache["mainCarousel"] = new MainCarouselComponent()
                .PropCarouselSource(debugService.LoadBannerFeedsAsync());

            _cache["interestPositions"] = new NormalTypePositionItemComponent()
                .HeaderText("내가 관심 있을 만한 포지션")
                .RightButtonText("전체보기")
                .IsRightButtonVisible(true)
                .PropJobList(debugService.LoadInterestPositionsAsync());

            _cache["promisingIndustry"] = new TagCompanyTypeItemComponent()
                .HeaderText("#유망산업")
                .RightButtonText("포지션으로 더보기")
                .IsRightButtonVisible(true)
                .PropCompanySet(debugService.LoadCompanyCollectionAsync());

            _cache["tagSearch"] = new TagSearchTypeItemComponent()
                .HeaderText("#태그로 탐색해 보세요")
                .IsRightButtonVisible(false)
                .PropTagSearchSet(debugService.LoadTagSearchAsync());

            _cache["themeCollection"] = new TagMoaTypeItemComponent()
                .HeaderText("테마로 모아보기")
                .IsRightButtonVisible(false)
                .PropThemeZipSet(debugService.LoadThemeZipAsync());

            _cache["risingPositions"] = new NormalTypePositionItemComponent()
                .HeaderText("요즘 뜨는 포지션")
                .IsRightButtonVisible(false)
                .PropJobList(debugService.LoadRizingPositionsAsync());
        }

        public T GetComponent<T>(string key) where T : class
        {
            //if (_cache.ContainsKey(key))
            //{
            //    return _cache[key] as T;
            //}
            if (_cache.TryGetValue(key, out var component) && component is T)
            {
                return component as T;
            }
            return null;
        }
    }
}
