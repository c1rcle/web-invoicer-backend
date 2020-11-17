using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Repositories.Data
{
    public class InvoiceItemRepository : CancellableOnRequestAbort, IInvoiceItemRepository
    {
        private readonly DatabaseContext context;

        public InvoiceItemRepository(DatabaseContext context, IHttpContextAccessor contextAccessor)
            : base(contextAccessor) => this.context = context;

        public async Task<TaskResult<InvoiceItem>> Create(InvoiceItem data)
        {
            context.InvoiceItems.Add(data);
            return await context.SaveContextChanges(GetCancellationToken(), data);
        }

        public async Task<TaskResult<IEnumerable<InvoiceItem>>> GetAll(int invoiceId)
        {
            return new TaskResult<IEnumerable<InvoiceItem>>(await context.InvoiceItems
                .Where(x => x.InvoiceId == invoiceId)
                .OrderBy(x => x.Index)
                .AsNoTracking()
                .ToListAsync(GetCancellationToken()));
        }

        public async Task<TaskResult> Update(InvoiceItem data)
        {
            var record = await context.InvoiceItems
                .SingleOrDefaultAsync(x => x.InvoiceItemId == data.InvoiceItemId,
                    GetCancellationToken());

            if (record == null)
            {
                return new TaskResult(TaskErrorType.NotFound);
            }

            record.Count = data.Count;
            record.Unit = data.Unit;
            record.ProductId = data.ProductId;

            return await context.SaveContextChanges(GetCancellationToken());
        }

        public async Task<TaskResult> Delete(int invoiceItemId)
        {
            var record = await context.InvoiceItems
                .SingleOrDefaultAsync(x => x.InvoiceItemId == invoiceItemId,
                    GetCancellationToken());

            if (record == null)
            {
                return new TaskResult(TaskErrorType.NotFound);
            }

            context.InvoiceItems.Remove(record);
            return await context.SaveContextChanges(GetCancellationToken());
        }
    }
}
