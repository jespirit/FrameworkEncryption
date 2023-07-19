using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineKeyTest
{
    class ParseViewStateClass
    {
        public void ParseViewState(object vs, int level, string prefix="")
        {
            if (vs == null)
            {
                Console.WriteLine("[{0}]: {1}{2}{3}", level.ToString(), Spaces(level), prefix, "null");
            }
            else if (vs.GetType() == typeof(System.Web.UI.Triplet))
            {
                Console.WriteLine("[{0}]: {1}{2}{3}", level.ToString(), Spaces(level), prefix, "Triplet");
                ParseViewState((System.Web.UI.Triplet)vs, level);
            }
            else if (vs.GetType() == typeof(System.Web.UI.Pair))
            {
                Console.WriteLine("[{0}]: {1}{2}{3}", level.ToString(), Spaces(level), prefix, "Pair");
                ParseViewState((System.Web.UI.Pair)vs, level);
            }
            else if (vs.GetType() == typeof(System.Collections.ArrayList))
            {
                System.Collections.ArrayList arrayList = (System.Collections.ArrayList)vs;
                Console.WriteLine("[{0}]: {1}{2}ArrayList ({3})", level.ToString(), Spaces(level), prefix, arrayList.Count);
                ParseViewStateIEnumerable((System.Collections.IEnumerable)vs, level);
            }
            else if (vs.GetType().IsArray)
            {
                Array array = (Array)vs;
                Console.WriteLine("[{0}]: {1}{2}Array ({3})", level.ToString(), Spaces(level), prefix, array.Length);
                ParseViewStateIEnumerable((System.Collections.IEnumerable)vs, level);
            }
            else if (vs.GetType() == typeof(System.String))
            {
                Console.WriteLine("[{0}]: {1}{2}'{3}'", level.ToString(), Spaces(level), prefix, vs.ToString());
            }
            else if (vs.GetType().IsPrimitive)
            {
                Console.WriteLine("[{0}]: {1}{2}{3} (primitive)", level.ToString(), Spaces(level), prefix, vs.ToString());
            }
            else
            {
                Console.WriteLine("[{0}]: {1}{2}{3} (else)", level.ToString(), Spaces(level), prefix, vs.GetType().ToString());
            }
        }
        private void ParseViewState(System.Web.UI.Triplet vs, int level)
        {
            ParseViewState(vs.First, level + 1);
            ParseViewState(vs.Second, level + 1);
            ParseViewState(vs.Third, level + 1);
        }
        private void ParseViewState(System.Web.UI.Pair vs, int level)
        {
            ParseViewState(vs.First, level + 1);
            ParseViewState(vs.Second, level + 1);
        }
        private void ParseViewState(System.Collections.IDictionary vs, int level)
        {
            foreach (System.Collections.DictionaryEntry entry in vs)
            {
                Console.WriteLine("[{0}]: {1}", level.ToString(), Spaces(level) + "'" + entry.Key.ToString() + "'");
                ParseViewState(entry.Value, level + 1);
            }
        }
        private void ParseViewState(System.Collections.IEnumerable vs, int level)
        {
            foreach (object item in vs)
            {
                ParseViewState(item, level + 1);
            }
        }
        private void ParseViewStateIEnumerable(System.Collections.IEnumerable vs, int level)
        {
            int count = 0;
            foreach (object item in vs)
            {
                //Console.Write(Spaces(level) + "[{0}]: ", count);
                string prefix = string.Format("[{0}]: ", count);
                ParseViewState(item, level + 1, prefix);
                count++;
            }
        }
        private string Spaces(int count)
        {
            string spaces = "";
            for (int index = 0; index < count; index++)
            {
                spaces += "   ";
            }
            return spaces;
        }
    }

    class ParseViewStateTraceClass
    {
        public void ParseViewState(object vs, int level)
        {
            if (vs == null)
            {
                Trace.TraceWarning("[{0}]: {1}", level.ToString(), Spaces(level) + "null");
            }
            else if (vs.GetType() == typeof(System.Web.UI.Triplet))
            {
                Trace.TraceWarning("[{0}]: {1}", level.ToString(), Spaces(level) + "Triplet");
                ParseViewState((System.Web.UI.Triplet)vs, level);
            }
            else if (vs.GetType() == typeof(System.Web.UI.Pair))
            {
                Trace.TraceWarning("[{0}]: {1}", level.ToString(), Spaces(level) + "Pair");
                ParseViewState((System.Web.UI.Pair)vs, level);
            }
            else if (vs.GetType() == typeof(System.Collections.ArrayList))
            {
                Trace.TraceWarning("[{0}]: {1}", level.ToString(), Spaces(level) + "ArrayList");
                ParseViewState((System.Collections.IEnumerable)vs, level);
            }
            else if (vs.GetType().IsArray)
            {
                Trace.TraceWarning("[{0}]: {1}", level.ToString(), Spaces(level) + "Array");
                ParseViewState((System.Collections.IEnumerable)vs, level);
            }
            else if (vs.GetType() == typeof(System.String))
            {
                Trace.TraceWarning("[{0}]: {1}", level.ToString(), Spaces(level) + "'" + vs.ToString() + "'");
            }
            else if (vs.GetType().IsPrimitive)
            {
                Trace.TraceWarning("[{0}]: {1}", level.ToString(), Spaces(level) + vs.ToString());
            }
            else
            {
                Trace.TraceWarning("[{0}]: {1}", level.ToString(), Spaces(level) + vs.GetType().ToString());
            }
        }
        private void ParseViewState(System.Web.UI.Triplet vs, int level)
        {
            ParseViewState(vs.First, level + 1);
            ParseViewState(vs.Second, level + 1);
            ParseViewState(vs.Third, level + 1);
        }
        private void ParseViewState(System.Web.UI.Pair vs, int level)
        {
            ParseViewState(vs.First, level + 1);
            ParseViewState(vs.Second, level + 1);
        }
        private void ParseViewState(System.Collections.IDictionary vs, int level)
        {
            foreach (System.Collections.DictionaryEntry entry in vs)
            {
                Trace.TraceWarning("[{0}]: {1}", level.ToString(), Spaces(level) + "'" + entry.Key.ToString() + "'");
                ParseViewState(entry.Value, level + 1);
            }
        }
        private void ParseViewState(System.Collections.IEnumerable vs, int level)
        {
            foreach (object item in vs)
            {
                ParseViewState(item, level + 1);
            }
        }
        private string Spaces(int count)
        {
            string spaces = "";
            for (int index = 0; index < count; index++)
            {
                spaces += "   ";
            }
            return spaces;
        }
    }
}