namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using System.Collections.Generic;

    public class Executor_OutOfProc : OutOfProcComponent
    {
        internal Executor_OutOfProc(VisualStudioInstance visualStudioInstance)
            : base(visualStudioInstance)
        {
            ExecutorInProc = CreateInProcComponent<Executor_InProc>(visualStudioInstance);
        }

        internal Executor_InProc ExecutorInProc
        {
            get;
        }

        public void Execute(Action method) => Execute(method);

        internal TResult Execute<TResult>(InProcComponent remote, Func<TResult> method)
            => (TResult)Execute(method);

        internal TResult Execute<T, TResult>(InProcComponent remote, Func<T, TResult> method)
            => (TResult)Execute(method);

        private object Execute(Delegate method)
        {
            var target = method.Target;
            var methodName = method.Method.Name;
            var targetType = target.GetType();
            var assemblyFile = targetType.Assembly.Location;
            var typeName = targetType.FullName;
            var localValues = GetLocalValues(target);
            return ExecutorInProc.Execute(assemblyFile, typeName, methodName, localValues);
        }

        private static Dictionary<string, object> GetLocalValues(object target)
        {
            var targetType = target.GetType();
            var localValues = new Dictionary<string, object>();
            foreach (var field in targetType.GetFields())
            {
                var name = field.Name;
                if (!name.StartsWith("<>"))
                {
                    var value = field.GetValue(target);
                    localValues[name] = value;
                }
            }

            return localValues;
        }
    }
}
