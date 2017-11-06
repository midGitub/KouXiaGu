using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.GameConsoles.UI
{

    /// <summary>
    /// UI 控制台输入;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIGameConsoleInput : MonoBehaviour
    {
        private UIGameConsoleInput()
        {
        }

        [SerializeField]
        private InputField input;

        /// <summary>
        /// 最大记录数目;
        /// </summary>
        [SerializeField, Range(5, 100)]
        private int maxRecord = 20;

        /// <summary>
        /// 输入记录;
        /// </summary>
        private Collections.LinkedList<string> importRecords;

        /// <summary>
        /// 当前指向的记录;
        /// </summary>
        private Collections.LinkedListNode<string> current;

        /// <summary>
        /// 输入控件是否为焦点?
        /// </summary>
        private bool isInputFocused;

        private void Awake()
        {
            importRecords = new Collections.LinkedList<string>();
        }

        private void Update()
        {
            if (isInputFocused)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    EnterText();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    PreviousImportRecord();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    NextImportRecord();
                }
            }
            isInputFocused = input.isFocused;
        }

        /// <summary>
        /// 对输入控件的内容进行操作;
        /// </summary>
        private void EnterText()
        {
            string text = input.text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                AddImportRecord(text);
                ClearInputField();

                try
                {
                    GameConsole.WriteMethod(text);
                    GameConsole.Do(text);
                }
                catch (Exception ex)
                {
                    GameConsole.WriteError(string.Format("执行失败:{0}", ex.Message));
                }
            }

            input.ActivateInputField();
        }

        /// <summary>
        /// 添加输入记录;
        /// </summary>
        private void AddImportRecord(string text)
        {
            importRecords.AddLast(text);
            if (importRecords.Count > maxRecord)
            {
                importRecords.RemoveFirst();
            }
        }

        /// <summary>
        /// 上一个输入内容;
        /// </summary>
        private void PreviousImportRecord()
        {
            if (current == null)
            {
                current = importRecords.Last;
                if (current != null)
                {
                    SetInputField(current.Value);
                }
            }
            else
            {
                if (current.Previous != null)
                {
                    current = current.Previous;
                }
                SetInputField(current.Value);
            }
        }

        /// <summary>
        /// 下一个输入内容;
        /// </summary>
        private void NextImportRecord()
        {
            if (current != null)
            {
                if (current.Next != null)
                {
                    current = current.Next;
                    SetInputField(current.Value);
                }
                else
                {
                    ClearInputField();
                }
            }
        }

        private void SetInputField(string text)
        {
            input.text = current.Value;
            input.MoveTextEnd(false);
        }

        private void ClearInputField()
        {
            input.text = string.Empty;
            current = null;
        }
    }
}
