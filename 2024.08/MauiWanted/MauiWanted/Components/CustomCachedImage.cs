using FFImageLoading.Maui;
using MauiReactor;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MauiWanted.Components
{
    [Scaffold(typeof(CachedImage))]
    public partial class CustomCachedImage
    {
        public FFImageLoading.Maui.CachedImage GetNative()
        {
            if (this.NativeControl != null)
            {
                return this.NativeControl;
            }
            return null;
        }

        public CustomCachedImage WebSource(string uri)
        {
            this.Source(new Uri(uri));

            return this;
        }

        //public CustomCachedImage WebSourceSSL(string url)
        //{

        //    try
        //    {
        //        // SSL 인증서 검증을 무시하는 HttpClientHandler 설정
        //        HttpClientHandler handler = new HttpClientHandler();
        //        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;

        //        HttpClient client = new HttpClient(handler);

        //        // 비동기 작업을 동기적으로 대기 (주의: UI 스레드에서 사용 시 주의 필요)
        //        var imageBytes = client.GetByteArrayAsync(url).Result;

        //        // 이미지 데이터를 메모리 스트림으로 변환하여 ImageSource 생성
        //        this.Source(Microsoft.Maui.Controls.ImageSource.FromStream(() => new MemoryStream(imageBytes)));

        //        return this;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"이미지 로드 실패: {ex.Message}");
        //        return null;
        //    }
        //} 
        //public CustomCachedImage WebSourceSSL(string url)
        //{
        //    try
        //    {
        //        // SSL 인증서 검증을 무시하는 HttpClientHandler 설정 (실제 배포 시에는 사용 금지)
        //        HttpClientHandler handler = new HttpClientHandler
        //        {
        //            ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
        //        };

        //        using (HttpClient client = new HttpClient(handler))
        //        {
        //            // 동기적으로 이미지 데이터를 가져옴 (주의: UI 스레드에서 실행하지 않도록 해야 함)
        //            var imageBytes = client.GetByteArrayAsync(url).GetAwaiter().GetResult();

        //            // 이미지 데이터를 메모리 스트림으로 변환하여 ImageSource 생성
        //            this.Source(Microsoft.Maui.Controls.ImageSource.FromStream(() => new MemoryStream(imageBytes)));

        //            return this;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"이미지 로드 실패: {ex.Message}");
        //        // 예외 발생 시 대체 이미지 제공
        //        //this.Source("error_image_placeholder.png");
        //        return this;
        //    }
        //}

        public CustomCachedImage WebSourceSSL(string url, TimeSpan cacheDuration)
        {
            try
            {
                // 앱 전용 디렉터리에 캐시 경로 정의
                var cacheDirectory = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "imageCache");
                Directory.CreateDirectory(cacheDirectory);

                // 이미지의 해시값을 파일명으로 사용
                var cacheFileName = Path.Combine(cacheDirectory, GetHash(url) + ".img");

                // 캐시 만료 및 정리 처리
                ClearExpiredCache(cacheDirectory, cacheDuration);

                // 캐시된 이미지가 있는지 확인하고 만료되지 않았는지 확인
                if (File.Exists(cacheFileName))
                {
                    var fileInfo = new FileInfo(cacheFileName);

                    // 파일이 캐시 만료 시간을 초과하지 않았는지 확인
                    if (DateTime.Now - fileInfo.CreationTime < cacheDuration)
                    {
                        // 캐시된 이미지 로드
                        var imageBytes = File.ReadAllBytes(cacheFileName);
                        this.Source(Microsoft.Maui.Controls.ImageSource.FromStream(() => new MemoryStream(imageBytes)));
                        return this;
                    }
                }

                // SSL 인증서 검증을 무시하는 HttpClientHandler 설정 (실제 배포 시에는 사용 금지)
                HttpClientHandler handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
                };

                using (HttpClient client = new HttpClient(handler))
                {
                    // 동기적으로 이미지 데이터를 가져옴 (주의: UI 스레드에서 실행하지 않도록 해야 함)
                    var imageBytes = client.GetByteArrayAsync(url).GetAwaiter().GetResult();

                    // 캐시에 저장
                    File.WriteAllBytes(cacheFileName, imageBytes);

                    // 이미지 데이터를 메모리 스트림으로 변환하여 ImageSource 생성
                    this.Source(Microsoft.Maui.Controls.ImageSource.FromStream(() => new MemoryStream(imageBytes)));
                }

                return this;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"이미지 로드 실패: {ex.Message}");
                this.Source("error_image_placeholder.png");
                return this;
            }
        }

        // 캐시 만료 시간을 넘긴 파일들을 삭제하는 메서드
        private void ClearExpiredCache(string cacheDirectory, TimeSpan cacheDuration)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(cacheDirectory);

                foreach (var file in directoryInfo.GetFiles())
                {
                    // 파일의 생성 시간과 캐시 만료 시간을 비교하여 만료된 파일 삭제
                    if (DateTime.Now - file.CreationTime > cacheDuration)
                    {
                        file.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"캐시 정리 중 오류 발생: {ex.Message}");
            }
        }

        // URL 해시값 계산
        private string GetHash(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }


    }
}
