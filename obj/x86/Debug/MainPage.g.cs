﻿#pragma checksum "D:\SELA projects\GameProject\GameProject\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6BC5BBB70815D171D895D7603B62AC0A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameProject
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainPage.xaml line 12
                {
                    this.GameCanvas = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 3: // MainPage.xaml line 14
                {
                    this.CommandBarUI = (global::Windows.UI.Xaml.Controls.CommandBar)(target);
                }
                break;
            case 4: // MainPage.xaml line 25
                {
                    this.OverlayText = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5: // MainPage.xaml line 26
                {
                    this.OverlaySubtext = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 6: // MainPage.xaml line 27
                {
                    this.LevelDisplay = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 7: // MainPage.xaml line 28
                {
                    this.MusicPlayer = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.MusicPlayer).MediaEnded += this.MediaEnded;
                }
                break;
            case 8: // MainPage.xaml line 16
                {
                    this.Title = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9: // MainPage.xaml line 18
                {
                    this.MuteButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.MuteButton).Click += this.MuteButton_Click;
                }
                break;
            case 10: // MainPage.xaml line 19
                {
                    this.LoadButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.LoadButton).Click += this.LoadButton_Click;
                }
                break;
            case 11: // MainPage.xaml line 20
                {
                    this.SaveButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.SaveButton).Click += this.SaveButton_Click;
                }
                break;
            case 12: // MainPage.xaml line 21
                {
                    this.RestartButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.RestartButton).Click += this.RestartButton_click;
                }
                break;
            case 13: // MainPage.xaml line 22
                {
                    this.PauseButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.PauseButton).Click += this.PauseButton_click;
                }
                break;
            case 14: // MainPage.xaml line 23
                {
                    this.CancelButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.CancelButton).Click += this.CloseButton_click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
