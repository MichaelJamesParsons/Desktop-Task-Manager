using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Service;
using TimeTracker.Services;
using TimeTracker.ViewModels;

namespace TimeTracker.Ioc
{
    class IocResolver : NinjectModule
    {
        public override void Load()
        {
            Bind<ITaskService>().To<TaskRestService>().InSingletonScope();
            Bind<MasterViewModel>().To<MasterViewModel>().InSingletonScope();
        }
    }
}
