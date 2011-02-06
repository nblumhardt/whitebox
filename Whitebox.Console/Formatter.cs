using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace Whitebox.Console
{
    static class Formatter
    {
        public static string Describe(object o)
        {
            var sb = new StringBuilder();
            Describe(o, sb);
            return sb.ToString();
        }

        static void Describe(object o, StringBuilder sb)
        {
            if (o == null)
            {
                sb.Append("null");
                return;
            }

            var oType = o.GetType();

            if (o is string)
            {
                DescribeString(o, sb);
                return;
            }

            if (o is IEnumerable)
            {
                DescribeEnumerable(o, sb);
                return;
            }

            if (oType.IsValueType || oType.IsEnum)
            {
                DescribeSimpleType(o, sb);
                return;
            }

            DescribeStructure(o, sb);
        }

        static void DescribeEnumerable(object o, StringBuilder sb)
        {
            sb.Append("[");
            var first = true;
            foreach (var e in (IEnumerable)o)
            {
                if (!first)
                    sb.Append(", ");
                first = false;
                Describe(e, sb);
            }
            sb.Append("]");
        }

        static void DescribeString(object o, StringBuilder sb)
        {
            sb.Append("\"");
            sb.Append(o);
            sb.Append("\"");
        }

        static void DescribeSimpleType(object o, StringBuilder sb)
        {
            sb.Append(o);
        }

        static void DescribeStructure(object o, StringBuilder sb)
        {
            sb.Append(o.GetType().Name);
            sb.Append(" { ");
            var first = true;
            foreach (var property in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public))
            {
                if (!first)
                    sb.Append(", ");
                first = false;
                sb.Append(property.Name);
                sb.Append(" = ");
                Describe(property.GetValue(o, null), sb);
            }
            sb.Append(" }");
        }
    }
}
