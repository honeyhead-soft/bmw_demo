using MauiReactor;
using MauiReactor.Parameters;
using MauiWanted.Components;
using MauiWanted.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Pages
{
    public class TempPageState
    {
        public string currentText { get; set; } = String.Empty; //안하면 오류

        public Microsoft.Maui.Controls.ImageSource imgSource { get; set; }

        public bool IsShown { get; set; }
        public bool? Result { get; set; }

        public bool IsPopupBottom { get; set; }
    }

    public partial class TempPage : Component<TempPageState>
    {
        [Prop]
        bool isVisible = false;

        [Param]
        IParameter<CustomStepperParameter> _customParameter;

        string imgPath = "https://static.tossinvestcdn.com/assets/image/detail-news-default/image12.png";

        private CommunityToolkit.Maui.Views.Popup? _popup;
        private CommunityToolkit.Maui.Views.Popup? _popupTop;


        public override VisualNode Render()
        {
            State.IsPopupBottom = false;

            return Grid(
                new AppBarComponent().AppBarText("데모").GridRow(0),

                VStack(
                    //Image().Source(State.imgSource).HeightRequest(200),
                    new ImageBox().ImageSize(200).ImageChangeAction(() =>
                    {
                        this.Invalidate();
                    }),
                    Label().Text("텍스트 입력").Padding(0,20,0,0).TextColor(Colors.Gray),
                    Entry().OnTextChanged((sender, item) =>
                    {
                        SetState(_ => _.currentText = item.NewTextValue);
                    }),
                    Label().Text(!Helpers.Utilities.IsNumeric(State.currentText) && !string.IsNullOrEmpty(State.currentText) ? "숫자만 입력해주세요" : State.currentText).TextColor(Colors.DeepPink),

                    new PopupHost(r => _popup = r)
                     {
                            new VStack(spacing: 10)
                            {
                                Border(
                                    VStack(
                                        Grid(
                                            Label("데모 안내").TextColor(Color.FromArgb("#32312D")).HCenter().VCenter().FontSize(18).FontFamily(Helpers.FontHelper.PretendardSemiBold),
                                            Label(IconHelper.Xmark).TextColor(Colors.White).TextColor(Color.FromArgb("#32312D")).HEnd().VCenter().FontSize(20)
                                            .FontFamily(Helpers.FontHelper.FontAwsomeSolid).Margin(5).BackgroundColor(Colors.Transparent).Padding(5)
                                            .OnTapped(async () =>
                                            {
                                                await _popup?.CloseAsync(true);
                                            })
                                        ),
                                        Grid().HeightRequest(1).BackgroundColor(Color.FromArgb("#E6E9E9")),
                                        Label().Text("오신 분들 너무 감사합니다").TextColor(Color.FromArgb("#32312D")).FontFamily(Helpers.FontHelper.PretendardSemiBold).HStart().FontSize(18).Padding(10,10,10,0),
                                        Label().Text("MAUI이 많이 사랑해주세요").TextColor(Color.FromArgb("#3d3d3d")).FontFamily(Helpers.FontHelper.PretendardRegular).HStart().FontSize(16).Padding(10,0,10,10),
                                        Button().Text("알겠습니다").GridColumn(1).BackgroundColor(Color.FromArgb("#333333")).TextColor(Colors.White).FontFamily(FontHelper.PretendardBold).OnClicked(async () =>
                                        {

                                        }).Margin(10,0)
                                    ).Padding(10).Spacing(15)
                                ).BackgroundColor(Colors.White).WidthRequest(Helpers.Utilities.GetDeviceSize().Width).StrokeCornerRadius(State.IsPopupBottom ? new CornerRadius(10,10,0,0) : new CornerRadius(10)).Padding(0,0,0,10).StrokeThickness(0)
                            }
                        }
                      .IsShown(State.IsShown)
                      .IsBottom(State.IsPopupBottom)
                      .OnClosed(result => SetState(s =>
                      {
                          s.IsShown = false;
                          s.Result = (bool?)result;
                      })),
                      Button().Text("팝업 열기").BackgroundColor(Color.FromArgb("#333333")).OnClicked(()=> SetState(_=> _.IsShown = true)),

                      new CustomStepperComponent().MinValue(1).MaxValue(10),

                      Label().Text(_customParameter.Value.Numeric.ToString())


                      //new CustomStepperComponent()

                ).GridRow(1).Padding(20,10).Spacing(5)

            ).Rows("Auto,*").IsVisible(isVisible); 
        }
    }
    


    public class ImageBoxState
    {
        public Microsoft.Maui.Controls.ImageSource imgSource { get; set; }
    }

    public partial class ImageBox : Component<ImageBoxState>
    {
        [Prop]
        public double imageSize = 200;

        [Prop]
        public Action imageChangeAction;
        public override VisualNode Render()
        {
            return Grid(
                Image().BackgroundColor(Colors.LightGray).Source(State.imgSource).Aspect(Aspect.AspectFit).HeightRequest(imageSize).OnTapped(async ()=>
                {
                    await ChangeImage();

                    imageChangeAction?.Invoke();
                })
            );
        }

        public async Task ChangeImage()
        {
            var result = await Microsoft.Maui.Storage.FilePicker.PickAsync(new Microsoft.Maui.Storage.PickOptions
            {
                PickerTitle = "Pick Image",
                FileTypes = Microsoft.Maui.Storage.FilePickerFileType.Images,
            });

            if (result == null)
            {
                return;
            }

            try
            {
                // 파일로부터 스트림을 가져옵니다.
                var imageStream = await result.OpenReadAsync();

                // 메모리 스트림에 복사합니다.
                using (var memoryStream = new MemoryStream())
                {
                    await imageStream.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin); // 스트림을 처음으로 되돌림

                    // 새로운 스트림을 통해 ImageSource 생성
                    var imageSource = Microsoft.Maui.Controls.ImageSource.FromStream(() =>
                    {
                        var copiedStream = new MemoryStream(memoryStream.ToArray());
                        return copiedStream;
                    });

                    SetState((_) => _.imgSource = imageSource);
                }
            }
            catch (Exception ex)
            {
                // 에러 로그 추가 가능
            }
        }
    }


    class CustomStepperParameter
    {
        public double Numeric { get; set; } = 3;
    }


    public partial class CustomStepperComponent : MauiReactor.Component
    {
        [Param]
        IParameter<CustomStepperParameter> _customParameter;

        [Prop]
        protected double minValue = 1;
        [Prop]
        protected double maxValue = 10;
        public override VisualNode Render()
        {
            return Grid(Stepper().Value(_customParameter.Value.Numeric).Maximum(maxValue).Minimum(minValue).OnValueChanged((item,value) =>
            {
                // _customParameter.Value.Numeric = value.NewValue; 이러면 안됨

                _customParameter.Set(_ => _.Numeric = value.NewValue);
            }));
        }
    }
}
