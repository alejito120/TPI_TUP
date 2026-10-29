using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Enums;

namespace HardwareTech.Application.Services
{
    public class RepairService : IRepairService
    {
        private readonly IRepositoryGeneric<Repair> _repairRepository;
        private readonly IRepositoryGeneric<User> _userRepository;
        private readonly IRepositoryGeneric<EquipmentHardware> _equipmentRepository;

        public RepairService(
            IRepositoryGeneric<Repair> repairRepository,
            IRepositoryGeneric<User> userRepository,
            IRepositoryGeneric<EquipmentHardware> equipmentRepository)
        {
            _repairRepository = repairRepository;
            _userRepository = userRepository;
            _equipmentRepository = equipmentRepository;
        }

        public Task<RepairResponse> CreateAsync(RepairRequest request) //Necesita el repositorio generico <repair>
        {
            throw new NotImplementedException();
        }

        public Task<RepairResponse?> GetByIdAsync(int id) //Necesita el repositorio generico <repair>
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RepairResponse>> GetAllAsync() //Necesita el repositorio generico <repair>
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RepairResponse>> GetByStateAsync( //Necesita el repositorio generico <repair>
            RepairStatus state)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RepairResponse>> GetByTechnicianAsync( //Necesita el repositorio generico <user>
            int technicianId)
        {
            throw new NotImplementedException();
        }

        public Task AssignTechnicianAsync( //Necesita el repositorio generico <repair> <user> 
            int repairId,
            int technicianId)
        {
            throw new NotImplementedException();
        }

        public Task ChangeStateAsync( //Necesita el repositorio generico <repair>
            int repairId,
            RepairStatus state)
        {
            throw new NotImplementedException();
        }

        public Task CompleteRepairAsync( //Necesita el repositorio generico <repair> y posteriormente <historial> y <notificaciones>
            int repairId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync( //Necesita el repositorio generico <repair>
            int repairId)
        {
            throw new NotImplementedException();
        }
    }
}