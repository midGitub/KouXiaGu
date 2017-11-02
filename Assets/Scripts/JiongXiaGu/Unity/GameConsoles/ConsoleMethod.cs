using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 控制台方法抽象类;
    /// </summary>
    public abstract class ConsoleMethod
    {
        /// <summary>
        /// 方法描述;
        /// </summary>
        public ConsoleMethodDesc Description { get; private set; }

        /// <summary>
        /// 方法执行的前提条件;
        /// </summary>
        private Func<bool> prerequisite;

        /// <summary>
        /// 参数数量;
        /// </summary>
        public abstract int ParameterCount { get; }

        /// <summary>
        /// 完全名,如 ConsoleMethod.Info.Run ;
        /// </summary>
        public string Name
        {
            get { return Description.Name; }
        }

        /// <summary>
        /// 预留消息;
        /// </summary>
        public string Message
        {
            get { return Description.Message; }
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="description">方法描述</param>
        /// <param name="prerequisite">前提,为null,则代表不存在前提</param>
        private ConsoleMethod(ConsoleMethodDesc description, Func<bool> prerequisite)
        {
            Description = description;
            this.prerequisite = prerequisite;
        }

        /// <summary>
        /// 当前该方法是否可用?
        /// </summary>
        public bool Prerequisite()
        {
            return prerequisite != null ? prerequisite.Invoke() : true;
        }

        /// <summary>
        /// 调用当前方法,若不存在参数,则传入null;(参考 System.Reflection.MethodBase.Invoke 方法)
        /// </summary>
        public abstract void Invoke(string[] parameters);

        /// <summary>
        /// 当传入参数不正确时返回异常(仅限零参数);
        /// </summary>
        protected void ThrowIfParametersIsNotNull(string[] parameters)
        {
            if (parameters != null)
            {
                throw new TargetParameterCountException(string.Format("该控制台方法不存在参数,[{0}]应该为 null;", nameof(parameters)));
            }
        }

        /// <summary>
        /// 当传入参数不正确时返回异常(不包括0个参数);
        /// </summary>
        protected void ThrowIfParametersIsNotRight(string[] parameters)
        {
            if (parameters == null || parameters.Length != ParameterCount)
            {
                throw new TargetParameterCountException(string.Format("[{0}]数组长度 {1},不符合方法参数数目 {2};", nameof(parameters), parameters.Length, ParameterCount));
            }
        }


        public delegate void Action1(string v1);
        public delegate void Action2(string v1, string v2);
        public delegate void Action3(string v1, string v2, string v3);
        public delegate void Action4(string v1, string v2, string v3, string v4);
        public delegate void Action5(string v1, string v2, string v3, string v4, string v5);
        public delegate void Action6(string v1, string v2, string v3, string v4, string v5, string v6);

        /// <summary>
        /// 创建一个新的控制台方法;
        /// </summary>
        /// <param name="methodInfo">对应的方法</param>
        /// <param name="target">若未静态方法则为null,否则传入实例</param>
        public static ConsoleMethod Create(MethodInfo methodInfo, Object target, ConsoleMethodDesc description, Func<bool> prerequisite = null)
        {
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            foreach (var parameterInfo in parameterInfos)
            {
                if (parameterInfo.ParameterType != typeof(string))
                {
                    throw new ArgumentException(string.Format("[{0}]参数类型存在一个或多个不为 string 类型;"));
                }
            }

            switch (parameterInfos.Length)
            {
                case 0:
                    var method0 = CreateDelegate<Action>(methodInfo, target);
                    return Create(method0, description, prerequisite);

                case 1:
                    var method1 = CreateDelegate<Action1>(methodInfo, target);
                    return Create(method1, description, prerequisite);

                case 2:
                    var method2 = CreateDelegate<Action2>(methodInfo, target);
                    return Create(method2, description, prerequisite);

                case 3:
                    var method3 = CreateDelegate<Action3>(methodInfo, target);
                    return Create(method3, description, prerequisite);

                case 4:
                    var method4 = CreateDelegate<Action4>(methodInfo, target);
                    return Create(method4, description, prerequisite);

                case 5:
                    var method5 = CreateDelegate<Action5>(methodInfo, target);
                    return Create(method5, description, prerequisite);

                case 6:
                    var method6 = CreateDelegate<Action6>(methodInfo, target);
                    return Create(method6, description, prerequisite);

                default:
                    throw new NotImplementedException(string.Format("还未支持方法参数数目为 {0} 的方法;", parameterInfos.Length));
            }
        }

        /// <summary>
        /// 转换成委托;
        /// </summary>
        private static T CreateDelegate<T>(MethodInfo methodInfo, Object target = null)
            where T : class
        {
            if (target == null)
            {
                return methodInfo.CreateDelegate(typeof(T)) as T;
            }
            else
            {
                return methodInfo.CreateDelegate(typeof(T), target) as T;
            }
        }

        /// <summary>
        /// 创建一个新的控制台方法;
        /// </summary>
        public static ConsoleMethod Create(Action method, ConsoleMethodDesc description, Func<bool> prerequisite = null)
        {
            return new ConsoleMethod_Action0(method, description, prerequisite);
        }

        /// <summary>
        /// 创建一个新的控制台方法;
        /// </summary>
        public static ConsoleMethod Create(Action1 method, ConsoleMethodDesc description, Func<bool> prerequisite = null)
        {
            return new ConsoleMethod_Action1(method, description, prerequisite);
        }

        /// <summary>
        /// 创建一个新的控制台方法;
        /// </summary>
        public static ConsoleMethod Create(Action2 method, ConsoleMethodDesc description, Func<bool> prerequisite = null)
        {
            return new ConsoleMethod_Action2(method, description, prerequisite);
        }

        /// <summary>
        /// 创建一个新的控制台方法;
        /// </summary>
        public static ConsoleMethod Create(Action3 method, ConsoleMethodDesc description, Func<bool> prerequisite = null)
        {
            return new ConsoleMethod_Action3(method, description, prerequisite);
        }

        /// <summary>
        /// 创建一个新的控制台方法;
        /// </summary>
        public static ConsoleMethod Create(Action4 method, ConsoleMethodDesc description, Func<bool> prerequisite = null)
        {
            return new ConsoleMethod_Action4(method, description, prerequisite);
        }

        /// <summary>
        /// 创建一个新的控制台方法;
        /// </summary>
        public static ConsoleMethod Create(Action5 method, ConsoleMethodDesc description, Func<bool> prerequisite = null)
        {
            return new ConsoleMethod_Action5(method, description, prerequisite);
        }

        /// <summary>
        /// 创建一个新的控制台方法;
        /// </summary>
        public static ConsoleMethod Create(Action6 method, ConsoleMethodDesc description, Func<bool> prerequisite = null)
        {
            return new ConsoleMethod_Action6(method, description, prerequisite);
        }

        /// <summary>
        /// 0个参数的控制台方法;
        /// </summary>
        private class ConsoleMethod_Action0 : ConsoleMethod
        {
            private Action method;

            public ConsoleMethod_Action0(Action method, ConsoleMethodDesc description, Func<bool> prerequisite) : base(description, prerequisite)
            {
                if (method == null)
                    throw new ArgumentNullException(nameof(method));

                this.method = method;
            }

            public override int ParameterCount
            {
                get { return 0; }
            }

            public override void Invoke(string[] parameters)
            {
                ThrowIfParametersIsNotNull(parameters);
                method.Invoke();
            }
        }

        /// <summary>
        /// 1个参数的控制台方法;
        /// </summary>
        private class ConsoleMethod_Action1 : ConsoleMethod
        {
            private Action1 method;

            public ConsoleMethod_Action1(Action1 method, ConsoleMethodDesc description, Func<bool> prerequisite) : base(description, prerequisite)
            {
                if (method == null)
                    throw new ArgumentNullException(nameof(method));

                this.method = method;
            }

            public override int ParameterCount
            {
                get { return 1; }
            }

            public override void Invoke(string[] parameters)
            {
                ThrowIfParametersIsNotRight(parameters);
                method.Invoke(parameters[0]);
            }
        }

        /// <summary>
        /// 2个参数的控制台方法;
        /// </summary>
        private class ConsoleMethod_Action2 : ConsoleMethod
        {
            private Action2 method;

            public ConsoleMethod_Action2(Action2 method, ConsoleMethodDesc description, Func<bool> prerequisite) : base(description, prerequisite)
            {
                if (method == null)
                    throw new ArgumentNullException(nameof(method));

                this.method = method;
            }

            public override int ParameterCount
            {
                get { return 2; }
            }

            public override void Invoke(string[] parameters)
            {
                ThrowIfParametersIsNotRight(parameters);
                method.Invoke(parameters[0], parameters[1]);
            }
        }

        /// <summary>
        /// 3个参数的控制台方法;
        /// </summary>
        private class ConsoleMethod_Action3 : ConsoleMethod
        {
            private Action3 method;

            public ConsoleMethod_Action3(Action3 method, ConsoleMethodDesc description, Func<bool> prerequisite) : base(description, prerequisite)
            {
                if (method == null)
                    throw new ArgumentNullException(nameof(method));

                this.method = method;
            }

            public override int ParameterCount
            {
                get { return 3; }
            }

            public override void Invoke(string[] parameters)
            {
                ThrowIfParametersIsNotRight(parameters);
                method.Invoke(parameters[0], parameters[1], parameters[2]);
            }
        }

        /// <summary>
        /// 4个参数的控制台方法;
        /// </summary>
        private class ConsoleMethod_Action4 : ConsoleMethod
        {
            private Action4 method;

            public ConsoleMethod_Action4(Action4 method, ConsoleMethodDesc description, Func<bool> prerequisite) : base(description, prerequisite)
            {
                if (method == null)
                    throw new ArgumentNullException(nameof(method));

                this.method = method;
            }

            public override int ParameterCount
            {
                get { return 4; }
            }

            public override void Invoke(string[] parameters)
            {
                ThrowIfParametersIsNotRight(parameters);
                method.Invoke(parameters[0], parameters[1], parameters[2], parameters[3]);
            }
        }

        /// <summary>
        /// 5个参数的控制台方法;
        /// </summary>
        private class ConsoleMethod_Action5 : ConsoleMethod
        {
            private Action5 method;

            public ConsoleMethod_Action5(Action5 method, ConsoleMethodDesc description, Func<bool> prerequisite) : base(description, prerequisite)
            {
                if (method == null)
                    throw new ArgumentNullException(nameof(method));

                this.method = method;
            }

            public override int ParameterCount
            {
                get { return 5; }
            }

            public override void Invoke(string[] parameters)
            {
                ThrowIfParametersIsNotRight(parameters);
                method.Invoke(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
            }
        }

        /// <summary>
        /// 6个参数的控制台方法;
        /// </summary>
        private class ConsoleMethod_Action6 : ConsoleMethod
        {
            private Action6 method;

            public ConsoleMethod_Action6(Action6 method, ConsoleMethodDesc description, Func<bool> prerequisite) : base(description, prerequisite)
            {
                if (method == null)
                    throw new ArgumentNullException(nameof(method));

                this.method = method;
            }

            public override int ParameterCount
            {
                get { return 6; }
            }

            public override void Invoke(string[] parameters)
            {
                ThrowIfParametersIsNotRight(parameters);
                method.Invoke(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5]);
            }
        }
    }
}
