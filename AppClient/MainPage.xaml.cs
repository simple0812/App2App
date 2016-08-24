using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace AppClient
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //声明一个AppService连接对象
        private readonly AppServiceConnection connection = new AppServiceConnection
        {
            AppServiceName = "appServicex",
            PackageFamilyName = "6fdb4336-44bf-47d4-8386-075b1ec51d04_wbwwzgv8bqypp"
        };
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }
        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var connectionStatus = await connection.OpenAsync();
            if (connectionStatus == AppServiceConnectionStatus.Success)
            {
                //订阅双向通信
                connection.RequestReceived += Connection_RequestReceived;
            }
            else
            {
                //可以提示并导航到商店 让用户下载该服务
                await new MessageDialog("服务连接不成功").ShowAsync();
            }
        }
        private void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            // To do something
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var message = new ValueSet();
            message.Add("Command", "CalcSum");
            message.Add("Value1", int.Parse(txtNum1.Text));
            message.Add("Value2", int.Parse(txtNum2.Text));
            AppServiceResponse response = await connection.SendMessageAsync(message);
            if (response.Status == AppServiceResponseStatus.Success)
            {
                string sum = response.Message["Rusult"].ToString();
                await new MessageDialog(sum).ShowAsync();
            }
            else
            { }
        }
    }
}
