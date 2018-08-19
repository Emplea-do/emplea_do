using System;
using System.Collections.Generic;
using System.Linq;
using AppService.Framework;
using Data.Repositories;
using Domain;
using Domain.Framework;

namespace AppService.Services
{
    public class HireTypeService :  BaseService, IHireTypeService
    {
        private readonly IHireTypeRepository _hireTypesRepository;

        public HireTypeService(IHireTypeRepository hireTypesRepository)
        {
            _hireTypesRepository = hireTypesRepository;
        }

        public HireType GetById(int id)
        {
            return _hireTypesRepository.GetById(id);
        }

        public PagingResult<HireType> GetByPagination(PaginationFilter paginationFilter)
        {
            return new PagingResult<HireType>
            {
                Data = _hireTypesRepository.GetAll(),
                ItemsPerPage = paginationFilter.ItemsPerPage,
                Page = paginationFilter.Page,
                TotalItems = _hireTypesRepository.Count()
            };
        }

        public IEnumerable<HireType> GetHireTypes()
        {
            return _hireTypesRepository.GetAll();
        }
    }

    public interface IHireTypeService
    {
        PagingResult<HireType> GetByPagination(PaginationFilter paginationFilter);

        HireType GetById(int id);

        IEnumerable<HireType> GetHireTypes();
    }
}
