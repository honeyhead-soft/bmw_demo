
using MauiReactor;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using System;
using System.Runtime.CompilerServices;



namespace MauiWanted.Components.Native
{
    public class BorderlessEntry : MauiControls.Entry
    {
        public BorderlessEntry()
        {
        }

        public void Focus()
        {
            if (Handler == null || Handler.PlatformView == null)
            {
                throw new InvalidOperationException("Handler is not initialized.");
            }

#if ANDROID
            ((Android.Views.View)Handler.PlatformView).RequestFocus();
#elif IOS || MACCATALYST
            ((UIKit.UIView)Handler.PlatformView).BecomeFirstResponder();
#elif WINDOWS
            ((Microsoft.UI.Xaml.Controls.Control)Handler.PlatformView).Focus(Microsoft.UI.Xaml.FocusState.Programmatic);
#endif
        }

#if ANDROID
        private static Android.Graphics.Drawables.Drawable CreateHandleDrawable(Android.Graphics.Color color)
        {
            //var shape = new Android.Graphics.Drawables.ShapeDrawable(new Android.Graphics.Drawables.Shapes.RectShape());
            //shape.Paint.Color = color;
            //shape.SetIntrinsicWidth(32); // Set desired width
            //shape.SetIntrinsicHeight(32); // Set desired height
            //return shape;

            var shape = new Android.Graphics.Drawables.ShapeDrawable(new Android.Graphics.Drawables.Shapes.OvalShape());
            shape.Paint.Color = color;
            shape.SetIntrinsicWidth(32); // Set desired width
            shape.SetIntrinsicHeight(32); // Set desired height
            return shape;
        }
#endif
        public static void Configure()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("BorderlessEntry", (handler, view) =>
            {
                if (view is BorderlessEntry)
                {
#if ANDROID
                    //handler.PlatformView.InputType = handler.PlatformView.InputType | Android.Text.InputTypes.TextFlagNoSuggestions | Android.Text.InputTypes.TextVariationVisiblePassword | Android.Text.InputTypes.TextVariationPassword;
                    handler.PlatformView.SetSelectAllOnFocus(true);
                    handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);

                    //// PlaceHolder 텍스트 스타일 설정
                    //var hintText = handler.PlatformView.Hint;
                    //var spannableString = new Android.Text.SpannableString(hintText);
                    //spannableString.SetSpan(new Android.Text.Style.RelativeSizeSpan(1.0f), 0, hintText.Length, Android.Text.SpanTypes.ExclusiveExclusive);
                    //spannableString.SetSpan(new Android.Text.Style.ForegroundColorSpan(Android.Graphics.Color.Gray), 0, hintText.Length, Android.Text.SpanTypes.ExclusiveExclusive);
                    //handler.PlatformView.HintFormatted = spannableString;



                    handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                    handler.PlatformView.FocusChange += (s, e) =>
                    {
                        if (!e.HasFocus &&
                            Microsoft.Maui.ApplicationModel.Platform.CurrentActivity != null)
                            Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.HideKeyboard(handler.PlatformView);
                    };

                    #region 일단 보류
                        //var primaryColor = Microsoft.Maui.Graphics.Color.FromArgb("#dbff00");
                        //Android.Graphics.Color primaryColorForAndroid = Android.Graphics.Color.Argb((int)(primaryColor.Alpha * 255),
                        //(int)(primaryColor.Red * 255), (int)(primaryColor.Green * 255), (int)(primaryColor.Blue * 255));
                        //handler.PlatformView.TextCursorDrawable?.SetTint(primaryColorForAndroid); 
                    #endregion


                    //handler.PlatformView.SetHighlightColor(primaryColorForAndroid);
                    //handler.PlatformView.SetHintTextColor(primaryColorForAndroid);

                    //handler.PlatformView.TextSelectHandleLeft = CreateHandleDrawable(primaryColorForAndroid); //왼쪽 커서
                    //handler.PlatformView.TextSelectHandleRight = CreateHandleDrawable(primaryColorForAndroid); //오른쪽 커서
                    //handler.PlatformView.TextSelectHandle = CreateHandleDrawable(primaryColorForAndroid); //텍스트 단일 커서

                    //handler.PlatformView.SetHighlightColor(Android.Graphics.Color.Argb(255, 0, 255, 209));
                    //handler.PlatformView.SetHintTextColor(Android.Graphics.Color.Argb(255, 0, 255, 209));
                    //handler.PlatformView.TextCursorDrawable?.SetTint(Android.Graphics.Color.Argb(255, 0, 255, 209));
                    //handler.PlatformView.TextSelectHandle?.SetTint(Android.Graphics.Color.Argb(255, 0, 255, 209));
                    //handler.PlatformView.TextSelectHandleLeft?.SetTint(Android.Graphics.Color.Argb(255, 0, 255, 209));
                    //handler.PlatformView.TextSelectHandleRight?.SetTint(Android.Graphics.Color.Argb(255, 0, 255, 209));


#elif IOS || MACCATALYST
                    handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
                    handler.PlatformView.EditingDidBegin += (s, e) =>
                    {
                        handler.PlatformView.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
                    };
#elif WINDOWS
                    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                    handler.PlatformView.Background = null;

                    handler.PlatformView.GotFocus += (s, e) =>
                    {
                        handler.PlatformView.SelectAll();
                    };
#endif
                }
            });
        }
    }
}

namespace MauiWanted.Components.Controls
{
    [Scaffold(typeof(Native.BorderlessEntry))]
    public partial class BorderlessEntry
    {
        public void UpdateStyle()
        {
            MauiWanted.Components.Native.BorderlessEntry.Configure();
        }
        public void SetFocus()
        {
            NativeControl?.Focus();
        }
        public void SetEnabled(bool enable)
        {
            if (NativeControl != null)
            {
                NativeControl.IsEnabled = enable;
            }
        }

        public Native.BorderlessEntry GetNative()
            => NativeControl;
    }
}
