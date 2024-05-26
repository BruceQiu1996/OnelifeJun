namespace LiJunSpace.Helpers
{
    public class HttpRequestUrls
    {
        public const string Login = "api/account/login";
        public const string account_edit = "api/account/edit";
        public const string account_checkin = "api/account/check-in";
        public const string upload_account_avatar = "api/account/upload-avatar";
        public const string profile = "api/account/profile/{0}";
        public const string upload_record_image = "api/records/upload-image";
        public const string record = "api/records";
        public const string record_detail = "api/records/detail/{0}";
        public const string record_edit = "api/records/edit";
        public const string record_delete = "api/records/delete/{0}";
        public const string record_image_prefix = "api/record/images/{0}/{1}";
        public const string record_comment = "api/records/comment";
        public const string event_url = "api/events";
        public const string event_url_main = "api/events/main";
        public const string event_url_delete = "api/events/delete/{0}";
    }
}
