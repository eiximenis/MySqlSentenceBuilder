using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace MySqlSentenceBuilder
{
    public abstract class SqlSentenceBuilderBase
    {
        protected List<string> _orderby;
        protected int? _start;
        protected int? _size;
        protected bool _distinct;
        protected List<WhereSpec> _filters;

        protected SqlSentenceBuilderBase()
        {
            _orderby = new List<string>();
        }

        protected virtual void OrderByImpl(params string[] fields)
        {
            if (fields != null && fields.Any())
            {
                _orderby.AddRange(fields);
            }

        }

        protected virtual string GetOrderByClausule()
        {
            return string.Format(" ORDER BY {0}", string.Join(",", _orderby));
        }

        protected virtual void SetLimitImpl(int start, int size)
        {
            _start = start;
            _size = size;
        }

        protected virtual void WhereParametersImpl(string[] parameters, string tablename, WhereSpecOperator whereOperator)
        {
            _filters = _filters ?? new List<WhereSpec>();
            foreach (var parameter in parameters)
            {
                var columnfullname = string.IsNullOrEmpty(tablename)
                    ? parameter
                    : string.Format("{0}.{1}", tablename, parameter);
                _filters.Add(WhereSpec.CreateSpec(columnfullname, parameter, whereOperator));
            }
        }


        protected virtual void WhereParametersImpl(object parameter, string tablename, WhereSpecOperator whereOperator)
        {

            if (parameter == null) return;
            var properties = parameter.GetType().GetProperties().
                Where(p => p.CanRead && !p.GetMethod.IsVirtual).Select(p => p.Name);

            WhereParametersImpl(properties.ToArray(), tablename, whereOperator);

        }

 
        protected virtual string GetWhereClausule()
        {

            var allWheres = string.Join(" AND ", _filters.Select(f => f.ToSql()));
            return string.Concat(" WHERE ", allWheres);
        }


        protected string GetValueNames(Type type, string[] values)
        {
            if (values == null)
            {
                return string.Join(",", GetAllColumns(type));
            }
            if (!values.Any()) return string.Empty;

            var tn = SqlBuilderHelper.GetTableName(type);
            return string.Join(",", values.Select(v => string.Concat(tn, ".", v)));
        }

        protected string GetValueNames<T>(string[] values)
        {
            return GetValueNames(typeof(T), values);
        }

        protected void DistinctImpl()
        {
            _distinct = true;
        }

        private IEnumerable<string> GetAllColumns(Type type)
        {
            var tablename = SqlBuilderHelper.GetTableName(type);
            return type.GetProperties().Where(p => p.CanRead && !p.GetMethod.IsVirtual).
                Select(p => tablename + "." + p.Name);
        }

    }
}
