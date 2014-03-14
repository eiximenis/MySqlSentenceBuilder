using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MySqlSentenceBuilder.Extensions;

namespace MySqlSentenceBuilder
{
    public class SqlSentenceBuilder<TL, TR> : SqlSentenceBuilderBase<SqlSentenceBuilder<TL, TR>>
    {
        private string[] _leftcols;
        private string[] _rightcols;
        private JoinSpec<TL, TR> _join;

        public SqlSentenceBuilder<TL, TR> WithLeftColumns(params string[] values)
        {
            _leftcols = values;
            return this;

        }
        public SqlSentenceBuilder<TL, TR> WithRightColumns(params string[] values)
        {
            _rightcols = values;
            return this;
        }

        public SqlSentenceBuilder<TL, TR> WithNoLeftColumns()
        {
            _leftcols = new string[0];
            return this;
        }
        public SqlSentenceBuilder<TL, TR> WithNoRightColumns()
        {
            _rightcols = new string[0];
            return this;
        }


        public SqlSentenceBuilder<TL, TR> WhereParameters(object letfparameters, object rightparameters, WhereSpecOperator whereOperator = WhereSpecOperator.EQ)
        {
            if (letfparameters != null) WhereParametersImpl(letfparameters, SqlBuilderHelper.GetTableName<TL>(), whereOperator);
            if (rightparameters != null) WhereParametersImpl(rightparameters, SqlBuilderHelper.GetTableName<TR>(), whereOperator);

            return this;
        }

        public SqlSentenceBuilder<TL, TR> WhereParametersTypeLeft(WhereSpecOperator whereOperator, params Expression<Func<TL, dynamic>>[] parameters)
        {
            if (!parameters.Any()) return this;
            WhereParametersTypeImpl(whereOperator, parameters);
            return this;
        }

        public SqlSentenceBuilder<TL, TR> WhereParametersTypeRight(WhereSpecOperator whereOperator, params Expression<Func<TR, dynamic>>[] parameters)
        {
            if (!parameters.Any()) return this;
            WhereParametersTypeImpl(whereOperator, parameters);
            return this;
        }

        private void WhereParametersTypeImpl<T>(WhereSpecOperator whereOperator,
            params Expression<Func<T, dynamic>>[] parameters)
        {
            var names = parameters.Select(p => p.AsPropertyName()).Where(n => !string.IsNullOrEmpty(n)).ToArray();
            WhereParametersImpl(names, SqlBuilderHelper.GetTableName<T>(), whereOperator);
        }


        public SqlSentenceBuilder<TL, TR> Join(string field1, string field2, JoinSpecOperator joinOperator = JoinSpecOperator.INNER)
        {
            if (!String.IsNullOrEmpty(field1) && !String.IsNullOrEmpty(field2))
            {
                _join = JoinSpec<TL, TR>.CreateSpec(field1, field2, joinOperator);
            }
            return this;
        }
        public SqlSentenceBuilder<TL, TR> Join(Expression<Func<TL, dynamic>> field1, Expression<Func<TR, dynamic>> field2, JoinSpecOperator joinOperator = JoinSpecOperator.INNER)
        {
            return Join(field1.AsPropertyName(), field2.AsPropertyName(), joinOperator);
        }

        public string ToSql()
        {
            var sb = new StringBuilder();

            var distinct = _distinct ? "DISTINCT" : String.Empty;

            var comma = _leftcols != null && _rightcols != null && _leftcols.Any() && _rightcols.Any();

            sb.AppendFormat("select {0} {1} {2} {3} from {4}", distinct,
                GetValueNames<TL>(_leftcols), comma ? "," : string.Empty, GetValueNames<TR>(_rightcols),
                SqlBuilderHelper.GetTableName<TL>());

            if (_join != null)
            {
                sb.Append(SqlBuilderHelper.GetJoinByClausure(_join));
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

    }
}