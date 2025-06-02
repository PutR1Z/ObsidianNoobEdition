using Obsidian;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace NotesApp
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    partial class Form1 : MainForm, IForm1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private string text;

        public new AutoScaleMode AutoScaleMode { get; private set; }
        public new Size ClientSize { get; private set; }
        public string GetText()
        {
            return text;
        }

        private void SetText(string value)
        {
            text = value;
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 1000);
            this.SetText("Form1");
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }

        #endregion
    }
}

