using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Test.Mocks
{
    internal class MockFileSystem : IMock
    {
        internal static MockFileSystem Data { get; } = new MockFileSystem();
        private MockFileSystem() { }
        private static readonly Dictionary<string, FileSystemDirectory> Roots = new Dictionary<string, FileSystemDirectory>();

        public void Reset()
        {
            foreach (var pair in Roots)
            {
                pair.Value.Delete(true);
            }
            Roots.Clear();
        }

        public byte[] ReadFile(string path)
        {
            var element = FindElement(path);
            if (element != null && element is FileSystemFile f)
            {
                var d = new byte[f.Data.Length];
                f.Data.CopyTo(d, 0);
                return d;
            }
            else throw new System.IO.FileNotFoundException(path);
        }

        public void WriteFile(string path, byte[] data)
        {
            var element = FindElement(path, true);
            if (element is FileSystemFile f)
            {
                var d = new byte[data.Length];
                data.CopyTo(d, 0);
                f.Data = d;
            }
            else throw new System.IO.IOException(path);
        }

        public void AppendFile(string path, byte[] data)
        {
            var element = FindElement(path, true);
            if (element is FileSystemFile f)
            {
                var d = new byte[f.Data.Length + data.Length];
                f.Data.CopyTo(d, 0);
                data.CopyTo(d, f.Data.Length);
                f.Data = d;
            }
            else throw new System.IO.IOException(path);
        }

        public void DeleteFile(string path)
        {
            var element = FindElement(path);
            if (element != null && element is FileSystemFile f)
            {
                f.Delete();
            }
            else throw new System.IO.FileNotFoundException(path);
        }

        public void CreateFolder(string path)
        {
            var discard = FindElement(path, true);
        }

        public void DeleteFolder(string path, bool recursive = false)
        {
            var element = FindElement(path);
            if (element != null && element is FileSystemDirectory dir)
            {
                dir.Delete(recursive);
            }
            else throw new System.IO.DirectoryNotFoundException(path);
        }

        public bool ElementExits(string path)
        {
            return FindElement(path) != null;
        }

        public bool FileExits(string path)
        {
            var element = FindElement(path);
            return element != null && element is FileSystemFile;
        }

        public bool DirectoryExits(string path)
        {
            var element = FindElement(path);
            return element != null && element is FileSystemDirectory;
        }

        public void MoveDirectory(string sourceDirName, string destDirName)
        {
            if (!DirectoryExits(sourceDirName)) throw new System.IO.DirectoryNotFoundException(sourceDirName);
            if (!DirectoryExits(destDirName)) throw new System.IO.DirectoryNotFoundException(destDirName);
            var source = FindElement(sourceDirName) as FileSystemDirectory;
            var dest = FindElement(destDirName) as FileSystemDirectory;
            (source.Parent as FileSystemDirectory ?? throw new System.IO.IOException("Cannot move drive root")).RemoveChild(source);
            source.Parent = null;
            dest.AddChild(source);
        }
        public string[] GetDirectoryContent(string path, bool recursive)
        {
            if (!DirectoryExits(path)) throw new System.IO.DirectoryNotFoundException();
            return (FindElement(path) as FileSystemDirectory).GetFiles(recursive).Select(f => f.GetPath()).ToArray();
        }

        public DateTime GetCreationTime(string paht)
        {
            var element = FindElement(paht) ?? throw new System.IO.IOException();
            return element.CreationTime;
        }

        private FileSystemElement FindElement(string path, bool create = false)
        {
            string absPath = System.IO.Path.GetFullPath(path);
            string root = System.IO.Path.GetPathRoot(path);
            if (Roots.ContainsKey(root))
                return FindElement(absPath.Substring(root.Length), Roots[root], create);
            else if (create)
            {
                Roots.Add(root, new FileSystemDirectory(root));
                return FindElement(absPath.Substring(root.Length), Roots[root], create);
            }
            return null;

        }

        private FileSystemElement FindElement(string path, FileSystemDirectory e, bool create = false)
        {
            var parts = path.Split(System.IO.Path.DirectorySeparatorChar).ToList();
            if (string.IsNullOrEmpty(parts.LastOrDefault()))
                parts.RemoveAt(parts.Count - 1);
            if (parts.Count == 1)
            {
                var element = e.GetContent().FirstOrDefault(el => el.Name == parts[0]);
                if (element == null && create)
                {
                    if (path.EndsWith("" + System.IO.Path.DirectorySeparatorChar))
                        element = new FileSystemDirectory(parts[0].TrimEnd(System.IO.Path.DirectorySeparatorChar));
                    else
                        element = new FileSystemFile(parts[0], new byte[] { });
                    e.AddChild(element);
                }
                return element;
            }
            else
            {
                var subDir = e.GetSubDirectories().FirstOrDefault(d => d.Name == parts[0]);
                if (subDir == null)
                    if (create)
                    {
                        subDir = new FileSystemDirectory(parts[0]);
                        e.AddChild(subDir);
                    }
                    else
                        return null;
                parts.RemoveAt(0);
                return FindElement(string.Join("" + System.IO.Path.DirectorySeparatorChar, parts) + (path.EndsWith("" + System.IO.Path.DirectorySeparatorChar) ? "" + System.IO.Path.DirectorySeparatorChar : ""), subDir, create);
            }
        }

        private abstract class FileSystemElement
        {
            protected FileSystemElement(string name) : this(name, DateTime.Now)
            { }

            protected FileSystemElement(string name, DateTime creation)
            {
                Name = name;
                CreationTime = creation;
            }

            public string Name { get; set; }
            public FileSystemElement Parent { get; set; } = null;
            public DateTime CreationTime { get; internal set; }

            public virtual void Delete(bool recursive = false)
            {
                if (Parent is FileSystemDirectory dir)
                    dir.RemoveChild(this);
                Parent = null;
            }

            public abstract string GetPath();
        }

        private class FileSystemDirectory : FileSystemElement
        {
            public FileSystemDirectory(string name) : base(name)
            { }

            private List<FileSystemElement> Children { get; } = new List<FileSystemElement>();

            public IReadOnlyList<FileSystemDirectory> GetSubDirectories(bool recursive = false)
            {
                List<FileSystemDirectory> subDirs = new List<FileSystemDirectory>();
                foreach (var element in Children)
                {
                    if (element is FileSystemDirectory dir)
                    {
                        subDirs.Add(dir);
                        if (recursive) subDirs.AddRange(dir.GetSubDirectories(recursive));
                    }
                }
                return subDirs;
            }

            public IReadOnlyList<FileSystemFile> GetFiles(bool recursive = false)
            {
                List<FileSystemFile> subFiles = new List<FileSystemFile>();
                foreach (var element in Children)
                {
                    if (element is FileSystemFile file)
                        subFiles.Add(file);
                    if (recursive && element is FileSystemDirectory dir)
                        subFiles.AddRange(dir.GetFiles(recursive));
                }
                return subFiles;
            }

            public IReadOnlyList<FileSystemElement> GetContent(bool recursive = false)
            {
                List<FileSystemElement> subElements = new List<FileSystemElement>();
                foreach (var element in Children)
                {
                    subElements.Add(element);
                    if (recursive && element is FileSystemDirectory dir)
                        subElements.AddRange(dir.GetContent());
                }
                return subElements;
            }

            public override void Delete(bool recursive = false)
            {
                if (Children.Count > 0 && !recursive) throw new System.IO.IOException("Directory is not empty");
                base.Delete();
                while (Children.Count > 0)
                    Children.First().Delete(recursive);
            }

            public void AddChild(FileSystemElement element)
            {
                Children.Add(element);
                element.Parent = this;
            }

            internal void RemoveChild(FileSystemElement element)
            {
                if (Children.Contains(element)) Children.Remove(element);
            }

            public override string GetPath()
            {
                return (Parent?.GetPath() ?? "") + Name + System.IO.Path.DirectorySeparatorChar;
            }
        }

        private class FileSystemFile : FileSystemElement
        {

            public byte[] Data { get; set; }

            public FileSystemFile(string name, byte[] data) : base(name)
            {
                Data = data;
            }

            public override string GetPath()
            {
                return (Parent?.GetPath() ?? "") + Name;
            }


        }
    }
}
