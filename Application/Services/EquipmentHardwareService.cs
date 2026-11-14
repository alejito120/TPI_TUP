using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Exceptions;
using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Entities;

namespace HardwareTech.Application.Services
{
    public class EquipmentHardwareService(
        IRepositoryGeneric<EquipmentHardware> equipmentRepo,
        IRepositoryGeneric<User> userRepo) : IEquipmentHardwareService
    {
        // Guardamos las referencias al repositorio de equipos y al de usuarios
        private readonly IRepositoryGeneric<EquipmentHardware> _equipmentRepo = equipmentRepo;
        private readonly IRepositoryGeneric<User> _userRepo = userRepo;

        public async Task<EquipmentHardwareResponse> CreateAsync(EquipmentHardwareRequest request)
        {
            // Validamos que el cliente (UserId) exista en la base de datos
            if (!await _userRepo.ExistsAsync(u => u.Id == request.UserId))
                throw new NotFoundException($"No se encontró el cliente con ID {request.UserId}.");

            // Normalizamos el número de serie y validamos que no esté repetido
            var serial = request.SerialNumber.Trim();
            if (await _equipmentRepo.ExistsAsync(e => e.SerialNumber == serial))
                throw new BusinessException($"Ya existe un equipo con el número de serie {serial}.");

            // Creamos el equipo y lo guardamos en la base de datos
            var equipment = new EquipmentHardware
            {
                Type = request.Type.Trim(),
                Brand = request.Brand.Trim(),
                Model = request.Model.Trim(),
                SerialNumber = serial,
                ClientId = request.UserId
            };

            await _equipmentRepo.AddAsync(equipment);

            return MapToResponse(equipment);
        }

        public async Task<EquipmentHardwareResponse> GetByIdAsync(int id)
        {
            var equipment = await _equipmentRepo.GetByIdAsync(id);
            if (equipment == null)
                throw new NotFoundException($"No se encontró el equipo con ID {id}.");

            return MapToResponse(equipment);
        }

        public async Task<IEnumerable<EquipmentHardwareResponse>> GetAllAsync()
        {
            var equipments = await _equipmentRepo.GetAllAsync();
            return equipments.Select(MapToResponse);
        }

        public async Task<IEnumerable<EquipmentHardwareResponse>> GetByClientIdAsync(int clientId)
        {
            if (!await _userRepo.ExistsAsync(u => u.Id == clientId))
                throw new NotFoundException($"No se encontró el cliente con ID {clientId}.");

            var equipments = await _equipmentRepo.FindAsync(e => e.ClientId == clientId);
            return equipments.Select(MapToResponse);
        }

        public async Task<EquipmentHardwareResponse> UpdateAsync(int id, EquipmentHardwareRequest request)
        {
            var equipment = await _equipmentRepo.GetByIdAsync(id);
            if (equipment == null)
                throw new NotFoundException($"No se encontró el equipo con ID {id}.");

            if (!await _userRepo.ExistsAsync(u => u.Id == request.UserId))
                throw new NotFoundException($"No se encontró el cliente con ID {request.UserId}.");

            equipment.Type = request.Type.Trim();
            equipment.Brand = request.Brand.Trim();
            equipment.Model = request.Model.Trim();
            equipment.SerialNumber = request.SerialNumber.Trim();
            equipment.ClientId = request.UserId;

            await _equipmentRepo.UpdateAsync(equipment);

            return MapToResponse(equipment);
        }

        public async Task DeleteAsync(int id)
        {
            var equipment = await _equipmentRepo.GetByIdAsync(id);
            if (equipment == null)
                throw new NotFoundException($"No se encontró el equipo con ID {id}.");

            await _equipmentRepo.DeleteAsync(equipment);
        }

        private static EquipmentHardwareResponse MapToResponse(EquipmentHardware equipment)
        {
            return new EquipmentHardwareResponse(
                equipment.Id,
                equipment.Type,
                equipment.Brand,
                equipment.Model,
                equipment.SerialNumber,
                equipment.ClientId
            );
        }
    }
}