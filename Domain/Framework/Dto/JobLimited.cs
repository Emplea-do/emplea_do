using System;
namespace Domain.Framework.Dto
{
    public class JobLimited
    {
        #region The important stuff
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string HowToApply { get; set; }
        #endregion

        #region Company Info
        public string CompanyName { get; set; }
        public string CompanyUrl { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyLogoUrl { get; set; }
        #endregion

        #region Location
        public string LocationName { get; set; }
        public double LocationLongitude { get; set; }
        public double LocationLatitude { get; set; }
        #endregion

        #region Metadata and categorization
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int HireTypeId { get; set; }
        public string HireTypeName { get; set; }
        public int ViewCount { get; set; }
        public bool IsRemote { get; set; }
        public DateTime PublishedDateRaw { get; set; }
        public string PublishedDate => PublishedDateRaw.ToShortDateString();
        #endregion
    }
}
