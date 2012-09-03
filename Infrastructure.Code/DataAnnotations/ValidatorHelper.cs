
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.Infrastructure.Code.DataAnnotations
{
    /// <summary>
    /// Helper class to invoke the Validator object.
    /// </summary>
    public static class ValidatorHelper
    {
        /// <summary>
        /// The registered validations.
        /// </summary>
        private static Dictionary<Type, Delegate> validations;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ValidatorHelper()
        {
            // Initialize the validations.
            validations = new Dictionary<Type, Delegate>();
        }

        /// <summary>
        /// Builds an expression that validates the specified object.
        /// </summary>
        private static Expression<Action<T, List<ValidationError<TErrorCode>>>> BuildObjectValidation<T, TErrorCode>(ParameterExpression validatedObject, ParameterExpression validationErrorsList)
        {
            // Build the parameters.
            ParameterExpression[] parameters = new ParameterExpression[] { validatedObject, validationErrorsList };

            // Build the variables.
            ParameterExpression dummy = Expression.Variable(typeof(string), "dummy");
            ParameterExpression[] variables = new ParameterExpression[] { dummy };

            // Build the expressions.
            List<Expression> expressions = new List<Expression>();
            expressions.Add(dummy);

            // ***

            // Build the object validation.
            BlockExpression blockExpression = Expression.Block(variables, expressions);
            Expression<Action<T, List<ValidationError<TErrorCode>>>> expression = Expression.Lambda<Action<T, List<ValidationError<TErrorCode>>>>(blockExpression, parameters);
            return expression;
        }

        /// <summary>
        /// Registers the specified validation.
        /// </summary>
        public static void RegisterValidation<T, TErrorCode>()
            where T : class
        {
            // Build the parameters.
            ParameterExpression validatedObject = Expression.Parameter(typeof(T), "validatedObject");
            ParameterExpression[] parameters = new ParameterExpression[] { validatedObject };

            // Build the variables.
            ParameterExpression validationErrorsList = Expression.Variable(typeof(List<ValidationError<TErrorCode>>), "validationErrorList");
            ParameterExpression validationErrorsArray = Expression.Variable(typeof(ValidationError<TErrorCode>[]), "validationErrorArray");
            ParameterExpression[] variables = new ParameterExpression[] { validationErrorsList, validationErrorsArray };

            // Build the expressions.
            List<Expression> expressions = new List<Expression>();

            // Instanciate the validation errors list.
            Expression validationErrorsListInstanciation = Expression.New(typeof(List<ValidationError<TErrorCode>>));
            Expression validationErrorsListAssignation = Expression.Assign(validationErrorsList, validationErrorsListInstanciation);
            expressions.Add(validationErrorsListAssignation);

            // Validate the specified object.
            Expression<Action<T, List<ValidationError<TErrorCode>>>> validateObject = BuildObjectValidation<T, TErrorCode>(validatedObject, validationErrorsList);
            Expression validateObjectInvocation = Expression.Invoke(validateObject, validatedObject, validationErrorsList);
            expressions.Add(validateObjectInvocation);

            // Convert the validation errors list to an array.
            MethodCallExpression validationErrorsListToArray = Expression.Call(validationErrorsList, "ToArray", new Type[0]);
            Expression validationErrorsArrayAssignation = Expression.Assign(validationErrorsArray, validationErrorsListToArray);
            expressions.Add(validationErrorsArrayAssignation);

            // Compile the validation.
            BlockExpression blockExpression = Expression.Block(variables, expressions);
            Expression<Func<T, ValidationError<TErrorCode>[]>> expression = Expression.Lambda<Func<T, ValidationError<TErrorCode>[]>>(blockExpression, parameters);
            Func<T, ValidationError<TErrorCode>[]> validation = expression.Compile();

            // Register the validation.
            validations.Add(typeof(T), validation);
        }

        /// <summary>
        /// Validates the specified object.
        /// </summary>
        public static ValidationError<TErrorCode>[] ValidateObject<T, TErrorCode>(T target)
            where T: class
        {
            // Get the registered validation.
            Func<T, ValidationError<TErrorCode>[]> validation = validations[typeof(T)] as Func<T, ValidationError<TErrorCode>[]>;

            // Invoke the validation.
            ValidationError<TErrorCode>[] validationErrors = validation(target);
            return validationErrors;
        }

        /// <summary>
        /// Returns whether the specified property is valid.
        /// </summary>
        public static bool ValidateProperty(object instance, string propertyName, object propertyValue)
        {
            // Build a validation context for the specified property.
            ValidationContext validationContext = new ValidationContext(instance, null, null);
            validationContext.MemberName = propertyName;

            // Validate the specified property value.
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isPropertyValid = Validator.TryValidateProperty(propertyValue, validationContext, validationResults);

            // Return whether the property is valid.
            return isPropertyValid;
        }
    }
}
