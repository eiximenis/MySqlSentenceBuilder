using System;

namespace MySqlSentenceBuilder
{
    class JoinSpec
    {
        public Type TypeLeft { get; protected set; }
        public Type TypeRight { get; protected set; }
        public string ColNameTL { get; set; }
        public string ColNameTR { get; set; }
        public string Type { get; set; }
        public object Table { get; set; }

        protected JoinSpec()
        {
        }


        public static JoinSpec CreateSpec(Type tleft, Type tright, string colname1, string colname2, JoinSpecOperator op)
        {
            switch (op)
            {
                case JoinSpecOperator.INNER:
                    return CreateInnerSpec(tleft, tright, colname1, colname2);

            }
            return null;
        }

        private static JoinSpec CreateInnerSpec(Type tl, Type tr, string colname1, string colname2)
        {
            return new JoinSpec()
            {
                TypeLeft = tl,
                TypeRight = tr,
                ColNameTL = colname1,
                ColNameTR = colname2,
                Type = "INNER"
            };
        }
    }

    class JoinSpec<TL, TR> : JoinSpec
    {
        private JoinSpec()
        {
            TypeLeft = typeof(TL);
            TypeRight = typeof(TR);
        }


        public static JoinSpec<TL, TR> CreateInnerSpec(string colname1, string colname2)
        {
            var table = SqlBuilderHelper.GetTableName<TR>();
            return new JoinSpec<TL, TR>() { ColNameTL = colname1, ColNameTR = colname2, Table = table, Type = "INNER" };
        }

        public static JoinSpec<TL, TR> CreateSpec(string colname1, string colname2, JoinSpecOperator op)
        {
            switch (op)
            {
                case JoinSpecOperator.INNER:
                    return CreateInnerSpec(colname1, colname2);

            }
            return null;
        }
    }
}