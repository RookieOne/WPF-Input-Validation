using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using CommonLibrary.Wpf;

namespace CommonLibrary.Extensions
{
    public static class ExpressionExtensions
    {
        public static ICommand AsCommand(this Action action)
        {
            return new ActionCommand(action);
        }

        public static string GetPropertyName(this LambdaExpression lambdaExpression)
        {
            Expression e = lambdaExpression;

            bool done = false;

            while (!done)
            {
                switch (e.NodeType)
                {
                    case ExpressionType.Convert:
                        e = ((UnaryExpression) e).Operand;
                        break;
                    case ExpressionType.Lambda:
                        e = ((LambdaExpression) e).Body;
                        break;

                    case ExpressionType.MemberAccess:
                        var propertyInfo = ((MemberExpression) e).Member as PropertyInfo;

                        return propertyInfo != null
                                   ? propertyInfo.Name
                                   : string.Empty;

                    default:
                        done = true;
                        break;
                }
            }

            return string.Empty;
        }
    }
}