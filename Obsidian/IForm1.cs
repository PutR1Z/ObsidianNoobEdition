using System.Drawing;
using System.Windows.Forms;

namespace NotesApp
{
    internal interface IForm1
    {
        AutoScaleMode AutoScaleMode { get; }
        Size ClientSize { get; }

        string GetText();
    }
}