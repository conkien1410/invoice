using System.Text.Json;
using System.Linq.Expressions;
using ManagementApi.Models;


namespace ManagementApi.Services
{

    enum MathOperator 
    {
        
    }


    class Parser {


        public Expression BuildAssignment(int startInx, List<string> tokens, Dictionary<string, object> assignmentContext) 
        {
            int i = startInx;
            Expression leftOperand = null;

            if(i < tokens.Count)
            {
                string symbol = tokens.ElementAt(i);

                // Expression leftOperand = null;
                double number;
                object contextData = assignmentContext.GetValueOrDefault(symbol, null);
                if(contextData != null)
                {
        
                    double.TryParse(contextData.ToString(), out number);
                    leftOperand = Expression.Constant(number);
                }
                else if(double.TryParse(symbol, out number)) 
                {
                    leftOperand = Expression.Constant(number);

                }
                else if(symbol == "(") 
                {
                    List<string> childTokens = GetSubOperand(tokens, i + 1);
                    leftOperand = BuildAssignment(0, childTokens, assignmentContext);
                }
                if(i + 2 < tokens.Count) 
                {
                    return JoinOperand(tokens.ElementAt(i+1), leftOperand, BuildAssignment(i + 2, tokens, assignmentContext));
                }
            }
            return leftOperand;
        }

        private List<string> GetSubOperand(List<String> tokens, int startIndex)
        {
            List<string> childTokens = new List<string>(100);
            int i = startIndex;
            while(i < tokens.Count && tokens.ElementAt(i) != ")") 
            {
                childTokens.Append(tokens.ElementAt(i));
            }
            return childTokens;
        }
  
        private Expression JoinOperand(string symbol, Expression left, Expression right)
        {
            switch(symbol) 
            {
                case "+": return Expression.Add(left, right);
                case "-": return Expression.Subtract(left, right);
                case "*": return Expression.Multiply(left, right);
                case "/": return Expression.Divide(left, right);
               
                default:
                    throw new Exception("No supported");
            }
        }




        public Expression BuildCondition(Rule rule, Dictionary<string, object> dict) 
        {
            if (rule.join != null)
            {
                if (rule.join != "or" && rule.join != "and")
                    throw new Exception($"Condition [{rule.join}] is invalid.");

                if (rule.rules == null || rule.rules.Count < 2)
                    throw new Exception($"Cannot create [{rule.join}] expression.");

                IList<Expression> expressions = rule.rules
                    .Select(rule => BuildCondition(rule, dict))
                    .ToList();

                if (rule.join == "or")
                    return JoinExpressions(expressions, (left, right) => Expression.Or(left, right));
                else
                    return JoinExpressions(expressions, (left, right) => Expression.And(left, right));
            }
            else
            {
                Expression constant = Expression.Constant(rule.value);
                // Expression property = Expression.Property(paramExpr, rule.field);
                Expression property = Expression.Constant(dict.GetValueOrDefault(rule.field, null));

                // added to allow any value to be included.
                // if (rule.value == "ANY")
                // {
                //     Type type = GetType(rule.type);
                //     if (!type.IsPrimitive)
                //     {
                //         constant = Expression.Constant(null, type);

                //         switch (rule.oper)
                //         {
                //             case "equal": return Expression.NotEqual(property, constant);
                //             case "notequal": return Expression.Equal(property, constant);
                //             default:
                //                 throw new Exception($"Operator [{rule.oper}] is invalid for ANY filter. Use only equal or notequal.");
                //         }
                //     }

                //     constant = property;
                // }
                if (rule.type == "int") 
                {
                    constant = Expression.Constant(int.Parse(rule.value));
                }
                else if (rule.type == "date") 
                {
                    constant = Expression.Constant(DateOnly.Parse(rule.value));
                }


                switch (rule.oper)
                {
                    case "equal": return Expression.Equal(property, constant);
                    case "notequal": return Expression.NotEqual(property, constant);
                    case "greaterthan": return Expression.GreaterThan(property, constant);
                    case "greaterthanorequal": return Expression.GreaterThanOrEqual(property, constant);
                    case "lessthan": return Expression.LessThan(property, constant);
                    case "lessthanorequal": return Expression.LessThanOrEqual(property, constant);
                    default:
                        throw new Exception($"Operator [{rule.oper}] is invalid.");
                }
            }
        }

        private Expression JoinExpressions(IList<Expression> expressions, Func<Expression, Expression, Expression> join)
        {
            Expression andExpression = null;

            foreach (Expression expression in expressions)
            {
                if (andExpression == null)
                    andExpression = expression;
                else
                    andExpression = join(andExpression, expression);
            }

            return andExpression;
        }

        public static Type GetType(string typeName)
        {
            switch (typeName)
            {
                case "string": 
                    return typeof(string);
                default:
                    throw new Exception($"Type [{typeName}] is invalid.");
            }
        }

        
    }
    
    
}