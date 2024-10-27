using System.Configuration;
using System.Data;
using System.Windows;

namespace EventManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static public EM_Context Context { get; private set; }

        public App()
        {
            Context = new EM_Context();
            new Seeder(Context);
        }
    }

}
