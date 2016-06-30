using System.Collections;
using System.Collections.Generic;

namespace LifesGreat.RealSolution.Lib.Job
{
    public class JobCollection<T> : IEnumerable
    {
        private readonly IList<T> _list;

        public JobCollection(IList<T> list)
        {
            _list = new List<T>(list);
        }

        public JobCollection()
        {
            _list = new List<T>();
        }
        
        public int Count()
        {
            return _list.Count;
        }

        public void Add(T obj)
        {
            _list.Add(obj);
        }

        public int IndexOf(T obj)
        {
            return _list.IndexOf(obj);
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public IList<T> GetCopy()
        {
            return _list;
        }

        public void RemoveAt(int jobIndex)
        {
            _list.RemoveAt(jobIndex);
        }

        public void Insert(int index, T obj)
        {
            _list.Insert(index, obj);
        }
    }
}