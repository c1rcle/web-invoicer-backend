using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Repositories.Data
{
    public class InvoiceRepository : CancellableOnRequestAbort, IDataRepository<Invoice>
    {
        private readonly DatabaseContext context;

        private readonly IMapper mapper;

        public InvoiceRepository(DatabaseContext context, IMapper mapper,
            IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TaskResult<Invoice>> Create(Invoice data, string email)
        {
            if (!await ValidForeignIds(data, email))
            {
                return new TaskResult<Invoice>(TaskErrorType.Unauthorized);
            }

            var user = await context.Users
                .SingleOrDefaultAsync(x => x.Email == email, GetCancellationToken());

            data.UserId = user.Id;
            context.Invoices.Add(data);

            return await context.SaveContextChanges(GetCancellationToken(), data);
        }

        public async Task<TaskResult<Invoice>> Get(int resourceId, string email)
        {
            var record = await context.Invoices.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.InvoiceId == resourceId,
                    GetCancellationToken());

            if (record == null || record.User.Email != email)
            {
                return new TaskResult<Invoice>(TaskErrorType.NotFound);
            }

            return new TaskResult<Invoice>(record);
        }

        public async Task<TaskResult<IEnumerable<Invoice>>> GetAll(string email)
        {
            return new TaskResult<IEnumerable<Invoice>>(await context.Invoices
                .Where(x => x.User.Email == email)
                .OrderBy(x => x.Date)
                .AsNoTracking()
                .ToListAsync(GetCancellationToken()));
        }

        public async Task<TaskResult<Invoice>> Update(Invoice data, string email)
        {
            if (!await ValidForeignIds(data, email))
            {
                return new TaskResult<Invoice>(TaskErrorType.Unauthorized);
            }

            var record = await context.Invoices.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.InvoiceId == data.InvoiceId,
                    GetCancellationToken());

            if (record == null || record.User.Email != email)
            {
                return new TaskResult<Invoice>(TaskErrorType.NotFound);
            }

            record.Number = data.Number ?? record.Number;
            record.Date = data.Date ?? record.Date;
            record.EmployeeId = data.EmployeeId ?? record.EmployeeId;
            record.CounterpartyId = data.CounterpartyId ?? record.CounterpartyId;
            record.PaymentType = data.PaymentType ?? record.PaymentType;
            record.PaymentDeadline = data.PaymentDeadline ?? record.PaymentDeadline;
            record.NetTotal = data.NetTotal ?? record.NetTotal;
            record.GrossTotal = data.GrossTotal ?? record.GrossTotal;

            return await context.SaveContextChanges(GetCancellationToken(), record);
        }

        public async Task<TaskResult> Delete(int resourceId, string email)
        {
            var record = await context.Invoices.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.InvoiceId == resourceId,
                    GetCancellationToken());

            if (record == null || record.User.Email != email)
            {
                return new TaskResult(TaskErrorType.NotFound);
            }

            context.Invoices.Remove(record);
            return await context.SaveContextChanges(GetCancellationToken());
        }

        private async Task<bool> ValidForeignIds(Invoice data, string email)
        {
            var employee = await context.Employees.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.EmployeeId == data.EmployeeId,
                    GetCancellationToken());

            var counterparty = await context.Counterparties.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.CounterpartyId == data.CounterpartyId,
                    GetCancellationToken());

            var employeeEmail = employee?.User?.Email ?? email;
            var counterpartyEmail = counterparty?.User?.Email ?? email;

            if (employeeEmail != email || counterpartyEmail != email)
            {
                return false;
            }

            return true;
        }
    }
}
