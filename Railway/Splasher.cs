using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RailwayUI
{
    public class Splasher<T> where T : Form, new()
    {
        private static Thread splashThread;
        private static T splash;
        static void ShowThread()
        {
            splash = new T();
            splash.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            splash.Text = "Обрабатываю данные…";
            splash.StartPosition = FormStartPosition.CenterScreen;
            splash.ShowDialog();
            Application.Run(splash);
        }
        /// <summary>
        /// Отображение
        /// </summary>
        static public void Show()
        {
            if (splashThread != null)
                return;
            splashThread = new Thread(ShowThread) { IsBackground = true };
            //splashThread.ApartmentState = ApartmentState.MTA;
            splashThread.Start();
        }
        /// <summary>
        /// Скрытие
        /// </summary>
        static public void Close()
        {
            if (splashThread == null) return;
            if (splash == null) return;
            try
            {
                splash.Invoke(new MethodInvoker(splash.Close));
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message, ex);
            }
            splashThread = null;
            splash = null;
        }
        /// <summary>
        /// Задание Статуса(лейбла)
        /// </summary>
        static public string Status
        {
            set
            {
                if (splash == null)
                {
                    Thread.CurrentThread.Join(600);
                }
                try
                {
                    //splash.StatusInfo = value;
                }
                catch (NullReferenceException)
                { }
            }
            get
            {
                if (splash == null)
                {
                    return "";
                }
                return null;//splash.StatusInfo;
            }
        }

    }
}