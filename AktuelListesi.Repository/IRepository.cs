using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;

namespace AktuelListesi.Repository
{
    public interface IRepository<T, TDto> where T : class where TDto : class
    {
        DbSet<T> Table { get; }
        IMapper Mapper { get; }
        bool IsExists<TProperty>(TProperty Id);
        bool IsConfirmDefault<TProperty>(T entity);

        #region All

        IEnumerable<TDto> All();
        IEnumerable<TDto> AllByDefault<TProperty>();
        IEnumerable<TDto> AllActives<TProperty>();
        IEnumerable<TDto> AllDeleted<TProperty>();

        #endregion

        #region Where

        IEnumerable<TDto> Where(Expression<Func<T, bool>> where);
        IEnumerable<TDto> WhereWithDefault<TProperty>(Expression<Func<T, bool>> where);
        IEnumerable<TDto> WhereActives<TProperty>(Expression<Func<T, bool>> where);
        IEnumerable<TDto> WhereDeleted<TProperty>(Expression<Func<T, bool>> where);

        #endregion

        #region Paging

        IEnumerable<TDto> Paging(int pageIndex = 0, int itemPerPage = 10);
        IEnumerable<TDto> Paging(int pageIndex = 0, int itemPerPage = 10, Expression<Func<T, bool>> where = null);
        IEnumerable<TDto> Paging<TKey>(int pageIndex = 0, int itemPerPage = 10, Expression<Func<T, bool>> where = null, Expression<Func<T, TKey>> orderBy = null, bool isDesc = false);

        #endregion

        TDto GetById<TProperty>(TProperty Id);
        TDto First(Expression<Func<T, bool>> first = null);
        TDto Single(Expression<Func<T, bool>> single);
        IEnumerable<TDto> OrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc);

        TDto Add(TDto entity);
        IEnumerable<TDto> AddRange(List<TDto> dtos);
        TDto Update(TDto entity);
        IEnumerable<TDto> UpdateRange(IEnumerable<TDto> dtos);
        TDto Delete<TProperty>(TDto entity, bool isSoftDelete = true);
        bool Save();

        bool BeginTransaction();
        bool Commit();
        bool Rollback();
    }
}
