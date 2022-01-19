namespace VL.NLog
{
    /// <summary>
    /// Log verbocity.
    /// </summary>
    public enum LogLevel
    {
        // TODO: [maybe] add Fatal log level?

        /// <summary>
        /// Error is a serious issue and represents the failure of something important.
        /// </summary>
        Error = 0,

        /// <summary>
        /// Warning indicates possible problem or unusual situation.
        /// </summary>
        Warning = 1,

        /// <summary>
        /// Normal application behavior and milestones.
        /// </summary>
        Info = 2,

        /// <summary>
        /// Diagnostic information.
        /// </summary>
        Debug = 3
    }
}
