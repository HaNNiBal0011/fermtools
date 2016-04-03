using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fermtools
{
    static class Program
    {
        private static string appGuid = "8529891A-6F21-4A21-9B36-D20FA1E3AE6A";
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Проверяем не запущена ли программа, если уже, то выходим
            Mutex mutex = new Mutex(false, @"Global\" + appGuid);
            if(!mutex.WaitOne(0, false)) return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Создаем форму, но не показываем ее, иначе Application.Run(new Form1());
            new Form1(args);
            Application.Run();
        }
    }
}
