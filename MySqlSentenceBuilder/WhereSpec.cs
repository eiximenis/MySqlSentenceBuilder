namespace MySqlSentenceBuilder
{
    public class WhereSpec
    {

        public string ColName { get; set; }
        public string Operator { get; set; }
        public string ParamName { get; set; }

        public static WhereSpec CreateEqualitySpec(string colname, string paramname)
        {
            return new WhereSpec() { ColName = colname, Operator = "=", ParamName = "@" + paramname };
        }

        public static WhereSpec CreateIsNullSpec(string colname, string paramname)
        {
            return new WhereSpec() { ColName = colname, Operator = "IS", ParamName = null };
        }

        public static WhereSpec CreateContainsSpec(string colname, string paramname)
        {
            return new WhereSpec() { ColName = colname, Operator = "like", ParamName = "@" + paramname };
        }


        public static WhereSpec CreateGreaterSpec(string colname, string paramname)
        {
            return new WhereSpec() { ColName = colname, Operator = ">", ParamName = "@" + paramname };
        }


        public static WhereSpec CreateGreaterOrEqualSpec(string colname, string paramname)
        {
            return new WhereSpec() { ColName = colname, Operator = ">=", ParamName = "@" + paramname };
        }

        public static WhereSpec CreateLessSpec(string colname, string paramname)
        {
            return new WhereSpec() { ColName = colname, Operator = "<", ParamName = "@" + paramname };
        }

        public static WhereSpec CreateLessOrEqualSpec(string colname, string paramname)
        {
            return new WhereSpec() { ColName = colname, Operator = "<=", ParamName = "@" + paramname };
        }



        public static WhereSpec CreateSpec(string colname, string paramname, WhereSpecOperator op)
        {
            switch (op)
            {
                case WhereSpecOperator.EQ:
                    return CreateEqualitySpec(colname, paramname);
                case WhereSpecOperator.GE:
                    return CreateGreaterOrEqualSpec(colname, paramname);
                case WhereSpecOperator.GT:
                    return CreateGreaterSpec(colname, paramname);
                case WhereSpecOperator.LT:
                    return CreateLessSpec(colname, paramname);
                case WhereSpecOperator.LE:
                    return CreateLessOrEqualSpec(colname, paramname);
                case WhereSpecOperator.CONT:
                    return CreateContainsSpec(colname, paramname);
            }
            return null;
        }

        public string ToSql()
        {
            return string.Format("{0} {1} {2}", ColName, Operator, ParamName ?? "NULL");
        }

    }
}