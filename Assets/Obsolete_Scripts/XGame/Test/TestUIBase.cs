using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XGame.Test
{

    /// <summary>
    /// 用于测试使用的基类;
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class TestUIBase : MonoBehaviour
    {

        /// <summary>
        /// 更新速度;
        /// </summary>
        [SerializeField]
        private float updateSpeed = 0.5f;

        [SerializeField]
        private Text textObject;

        [SerializeField, HideInInspector]
        protected WaitForSecondsRealtime waitForSeconds;

        /// <summary>
        /// 返回显示在屏幕上的内容;
        /// </summary>
        /// <returns></returns>
        protected abstract string Log();

        protected virtual void Awake()
        {
            waitForSeconds = new WaitForSecondsRealtime(updateSpeed);

            if (textObject == null)
                textObject = GetComponent<Text>();
        }

        protected virtual void Start()
        {
            if (textObject != null)
                StartCoroutine(YUpdate());
            else
                Debug.LogWarning("UITest缺少 Text 脚本!" + name);
        }

        protected virtual void OnValidate()
        {
            waitForSeconds = new WaitForSecondsRealtime(updateSpeed);
        }

        private IEnumerator YUpdate()
        {
            while (true)
            {
                textObject.text = Log();
                yield return waitForSeconds;
            }
        }


    }

}
