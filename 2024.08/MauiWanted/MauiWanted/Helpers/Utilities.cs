using MauiReactor;
using Microsoft.Maui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MauiWanted.Helpers
{
    public class Utilities()
    {
        public static (double Width, double Height) GetDeviceSize()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            // Screen width and height in pixels
            double screenWidth = mainDisplayInfo.Width;
            double screenHeight = mainDisplayInfo.Height;

            // Screen density
            double density = mainDisplayInfo.Density;

            // Screen width and height in device-independent units (DIPs)
            double screenWidthDip = screenWidth / density;
            double screenHeightDip = screenHeight / density;

            return (screenWidthDip, screenHeightDip);
        }
        public static (double Width, double Height) GetSizeByAspectRatio(double aspectWidth, double aspectHeight)
        {
            // Get the device width using GetDeviceSize
            var (deviceWidth, _) = GetDeviceSize();

            // Calculate the height based on the provided aspect ratio
            double height = (deviceWidth * aspectHeight) / aspectWidth;

            return (deviceWidth, height);
        }

        public static bool IsNumeric(string input)
        {
            // 숫자만 포함된 문자열인지 검사하는 정규식
            string pattern = @"^\d+$";
            return Regex.IsMatch(input, pattern);
        }
    }
}
