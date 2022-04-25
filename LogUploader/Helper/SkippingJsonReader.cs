using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Helper
{
    public class SkippingJsonReader : Newtonsoft.Json.JsonTextReader
    {
        private readonly ISet<string> tokensToSkip;

        public SkippingJsonReader(string json, ISet<string> tokensToSkip)
            : this(new StringReader(json), tokensToSkip)
        { }

        public SkippingJsonReader(TextReader reader, ISet<string> tokensToSkip)
            : base(reader)
        {
            this.tokensToSkip = tokensToSkip;
        }

        public override bool Read()
        {
            var res = base.Read();
            if (!res)
            {
                return res;
            }

            if (TokenType == Newtonsoft.Json.JsonToken.PropertyName)
            {
                var propName = Value as string;

                if (tokensToSkip.Contains(propName))
                {
                    return SkipProperty();
                }
            }

            return res;

        }

        private bool SkipProperty()
        {
            if (!base.Read())
            {
                return false;
            }
             
            switch (TokenType)
            {
                case Newtonsoft.Json.JsonToken.None:
                case Newtonsoft.Json.JsonToken.Comment:
                    return SkipProperty();
                case Newtonsoft.Json.JsonToken.StartObject:
                    return SkipObject();
                case Newtonsoft.Json.JsonToken.StartArray:
                    return SkipArray();
                case Newtonsoft.Json.JsonToken.StartConstructor:
                    return SkipConstructor();
                case Newtonsoft.Json.JsonToken.EndObject:
                case Newtonsoft.Json.JsonToken.EndArray:
                case Newtonsoft.Json.JsonToken.EndConstructor:
                case Newtonsoft.Json.JsonToken.PropertyName:
                    return true;
                case Newtonsoft.Json.JsonToken.Undefined:
                case Newtonsoft.Json.JsonToken.Date:
                case Newtonsoft.Json.JsonToken.Bytes:
                case Newtonsoft.Json.JsonToken.Raw:
                case Newtonsoft.Json.JsonToken.Integer:
                case Newtonsoft.Json.JsonToken.Float:
                case Newtonsoft.Json.JsonToken.String:
                case Newtonsoft.Json.JsonToken.Boolean:
                case Newtonsoft.Json.JsonToken.Null:
                    return SkipValue();
                default:
                    return true;
            }
        }

        private bool SkipConstructor()
        {
            int objectCounter = 1;

            while (base.Read() && objectCounter > 0)
            {
                switch (TokenType)
                {
                    case Newtonsoft.Json.JsonToken.StartConstructor:
                        objectCounter++;
                        break;
                    case Newtonsoft.Json.JsonToken.EndConstructor:
                        objectCounter--;
                        break;
                    default:
                        break;
                }
            }

            return objectCounter == 0;
        }

        private bool SkipArray()
        {
            int objectCounter = 1;

            while (base.Read() && objectCounter > 0)
            {
                switch (TokenType)
                {
                    case Newtonsoft.Json.JsonToken.StartArray:
                        objectCounter++;
                        break;
                    case Newtonsoft.Json.JsonToken.EndArray:
                        objectCounter--;
                        break;
                    default:
                        break;
                }
            }

            return objectCounter == 0;
        }

        private bool SkipObject()
        {
            int objectCounter = 1;

            while (base.Read() && objectCounter > 0)
            {
                switch (TokenType)
                {
                    case Newtonsoft.Json.JsonToken.StartObject:
                        objectCounter++;
                        break;
                    case Newtonsoft.Json.JsonToken.EndObject:
                        objectCounter--;
                        break;
                    default:
                        break;
                }
            }

            return objectCounter == 0;
        }

        private bool SkipValue()
        {
            return base.Read();
        }
    }
}
