using Autofac.Builder;

namespace Whitebox.Core
{
    public static class RegistrationExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle>
            InstancePerProfilerSession<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            return builder.InstancePerMatchingLifetimeScope(Constants.ProfilerSessionScopeTag);
        }
    }
}
