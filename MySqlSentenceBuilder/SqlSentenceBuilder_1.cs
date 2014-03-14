using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MySqlSentenceBuilder.Extensions;

namespace MySqlSentenceBuilder
{
    public class SqlSentenceBuilder<T> : 
        SqlSentenceBuilderBase<SqlSentenceBuilder<T>>
    {
        private string[] _columns;



        private string GetValueNames(string[] values)
        {
            return GetValueNames<T>(values);
        }


        public SqlSentenceBuilder<T> WithColumns(params string[] values)
        {
            _columns = values;
            return this;
        }

        public SqlSentenceBuilder<T> WithColumns(params Expression<Func<T, object>>[] props  )
        {
            _columns = props.Select(p => p.AsPropertyName()).Where(n => !string.IsNullOrEmpty(n)).ToArray();
            return this;
        }

        public SqlSentenceBuilder<T> WhereParameters(object parameters,
            WhereSpecOperator whereOperator = WhereSpecOperator.EQ)
        {
            WhereParametersImpl(parameters, SqlBuilderHelper.GetTableName<T>(), whereOperator);
            return this;
        }

        public SqlSentenceBuilder<T> WhereParameters(params Expression<Func<T, dynamic>>[] parameters)
        {
            return WhereParameters(WhereSpecOperator.EQ, parameters);
        }

        public SqlSentenceBuilder<T> WhereParameters(WhereSpecOperator whereOperator, params Expression<Func<T, dynamic>>[] parameters)
        {
            var names = parameters.Select(p => p.AsPropertyName()).Where(n => !string.IsNullOrEmpty(n)).ToArray();
            WhereParametersImpl(names, SqlBuilderHelper.GetTableName<T>(), whereOperator);
            return this;
        }
 

        public SqlSentenceBuilder<T> OrderBy(params Expression<Func<T, dynamic>>[] props)
        {
            var names = props.Select(p => p.AsPropertyName()).Where(pn => !string.IsNullOrEmpty(pn)).ToArray();
            OrderBy<T>(names);
            return this;
        }


        public string ToSql()
        {
            var sb = new StringBuilder();

            string distinct = _distinct ? "DISTINCT" : String.Empty;

            sb.AppendFormat("select {0} {1} from {2}", distinct, GetValueNames(_columns), SqlBuilderHelper.GetTableName<T>());


            if (_filters != null && _filters.Any())
            {
                sb.Append(GetWhereClausule());
            }



            if (_orderby.Any())
            {
                sb.Append(GetOrderByClausule());
            }
            if (_start != null && _size != null)
            {
                sb.AppendFormat(" LIMIT {0},{1}", _start, _size);
            }


            return sb.ToString();
        }



    }
}