public static class FeatureFlags
{
    public static readonly string UseFacebookAuthentication = "use-facebook-authentication";
    public static readonly string UseGoogleAuthentication = "use-google-authentication";
    public static readonly string UseMicrosoftAuthentication = "use-microsoft-authentication";
    public static readonly string UseLinkedInAuthentication = "use-linkedin-authentication";
    public static readonly string UseGithubAuthentication = "use-github-authentication";
    public static readonly string EnableSearch = "jobscontroller-enable-search";
    public static readonly string EnableApplyForJob = "jobscontroller-enable-apply-for-job";
    public static readonly string ShowPreviewWarning = "jobscontroller-show-preview-warning";
    public static readonly string AllowBookmarking = "jobscontroller-allow-bookmarking";

    public static class LegacyClient
    {
        public static readonly string UseMockData = "use-mock-data";
    }
}
