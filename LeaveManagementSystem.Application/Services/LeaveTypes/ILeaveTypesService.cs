﻿using LeaveManagementSystem.Application.Models.LeaveTypes;

namespace LeaveManagementSystem.Application.Services.LeaveTypes
{
    public interface ILeaveTypesService
    {
        Task Create(LeaveTypeCreateVM model);
        Task Edit(LeaveTypeEditVM model);
        Task<T?> Get<T>(int id) where T : class;
        Task<List<LeaveTypeReadOnlyVM>> GetAll();
        Task Delete(int id);
        bool LeaveTypeExists(int id);
        Task<bool> CheckLeaveTypeNameExists(string name);
        Task<bool> CheckLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeEdit);
        Task<bool> DaysExceedMaximum(int leaveTypeId, int days);

    }
}
