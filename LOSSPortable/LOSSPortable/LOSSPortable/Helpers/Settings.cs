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

        private const string login = "login_key";
        private static readonly bool loginDefault = false;

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

        private const string UserNameKey = "username_key";
        private static readonly string UserNameDefault = "";

        private const string UserIDKey = "userid_key";
        private static readonly string UserIDDefault = "";

        private const string EndpointArn = "endpointarn";
        private static readonly string EndpointArnDefault = "";

        private const string favoriteKey = "favorite_key";
        private static readonly bool FavoriteDefault = false;

        private const string chatActive = "chatActive";
        private static readonly bool chatActiveDefault = false;

        private const string handShakeDone = "handshakeDone";
        private static readonly bool handShakeDefault = false;

        private const string conversationOn = "conversationOn";
        private static readonly bool conversationOnDefault = false;

        private const string isVolunteer = "volunteer";
        private static readonly bool isVolunteerDefault = false;

        private const string displayName = "displayName";
        private static readonly string displayNameDefault = "Anonymous";

        private const string toFromArn = "toFromArn";
        private static readonly string toFromArndefault = "";

        private const string messageCacheKey = "messageCacheKey";
        private static readonly string messageCacheKeyDefault = "";

        #endregion

        public static bool LoginSetting
        {
            get {  return AppSettings.GetValueOrDefault(login, loginDefault); }
            set {  AppSettings.AddOrUpdateValue(login, value); }
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

        public static string UsernameSetting
        {
            get { return AppSettings.GetValueOrDefault(UserNameKey, UserNameDefault); }
            set { AppSettings.AddOrUpdateValue(UserNameKey, value); }
        }

        public static string UserIDSetting
        {
            get { return AppSettings.GetValueOrDefault(UserIDKey, UserIDDefault); }
            set { AppSettings.AddOrUpdateValue(UserIDKey, value); }
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

        public static bool HandShakeDone
        {
            get { return AppSettings.GetValueOrDefault(handShakeDone, handShakeDefault); }
            set { AppSettings.AddOrUpdateValue(handShakeDone, value); }
        }

        public static bool ConversationOn
        {
            get { return AppSettings.GetValueOrDefault(conversationOn, conversationOnDefault); }
            set { AppSettings.AddOrUpdateValue(conversationOn, value); }
        }

        public static bool IsVolunteer
        {
            get { return AppSettings.GetValueOrDefault(isVolunteer, isVolunteerDefault); }
            set { AppSettings.AddOrUpdateValue(isVolunteer, value); }
        }

        public static string DisplayName
        {
            get { return AppSettings.GetValueOrDefault(displayName, displayNameDefault); }
            set { AppSettings.AddOrUpdateValue(displayName, value); }
        }

        public static string ToFromArn
        {
            get { return AppSettings.GetValueOrDefault(toFromArn, toFromArndefault); }
            set { AppSettings.AddOrUpdateValue(toFromArn, value); }
        }

        public static string MessageCacheKey
        {
            get { return AppSettings.GetValueOrDefault(messageCacheKey, messageCacheKeyDefault); }
            set { AppSettings.AddOrUpdateValue(messageCacheKey, value); }
        }
    }
}