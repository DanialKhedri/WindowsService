using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

[RunInstaller(true)]
public partial class ProjectInstaller : Installer
{
    private ServiceInstaller serviceInstaller1;
    private ServiceProcessInstaller serviceProcessInstaller1;

    public ProjectInstaller()
    {
        // ایجاد و تنظیم ServiceInstaller
        serviceInstaller1 = new ServiceInstaller();
        serviceInstaller1.ServiceName = "WindowsService";
        serviceInstaller1.StartType = ServiceStartMode.Automatic;  

        // ایجاد و تنظیم ServiceProcessInstaller
        serviceProcessInstaller1 = new ServiceProcessInstaller();
        serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;  

        // اضافه کردن نصب‌کننده‌ها
        Installers.Add(serviceProcessInstaller1);
        Installers.Add(serviceInstaller1);
    }
}
