using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace QuickCurrency
{
    public class ApiResponse
    {
        public string Result { get; set; }
        public string Documentation { get; set; }
        public string Terms_Of_Use { get; set; }
        public long Time_Last_Update_Unix { get; set; }
        public string Time_Last_Update_Utc { get; set; }
        public long Time_Next_Update_Unix { get; set; }
        public string Time_Next_Update_Utc { get; set; }
        public string Base_Code { get; set; }
        public Dictionary<string, decimal> Conversion_Rates { get; set; }
    }
    class RequestModel
    {
    }
}
