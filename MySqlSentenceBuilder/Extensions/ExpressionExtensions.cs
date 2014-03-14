using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MySqlSentenceBuilder.Extensions
{
    static class ExpressionExtensions
    {
        public static string AsPropertyName<T, TR>(this Expression<Func<T, TR>>  expr)
        {
            var memberExp = expr.Body as MemberExpression;
            if (memberExp == null)
            {
                var unary = expr.Body as UnaryExpression;
                if (unary != null && unary.NodeType == ExpressionType.Convert)
                {
                    memberExp = unary.Operand as MemberExpression;
                }
            }

            if (memberExp != null)
            {
                var property = memberExp.Member as PropertyInfo;
                if (property != null)
                {
                    return property.Name;
                }
            }
            return null;
        }
    }
}
