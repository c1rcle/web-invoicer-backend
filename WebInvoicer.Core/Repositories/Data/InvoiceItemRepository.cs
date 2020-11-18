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

        public async Task<TaskResult> Create(InvoiceItem data, string email)
        {
            var invoice = await context.Invoices.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.InvoiceId == data.InvoiceId, GetCancellationToken());

            if (invoice == null || invoice.User.Email != email)
            {
                return new TaskResult(TaskErrorType.Unauthorized);
            }

            context.InvoiceItems.Add(data);
            return await context.SaveContextChanges(GetCancellationToken());
        }

        public async Task<TaskResult<IEnumerable<InvoiceItem>>> GetAll(int invoiceId, string email)
        {
            return new TaskResult<IEnumerable<InvoiceItem>>(await context.InvoiceItems
                .Where(x => x.InvoiceId == invoiceId && x.Invoice.User.Email == email)
                .OrderBy(x => x.Index)
                .AsNoTracking()
                .ToListAsync(GetCancellationToken()));
        }

        public async Task<TaskResult> Update(InvoiceItem data, string email)
        {
            var record = await context.InvoiceItems.Include(x => x.Invoice.User)
                .SingleOrDefaultAsync(x => x.InvoiceItemId == data.InvoiceItemId,
                    GetCancellationToken());

            if (record == null || record.Invoice.User.Email != email)
            {
                return new TaskResult(TaskErrorType.NotFound);
            }

            record.Count = data.Count ?? record.Count;
            record.Unit = data.Unit ?? record.Unit;
            record.ProductId = data.ProductId ?? record.ProductId;

            return await context.SaveContextChanges(GetCancellationToken());
        }

        public async Task<TaskResult> Delete(int invoiceItemId, string email)
        {
            var record = await context.InvoiceItems.Include(x => x.Invoice.User)
                .SingleOrDefaultAsync(x => x.InvoiceItemId == invoiceItemId,
                    GetCancellationToken());

            if (record == null || record.Invoice.User.Email != email)
            {
                return new TaskResult(TaskErrorType.NotFound);
            }

            context.InvoiceItems.Remove(record);
            return await context.SaveContextChanges(GetCancellationToken());
        }
    }
}
