//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace KouXiaGu.Concurrent
//{


//    public static class AwaiterExtensions
//    {

//        public static RequestAwaiter GetAwaiter(this IRequest request)
//        {
//            return new RequestAwaiter(request);
//        }

//        public struct RequestAwaiter : INotifyCompletion
//        {
//            public RequestAwaiter(IRequest request)
//            {
//                this.request = request;
//            }

//            IRequest request;

//            public bool IsCompleted
//            {
//                get { return request.IsCompleted; }
//            }

//            public void OnCompleted(Action continuation)
//            {
//                RequestAwaiter t = this;
//                Task.Run(delegate ()
//                {
//                    while (!t.request.IsCompleted)
//                    {
//                    }
//                    continuation();
//                });
//            }

//            public void GetResult()
//            {
//                return;
//            }
//        }



//        public static RequestAwaiter<T> GetAwaiter<T>(this IRequest<T> request)
//        {
//            return new RequestAwaiter<T>(request);
//        }

//        public struct RequestAwaiter<T> : INotifyCompletion
//        {
//            public RequestAwaiter(IRequest<T> request)
//            {
//                this.request = request;
//            }

//            IRequest<T> request;

//            public bool IsCompleted
//            {
//                get { return request.IsCompleted; }
//            }

//            public void OnCompleted(Action continuation)
//            {
//                RequestAwaiter<T> t = this;
//                Task.Run(delegate()
//                {
//                    while (!t.request.IsCompleted)
//                    {
//                    }
//                    continuation();
//                });
//            }

//            void OnCompletedInternal(Action continuation)
//            {
//                while (!request.IsCompleted)
//                {
//                }
//                continuation();
//            }

//            public T GetResult()
//            {
//                return request.Result;
//            }
//        }


//        public static AssetBundleCreateRequestAwaiter GetAwaiter(this AssetBundleCreateRequest request)
//        {
//            return new AssetBundleCreateRequestAwaiter(request);
//        }

//        public struct AssetBundleCreateRequestAwaiter : INotifyCompletion
//        {
//            public AssetBundleCreateRequestAwaiter(AssetBundleCreateRequest request)
//            {
//                this.request = request;
//            }

//            AssetBundleCreateRequest request;

//            public bool IsCompleted
//            {
//                get { return request.isDone; }
//            }

//            public void OnCompleted(Action continuation)
//            {
//                continuation();
//            }

//            public AssetBundle GetResult()
//            {
//                return request.assetBundle;
//            }
//        }
//    }
//}
