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

        private const string portalAccessed = "portal_key";
        private static readonly bool pAccessDefault = false;

        private const string countKey = "portal_count";
        private static readonly int countDefault = 0;

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
        private static readonly string EndpointArnDefault = "";

        private const string favoriteKey = "favorite_key";
        private static readonly bool FavoriteDefault = false;

        private const string chatActive = "chatActive";
        private static readonly bool chatActiveDefault = false;
        #endregion

        public static bool portalAccessedSetting
        {
            get {  return AppSettings.GetValueOrDefault(portalAccessed, pAccessDefault); }
            set {  AppSettings.AddOrUpdateValue(portalAccessed, value); }
        }
        public static int portalAccessCount
        {
            get { return AppSettings.GetValueOrDefault(countKey, countDefault); }
            set { AppSettings.AddOrUpdateValue(countKey, value); }
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
            set { AppSettings.AddOrUpdateValue(EndpointArn, value);  }
        }

        public static bool FavoriteSetting
        {
            get { return AppSettings.GetValueOrDefault(favoriteKey, FavoriteDefault); }
            set { AppSettings.AddOrUpdateValue(favoriteKey, value); }
        }

        public static bool ChatActiveSetting
        {
            get { return AppSettings.GetValueOrDefault(chatActive, chatActiveDefault); }
            set { AppSettings.AddOrUpdateValue(chatActive, value); }
        }

    }
}