using AktuelListesi.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AktuelListesi.Repository
{
    public class Repository<T, TDto> : IRepository<T, TDto> where T : class where TDto : class
    {
        private readonly AktuelDbContext dbContext;
        private readonly IMapper mapper;
        private DbSet<T> table;

        public Repository(AktuelDbContext dbContext,
                          IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.table = this.dbContext.Set<T>();
        }


        public DbContext Context => this.dbContext;

        public DbSet<T> Table { get => this.table; }


        #region Queries

        #region All

        public IEnumerable<TDto> All()
        {
            return Table.AsEnumerable().Select(x => mapper.Map<T, TDto>(x));
        }


        public IEnumerable<TDto> AllByDefault<TProperty>()
        {
            return Table.Where(x => (x as BaseEntity<TProperty>).IsActive & !(x as BaseEntity<TProperty>).IsDeleted).Select(x => mapper.Map<T, TDto>(x));
        }

        public IEnumerable<TDto> AllActives<TProperty>()
        {
            return Table.Where(x => (x as BaseEntity<TProperty>).IsActive).Select(x => mapper.Map<T, TDto>(x));
        }

        public IEnumerable<TDto> AllDeleted<TProperty>()
        {
            return Table.Where(x => (x as BaseEntity<TProperty>).IsDeleted).Select(x => mapper.Map<T, TDto>(x));
        }


        #endregion

        #region First

        public TDto First(Expression<Func<T, bool>> first = null)
        {
            if (first != null)
                return mapper.Map<T, TDto>(table.FirstOrDefault(first));
            return mapper.Map<T, TDto>(table.FirstOrDefault());
        }

        public TDto FirstWithDefault<TProperty>(Expression<Func<T, bool>> first = null)
        {
            if (first != null)
                return mapper.Map<T, TDto>(table.Where(x => (x as BaseEntity<TProperty>).IsActive & !(x as BaseEntity<TProperty>).IsDeleted).FirstOrDefault(first));
            return mapper.Map<T, TDto>(table.FirstOrDefault());
        }

        #endregion

        #region Where

        public IEnumerable<TDto> Where(Expression<Func<T, bool>> where)
        {
            return Table.Where(where).AsEnumerable().Select(x => mapper.Map<T, TDto>(x));
        }


        public IEnumerable<TDto> WhereWithDefault<TProperty>(Expression<Func<T, bool>> where)
        {
            return Table.Where(x => (x as BaseEntity<TProperty>).IsActive & !(x as BaseEntity<TProperty>).IsDeleted).Where(where).AsEnumerable().Select(x => mapper.Map<T, TDto>(x));
        }


        public IEnumerable<TDto> WhereActives<TProperty>(Expression<Func<T, bool>> where)
        {
            return Table.Where(x => (x as BaseEntity<TProperty>).IsActive).Where(where).AsEnumerable().Select(x => mapper.Map<T, TDto>(x));
        }

        public IEnumerable<TDto> WhereDeleted<TProperty>(Expression<Func<T, bool>> where)
        {
            return Table.Where(x => (x as BaseEntity<TProperty>).IsDeleted).Where(where).AsEnumerable().Select(x => mapper.Map<T, TDto>(x));
        }

        #endregion

        #region GetById

        public TDto GetById<TProperty>(TProperty Id)
        {
            return mapper.Map<T, TDto>(table.SingleOrDefault(x => (x as BaseEntity<TProperty>).Id.Equals(Id)));
        }


        #endregion

        #region OrderBy

        public IEnumerable<TDto> OrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc)
        {
            if (isDesc)
                return table.OrderByDescending(orderBy).Select(x => mapper.Map<T, TDto>(x));
            return table.OrderBy(orderBy).Select(x => mapper.Map<T, TDto>(x));
        }

        #endregion

        #region Single

        public TDto Single(Expression<Func<T, bool>> single)
        {
            return mapper.Map<T, TDto>(table.SingleOrDefault(single));
        }


        #endregion

        #region Paging

        public IEnumerable<TDto> Paging(int pageIndex = 0, int itemPerPage = 10)
        {
            return Table
                    .Skip(pageIndex * itemPerPage)
                    .Take(itemPerPage)
                    .Select(x => mapper.Map<T, TDto>(x))
                    .AsEnumerable();
        }

        public IEnumerable<TDto> Paging(int pageIndex = 0, int itemPerPage = 10, Expression<Func<T, bool>> where = null)
        {
            if (where != null)
                return Table
                    .Where(where)
                    .Skip(pageIndex * itemPerPage)
                    .Take(itemPerPage)
                    .Select(x => mapper.Map<T, TDto>(x))
                    .AsEnumerable();
            return Paging(pageIndex, itemPerPage);

        }

        public IEnumerable<TDto> Paging<TKey>(int pageIndex = 0, int itemPerPage = 10, Expression<Func<T, bool>> where = null, Expression<Func<T, TKey>> orderBy = null, bool isDesc = false)
        {
            IQueryable<T> query = null;
            if (where != null)
                query = Table.Where(where);
            if (orderBy != null && query != null)
                query = (isDesc) ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            else
                query = query
                    .Skip(pageIndex * itemPerPage)
                    .Take(itemPerPage);

            return query
                    .Select(x => mapper.Map<T, TDto>(x))
                   .AsEnumerable();
        }

        #endregion

        public bool IsExists<TProperty>(TProperty Id)
        {
            return Table.Any(x => (x as BaseEntity<TProperty>).Id.Equals(Id));
        }

        public bool IsConfirmDefault<TProperty>(T entity)
        {
            return (entity as BaseEntity<TProperty>).IsActive & !(entity as BaseEntity<TProperty>).IsDeleted;
        }

        #endregion

        #region Execution

        public TDto Add(TDto entity)
        {
            var obj = mapper.Map<TDto, T>(entity);
            Table.Add(obj);
            bool isSuccess = Save();
            entity = mapper.Map<T, TDto>(obj);
            return entity;
        }

        public TDto Update(TDto entity)
        {
            var obj = mapper.Map<TDto, T>(entity);

            dbContext.Entry<T>(obj).State = EntityState.Modified;
            bool isSuccess = Save();
            entity = mapper.Map<T, TDto>(obj);
            return entity;
        }

        public TDto Delete<TProperty>(TDto entity, bool isSoftDelete = true)
        {
            var obj = mapper.Map<TDto, T>(entity);
            if (!isSoftDelete)
                table.Remove(obj);
            else
            {
                (obj as BaseEntity<TProperty>).IsDeleted = isSoftDelete;
                dbContext.Entry<T>(obj).State = EntityState.Modified;
            }
            bool isSuccess = Save();
            entity = mapper.Map<T, TDto>(obj);
            return entity;
        }

        public bool BeginTransaction()
        {
            if (dbContext.Database.CurrentTransaction != null) return false;
            try
            {
                dbContext.Database.BeginTransaction();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Commit()
        {
            if (dbContext.Database.CurrentTransaction == null) return false;
            dbContext.Database.CommitTransaction();
            return true;
        }

        public bool Rollback()
        {
            if (dbContext.Database.CurrentTransaction == null) return false;
            dbContext.Database.RollbackTransaction();
            return true;
        }

        public bool Save()
        {
            try
            {
                dbContext.SaveChanges();
                return true;
            }
            catch
            {
                //Logger.LogError(ex, (ex.InnerException != null) ? ex.InnerException.Message : "");
                return false;
            }
        }

        #endregion

    }
}
