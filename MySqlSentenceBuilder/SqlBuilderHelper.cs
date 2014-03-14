using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace MySqlSentenceBuilder
{
    static class SqlBuilderHelper
    {
        internal static string GetTableName<T>()
        {
            return GetTableName(typeof(T));

        }

        internal static string GetTableName(Type tableType)
        {
            var attr = tableType.GetCustomAttribute<TableAttribute>();
            var customName = attr != null ? attr.Name : null;
            return string.IsNullOrEmpty(customName) ? tableType.Name : customName;
        }

        internal static string GetJoinByClausure(JoinSpec join)
        {
            return string.Format(" {0} join {1} on {2}.{3} = {1}.{4} ", join.Type, 
                SqlBuilderHelper.GetTableName(join.TypeRight),
                SqlBuilderHelper.GetTableName(join.TypeLeft), join.ColNameTL, join.ColNameTR);
        }

    }
}