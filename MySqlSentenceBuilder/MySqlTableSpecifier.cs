using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MySqlSentenceBuilder.Extensions;

namespace MySqlSentenceBuilder
{
    public class MySqlTableSpecifier<T>
    {
        private readonly MultiSqlSentenceBuilder _owner;
        internal MySqlTableSpecifier(MultiSqlSentenceBuilder owner)
        {
            _owner = owner;
        }

        public MultiSqlSentenceBuilder WithColumns(params string[] colnames)
        {
            _owner.Columns[typeof(T)] = colnames;
            return _owner;
        }

        public MultiSqlSentenceBuilder WithColumns(params Expression<Func<T, object>>[] colnames)
        {
            var names = new string[colnames.Length];
            var idx = 0;
            foreach (var colname in colnames)
            {
                var pname = colname.AsPropertyName();
                if (!string.IsNullOrEmpty(pname))
                {
                    names[idx++] = pname;
                }
            }

            _owner.Columns[typeof(T)] = names.Where(n => !string.IsNullOrEmpty(n)).ToArray();

            return _owner;
        }

        public MultiSqlSentenceBuilder WithNoColumns()
        {
            _owner.Columns[typeof(T)] = new string[0];
            return _owner;
        }


        public MultiSqlSentenceBuilder WithAllColumns()
        {
            _owner.Columns[typeof (T)] = null;
            return _owner;
        }
    }
}