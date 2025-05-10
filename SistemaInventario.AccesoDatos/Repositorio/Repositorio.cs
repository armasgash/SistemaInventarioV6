using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad);      //INSERT TO TABLE
        }

        public async Task<T> Obtener(int id)
        {
            return await dbSet.FindAsync(id);   //SELECT *FROM TABLE WHERE ID = id
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
                                                        string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if(filtro != null)
            {
                query = query.Where(filtro);    //SELECT * FROM TABLE WHERE filtro
            }

            if(incluirPropiedades != null)
            {
                foreach (var propiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propiedad); //JOIN CON OTRA TABLA
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!isTracking)
            {
                query = query.AsNoTracking(); //NO SE REALIZAN CAMBIOS EN LA TABLA
            }

            return await query.ToListAsync(); //SELECT * FROM TABLE
        }

        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, 
                                            string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);    //SELECT * FROM TABLE WHERE filtro
            }

            if (incluirPropiedades != null)
            {
                foreach (var propiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propiedad); //JOIN CON OTRA TABLA
                }
            }

            if (!isTracking)
            {
                query = query.AsNoTracking(); //NO SE REALIZAN CAMBIOS EN LA TABLA
            }

            return await query.FirstOrDefaultAsync(); //SELECT * FROM TABLE
        }

        public void Remover(T entidad)
        {
            dbSet.Remove(entidad); //DELETE FROM TABLE WHERE ID = id
        }

        public void RemoverRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad); //DELETE FROM TABLE WHERE ID IN (id1, id2, id3)
        }
    }
}
