using UnityEngine;
using UnityEngine.UI;
using JiongXiaGu.Collections;

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
        private DoublyLinkedList<string> importRecords;

        /// <summary>
        /// 当前指向的记录;
        /// </summary>
        private DoublyLinkedListNode<string> current;

        /// <summary>
        /// 输入控件是否为焦点?
        /// </summary>
        private bool isInputFocused;

        private void Awake()
        {
            importRecords = new DoublyLinkedList<string>();
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
            DoMethod(text);
            ActivateInputField();
        }

        /// <summary>
        /// 执行对应方法;
        /// </summary>
        public void DoMethod(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                AddImportRecord(message);
                ClearInputField();
                GameConsole.DoMethod(message);
            }
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

        /// <summary>
        /// 指定焦点在输入空间上;
        /// </summary>
        [ContextMenu(nameof(ActivateInputField))]
        public void ActivateInputField()
        {
            input.ActivateInputField();
            Debug.Log("ActivateInputField");
        }

        [ContextMenu(nameof(DeactivateInputField))]
        public void DeactivateInputField()
        {
            input.DeactivateInputField();
            Debug.Log("DeactivateInputField");
        }

        public void ClearInputField()
        {
            input.text = string.Empty;
            current = null;
        }

        public void SetInputField(string text)
        {
            input.text = current.Value;
            input.MoveTextEnd(false);
        }
    }
}
