using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebInvoicer.Core.Repositories.Data;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Services
{
    public class DataService<TCreateDto, TUpdateDto, TModel> : IDataService<TCreateDto, TUpdateDto>
    {
        private readonly IDataRepository<TModel> repository;

        private readonly IMapper mapper;

        public DataService(IDataRepository<TModel> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ResultHandler> Create(TCreateDto data, string email)
        {
            var result = ResultHandler
                .HandleTaskResult(await repository.Create(mapper.Map<TModel>(data), email));
            result.MapPayload<TModel, TUpdateDto>(mapper);
            return result;
        }

        public async Task<ResultHandler> Get(int resourceId, string email)
        {
            var result = ResultHandler.HandleTaskResult(await repository.Get(resourceId, email));
            result.MapPayload<TModel, TUpdateDto>(mapper);
            return result;
        }

        public async Task<ResultHandler> GetAll(string email)
        {
            var result = ResultHandler.HandleTaskResult(await repository.GetAll(email));
            result.MapPayload<TModel, TUpdateDto>(mapper);
            return result;
        }

        public async Task<ResultHandler> Update(TUpdateDto data, string email)
        {
            var result = ResultHandler
                .HandleTaskResult(await repository.Update(mapper.Map<TModel>(data), email));
            result.MapPayload<TModel, TUpdateDto>(mapper);
            return result;
        }

        public async Task<ResultHandler> Delete(int resourceId, string email)
        {
            try
            {
                return ResultHandler.HandleTaskResult(await repository.Delete(resourceId, email));
            }
            catch (DbUpdateException)
            {
                return new ResultHandler(HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
