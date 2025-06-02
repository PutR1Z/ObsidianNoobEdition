using System;
using System.IO;
using System.Windows.Forms;
using Markdig;

namespace NotesApp
{
    public class MainForm : Form
    {
        private readonly NoteManager noteManager;
        private TreeView treeView;
        private TextBox textBoxEditor;
        private WebBrowser webBrowserPreview;
        private Button btnSave;
        private Button btnNew;
        private Button btnDelete;

        public MainForm()
        {
            noteManager = new NoteManager(Path.Combine(Environment.CurrentDirectory, "Notes"));
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Настройка формы
            this.Text = "Notes App (Obsidian Style)";
            this.Size = new System.Drawing.Size(800, 600);

            // Панель с разделителем
            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 200
            };

            // Дерево папок и заметок
            treeView = new TreeView
            {
                Dock = DockStyle.Fill
            };
            treeView.AfterSelect += TreeView_AfterSelect;
            splitContainer.Panel1.Controls.Add(treeView);

            // Правая панель: редактор и просмотр
            var rightPanel = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 300
            };

            // Текстовый редактор
            textBoxEditor = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                AcceptsReturn = true,
                AcceptsTab = true,
                Font = new System.Drawing.Font("Consolas", 10)
            };
            rightPanel.Panel1.Controls.Add(textBoxEditor);

            // Предпросмотр Markdown
            webBrowserPreview = new WebBrowser
            {
                Dock = DockStyle.Fill
            };
            rightPanel.Panel2.Controls.Add(webBrowserPreview);

            splitContainer.Panel2.Controls.Add(rightPanel);

            // Кнопки управления
            var panelButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 40
            };

            btnNew = new Button { Text = "New Note", Width = 100 };
            btnNew.Click += BtnNew_Click;
            btnSave = new Button { Text = "Save", Width = 100 };
            btnSave.Click += BtnSave_Click;
            btnDelete = new Button { Text = "Delete", Width = 100 };
            btnDelete.Click += BtnDelete_Click;

            panelButtons.Controls.Add(btnNew);
            panelButtons.Controls.Add(btnSave);
            panelButtons.Controls.Add(btnDelete);

            this.Controls.Add(panelButtons);
            this.Controls.Add(splitContainer);

            // Загрузка структуры заметок
            UpdateTreeView();
        }

        private void UpdateTreeView()
        {
            treeView.Nodes.Clear();
            treeView.Nodes.Add(noteManager.GetFolderStructure());
            treeView.ExpandAll();
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Note note)
            {
                textBoxEditor.Text = note.Content;
                RenderMarkdown(note.Content);
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            using (var dialog = new Form
            {
                Text = "New Note",
                Size = new System.Drawing.Size(300, 150),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                var txtTitle = new TextBox { Width = 200, Top = 20, Left = 50 };
                var txtFolder = new TextBox { Width = 200, Top = 50, Left = 50 };
                var btnOk = new Button { Text = "Create", Top = 80, Left = 50, Width = 100 };
                var btnCancel = new Button { Text = "Cancel", Top = 80, Left = 160, Width = 100 };

                dialog.Controls.Add(new Label { Text = "Title:", Top = 20, Left = 10 });
                dialog.Controls.Add(new Label { Text = "Folder:", Top = 50, Left = 10 });
                dialog.Controls.Add(txtTitle);
                dialog.Controls.Add(txtFolder);
                dialog.Controls.Add(btnOk);
                dialog.Controls.Add(btnCancel);

                btnOk.Click += (s, ev) =>
                {
                    if (!string.IsNullOrEmpty(txtTitle.Text))
                    {
                        noteManager.CreateNote(txtTitle.Text, "", txtFolder.Text);
                        UpdateTreeView();
                        dialog.Close();
                    }
                    else
                    {
                        MessageBox.Show("Title cannot be empty.");
                    }
                };
                btnCancel.Click += (s, ev) => dialog.Close();

                dialog.ShowDialog();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode?.Tag is Note note)
            {
                noteManager.EditNote(note.Title, textBoxEditor.Text);
                RenderMarkdown(textBoxEditor.Text);
                UpdateTreeView();
            }
            else
            {
                MessageBox.Show("Select a note to save.");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode?.Tag is Note note)
            {
                if (MessageBox.Show($"Delete '{note.Title}'?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    noteManager.DeleteNote(note.Title);
                    UpdateTreeView();
                    textBoxEditor.Clear();
                    webBrowserPreview.DocumentText = "";
                }
            }
            else
            {
                MessageBox.Show("Select a note to delete.");
            }
        }

        private void RenderMarkdown(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var html = Markdown.ToHtml(markdown, pipeline);
            var fullHtml = $@"<html><head><style>
                body {{ font-family: Arial, sans-serif; padding: 10px; }}
                h1, h2, h3 {{ color: #333; }}
                code {{ background: #f4f4f4; padding: 2px 4px; }}
                </style></head><body>{html}</body></html>";
            webBrowserPreview.DocumentText = fullHtml;
        }

        [STAThread]
        private static void Main5()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    // Главная форма приложения
    
}