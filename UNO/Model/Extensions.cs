using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNO.Model.Karten;

namespace UNO.Model
{
    public static class Extensions
    {
        public static Queue<T> Mischen<T>(this Queue<T> q)
        {
            Queue<T> result = new Queue<T>();
            List<T> l = new List<T>();
            Random r = new Random();
            foreach (T item in q)
            {
                l.Insert(r.Next(0, l.Count + 1), item);
            }

            foreach (T item in l)
            {
                result.Enqueue(item);
            }

            return result;
        }
    }
}
