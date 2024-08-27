using MauiWanted.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.AppServices
{
    public class DebugService
    {
        public async Task<List<BannerFeed>> LoadBannerFeedsAsync()
        {
            // 리소스 네임스페이스 지정 (폴더 경로에 맞게 수정)
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MauiWanted.Data.banner.json"; // 실제 네임스페이스로 변경

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new FileNotFoundException("JSON 파일을 찾을 수 없습니다.");

            using StreamReader reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync();

            // Newtonsoft.Json을 사용하여 JSON을 BannerFeed 모델의 리스트로 역직렬화
            var bannerFeeds = JsonConvert.DeserializeObject<List<BannerFeed>>(jsonContent);

            return bannerFeeds;
        }

        public async Task<List<ThemeZip>> LoadThemeZipAsync()
        {
            // 리소스 네임스페이스 지정 (폴더 경로에 맞게 수정)
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MauiWanted.Data.themeZip.json"; // 실제 네임스페이스로 변경

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new FileNotFoundException("JSON 파일을 찾을 수 없습니다.");

            using StreamReader reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync();

            // Newtonsoft.Json을 사용하여 JSON을 BannerFeed 모델의 리스트로 역직렬화
            var dataSet = JsonConvert.DeserializeObject<List<ThemeZip>>(jsonContent);

            return dataSet;
        }

        public async Task<List<JobPosition>> LoadRecentPositionsAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MauiWanted.Data.recentPosition.json"; // 리소스 경로를 실제로 맞춰야 함

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new FileNotFoundException("JSON 파일을 찾을 수 없습니다.");

            using StreamReader reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync();

            // JobPosition 리스트로 역직렬화
            var recentPositions = JsonConvert.DeserializeObject<List<JobPosition>>(jsonContent);
            return recentPositions;
        }

        public async Task<List<JobPosition>> LoadRizingPositionsAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MauiWanted.Data.rizingtPosition.json"; // 리소스 경로를 실제로 맞춰야 함

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new FileNotFoundException("JSON 파일을 찾을 수 없습니다.");

            using StreamReader reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync();

            // JobPosition 리스트로 역직렬화
            var recentPositions = JsonConvert.DeserializeObject<List<JobPosition>>(jsonContent);
            return recentPositions;
        }

        public async Task<List<JobPosition>> LoadInterestPositionsAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MauiWanted.Data.interestPosition.json"; // 리소스 경로를 실제로 맞춰야 함

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new FileNotFoundException("JSON 파일을 찾을 수 없습니다.");

            using StreamReader reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync();

            // JobPosition 리스트로 역직렬화
            var recentPositions = JsonConvert.DeserializeObject<List<JobPosition>>(jsonContent);
            return recentPositions;
        }

        public async Task<CompanyCollection> LoadCompanyCollectionAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MauiWanted.Data.companiesTag.json"; // 실제 리소스 파일 경로에 맞게 변경 필요

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new FileNotFoundException("JSON 파일을 찾을 수 없습니다.");

            using StreamReader reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync();

            var companyCollection = JsonConvert.DeserializeObject<CompanyCollection>(jsonContent);

            return companyCollection;
        }

        public async Task<List<TagSearch>> LoadTagSearchAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MauiWanted.Data.tagSearch.json"; // 실제 리소스 파일 경로에 맞게 변경 필요

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new FileNotFoundException("JSON 파일을 찾을 수 없습니다.");

            using StreamReader reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync();

            var collection = JsonConvert.DeserializeObject<List<TagSearch>>(jsonContent);

            return collection;
        }
    }
}
