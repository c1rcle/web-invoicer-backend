using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Repositories.Data
{
    public class ProductRepository : CancellableOnRequestAbort, IDataRepository<Product>
    {
        private readonly DatabaseContext context;

        public ProductRepository(DatabaseContext context, IHttpContextAccessor contextAccessor)
            : base(contextAccessor) => this.context = context;
        public async Task<TaskResult> Create(Product data, string email)
        {
            var user = await context.Users
                .SingleOrDefaultAsync(x => x.Email == email, GetCancellationToken());

            data.UserId = user.Id;
            context.Products.Add(data);
            return await context.SaveContextChanges(GetCancellationToken());
        }

        public async Task<TaskResult<IEnumerable<Product>>> GetAll(string email)
        {
            return new TaskResult<IEnumerable<Product>>(await context.Products
                .Where(x => x.User.Email == email)
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync(GetCancellationToken()));
        }

        public async Task<TaskResult> Update(Product data, string email)
        {
            var record = await context.Products.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.ProductId == data.ProductId,
                    GetCancellationToken());

            if (record == null || record.User.Email != email)
            {
                return new TaskResult(TaskErrorType.NotFound);
            }

            record.Name = data.Name ?? record.Name;
            record.Description = data.Description ?? record.Description;
            record.NetPrice = data.NetPrice ?? record.NetPrice;
            record.GrossPrice = data.GrossPrice ?? record.GrossPrice;
            record.VatRate = data.VatRate ?? record.VatRate;

            return await context.SaveContextChanges(GetCancellationToken());
        }

        public async Task<TaskResult> Delete(int resourceId, string email)
        {
            var record = await context.Products.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.ProductId == resourceId,
                    GetCancellationToken());

            if (record == null || record.User.Email != email)
            {
                return new TaskResult(TaskErrorType.NotFound);
            }

            context.Products.Remove(record);
            return await context.SaveContextChanges(GetCancellationToken());
        }
    }
}
