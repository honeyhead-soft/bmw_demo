using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Components
{
    public class MoldStepPageState
    {
        public double DeviceWidth { get; set; }
        public double DeviceHeight { get; set; }
    }
    public partial class MoldStepPage : Component<MoldStepPageState>
    {
        private readonly VisualNode? _child;

        [Prop]
        int maxStep;

        [Prop]
        int currentStep;

        [Prop]
        string headerTitle = string.Empty;

        [Prop]
        string subTitle = string.Empty;

        [Prop]
        string buttonText = "Next";

        [Prop]
        string buttonEnabledText = "Completed";

        [Prop]
        Action onButtonFire;

        [Prop]
        bool isButtonEnabled = false;

        [Prop]
        bool isBackButtonVisible = true;

        [Prop]
        bool isBottomButtonVisible = true;

        [Prop]
        string centerText = "Empty";

        [Prop]
        bool isCenterText = false;

        [Prop]
        bool isRightText = false;

        [Prop]
        string rightText = "Empty";

        [Prop]
        Action onRightButtonAction;

        [Prop]
        string rightButtonFontFamily = Helpers.FontHelper.PretendardRegular;

        [Prop]
        Microsoft.Maui.Graphics.Color rightButtonTextColor = Color.FromArgb("#828282");

        public MoldStepPage(VisualNode? child = null)
        {
            _child = child;
        }

        private void GetDeviceSize()
        {
            var size = Helpers.Utilities.GetDeviceSize();
            SetState(_ => _.DeviceWidth = size.Width, false);
            SetState(_ => _.DeviceHeight = size.Height, false);
        }

        protected override void OnMounted()
        {
            base.OnMounted();
            GetDeviceSize();
        }
        public override VisualNode Render()
        {
            return ContentPage(
                Grid(
                Grid().GridRow(0).HeightRequest(1).BackgroundColor(Color.FromArgb("#dbff00")).WidthRequest(CalculateWidth()).HStart(),
                new AppBarComponent().AppBarText("전체").GridRow(1),
                Label().Text(centerText).TextColor(Colors.White).FontFamily(Helpers.FontHelper.PretendardSemiBold).GridRow(1).Center().FontSize(16).IsVisible(isCenterText),
                Label().Text(rightText).FontFamily(rightButtonFontFamily).TextColor(rightButtonTextColor).GridRow(1).VCenter().HEnd()
                 .HorizontalTextAlignment(TextAlignment.Start).FontSize(14).IsVisible(isRightText).Margin(0, 0, 5, 0).Padding(20, 10).OnTapped(() =>
                 {
                     if (onRightButtonAction != null)
                     {
                         onRightButtonAction.Invoke();
                     }
                 }),
                CreateTitleSection().GridRow(2),
              Grid(
                _child
              ).GridRow(3),

              Button().GridRow(4).Padding(20).Margin(20)
              .Text(isButtonEnabled ? buttonEnabledText : buttonText)
              .IsVisible(isBottomButtonVisible)
              .BackgroundColor(isButtonEnabled ? Color.FromArgb("#333333") : Color.FromArgb("#222222"))
              .TextColor(isButtonEnabled ? Color.FromArgb("#161616") : Color.FromArgb("#33FFFFFF"))
              .FontFamily(isButtonEnabled ? Helpers.FontHelper.PretendardBold : Helpers.FontHelper.PretendardSemiBold).FontSize(18)
              .OnTapped(() =>
              {
                  if (onButtonFire != null)
                  {
                      onButtonFire.Invoke();
                  }
              })
           ).Rows("Auto,Auto,Auto,*,Auto")
           ).HasNavigationBar(false).BackgroundColor(Color.FromArgb("#000000"));
        }

        public VStack CreateTitleSection()
        {
            if (string.IsNullOrEmpty(headerTitle))
            {
                return VStack().Padding(0);
            }
            return VStack(
              Label(text: headerTitle).TextColor(Colors.White).FontSize(24).FontFamily(Helpers.FontHelper.PretendardSemiBold),
              Label(text: subTitle).TextColor(Color.FromArgb("#adadad")).FontSize(16).FontFamily(Helpers.FontHelper.PretendardRegular).IsVisible(!string.IsNullOrEmpty(subTitle))
            ).Spacing(10).Padding(20, 30, 20, 20);
        }

        public double CalculateWidth()
        {
            if (maxStep == 0)
            {
                return 0;
            }

            // maxStep을 DeviceWidth로 나눈 값에 currentStep을 곱하여 width 계산
            double width = (State.DeviceWidth / maxStep) * currentStep;

            return width;
        }
    }
}
