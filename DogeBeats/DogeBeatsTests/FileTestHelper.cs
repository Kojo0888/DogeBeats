using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeatsTests.Data
{
    public static class FileTestHelper
    {
        public static void CreatePath(string fullpath)
        {
            string[] pathSplitted = fullpath.Split('\\', '/');
            for (int i = 0; i < pathSplitted.Length; i++)
            {
                string path = Path.Combine(pathSplitted.Take(i + 1).ToArray());
                CreateFolder(path);
            }
        }

        public static void CreateFolder(string path)
        {
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void Init()
        {
            FileTestHelper.CreatePath("Data\\Resources\\TimeLines");
            FileTestHelper.CreatePath("Data\\Resources\\Images");
            FileTestHelper.CreatePath("Data\\Resources\\Music");
            FileTestHelper.CreatePath("Data\\Resources\\Shapes");
            FileTestHelper.CreatePath("Data\\Resources\\Nested1\\Nested2\\Nested3");
            FileTestHelper.CreatePath("Data\\Resources\\TestSerialization");

        }

        public static void CreateDummyFile(string type, string name, string value = "whatever")
        {
            CreateFolder("Data\\Resources\\" + type);

            if (name.Contains("."))
                File.WriteAllText(Path.Combine("Data\\Resources", type, name), value);
            else
                File.WriteAllText(Path.Combine("Data\\Resources", type, name + ".txt"), value);
        }
    }
}
