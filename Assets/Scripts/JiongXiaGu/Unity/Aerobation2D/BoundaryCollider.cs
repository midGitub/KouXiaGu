using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Aerobation2D
{

    public interface IRemovableObject
    {
        void Remove();
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public sealed class BoundaryCollider : MonoBehaviour
    {
        private BoundaryCollider()
        {
        }

        private void OnTriggerExit(Collider other)
        {
            var removableObject = other.GetComponent<IRemovableObject>();
            if (removableObject != null)
            {
                removableObject.Remove();
            }
            else
            {
                GameObject.Destroy(other.gameObject);
            }
        }
    }
}
