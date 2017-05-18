/*****************************************************************
 * 版权所有 (C) Lave.Zhang@outlook.com 2012
 * 本源代码仅供学习研究之用，不得用于商业目的。
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MetroGraphApp
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
