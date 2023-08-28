using System.Diagnostics;
using System.Text.Json;

namespace ServiceKeeper.Producer.Sample.Domain
{
    public class SaveHelper
    {
        private string DirectoryString { get; init; }
        public SaveHelper(string DirectoryName = "config")
        {
            try
            {
                DirectoryString = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DirectoryName);
                if (!Directory.Exists(DirectoryString))
                    Directory.CreateDirectory(DirectoryString);
            }
            catch
            {
                throw;
            }
        }
        public T? Load<T>(string name)
        {
            try
            {
                if (File.Exists(@$"{DirectoryString}\{name}"))
                {
                    string json = File.ReadAllText(@$"{DirectoryString}\{name}");
                    T? result = JsonSerializer.Deserialize<T>(json);
                    return result == null ? throw new Exception(@$"加载{DirectoryString}\{name}文件失败,加载内容为空") : result;
                }
                else
                {
                    throw new Exception(@$"无法找到{DirectoryString}\{name}文件");
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                return default; // 返回默认值，即null
            }
        }


        public void Save<T>(T confObject, string name)
        {
            try
            {
                Delete(name);
                CheckPath(DirectoryString);
                string json = JsonSerializer.Serialize(confObject);
                File.WriteAllText(@$"{DirectoryString}\{name}", json);
            }
            catch
            {
                throw;
            }
        }

        public void Delete(string name)
        {
            if (File.Exists(@$"{DirectoryString}\{name}"))
                File.Delete(@$"{DirectoryString}\{name}");
        }

        /// <summary>
        /// 检查路径是否存在，不存在则创建
        /// </summary>
        /// <param name="path">路径 x:/a/b/</param>
        public static void CheckPath(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
