using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.UI
{


    [RequireComponent(typeof(Animator))]
    public class AnimatorSwitcher : MonoBehaviour
    {

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        [ContextMenu("Display")]
        private void Display()
        {
            animator.SetBool("Display", true);
        }
    }
}
