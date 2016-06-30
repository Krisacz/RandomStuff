using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LifesGreat.RealSolution.Lib.Job;

namespace LifesGreat.RealSolution.Lib.Sort
{
    public interface IJobSorter<T>
    {
        T Sort(T jobs);
    }
}
