namespace asp_net_3 {
    public static class AppBuildSettings {
#if DEBUG
        public const bool ShowAdminPanel = true;
#else
        public const bool ShowAdminPanel = false;
#endif
    }
}
