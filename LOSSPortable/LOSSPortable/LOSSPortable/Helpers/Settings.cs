// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace LOSSPortable.Helpers
{
      /// <summary>
      /// This is the Settings static class that can be used in your Core solution or in any
      /// of your client applications. All settings are laid out the same exact way with getters
      /// and setters. 
      /// </summary>
      public static class Settings
      {
        private static ISettings AppSettings
        {
          get
          {
            return CrossSettings.Current;
          }
        }

        #region Setting Constants

        private const string firstTimeKey = "first_key";
        private static readonly bool firstDefault = true;

        private const string AnonymousKey = "anonymous_key";
        private static readonly bool AnonymousDefault = false;

        private const string ContrastKey = "contrast_key";
        private static readonly bool ContrastDefault = false;

        private const string PushKey = "push_key";
        private static readonly bool PushDefault = true;

        private const string SpeechKey = "speech_key";
        private static readonly bool SpeechDefault = false;

        private const string UserEmailKey = "useremail_key";
        private static readonly string UserEmailDefault = "";

        private const string PasswordKey = "password_key";
        private static readonly string PasswordDefault = "";

        private const string EndpointArn = "endpointarn";
        private static readonly string EndpointArnDefault = null;

        #endregion

        public static bool FirstTimeSetting
        {
            get {  return AppSettings.GetValueOrDefault(firstTimeKey, firstDefault); }
            set {  AppSettings.AddOrUpdateValue(firstTimeKey, value); }
        }

        public static bool ContrastSetting
        {
            get {   return AppSettings.GetValueOrDefault(ContrastKey, ContrastDefault); }
            set {   AppSettings.AddOrUpdateValue(ContrastKey, value);  }
        }

        public static bool AnonymousSetting
        {
            get { return AppSettings.GetValueOrDefault(AnonymousKey, AnonymousDefault); }
            set { AppSettings.AddOrUpdateValue(AnonymousKey, value); }
        }

        public static bool PushSetting
        {
            get { return AppSettings.GetValueOrDefault(PushKey, PushDefault); }
            set { AppSettings.AddOrUpdateValue(PushKey, value); }
        }

        public static bool SpeechSetting
        {
            get { return AppSettings.GetValueOrDefault(SpeechKey, SpeechDefault); }
            set { AppSettings.AddOrUpdateValue(SpeechKey, value); }
        }

        public static string EmailSetting
        {
            get { return AppSettings.GetValueOrDefault(UserEmailKey, UserEmailDefault); }
            set { AppSettings.AddOrUpdateValue(UserEmailKey, value); }
        }

        public static string PasswordSetting
        {
            get { return AppSettings.GetValueOrDefault(PasswordKey, PasswordDefault); }
            set { AppSettings.AddOrUpdateValue(PasswordKey, value); }
        }

        public static string EndpointArnSetting
        {
            get { return AppSettings.GetValueOrDefault(EndpointArn, EndpointArnDefault); }
            set { AppSettings.AddOrUpdateValue(EndpointArnSetting, value);  }
        }
    }
}