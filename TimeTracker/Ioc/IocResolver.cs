using Ninject.Modules;
using TimeTracker.Services;
using TimeTracker.ViewModels;

namespace TimeTracker.Ioc
{
    class IocResolver : NinjectModule
    {
        public override void Load()
        {
            Bind<ITaskService>().To<TaskRestService>().InSingletonScope();
            Bind<IReportsRestService>().To<ReportsRestService>().InSingletonScope();
            Bind<MasterViewModel>().To<MasterViewModel>().InSingletonScope();
        }
    }
}
