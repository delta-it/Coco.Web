﻿using System.Text;

namespace Camino.Core.Contracts.Providers
{
    public interface IFileProvider
    {
        string ReadText(string path, Encoding encoding);
        void WriteText(string path, string contents, Encoding encoding);
        bool FileExists(string filePath);
        void CreateFile(string path);
        void CreateDirectory(string path);
        bool DirectoryExists(string path);
        void DeleteFile(string filePath);
    }
}
