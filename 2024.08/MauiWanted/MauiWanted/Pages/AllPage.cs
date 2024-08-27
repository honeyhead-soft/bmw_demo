using MauiReactor;
using MauiWanted.AppServices;
using MauiWanted.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Pages
{
    public partial class AllPage : Component
    {
        [Prop]
        bool isVisible = false;

        [Inject]
        AppServices.DebugService debugService;

        [Inject]
        ComponentCacheService _componentCache;

        protected override async void OnMounted()
        {
            base.OnMounted();
            //_componentCache.CacheComponents(debugService);
        }

        public override VisualNode Render()
        {
            return Grid(
                new AppBarComponent().AppBarText("전체").RightButtons(new RightButtonItem[]
                {
                                new RightButtonItem
                                {
                                    ButtonType = RightButtonType.Text,
                                    Content = Helpers.IconHelper.Meteor,
                                    FontFamily = Helpers.FontHelper.FontAwsomeSolid,
                                    OnTapped = () => Console.WriteLine("Search tapped")
                                },
                                new RightButtonItem
                                {
                                    ButtonType = RightButtonType.Text,
                                    Content = Helpers.IconHelper.Gear,
                                    FontFamily = Helpers.FontHelper.FontAwsomeSolid,
                                    OnTapped = () => Console.WriteLine("Search tapped")
                                }
                }),
                ScrollView(
                    VStack(
                         new IconBoxComponent().IconItems(new (string LabelText, string ImageSource, string path)[]
                         {
                                         ("면접 코칭받기", "Resources/Images/appicon_05.png", "/tab1"),
                                         ("다음 커리어 찾기", "Resources/Images/appicon_06.png", "/tab2"),
                                         ("나만의 회사 찾기", "Resources/Images/appicon_07.png", "/tab3")
                         }),

                         new OptionBoxComponent().HeaderText("취업ㆍ이직")


                    ).Padding(0, 20)
                ).GridRow(1)
            ).Rows("Auto,*").IsVisible(isVisible);
        }
    }


    class OptionBoxComponentState
    {
        
    }
    class OptionBoxItem
    {
        public string imgUrl { get; set; }
        public string itemText { get; set; }
        
        public string link { get; set; }
        public bool isShare { get; set; }
    }
    partial class OptionBoxComponent : Component<OptionBoxComponentState>
    {
        [Prop]
        string headerText;

        [Prop]
        protected OptionBoxItem[] optionSet = Array.Empty<OptionBoxItem>();

        protected override void OnMounted()
        {
            base.OnMounted();
        }
        public override VisualNode Render()
        {
            return VStack(
                Label().Text(headerText)
            ).Padding(15,10);
        }
    }
}