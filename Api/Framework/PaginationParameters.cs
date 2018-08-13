using System;
namespace Api.Framework
{
    public class PaginationParameters
    {
        public int Start { get; set; } = 0;
        public int Length { get; set; } = 10;
        public string OrderByColumn { get; set; } = String.Empty;
        public string OrderBy { get; set; } = String.Empty;
    }
}
