using Domain.Entities;
using System;
namespace Domain.Framework.Dto
{
    public class CategoryCountDto
    {
        public Category Category { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }
    }
}