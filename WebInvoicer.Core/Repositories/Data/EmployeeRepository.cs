using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Repositories.Data
{
    public class EmployeeRepository : CancellableOnRequestAbort, IDataRepository<Employee>
    {
        private readonly DatabaseContext context;

        public EmployeeRepository(DatabaseContext context, IHttpContextAccessor contextAccessor)
            : base(contextAccessor) => this.context = context;

        public async Task<TaskResult<Employee>> Create(Employee data, string email)
        {
            var user = await context.Users
                .SingleOrDefaultAsync(x => x.Email == email, GetCancellationToken());

            data.UserId = user.Id;
            data.DateAdded = DateTime.Now;
            context.Employees.Add(data);

            return await context.SaveContextChanges(GetCancellationToken(), data);
        }

        public async Task<TaskResult<Employee>> Get(int resourceId, string email)
        {
            var record = await context.Employees.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.EmployeeId == resourceId,
                    GetCancellationToken());

            if (record == null || record.User.Email != email)
            {
                return new TaskResult<Employee>(TaskErrorType.NotFound);
            }

            return new TaskResult<Employee>(record);
        }

        public async Task<TaskResult<IEnumerable<Employee>>> GetAll(string email)
        {
            return new TaskResult<IEnumerable<Employee>>(await context.Employees
                .Where(x => x.User.Email == email)
                .OrderBy(x => x.DateAdded)
                .AsNoTracking()
                .ToListAsync(GetCancellationToken()));
        }

        public async Task<TaskResult<Employee>> Update(Employee data, string email)
        {
            var record = await context.Employees.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.EmployeeId == data.EmployeeId,
                    GetCancellationToken());

            if (record == null || record.User.Email != email)
            {
                return new TaskResult<Employee>(TaskErrorType.NotFound);
            }

            record.FullName = data.FullName ?? record.FullName;
            record.PhoneNumber = data.PhoneNumber ?? record.PhoneNumber;

            return await context.SaveContextChanges(GetCancellationToken(), record);
        }

        public async Task<TaskResult> Delete(int resourceId, string email)
        {
            var record = await context.Employees.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.EmployeeId == resourceId,
                    GetCancellationToken());

            if (record == null || record.User.Email != email)
            {
                return new TaskResult(TaskErrorType.NotFound);
            }

            context.Employees.Remove(record);
            return await context.SaveContextChanges(GetCancellationToken());
        }
    }
}
