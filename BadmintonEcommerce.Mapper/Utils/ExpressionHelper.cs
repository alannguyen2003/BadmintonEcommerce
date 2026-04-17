using System.Linq.Expressions;

namespace BadmintonEcommerce.Mapper.Utils;

public static class ExpressionHelper
{
    public static string GetMemberName(Expression expression)
    {
        if (expression is LambdaExpression lambda)
            expression = lambda.Body;

        if (expression is UnaryExpression unary)
            expression = unary.Operand;

        if (expression is MemberExpression member)
            return member.Member.Name;

        throw new Exception("Invalid expression");
    }
}