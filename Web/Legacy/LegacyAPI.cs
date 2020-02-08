using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LegacyAPI
{   
    public class JobCardDTO
	{
		public string Link{ get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
		public string JobType{ get; set; }
		public string Location{ get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsRemote{ get; set; }
        public int ViewCount { get; set; }
        public int Likes { get; set; }
        public string CompanyLogoUrl { get; set; }
        public string Description { get; set; }
        public string HowToApply { get; set; }
        public string Email { get; set; }
    }
}
