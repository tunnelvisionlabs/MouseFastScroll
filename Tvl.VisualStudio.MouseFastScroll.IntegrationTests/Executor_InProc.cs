namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using EnvDTE;

    internal class Executor_InProc : InProcComponent
    {
        private Executor_InProc()
        {
        }

        public static Executor_InProc Create()
            => new Executor_InProc();

        public object Execute(string assemblyFile, string typeName, string methodName, Dictionary<string, object> fieldValues)
        {
            var assembly = Assembly.LoadFrom(assemblyFile);
            var type = assembly.GetType(typeName);
            var obj = Activator.CreateInstance(type);
            foreach (var field in type.GetFields())
            {
                var name = field.Name;
                if (!name.StartsWith("<>"))
                {
                    var value = fieldValues[name];
                    field.SetValue(obj, value);
                }
            }

            var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var args = GetArguments(method);
            return method.Invoke(obj, args);
        }

        private object[] GetArguments(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var args = new object[parameters.Length];
            for (int n = 0; n < args.Length; n++)
            {
                args[n] = GetService(parameters[n].ParameterType);
            }

            return args;
        }

        private object GetService(Type type)
        {
            if (type == typeof(DTE))
            {
                return GetDTE();
            }

            throw new ApplicationException($"Service interface of type '{type.FullName}' not supported");
        }
    }
}
