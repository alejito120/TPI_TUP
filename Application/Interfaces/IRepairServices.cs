using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.Interfaces
{
    public interface IRepairService
    {
        Task<RepairResponse> CreateAsync(RepairRequest request);

        Task<RepairResponse?> GetByIdAsync(int id);

        Task<IEnumerable<RepairResponse>> GetAllAsync();

        Task<IEnumerable<RepairResponse>> GetByStateAsync(
            RepairStatus state);

        Task<IEnumerable<RepairResponse>> GetByTechnicianAsync(
            int technicianId);

        Task AssignTechnicianAsync(
            int repairId,
            int technicianId);

        Task ChangeStateAsync(
            int repairId,
            RepairStatus state);

        Task CompleteRepairAsync( //metodo que decido desarrollar, ya que a raiz de la finalizacion de la reparacion,
                                  //no solo del cambio de estado a finalizado, se disparan servicios del historial, y de notificaciones,
                                  //por eso vi conveniente que exista este metodo, para que de aca, podamos desarrollar la orquestacion del servicio completa.
            int repairId);

        Task DeleteAsync(
            int repairId);
    }
}
