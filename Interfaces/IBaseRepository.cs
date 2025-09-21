using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_Pract_2.Interfaces
{
    public interface IBaseRepository<T>
    {
        public int Insert(T entity);
        public bool Delete(T entity);
        public T Update(T entity);
        public IEnumerable<T> Select();
        public T GetById(int Id);
        public int GetIdName(string name);
    }
}
