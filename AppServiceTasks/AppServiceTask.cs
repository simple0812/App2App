using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace AppServiceTasks
{
    public sealed class AppServiceTask : IBackgroundTask
    {
        private static BackgroundTaskDeferral _serviceDeferral;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //订阅关闭事件
            taskInstance.Canceled += TaskInstance_Canceled; ;
            _serviceDeferral = taskInstance.GetDeferral();
            var appService = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            //验证调用者
            if (appService?.Name == "appServicex")
            {
                //订阅调用者请求
                appService.AppServiceConnection.RequestReceived += AppServiceConnection_RequestReceived;
            }
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            throw new NotImplementedException();
        }

        private async void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            Debug.Write(".................");
            var message = args.Request.Message;
            string command = message["Command"] as string;
            switch (command)
            {
                case "CalcSum":
                    var messageDeferral = args.GetDeferral();
                    int value1 = (int)message["Value1"];
                    int value2 = (int)message["Value2"];
                    var returnMessage = new ValueSet();
                    returnMessage.Add("Rusult", value1 + value2);
                    //回应调用者
                    var responseStatus = await args.Request.SendResponseAsync(returnMessage);
                    messageDeferral.Complete();
                    break;
                case "Quit":
                    _serviceDeferral.Complete();
                    break;
            }
        }
    }
}
