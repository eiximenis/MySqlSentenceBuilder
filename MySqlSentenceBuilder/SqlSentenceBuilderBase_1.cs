using System;
using System.Linq;
using System.Linq.Expressions;
using System.Timers;
using MySqlSentenceBuilder.Extensions;

namespace MySqlSentenceBuilder
{
    public abstract class SqlSentenceBuilderBase<TFluent> : SqlSentenceBuilderBase
        where TFluent : SqlSentenceBuilderBase<TFluent>
    {
        public TFluent SetLimit(int start = 0, int size = 100)
        {
            SetLimitImpl(start, size);
            return (TFluent)this;
        }


        public TFluent OrderBy(params string[] fields)
        {
            OrderByImpl(fields);
            return (TFluent)this;
        }

        public TFluent OrderBy<TF>(params string[] fields)
        {
            OrderByImpl(fields.Select(field => SqlBuilderHelper.GetTableName<TF>() + "." + field).ToArray());
            return (TFluent) this;
        }


       


        public TFluent Distinct()
        {
            DistinctImpl();
            return (TFluent) this;
        }



    }
}