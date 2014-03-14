using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MySqlSentenceBuilder.Extensions;

namespace MySqlSentenceBuilder
{
    public class MultiSqlSentenceBuilder : SqlSentenceBuilderBase<MultiSqlSentenceBuilder>
    {
        private readonly Dictionary<Type, string[]> _columns;
        private readonly List<JoinSpec> _joins;

        internal Dictionary<Type, string[]> Columns { get { return _columns; } }

        public MultiSqlSentenceBuilder()
        {
            _columns = new Dictionary<Type, string[]>();
            _joins = new List<JoinSpec>();
        }

        public MySqlTableSpecifier<T> WithTable<T>()
        {
            CreateEntryForClassType(typeof(T));
            return new MySqlTableSpecifier<T>(this);
        }

        public MultiSqlSentenceBuilder WithFullTable<T>()
        {
            var specifier = WithTable<T>();
            specifier.WithAllColumns();
            return this;
        }

        public MultiSqlSentenceBuilder WhereParameters<T>(params Expression<Func<T, dynamic>>[] parameters)
        {
            return WhereParameters(WhereSpecOperator.EQ, parameters);
        }

        public MultiSqlSentenceBuilder WhereParameters<T>(WhereSpecOperator whereOperator, params Expression<Func<T, dynamic>>[] parameters)
        {
            var names = parameters.Select(e => e.AsPropertyName()).Where(n => !string.IsNullOrEmpty(n)).ToArray();
            WhereParametersImpl(names, SqlBuilderHelper.GetTableName<T>(), whereOperator);
            return this;
        }

        public MultiSqlSentenceBuilder Join<TL, TR>(
            Expression<Func<TL, dynamic>> field1, Expression<Func<TR, dynamic>> field2,
            JoinSpecOperator joinOperator = JoinSpecOperator.INNER)
        {
            var field1Name = field1.AsPropertyName();
            var field2Name = field2.AsPropertyName();

            if (string.IsNullOrEmpty(field1Name) || string.IsNullOrEmpty(field2Name)) return this;
            _joins.Add(JoinSpec.CreateSpec(typeof(TL), typeof(TR), field1Name, field2Name, joinOperator));
            return this;
        }

        public string ToSql()
        {
            var sb = new StringBuilder();
            var distinct = _distinct ? "DISTINCT" : String.Empty;
            sb.AppendFormat("select {0} ", distinct);
            var fields = _columns.Select(entry => GetValueNames(entry.Key, entry.Value)).ToList();
            sb.Append(string.Join(",", fields));
            sb.Append(" from ");
            sb.Append(string.Join(",", _columns.Keys.Select(SqlBuilderHelper.GetTableName)));

            foreach (var join in _joins)
            {
                sb.Append(SqlBuilderHelper.GetJoinByClausure(join));
            }

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


        private void CreateEntryForClassType(Type type)
        {
            if (!_columns.ContainsKey(type))
            {
                _columns.Add(type, null);
            }
        }
    }
}
