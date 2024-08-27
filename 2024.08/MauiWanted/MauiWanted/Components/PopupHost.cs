using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using MauiReactor;
using MauiReactor.Internals;
using System;

namespace MauiWanted.Components
{
    [Scaffold(typeof(CommunityToolkit.Maui.Views.Popup))]
    partial class Popup
    {
        protected override void OnAddChild(VisualNode widget, MauiControls.BindableObject childNativeControl)
        {
            if (childNativeControl is MauiControls.View content)
            {
                Validate.EnsureNotNull(NativeControl);
                NativeControl.Content = content;
            }

            base.OnAddChild(widget, childNativeControl);
        }

        protected override void OnRemoveChild(VisualNode widget, MauiControls.BindableObject childNativeControl)
        {
            Validate.EnsureNotNull(NativeControl);

            if (childNativeControl is MauiControls.View content &&
                NativeControl.Content == content)
            {
                NativeControl.Content = null;
            }
            base.OnRemoveChild(widget, childNativeControl);
        }
    }

    class PopupHost : Component
    {
        private CommunityToolkit.Maui.Views.Popup? _popup;
        private bool _isShown;
        private bool _isBottom;
        private Action<object?>? _onCloseAction;
        private readonly Action<CommunityToolkit.Maui.Views.Popup?>? _nativePopupCreateAction;

        public PopupHost(Action<CommunityToolkit.Maui.Views.Popup?>? nativePopupCreateAction = null)
        {
            _nativePopupCreateAction = nativePopupCreateAction;
        }

        public PopupHost IsShown(bool isShown)
        {
            _isShown = isShown;
            return this;
        }

        public PopupHost IsBottom(bool isBottom)
        {
            _isBottom = isBottom;
            return this;
        }

        public PopupHost OnClosed(Action<object?> action)
        {
            _onCloseAction = action;
            return this;
        }

        protected override void OnMounted()
        {
            InitializePopup();
            base.OnMounted();
        }

        protected override void OnPropsChanged()
        {
            InitializePopup();
            base.OnPropsChanged();
        }

        void InitializePopup()
        {
            if (_isShown && MauiControls.Application.Current != null)
            {
                MauiControls.Application.Current?.Dispatcher.Dispatch(() =>
                {
                    if (ContainerPage == null || _popup == null)
                    {
                        return;
                    }
                    if (_isBottom)
                    {

                        _popup.VerticalOptions = Microsoft.Maui.Primitives.LayoutAlignment.End;
                        _popup.Color = Colors.Transparent;
                    }
                    _popup.CanBeDismissedByTappingOutsideOfPopup = true; //팝업 외부 영역 클릭 시 종료

                    try
                    {
                        ContainerPage.ShowPopup(_popup);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                        throw;
                    }
                });
            }
        }

        public override VisualNode Render()
        {
            var children = Children();
            return _isShown ?
                new Popup(r =>
                    {
                        if (r != null) //상위에 팝업 관련 된 게 존재하는지 질의
                        {
                            r.Color = Colors.Transparent; // 여기서 오류 발생 가능
                            _popup = r;
                        }
                        _nativePopupCreateAction?.Invoke(r);
                    }
                )
                {
                children[0]
                }
                .OnClosed(OnClosed)
                : null!;
        }

        void OnClosed(object? sender, PopupClosedEventArgs args)
        {
            _onCloseAction?.Invoke(args.Result);
        }
    }
}
