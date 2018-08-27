using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{

    public class Singleton

    {

        // 定义一个静态变量来保存类的实例

        private static Singleton uniqueInstance;



        // 定义一个标识确保线程同步

        private static readonly object locker = new object();



        // 定义私有构造函数，使外界不能创建该类实例

        private Singleton()

        {

        }



        /// <summary>

        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点

        /// </summary>

        /// <returns></returns>

        public static Singleton GetInstance()

        {// 双重锁定只需要一句判断就可以了

            if (uniqueInstance == null)

            {

                lock (locker)

                {

                    // 如果类的实例不存在则创建，否则直接返回

                    if (uniqueInstance == null)

                    {

                        uniqueInstance = new Singleton();

                    }

                }

            }

            return uniqueInstance;

        }

    }

}
