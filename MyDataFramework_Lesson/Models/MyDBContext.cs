using MyDataFramework_Lesson.Interfaces;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MyDataFramework_Lesson.Models
{
    public class MyDBContext : IMyDBContext
    {
        private string _fileFormat = ".mydb";

        public string DB_PATH { get; private set; }

        public void SaveChanges()
        {
            var allProps = GetProperties();
            foreach (var prop in allProps)
            {
                MyDbSet set = prop.GetValue(this) as MyDbSet;

                if (set.IsChanged)
                {
                    string setPath = Path.Combine(DB_PATH, prop.Name + _fileFormat);
                    string setAsStr = JsonConvert.SerializeObject(set);

                    File.WriteAllText(setPath, setAsStr);

                    set.SaveChanges();
                }
            }
        }

        public void SetDBPath(string path)
        {
            DB_PATH = path;

            Directory.CreateDirectory(path);

            GetExistValues();
        }

        private void GetExistValues()
        {
            var allProps = GetProperties();
            foreach (var prop in allProps)
            {
                string setPath = Path.Combine(DB_PATH, prop.Name + _fileFormat);

                if (File.Exists(setPath))
                {
                    MyDbSet set = prop.GetValue(this) as MyDbSet;
                    string data = File.ReadAllText(setPath);

                    set.Fill(data);
                }
            }
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            return this.GetType().GetProperties().Where(x => x.PropertyType.BaseType.Name == nameof(MyDbSet));
        }
    }
}
