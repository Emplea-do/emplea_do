using System;
using System.Linq;
using AppService.Framework;
using Data.Repositories;
using Domain;
using Domain.Framework;

namespace AppService.Services
{
    public class HireTypesService :  BaseService<HireType>, IHireTypesService
    {
        private readonly IHireTypeRepository _hireTypesRepository;

        public HireTypesService(IHireTypeRepository hireTypesRepository)
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
    }

    public interface IHireTypesService
    {
        PagingResult<HireType> GetByPagination(PaginationFilter paginationFilter);

        HireType GetById(int id);
    }
}
