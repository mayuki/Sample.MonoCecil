using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
        /// <summary>
        /// 差し込んだ側から呼び出してほしいメソッド
        /// </summary>
        public static void Initialize()
        {
            Console.WriteLine("差し込み案件です!");
        }
    }
}
