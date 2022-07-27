using MyDataFramework_Lesson.Interfaces;
using Newtonsoft.Json;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyDataFramework_Lesson.Models
{
    public class MyDbSet<T> : MyDbSet, IEnumerable<T> where T : IMyDBElement
    {
        private List<T> _elements = new List<T>();

        public void Add(T element)
        {
            int newId = (_elements.LastOrDefault()?.Id ?? 0) + 1;
            element.Id = newId;
            _elements.Add(element);

            IsChanged = true;
        }

        public void AddRange(IEnumerable<T> elements)
        {
            foreach (var element in elements)
            {
                Add(element);
            }
            IsChanged = true;
        }

        public void Remove(T element)
        {
            T elementToRemove = _elements.FirstOrDefault(x => x.Id == element.Id);
            _elements.Remove(elementToRemove);

            IsChanged = true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        public override void Fill(string data)
        {
            List<T> elements = JsonConvert.DeserializeObject<List<T>>(data);
            _elements = elements;
        }
    }

    public abstract class MyDbSet : IMyDBSet
    {
        public bool IsChanged { get; protected set; }

        public void SaveChanges()
        {
            IsChanged = false;
        }

        public MyDbSet()
        {
            IsChanged = false;
        }

        public abstract void Fill(string data);
    }
}
