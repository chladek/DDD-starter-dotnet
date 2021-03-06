using System;
using Nuke.Common;

namespace MyCompany.Crm.Nuke
{
    public partial class Build
    {
        [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
        public static Configuration BuildConfiguration { get; private set; } = IsLocalBuild
            ? Configuration.Debug
            : Configuration.Release;

        [Parameter]
        public static string ExecutingUser { get; private set; } = "1000";

        [Parameter]
        public static Environment Environment
        {
            get
            {
                if (!_environment.Equals(Nuke.Environment.Undefined))
                    return _environment;
                if (!AspNetCoreEnvironment.Equals(Nuke.Environment.Undefined))
                    return AspNetCoreEnvironment;
                if (!DotNetEnvironment.Equals(Nuke.Environment.Undefined))
                    return DotNetEnvironment;
                return Nuke.Environment.Development;
            }
            private set
            {
                if (value.Equals(Nuke.Environment.Undefined))
                    throw new ArgumentException(
                        $"{nameof(Environment)} can not be set to {Nuke.Environment.Undefined}");
                _environment = value;
            }
        }

        private static Environment _environment = Environment.Undefined;

        [Parameter]
        public static Environment DotNetEnvironment { get; private set; } = Environment.Undefined;

        [Parameter]
        public static Environment AspNetCoreEnvironment { get; private set; } = Environment.Undefined;
    }
}