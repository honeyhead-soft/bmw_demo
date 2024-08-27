using CommunityToolkit.Maui.Core;
using FFImageLoading.Maui;
using MauiReactor;
using MauiWanted.Components;
using MauiWanted.Helpers;
using MauiWanted.Models;
using MauiWanted.Resources;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiWanted.Pages
{
    public enum PageEnum
    {
        Home,
        Career,
        Social,
        MyWanted,
        All
    }

    public class MainPageState
    {
        public PageEnum CurrentPage { get; set; } = PageEnum.Home;
    }

    internal class MainPage : Component<MainPageState>
    {
        (PageEnum Page, string LabelText, string ImageSource, bool isNoChangeImage)[] dummyTabItems = new (PageEnum Page, string LabelText, string ImageSource, bool isNoChangeImage)[]
       {
            (PageEnum.Home, "채용", "Resources/Images/tab_01.png", false),
            (PageEnum.Career, "커리어", "Resources/Images/tab_02.png", false),
            (PageEnum.Social, "소셜", "Resources/Images/tab_03.png", false),
            (PageEnum.MyWanted, "MY 원티드", "https://www.blockmedia.co.kr/wp-content/uploads/2024/03/%EB%B0%88%EC%BD%94%EC%9D%B8_%EB%8F%84%EA%B7%B8%EC%9C%84%ED%94%84%ED%96%87_%ED%8E%98%ED%8E%98.webp", true),
            (PageEnum.All, "전체", "Resources/Images/tab_05.png", false)
       };

        public override VisualNode Render()
        {
            //StatusBarBehaviorExtensions.StatusBarColor(Microsoft.Maui.Controls.Application.Current.MainPage, Colors.Red);
            return ContentPage(
            new StatusBarBehavior()
                .StatusBarColor(Colors.White)
                .StatusBarStyle(StatusBarStyle.DarkContent),
                Grid(
                    RenderPage(),
#if !IOS

#endif
                    Grid().HeightRequest(1).BackgroundColor(Color.FromArgb("#E4E6E6")).GridRow(1).VStart(),
                    Grid(
                        new BottomTabComponent()
                        .ColumnCount(5)
                        .CurrentTab(State.CurrentPage)
                        .TabItems(dummyTabItems)
                        .OnTabChanged((idx)=>
                        {
                            SetState(_ => _.CurrentPage = (PageEnum)idx);
                        })                      
                    ).GridRow(1)
                ).Rows("*,Auto")
            ).HasNavigationBar(false).BackgroundColor(ThemePack.AppBackground);
        }
        //public VisualNode RenderPage()
        //{
        //    return State.CurrentPage switch
        //    {
        //        PageEnum.Home => new HomePage(),
        //        PageEnum.Career => Label("Home2").TextColor(Colors.Red).Center(),
        //        PageEnum.Social => Label("Home").TextColor(Colors.Red).Center(),
        //        PageEnum.MyWanted => Label("Home"),
        //        PageEnum.All => Label("Home")
        //    };
        //}

        Grid RenderPage()
        {
            return Grid(
                new HomePage().IsVisible(State.CurrentPage == PageEnum.Home ? true : false),

                new TempPage().IsVisible(State.CurrentPage == PageEnum.Career ? true : false),

                new TempPage().IsVisible(State.CurrentPage == PageEnum.Social ? true : false),

                new TempPage().IsVisible(State.CurrentPage == PageEnum.MyWanted ? true : false),

                new AllPage().IsVisible(State.CurrentPage == PageEnum.All ? true : false)
            );
        }
    }
    
}