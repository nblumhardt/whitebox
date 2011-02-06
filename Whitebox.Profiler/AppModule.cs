using System.Linq;
using System.Windows.Controls;
using Autofac;
using Autofac.Builder;
using Whitebox.Core;
using Whitebox.Core.Application;
using Whitebox.Profiler.ApplicationEventHandlers;
using Whitebox.Profiler.Features.Analysis;
using Whitebox.Profiler.Features.Components;
using Whitebox.Profiler.Features.Events;
using Whitebox.Profiler.Features.Messages;
using Whitebox.Profiler.Features.ResolveOperations;
using Whitebox.Profiler.Features.Session;
using Whitebox.Profiler.Features.Waiting;
using Whitebox.Profiler.Navigation;
using Whitebox.Profiler.Util;

namespace Whitebox.Profiler
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var thisAssembly = typeof(AppModule).Assembly;

            RegisterViewWithModel<ProfilerWindowView, ProfilerWindowViewModel>(builder)
                .SingleInstance();

            RegisterViewWithModel<SessionView, SessionViewModel>(builder)
                .InstancePerProfilerSession();

            RegisterViewWithModel<EventsView, EventsViewModel>(builder, "Events")
                .InstancePerProfilerSession();

            RegisterViewWithModel<WaitingView, WaitingViewModel>(builder, "Waiting to Connect")
                .InstancePerProfilerSession();

            RegisterViewWithModel<MessageView, MessageViewModel>(builder, "Message");

            RegisterViewWithModel<ResolveOperationView, ResolveOperationViewModel>(builder, "Resolve Operation");

            RegisterViewWithModel<ComponentsView, ComponentsViewModel>(builder, "Components")
                .InstancePerProfilerSession();

            RegisterViewWithModel<AnalysisView, AnalysisViewModel>(builder, "Analysis")
                .InstancePerProfilerSession();

            builder.Register(c => new DispatcherDispatcher(c.Resolve<ProfilerWindowView>().Dispatcher))
                .As<IDispatcher>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(thisAssembly)
                .InNamespaceOf<PreDisplayEventViewItemStore>()
                .InstancePerProfilerSession()
                .AsImplementedInterfaces();

            builder.RegisterType<HistoricalItemStore<ResolveOperation>>()
                .AsImplementedInterfaces()
                .InstancePerProfilerSession();

            builder.RegisterType<HistoricalItemStore<Component>>()
                .AsImplementedInterfaces()
                .InstancePerProfilerSession();

            builder.RegisterType<HistoricalItemStore<RegistrationSource>>()
                .AsImplementedInterfaces()
                .InstancePerProfilerSession();
        }

        static IRegistrationBuilder<TView, SimpleActivatorData, SingleRegistrationStyle> RegisterViewWithModel<TView, TModel>(ContainerBuilder builder, string title = "<Untitled>")
            where TView : Control, new()
        {
            builder.RegisterType<TModel>()
                .OnPreparing(e => e.Parameters = e.Parameters.Select(p =>
                    {
                        var np = p as NamedParameter;
                        if (np != null && np.Name.StartsWith("arg"))
                            return new TypedParameter(np.Value.GetType(), np.Value);
                        return p;
                    }));

            return builder.Register(c => new TView())
                .OnActivated(e => e.Instance.DataContext = e.Context.Resolve<TModel>(e.Parameters))
                .WithMetadata<IViewMetadata>(m => m.For(x => x.Title, title));
        }
    }
}
