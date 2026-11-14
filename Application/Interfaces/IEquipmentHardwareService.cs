using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.Interfaces
{
    public interface IEquipmentHardwareService
    {
        Task<EquipmentHardwareResponse> CreateAsync(EquipmentHardwareRequest request);
        Task<EquipmentHardwareResponse> GetByIdAsync(int id);
        Task<IEnumerable<EquipmentHardwareResponse>> GetAllAsync();
        Task<IEnumerable<EquipmentHardwareResponse>> GetByClientIdAsync(int clientId);
        Task<EquipmentHardwareResponse> UpdateAsync(int id, EquipmentHardwareRequest request);
        Task DeleteAsync(int id);
    }
}
