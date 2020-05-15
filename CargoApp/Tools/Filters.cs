using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoApp.Tools
{
    public class Filters
    {
        public struct Filter
        {
            [JsonProperty("field")]
            public string fileld;
            [JsonProperty("operator")]
            public string operation;
            [JsonProperty("value")]
            public string value;
        }

        private List<Filter> filters = null;

        public string Message { get; } = null;

        public Filters(string json) //: base()
        {

            try
            {
                if (json != null)
                    filters = JsonConvert.DeserializeObject<List<Filter>>(json);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        public string GetWhere(string table = "")
        {
            if (filters == null || filters.Count == 0)
                return "";
            if (table != "") table += ".";
            string where = " WHERE ";
            for(int i = 0; i < filters.Count; i++)
            {
                where += $" {table}{filters[i].fileld} {filters[i].operation} {filters[i].value} ";
                if (i < filters.Count - 1) where += " AND ";
            }
            return where;
        }
    }
}
