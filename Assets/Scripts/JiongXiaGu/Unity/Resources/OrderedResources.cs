//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JiongXiaGu.Unity.Resources
//{

//    /// <summary>
//    /// 经过排序的资源;
//    /// </summary>
//    public class OrderedResources
//    {
//        /// <summary>
//        /// 经过排序的资源,顺序由优先级低到高;
//        /// </summary>
//        private LinkedList<ILoadableResource> resources;

//        public OrderedResources()
//        {
//            resources = new LinkedList<ILoadableResource>();
//        }

//        /// <summary>
//        /// 提升读取优先顺序;
//        /// </summary>
//        public void MoveUp(LinkedNode node)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// 降低读取优先顺序;
//        /// </summary>
//        public void MoveDown(LinkedNode node)
//        {
//            throw new NotImplementedException();
//        }

//        public LinkedNode AddLast(ILoadableResource resource)
//        {
//            throw new NotImplementedException();
//        }

//        public LinkedNode AddFirst(ILoadableResource resource)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// 指定的现有节点后添加指定的新节点;
//        /// </summary>
//        public void AddAfter(LinkedNode node, LinkedNode newNode)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// 指定的现有节点后添加指定的新节点;
//        /// </summary>
//        public void AddAfter(LinkedNode node, ILoadableResource resource)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// 不可更改内容的连接点;
//        /// </summary>
//        public struct LinkedNode
//        {
//            internal LinkedList<ILoadableResource> LinkedList { get; private set; }
//            internal LinkedListNode<ILoadableResource> Node { get; private set; }

//            public ILoadableResource Value
//            {
//                get { return Node.Value; }
//            }

//            internal LinkedNode(LinkedList<ILoadableResource> linkedList, LinkedListNode<ILoadableResource> node)
//            {
//                LinkedList = linkedList;
//                Node = node;
//            }

//            /// <summary>
//            /// 尝试获取到下一个节点;
//            /// </summary>
//            public LinkedNode TryGetNext()
//            {
//                throw new NotImplementedException();
//            }

//            /// <summary>
//            /// 尝试获取到上一个节点;
//            /// </summary>
//            public LinkedNode TryGetPrevious()
//            {
//                throw new NotImplementedException();
//            }
//        }
//    }
//}
