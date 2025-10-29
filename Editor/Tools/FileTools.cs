using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Mirka.Payment.Editor.Editor.Tools
{
    public static class FileTools
    {
        public static void ToggleCommentsByKeyword(string relativePath, string keyword, bool comment)
        {
            var filePath = GetAbsolutePath(relativePath);

            if (!File.Exists(filePath) || !filePath.EndsWith(".cs"))
            {
                UnityEngine.Debug.LogWarning("Invalid C# file: " + filePath);
                return;
            }

            var fileLines = File.ReadAllLines(filePath).ToList();

            for (var i = 0; i < fileLines.Count; i++)
                ToggleComment(keyword, comment, fileLines, i);

            File.WriteAllLines(filePath, fileLines);
            AssetDatabase.Refresh();
        }

        public static void ToggleGradleCommentsByKeyword(string relativePath, string keyword, bool comment)
        {
            string filePath = GetAbsolutePath(relativePath);

            if (!File.Exists(filePath) || !filePath.EndsWith(".gradle"))
            {
                UnityEngine.Debug.LogWarning("Invalid Gradle file: " + filePath);
                return;
            }

            var fileLines = File.ReadAllLines(filePath).ToList();

            for (var i = 0; i < fileLines.Count; i++)
                ToggleComment(keyword, comment, fileLines, i);

            File.WriteAllLines(filePath, fileLines);
            AssetDatabase.Refresh();
        }

        public static void SetFileHidden(string filePath, bool hide)
        {
            if (filePath.StartsWith("Packages/"))
            {
                var projectRoot = Path.GetDirectoryName(Application.dataPath);
                filePath = Path.GetFullPath(Path.Combine(projectRoot, filePath));
            }

            var isFile = File.Exists(filePath);
            var isDirectory = Directory.Exists(filePath);

            if (!isFile && !isDirectory)
            {
                Debug.LogWarning("File or directory not found: " + filePath);
                return;
            }

            var parentDir = Path.GetDirectoryName(filePath);
            var name = Path.GetFileName(filePath);

            string targetName = hide
                ? "." + name
                : name.StartsWith(".")
                    ? name.Substring(1)
                    : name;

            var newPath = Path.Combine(parentDir, targetName);

            if (isDirectory)
            {
                if (Directory.Exists(newPath))
                {
                    MoveDirectoryContents(filePath, newPath, hide);
                    Directory.Delete(filePath, recursive: true);
                }
                else
                {
                    Directory.Move(filePath, newPath);
                }

                ApplyTildeRecursively(newPath, hide);
            }
            else if (isFile)
            {
                if (File.Exists(newPath))
                {
                    Debug.LogWarning($"Destination file already exists: {newPath}");
                    return;
                }

                File.Move(filePath, newPath);
            }

            AssetDatabase.Refresh();
        }

        public static void ToggleXmlCommentByKeyword(string relativePath, string keyword, bool comment)
        {
            var filePath = GetAbsolutePath(relativePath);

            if (!File.Exists(filePath) || !filePath.EndsWith(".xml"))
            {
                UnityEngine.Debug.LogWarning("Invalid XML file: " + filePath);
                return;
            }

            var fileLines = File.ReadAllLines(filePath).ToList();

            for (var i = 0; i < fileLines.Count; i++)
            {
                var line = fileLines[i];
                if (!line.Contains(keyword)) continue;

                var trimmed = line.Trim();

                if (comment && !trimmed.StartsWith("<!--"))
                {
                    var leadingSpaces = line.TakeWhile(char.IsWhiteSpace).Count();
                    fileLines[i] = new string(' ', leadingSpaces) + $"<!-- {trimmed} -->";
                }
                else if (!comment && trimmed.StartsWith("<!--") && trimmed.EndsWith("-->"))
                {
                    var leadingSpaces = line.TakeWhile(char.IsWhiteSpace).Count();
                    var uncommented = trimmed.Substring(4, trimmed.Length - 7).Trim();
                    fileLines[i] = new string(' ', leadingSpaces) + uncommented;
                }
            }

            File.WriteAllLines(filePath, fileLines);
            AssetDatabase.Refresh();
        }

        public static void ToggleXmlCommentBlockByTagName(string relativePath, string openTag, string closeTag,
            bool comment)
        {
            var filePath = GetAbsolutePath(relativePath);

            if (!File.Exists(filePath) || !filePath.EndsWith(".xml"))
            {
                UnityEngine.Debug.LogWarning("Invalid XML file: " + filePath);
                return;
            }

            var fileLines = File.ReadAllLines(filePath).ToList();

            var inBlock = false;
            var blockStart = -1;
            var blockEnd = -1;

            for (var i = 0; i < fileLines.Count; i++)
            {
                if (!inBlock && fileLines[i].Contains(openTag))
                {
                    // Search backward to find block start
                    for (var j = i; j >= 0; j--)
                    {
                        if (fileLines[j].Contains(openTag))
                        {
                            blockStart = j;
                            break;
                        }
                    }

                    // Search forward to find block end
                    for (var j = i; j < fileLines.Count; j++)
                    {
                        if (fileLines[j].Contains(closeTag))
                        {
                            blockEnd = j;
                            break;
                        }
                    }

                    if (blockStart >= 0 && blockEnd >= blockStart)
                    {
                        inBlock = true;
                        break;
                    }
                }
            }

            if (inBlock)
            {
                if (comment && !fileLines[blockStart].TrimStart().StartsWith("<!--"))
                {
                    fileLines[blockStart] = $"<!-- {fileLines[blockStart].Trim()}";
                    fileLines[blockEnd] = $"{fileLines[blockEnd].Trim()} -->";
                }
                else if (!comment &&
                         fileLines[blockStart].TrimStart().StartsWith("<!--") &&
                         fileLines[blockEnd].TrimEnd().EndsWith("-->"))
                {
                    fileLines[blockStart] = fileLines[blockStart].Replace("<!--", "").Trim();
                    fileLines[blockEnd] = fileLines[blockEnd].Replace("-->", "").Trim();
                }
            }

            File.WriteAllLines(filePath, fileLines);
            AssetDatabase.Refresh();
        }

        public static void CopyFileToPlugins(string sourcePath, string destinationFolder = "Assets/Plugins/Android")
        {
            var fullSourcePath = Path.GetFullPath(sourcePath);
            var fullDestinationDir = Path.GetFullPath(Path.Combine(Application.dataPath, "../", destinationFolder));
            var fileName = Path.GetFileName(fullSourcePath);
            var fullDestinationPath = Path.Combine(fullDestinationDir, fileName);

            if (!File.Exists(fullSourcePath))
            {
                Debug.LogWarning($"[JarInstaller] Source file not found: {fullSourcePath}");
                return;
            }

            if (!Directory.Exists(fullDestinationDir))
            {
                Directory.CreateDirectory(fullDestinationDir);
            }

            File.Copy(fullSourcePath, fullDestinationPath, true);
            AssetDatabase.Refresh();

            Debug.Log($"[JarInstaller] Copied {fileName} → {destinationFolder}");
        }

        private static void ApplyTildeRecursively(string dir, bool hide)
        {
            foreach (var path in Directory.GetFileSystemEntries(dir))
            {
                var parent = Path.GetDirectoryName(path);
                var name = Path.GetFileName(path);
                var newName = hide
                    ? (name.StartsWith(".") ? name : "." + name)
                    : (name.StartsWith(".") ? name.Substring(1) : name);

                var newPath = Path.Combine(parent, newName);

                if (newPath != path)
                {
                    if (File.Exists(path))
                        File.Move(path, newPath);
                    else if (Directory.Exists(path))
                        Directory.Move(path, newPath);
                }

                if (Directory.Exists(newPath))
                {
                    ApplyTildeRecursively(newPath, hide);
                }
            }
        }

        private static void MoveDirectoryContents(string sourceDir, string targetDir, bool hide)
        {
            foreach (var entry in Directory.GetFileSystemEntries(sourceDir))
            {
                var entryName = Path.GetFileName(entry);
                var newEntryName = hide
                    ? (entryName.StartsWith(".") ? entryName : "." + entryName)
                    : (entryName.StartsWith(".") ? entryName.Substring(1) : entryName);

                var destPath = Path.Combine(targetDir, newEntryName);

                // ✅ بررسی وجود مقصد و حذف در صورت نیاز
                if (File.Exists(destPath))
                {
                    File.Delete(destPath);
                }
                else if (Directory.Exists(destPath))
                {
                    Directory.Delete(destPath, recursive: true);
                }

                if (File.Exists(entry))
                    File.Move(entry, destPath);
                else if (Directory.Exists(entry))
                    Directory.Move(entry, destPath);
            }
        }

        private static void ToggleComment(string keyword, bool comment, List<string> fileLines, int i)
        {
            var line = fileLines[i];
            if (!line.Contains(keyword)) return;

            switch (comment)
            {
                case true when !line.TrimStart().StartsWith("//"):
                {
                    var leadingSpaces = line.TakeWhile(char.IsWhiteSpace).Count();
                    fileLines[i] = new string(' ', leadingSpaces) + "// " + line.TrimStart();
                    break;
                }
                case false when line.TrimStart().StartsWith("//"):
                {
                    var leadingSpaces = line.TakeWhile(char.IsWhiteSpace).Count();
                    var trimmed = line.TrimStart();
                    var uncommented = trimmed.Substring(2).TrimStart();
                    fileLines[i] = new string(' ', leadingSpaces) + uncommented;
                    break;
                }
            }
        }

        private static string GetAbsolutePath(string relativePath)
        {
            var projectRoot = Path.GetDirectoryName(UnityEngine.Application.dataPath);
            return Path.Combine(projectRoot, relativePath);
        }
    }
}
#endif