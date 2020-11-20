using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Repositories.Data
{
    public class CounterpartyRepository : CancellableOnRequestAbort, IDataRepository<Counterparty>
    {
        private readonly DatabaseContext context;

        public CounterpartyRepository(DatabaseContext context,
            IHttpContextAccessor contextAccessor) : base(contextAccessor) => this.context = context;

        public async Task<TaskResult<Counterparty>> Create(Counterparty data, string email)
        {
            var user = await context.Users
                .SingleOrDefaultAsync(x => x.Email == email, GetCancellationToken());

            data.UserId = user.Id;
            context.Counterparties.Add(data);

            try
            {
                return await context.SaveContextChanges(GetCancellationToken(), data);
            }
            catch (DbUpdateException)
            {
                return new TaskResult<Counterparty>(TaskErrorType.Unprocessable);
            }
        }

        public async Task<TaskResult<Counterparty>> Get(int resourceId, string email)
        {
            var record = await context.Counterparties.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.CounterpartyId == resourceId,
                    GetCancellationToken());

            if (record == null || record.User.Email != email)
            {
                return new TaskResult<Counterparty>(TaskErrorType.NotFound);
            }

            return new TaskResult<Counterparty>(record);
        }

        public async Task<TaskResult<IEnumerable<Counterparty>>> GetAll(string email)
        {
            return new TaskResult<IEnumerable<Counterparty>>(await context.Counterparties
                .Where(x => x.User.Email == email)
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync(GetCancellationToken()));
        }

        public async Task<TaskResult<Counterparty>> Update(Counterparty data, string email)
        {
            var record = await context.Counterparties.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.CounterpartyId == data.CounterpartyId,
                    GetCancellationToken());

            if (record == null || record.User.Email != email)
            {
                return new TaskResult<Counterparty>(TaskErrorType.NotFound);
            }

            record.Name = data.Name ?? record.Name;
            record.Nip = data.Nip ?? record.Nip;
            record.Address = data.Address ?? record.Address;
            record.PostalCode = data.PostalCode ?? record.PostalCode;
            record.City = data.City ?? record.City;
            record.PhoneNumber = data.PhoneNumber ?? record.PhoneNumber;

            return await context.SaveContextChanges(GetCancellationToken(), record);
        }

        public async Task<TaskResult> Delete(int resourceId, string email)
        {
            var record = await context.Counterparties.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.CounterpartyId == resourceId,
                    GetCancellationToken());

            if (record == null || record.User.Email != email)
            {
                return new TaskResult(TaskErrorType.NotFound);
            }

            context.Counterparties.Remove(record);
            return await context.SaveContextChanges(GetCancellationToken());
        }
    }
}
