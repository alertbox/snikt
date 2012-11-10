using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Snikt
{
    public class Materializer<T> where T: class
    {
        private readonly ParameterExpression recordParameter = Expression.Parameter(typeof(IDataRecord), "record");
        private readonly MethodInfo fieldOfTOrdinalMethod = typeof(DataExtensions).GetMethod("Field", new Type[] { typeof(IDataRecord), typeof(int) });

        private Func<IDataRecord, T> shaperDelegate;

        public Materializer(IDataReader reader)
        {
            shaperDelegate = GetDefaultShaper(reader);
        }

        public T Materialize(IDataRecord record)
        {
            return shaperDelegate(record);
        }

        private Func<IDataRecord, T> GetDefaultShaper(IDataReader reader)
        {
            IEnumerable<string> fieldNames = reader.GetFieldNames();
            IEnumerable<MemberBinding> memberBindings = GetMemberBindings(fieldNames);
            Expression<Func<IDataRecord, T>> shaper =
                Expression.Lambda<Func<IDataRecord, T>>(Expression.MemberInit(Expression.New(typeof(T)), memberBindings), recordParameter);

            return shaper.Compile();
        }

        private IEnumerable<MemberBinding> GetMemberBindings(IEnumerable<string> fieldNames)
        {
            Type t = typeof(T);
            return fieldNames.Select((name, ordinal) => CreateMemberBinding(t, name, ordinal));
        }

        private MemberBinding CreateMemberBinding(Type t, string fieldName, int ordinal)
        {
            PropertyInfo property = t.GetProperty(fieldName);
            MethodInfo setterMethod = property.GetSetMethod();
            MethodInfo fieldOfTMethod = fieldOfTOrdinalMethod.MakeGenericMethod(property.PropertyType);
            Expression fieldArg = Expression.Constant(ordinal);
            Expression getterArg = Expression.Call(fieldOfTMethod, recordParameter, fieldArg);

            return Expression.Bind(setterMethod, getterArg);
        }
    }
}
