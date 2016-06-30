using System.Collections;
using System.Collections.Generic;

namespace LifesGreat.RealSolution.Lib.Job
{
    public class JobCollection<T> : IEnumerable
    {
        private readonly IList<T> _list;
        
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
    }
}