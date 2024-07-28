using System.Collections.Generic;
using Godot;

namespace Modot.Portable;
public static class FileUtils {
    public static bool TryReadFile(string path, out string text){
        text = ReadFile(path);
        return text != string.Empty;
    }
    public static string ReadFile(string path){
        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        if(file != null){
            return file.GetAsText();
        }else{
            Scene.Instance.DebugText.DrawError($"File {path} is not exist.");
            return string.Empty;
        }
    }
    public static bool WriteFile(string path, string text){
        if (!FileAccess.FileExists(path))
        {
            using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
            if(file != null){
                file.StoreString(text);
            }else{
                Scene.Instance.DebugText.DrawError($"Failed to open file {path}");
                return false;
            }
        }else{
            Scene.Instance.DebugText.DrawError($"File {path} is exist.");
            return false;
        }
        return true;
    }

    public static List<string> GetFileNameFromDir(string directory){
        List<string> filePaths = new List<string>();
        using var dir = DirAccess.Open(directory);
        if(dir.DirExists(directory)){
            foreach (var fileName in dir.GetFiles())
                filePaths.Add(fileName);
        }else{
            Scene.Instance.DebugText.DrawError($"Directory {directory} is not exist.");
        }
        return filePaths;
    }
}