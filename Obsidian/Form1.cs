using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NotesApp
{
    // Класс для представления заметки
    public class Note
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Path { get; set; }
        public DateTime LastModified { get; set; }

        public Note(string title, string content, string path)
        {
            Title = title;
            Content = content;
            Path = path;
            LastModified = DateTime.Now;
        }
    }
    // Класс для управления заметками
    public class NoteManager
    {
        private readonly string rootDirectory;
        private readonly List<Note> notes;

        public NoteManager(string directory)
        {
            rootDirectory = directory;
            notes = new List<Note>();
            Directory.CreateDirectory(rootDirectory);
            LoadNotes();
        }

        public List<Note> Notes => notes;

        // Загрузка всех заметок
        private void LoadNotes()
        {
            notes.Clear();
            var files = Directory.GetFiles(rootDirectory, "*.md", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var title = Path.GetFileNameWithoutExtension(file);
                var content = File.ReadAllText(file);
                var lastModified = File.GetLastWriteTime(file);
                notes.Add(new Note(title, content, file) { LastModified = lastModified });
            }
        }

        // Создание новой заметки
        public void CreateNote(string title, string content, string folder = "")
        {
            var folderPath = Path.Combine(rootDirectory, folder);
            Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, $"{title}.md");
            File.WriteAllText(filePath, content);
            notes.Add(new Note(title, content, filePath));
        }

        // Редактирование заметки
        public void EditNote(string title, string newContent)
        {
            var note = notes.FirstOrDefault(n => n.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (note != null)
            {
                note.Content = newContent;
                note.LastModified = DateTime.Now;
                File.WriteAllText(note.Path, newContent);
            }
        }

        // Удаление заметки
        public void DeleteNote(string title)
        {
            var note = notes.FirstOrDefault(n => n.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (note != null)
            {
                File.Delete(note.Path);
                notes.Remove(note);
            }
        }

        // Получение структуры папок и заметок
        public TreeNode GetFolderStructure()
        {
            var rootNode = new TreeNode("Notes");
            var folders = notes.GroupBy(n => Path.GetDirectoryName(n.Path))
                              .Select(g => new { Folder = g.Key, Notes = g.ToList() });

            foreach (var folder in folders)
            {
                var folderPath = folder.Folder.Replace(rootDirectory, "").Trim(Path.DirectorySeparatorChar);
                var folderNode = new TreeNode(string.IsNullOrEmpty(folderPath) ? "Root" : folderPath);

                foreach (var note in folder.Notes)
                {
                    var noteNode = new TreeNode(note.Title) { Tag = note };
                    folderNode.Nodes.Add(noteNode);
                }

                rootNode.Nodes.Add(folderNode);
            }

            return rootNode;
        }
    }

    // Главная форма приложения
    
}