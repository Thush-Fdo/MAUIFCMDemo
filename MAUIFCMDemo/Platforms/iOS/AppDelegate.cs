using Foundation;
using Firebase.CloudMessaging;
using MAUIFCMDemo.Platforms.iOS;
using UIKit;
using UserNotifications;
using static UIKit.UIGestureRecognizer;

namespace MAUIFCMDemo;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate, IMessagingDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        Firebase.Core.App.Configure();

        if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
        {
            var authOption = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;

            UNUserNotificationCenter.Current.RequestAuthorization(authOption, (granted, error) => { });

            UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

            Messaging.SharedInstance.AutoInitEnabled = true;
            Messaging.SharedInstance.Delegate = this;
        }

        UIApplication.SharedApplication.RegisterForRemoteNotifications();

        return base.FinishedLaunching(application, launchOptions);
    }
    
    [Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
    public void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
    {
        Messaging.SharedInstance.ApnsToken = deviceToken;
    }
    
    [Export("messaging:didReceiveRegistrationToken:")]
    public void DidReceiveRegistrationToken(Messaging message, string regToken)
    {
        Console.WriteLine(message.FcmToken);
        if (Preferences.ContainsKey("DeviceToken"))
        {
            Preferences.Remove("DeviceToken");
        }
        Preferences.Set("DeviceToken", regToken);
    }

}