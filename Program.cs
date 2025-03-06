using System;
using System.Windows.Forms;
using InventoryManagementSystem.Forms;

namespace InventoryManagementSystem
{
    internal static class Program
    {
        [STAThread]

        static void Main()
        {
            Console.WriteLine("Application starting...");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}