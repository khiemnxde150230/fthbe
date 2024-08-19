﻿using backend.Repositories.StaffRepository;

namespace backend.Services.StaffService
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;

        public StaffService (IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        public object CheckInTicket(int ticketId, int staffId)
        {
            return _staffRepository.CheckInTicket (ticketId, staffId);
        }
        public async Task<object> GetCheckinHistoryByEvent(int staffId)
        {
            return await _staffRepository.GetCheckinHistoryByEvent (staffId);
        }
        public async Task<object> GetEventByStaff(int staffId)
        {
            return _staffRepository.GetEventByStaff (staffId);
        }
    }
}
