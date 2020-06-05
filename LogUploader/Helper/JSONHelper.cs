using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LogUploader.JSONHelper
{
    public class JSONHelper
    {
        /// <summary>
        /// Stores the string for desirealize
        /// </summary>
        private string str;

        public JSONObject Desirealize(string s)
        {
            str = s;
            var root = new JSONObject();
            ParseNext(root, 0);
            str = "";
            return root;
        }

        private int ParseNext(JSONObject parent, int i)
        {
            //LogProgress(i);
            var start = i;
            i += IgnoreWhiteSpace(i);
            if (str[i] != '{')
                throw new JSONParsException("Invalid JSON string at Ch " + i + "! Expected '{' Got: " + str[i]);
            i++;
            i += IgnoreWhiteSpace(i);
            while (str[i] != '}')
            {
                var type = DetirmenType(i);
                Tuple<string, int> res;
                string name = "<Default>";
                switch (type)
                {
                    case JSONType.STRING:
                        res = FindString(i);
                        name = res.Item1;
                        i += res.Item2;
                        if (str[i++] != ':')
                            throw new JSONParsException("Invalid JSON at Ch " + i + " Got: " + str[i - 1]);
                        i += IgnoreWhiteSpace(i);
                        type = DetirmenType(i);
                        break;
                    case JSONType.OBJECT_END:
                        break;
                    default:
                        throw new JSONParsException("Invalid JSON at Ch " + i + " Expected '\"' or '}' Got: " + str[i - 1]);

                }
                switch (type)
                {
                    case JSONType.STRING:
                        res = FindString(i);
                        i += res.Item2;
                        parent.Values.Add(name, res.Item1);
                        break;
                    case JSONType.TRUE:
                    case JSONType.FALSE:
                    case JSONType.NULL:
                        var resObj = FindSpecificString(i, type);
                        i += resObj.Item2;
                        parent.Values.Add(name, resObj.Item1);
                        break;
                    case JSONType.NUMBER:
                        var resNum = FindNumber(i);
                        i += resNum.Item2;
                        parent.Values.Add(name, resNum.Item1);
                        break;
                    case JSONType.LIST_START:
                        var tempRes = FindList(i);
                        i += tempRes.Item2;
                        parent.Values.Add(name, tempRes.Item1);
                        break;
                    case JSONType.OBJECT_START:
                        var child = new JSONObject();
                        parent.Values.Add(name, child);
                        i += ParseNext(child, i);
                        break;
                    case JSONType.OBJECT_END:
                        break;
                }
                if (str[i] != ',' && str[i] != '}')
                    throw new JSONParsException("Invalid JSON at Ch " + i + "! Expected ',' or '}' Got: " + str[i]);
                if (str[i] != '}')
                    i++;
                i += IgnoreWhiteSpace(i);
            }

            i++;
            i += IgnoreWhiteSpace(i);
            return i - start;
        }


        public int IgnoreWhiteSpace(int i)
        {
            var start = i;
            while (i < str.Length && char.IsWhiteSpace(str[i]))
            {
                i++;
            }
            return i - start;
        }


        public Tuple<string, int> FindString(int i) //Missing support for '\"'
        {
            string res = "";
            var start = i;
            bool escaped = false;
            if (str[i] != '"')
                throw new JSONParsException("Invalid JSON string at Ch " + i + "! Expected '\"' Got: " + str[i]);

            i++;
            while (str[i] != '"' || escaped)
            {
                res += str[i];
                if (!escaped && str[i] == '\\')
                    escaped = true;
                else
                    escaped = false;

                i++;
            }

            i++;
            i += IgnoreWhiteSpace(i);
            res = UnescapeString(res);

            return new Tuple<string, int>(res, i - start);
        }


        public Tuple<double, int> FindNumber(int i)
        {
            var num = "";
            var start = i;
            while ("0123456789Ee+-. \n\r\t".Contains(str[i] + ""))
            {
                if ("0123456789Ee+-.".Contains(str[i] + ""))
                    num += str[i];
                i++;
            }
            if (!double.TryParse(num, NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
                throw new JSONParsException("Invalid JSON at Ch " + start + "! Expected a number Got: " + str[i]);
            return new Tuple<double, int>(res, i - start);
        }

        Tuple<object, int> FindSpecificString(int i, JSONType type)
        {
            string s;
            object res;
            switch (type)
            {
                case JSONType.TRUE:
                    s = "true";
                    res = true;
                    break;
                case JSONType.FALSE:
                    s = "false";
                    res = false;
                    break;
                case JSONType.NULL:
                    s = "null";
                    res = null;
                    break;
                default:
                    throw new JSONParsException("Invalid type " + type.ToString() + "! Only TRUE, FALSE and NULL are allowed Got: " + str[i]);
            }


            var start = i;
            if (str.Substring(i, s.Length).ToLowerInvariant() != s.ToLowerInvariant())
                throw new JSONParsException("Invalid JSON at Ch " + i + "! Expected True/False/Null Got: " + str.Substring(i, 5) + "...");
            i += s.Length;
            i += IgnoreWhiteSpace(i);
            return new Tuple<object, int>(res, i - start);
        }


        Tuple<List<object>, int> FindList(int i)
        {
            var start = i;

            if (str[i] != '[')
                throw new JSONParsException("Invalid JSON string at Ch " + i + "! Expected '[' Got: " + str[i]);

            var res = new List<object>();
            Tuple<string, int> tempRes;

            i++;
            i += IgnoreWhiteSpace(i);

            while (str[i] != ']')
            {
                var type = DetirmenType(i);
                switch (type)
                {
                    case JSONType.STRING:
                        tempRes = FindString(i);
                        i += tempRes.Item2;
                        res.Add(tempRes.Item1);
                        break;
                    case JSONType.TRUE:
                    case JSONType.FALSE:
                    case JSONType.NULL:
                        var tempResObj = FindSpecificString(i, type);
                        i += tempResObj.Item2;
                        res.Add(tempResObj.Item1);
                        break;
                    case JSONType.NUMBER:
                        var tempResNum = FindNumber(i);
                        i += tempResNum.Item2;
                        res.Add(tempResNum.Item1);
                        break;
                    case JSONType.OBJECT_START:
                        var tempObj = new JSONObject();
                        i += ParseNext(tempObj, i);
                        res.Add(tempObj);
                        break;
                    case JSONType.LIST_START:
                        var tempList = FindList(i);
                        i += tempList.Item2;
                        res.Add(tempList.Item1);
                        break;
                    case JSONType.LIST_END:
                        break;
                    default:
                        throw new JSONParsException("Invalid JSON! Cannot pars list at Ch " + i + " Got: " + str[i]);
                }
                if (str[i] != ',' && str[i] != ']')
                    throw new JSONParsException("Invalid JSON at Ch " + i + "! Expected ',' Got: " + str[i]);
                if (str[i] != ']')
                    i++;
                i += IgnoreWhiteSpace(i);
            }

            i++;
            i += IgnoreWhiteSpace(i);
            return new Tuple<List<object>, int>(res, i - start);
        }

        JSONType DetirmenType(int i)
        {
            switch (str[i])
            {
                case '"':
                    return JSONType.STRING;
                case '[':
                    return JSONType.LIST_START;
                case '{':
                    return JSONType.OBJECT_START;
                case ']':
                    return JSONType.LIST_END;
                case '}':
                    return JSONType.OBJECT_END;
                case 't':
                case 'T':
                    return JSONType.TRUE;
                case 'f':
                case 'F':
                    return JSONType.FALSE;
                case 'n':
                case 'N':
                    return JSONType.NULL;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '.':
                case '-':
                case '+':
                    return JSONType.NUMBER;
                default:
                    throw new JSONParsException("Invalid JSON at Ch " + i + "! Expectet start of item! Got: " + str[i]);
            }
        }
        
        private bool CheckNull(int i) => CheckString(i, "null");

        private bool CheckTrue(int i) => CheckString(i, "true");

        private bool CheckFalse(int i) => CheckString(i, "false");

        private bool CheckString(int i, string s) => str.Substring(i, s.Length).ToLowerInvariant() == s.ToLowerInvariant();

        private int lastPercent = -1;

        private void LogProgress(int i)
        {
            var currentprogress = (int) Math.Floor((double) i / str.Length * 100);
            if (currentprogress > lastPercent + 4)
            {
                Console.WriteLine(currentprogress + "%");
                lastPercent = currentprogress;
            }
        }

        private static string UnescapeString(string s)
        {
            string res = "";

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\\')
                {
                    if (i + 1 < s.Length)
                        i++;
                    else
                    {
                        res += s[i];
                        continue;
                    }
                    switch (s[i])
                    {
                        case '\\':
                            res += '\\';
                            break;
                        case '/':
                            res += '/';
                            break;
                        case '\"':
                            res += '\"';
                            break;
                        case 'n':
                            res += '\n';
                            break;
                        case 'r':
                            res += '\r';
                            break;
                        case 'f':
                            res += '\f';
                            break;
                        case 't':
                            res += '\t';
                            break;
                        case 'b':
                            res += '\b';
                            break;
                        case 'u':
                            res += (char)short.Parse(s.Substring(i + 1, 4), NumberStyles.HexNumber);
                            i += 3;
                            break;
                        default:
                            res += s[i];
                            break;
                    }
                }
                else
                {
                    res += s[i];
                }
            }

            return res;
        }

        public string Serialize(JSONObject root)
        {
            return root.ToString();
        }
    }

    internal enum JSONType
    {
        STRING,
        LIST_START,
        LIST_END,
        OBJECT_START,
        OBJECT_END,
        TRUE,
        FALSE,
        NULL,
        NUMBER
    }

    public class JSONObject
    {
        public Dictionary<string, object> Values { get; } = new Dictionary<string, object>();

        public object GetElement(string path)
        {
            var processedPath = SplitPath(path);
            return NextJPath(processedPath);
        }

        public List<object> GetListElement(string path) => (List<object>) GetElement(path);

        [Obsolete("Use Generic implementation")]
        public JSONObject GetJSONElement(string path) => (JSONObject) GetElement(path);

        public T GetTypedElement<T>(string path)
        {
            try
            {
                return (T)GetElement(path);
            }
            catch (InvalidCastException)
            {
                return default;
            }
        }

        public List<T> GetTypedList<T>(string path)
        {
            try
            {
                return (GetElement(path) as List<object>).Cast<T>().ToList();
            }
            catch (InvalidCastException)
            {
                return new List<T>();
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        private object NextJPath(List<string> path)
        {
            var current = path[0];
            path.RemoveAt(0);
            if (path.Count > 0)
            {
                var res = AnalyzeName(current);
                if (res.Item2 > -1)
                    return (GetInnerListItem(current) as JSONObject).NextJPath(path);
                else if (Values.ContainsKey(current))
                    return (Values[current] as JSONObject).NextJPath(path);
                throw new JSONPathException("Cannot find path .../" + current + "/...");
            }
            else
            {
                var res = AnalyzeName(current);
                if (res.Item2 > -1 && Values.ContainsKey(res.Item1))
                    return GetInnerListItem(current);
                else if (Values.ContainsKey(current))
                    return Values[current];
                throw new JSONPathException("Cannot finde object .../" + current);
            }
        }

        private object GetInnerListItem(string name)
        {
            var res = AnalyzeName(name);
            if (res.Item2 > -1)
                return (GetInnerListItem(res.Item1) as List<object>)[res.Item2];
            else if (Values.ContainsKey(name))
                return Values[name];
            throw new JSONPathException("Cannot finde list .../" + name);
        }

        private static Tuple<string, int> AnalyzeName(string s)
        {
            var start = s.LastIndexOf('[');
            if (start == -1)
                return new Tuple<string, int>(s, -1);

            var name = s.Substring(0, start);
            var strIndex = s.Substring(start);
            if (strIndex[0] == '[' && strIndex.LastOrDefault() == ']' && strIndex.Length >= 3
                && int.TryParse(strIndex.Substring(1, strIndex.Length - 2), out int res))
            {
                if (res >= 0)
                    return new Tuple<string, int>(name, res);
                else
                    throw new JSONPathException("Array index out of bounds.");
            }
            return new Tuple<string, int>(s, -1);
        }

        public static List<string> SplitPath(string path)
        {
            var basic = path.Split('/').ToList();
            List<string> res = new List<string>();
            for (int i = 0; i < basic.Count; i++)
            {
                res.Add(GetNextPart(basic, i));
                i--;
            }
            return res;
        }

        private static string GetNextPart(List<string> basic, int i)
        {
            var count = 0;
            for (int j = basic[i].Length - 1; j >= 0; j--)
            {
                if (basic[i][j] == '\\')
                    count++;
                else
                    break;
            }

            string temp;

            if (count % 2 == 1 && basic.Count > i + 1)
                temp = basic[i].Substring(0, basic[i].Length - 1).Replace("\\\\", "\\") + "/" + GetNextPart(basic, i + 1);
            else
                temp = basic[i].Replace("\\\\", "\\");

            basic.RemoveAt(i);
            return temp;
        }

        public override string ToString()
        {
            var res = "{";
            foreach (var element in Values)
            {
                res += $"\"{element.Key}\":";
                res += ElementToString(element.Value);
            }

            res = res.TrimEnd(',');
            res += "}";
            return res;
        }

        private string ListToString(List<object> list)
        {
            var res = "[";
            foreach (var element in list)
            {
                res += ElementToString(element);
            }

            res = res.TrimEnd(',');
            res += "]";
            return res;
        }

        private string ElementToString(object element)
        {
            if (element == null)
                return $"null,";
            else if (element is string)
                return $"\"{EscapeString((string)element)}\",";
            else if (element is int)
                return $"{((int)element).ToString(CultureInfo.InvariantCulture)},";
            else if (element is double)
                return $"{((double)element).ToString(CultureInfo.InvariantCulture)},";
            else if (element is float)
                return $"{((float)element).ToString(CultureInfo.InvariantCulture)},";
            else if (element is bool)
                return $"{((bool)element ? "true" : "false")},";
            else if (element is JSONObject)
                return $"{((JSONObject)element).ToString()},";
            else if (element is List<object>)
                return $"{ListToString((List<object>)element)},";
            else
                return $"null,";
        }

        private static string EscapeString(string s)
        {
            s = s.Replace("\\", "\\\\");
            s = s.Replace("\"", "\\\"");
            s = s.Replace("\n", "\\n");
            s = s.Replace("\r", "\\r");
            s = s.Replace("\f", "\\f");
            s = s.Replace("\t", "\\t");
            s = s.Replace("\b", "\\b");
            s = s.Replace("/", "\\/");
            s = s.Replace("ä", "\\u00E4");
            s = s.Replace("ö", "\\u00F6");
            s = s.Replace("ü", "\\u00FC");
            s = s.Replace("Ä", "\\u00C4");
            s = s.Replace("Ö", "\\u00D6");
            s = s.Replace("Ü", "\\u00DC");
            return s;
        }
    }


    public class JSONException : Exception
    {
        public JSONException() : base()
        { }
        public JSONException(string message) : base(message)
        { }
        public JSONException(string message, Exception innerException) : base(message, innerException)
        { }
    }

    public class JSONParsException : JSONException
    {
        public JSONParsException() : base()
        { }
        public JSONParsException(string message) : base(message)
        { }
        public JSONParsException(string message, Exception innerException) : base(message, innerException)
        { }
    }

    public class JSONPathException : JSONException
    {
        public JSONPathException() : base()
        { }
        public JSONPathException(string message) : base(message)
        { }
        public JSONPathException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
//var a = new JSONHelper();
//var result = a.Desirealize("{\n\"root\":\"this is the root\",\n\"i'mNumber\" : -2.56e 2,\n\"FALSE\" : tRue,\n\"ReallyTrue\" : FALSE,\n\"notHere\": null,\n\"Listings here\": [\"A\", 1, true, null],\n\"IFeelEmpty\":[],\n\"Inceptione\": {\n	\"child\":\"this is the child\",\n	\"i'mNumber\" : +128e - 2,\n	\"FALSE\" : true,\n	\"ReallyTrue\" : fAlSE,\n	\"notHere\": NuLl,\n	\"Listings here\": [\"B\", 2, falsE, nuLl],\n	\"IFeelEmptyToo\":[]\n	}");
/*
{ "root":"this is the root", "i'mNumber" : -2.56e 2, "FALSE" : tRue, "ReallyTrue" : FALSE, "notHere": null, "Listings here": ["A", 1, true, null], "IFeelEmpty":[], "Inceptione": { 	"child":"this is the child", 	"i'mNumber" : +128e - 2, 	"FALSE" : true, 	"ReallyTrue" : fAlSE, 	"notHere": NuLl, 	"Listings here": ["B", 2, falsE, nuLl], 	"IFeelEmptyToo":[] 	}
*/
