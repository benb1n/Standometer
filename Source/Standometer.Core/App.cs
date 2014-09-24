using Cirrious.CrossCore.IoC;

namespace Standometer.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }
}