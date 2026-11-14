using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Exceptions;
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
        private readonly IRepositoryGeneric<RepairStatusHistory> _historyRepository;

        public RepairService(
            IRepositoryGeneric<Repair> repairRepository,
            IRepositoryGeneric<User> userRepository,
            IRepositoryGeneric<EquipmentHardware> equipmentRepository,
            IRepositoryGeneric<RepairStatusHistory> historyRepository)
        {
            _repairRepository = repairRepository;
            _userRepository = userRepository;
            _equipmentRepository = equipmentRepository;
            _historyRepository = historyRepository;
        }

        public async Task<RepairResponse> CreateAsync(RepairRequest request)
        {
            //  Validamos que exista el equipo
            var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId);
            if (equipment == null)
                throw new NotFoundException($"No se encontró el equipo con ID {request.EquipmentId}.");

            //  Validamos que exista el técnico y que su rol sea Technical
            var technician = await _userRepository.GetByIdAsync(request.TechnicianId);
            if (technician == null)
                throw new NotFoundException($"No se encontró el técnico con ID {request.TechnicianId}.");

            // lo pongo separado par adistinguir el error.
            if (technician.RolUser != Rol.Technical)
                throw new BusinessException("El usuario asignado no tiene el rol de Técnico.");

            // Creamos la reparación en estado Received
            var repair = new Repair
            {
                DateJoined = DateTime.UtcNow,
                State = RepairStatus.Received,
                ProblemDescription = request.ProblemDescription.Trim(),
                EquipmentID = request.EquipmentId,
                TechnicianId = request.TechnicianId
            };

            await _repairRepository.AddAsync(repair);

            // Registramos el ingreso inicial en el historial de estados
            var history = new RepairStatusHistory
            {
                RepairId = repair.Id,
                PreviousState = null,
                NewState = RepairStatus.Received,
                ChangeDate = DateTime.UtcNow,
                Observation = "Equipo ingresado al taller."
            };
            await _historyRepository.AddAsync(history);

            return MapToResponse(repair, equipment, technician);
        }

        public async Task<RepairResponse?> GetByIdAsync(int id)
        {
            var repair = await _repairRepository.GetByIdAsync(id);
            if (repair == null)
                throw new NotFoundException($"No se encontró la reparación con ID {id}.");

            var equipment = await _equipmentRepository.GetByIdAsync(repair.EquipmentID);
            var technician = await _userRepository.GetByIdAsync(repair.TechnicianId);

            return MapToResponse(repair, equipment, technician);
        }

        public async Task<IEnumerable<RepairResponse>> GetAllAsync()
        {
            var repairs = await _repairRepository.GetAllAsync();
            return await MapManyToResponseAsync(repairs);
        }

        public async Task<IEnumerable<RepairResponse>> GetByStateAsync(RepairStatus state)
        {
            if (!Enum.IsDefined(typeof(RepairStatus), state))
                throw new ValidationException("El estado especificado no es válido.");

            var repairs = await _repairRepository.FindAsync(r => r.State == state);
            return await MapManyToResponseAsync(repairs);
        }

        public async Task<IEnumerable<RepairResponse>> GetByTechnicianAsync(int technicianId)
        {
            if (!await _userRepository.ExistsAsync(u => u.Id == technicianId))
                throw new NotFoundException($"No se encontró el técnico con ID {technicianId}.");

            var repairs = await _repairRepository.FindAsync(r => r.TechnicianId == technicianId);
            return await MapManyToResponseAsync(repairs);
        }

        public async Task AssignTechnicianAsync(int repairId, int technicianId)
        {
            var repair = await _repairRepository.GetByIdAsync(repairId);
            if (repair == null)
                throw new NotFoundException($"No se encontró la reparación con ID {repairId}.");

            if (repair.State == RepairStatus.Completed || repair.State == RepairStatus.Rejected)
                throw new BusinessException("No se puede reasignar el técnico de una reparación finalizada o rechazada.");

            var technician = await _userRepository.GetByIdAsync(technicianId);
            if (technician == null)
                throw new NotFoundException($"No se encontró el técnico con ID {technicianId}.");

            if (technician.RolUser != Rol.Technical)
                throw new BusinessException("El usuario seleccionado no tiene el rol de Técnico.");

            repair.TechnicianId = technicianId;
            await _repairRepository.UpdateAsync(repair);
        }

        public async Task ChangeStateAsync(int repairId, RepairStatus state)
        {
            if (!Enum.IsDefined(typeof(RepairStatus), state))
                throw new ValidationException("El estado especificado no es válido.");

            var repair = await _repairRepository.GetByIdAsync(repairId);
            if (repair == null)
                throw new NotFoundException($"No se encontró la reparación con ID {repairId}.");

            if (repair.State == RepairStatus.Completed || repair.State == RepairStatus.Rejected)
                throw new BusinessException("La reparación ya se encuentra cerrada y no puede cambiar de estado.");

            var previousState = repair.State;
            repair.State = state;

            if (state == RepairStatus.Completed)
                repair.CompletionDate = DateTime.UtcNow;

            await _repairRepository.UpdateAsync(repair);

            // Guardamos el cambio en el historial
            await _historyRepository.AddAsync(new RepairStatusHistory
            {
                RepairId = repair.Id,
                PreviousState = previousState,
                NewState = state,
                ChangeDate = DateTime.UtcNow,
                Observation = $"Cambio de estado de {previousState} a {state}."
            });
        }

        public async Task CompleteRepairAsync(int repairId)
        {
            var repair = await _repairRepository.GetByIdAsync(repairId);
            if (repair == null)
                throw new NotFoundException($"No se encontró la reparación con ID {repairId}.");

            if (repair.State == RepairStatus.Completed)
                throw new BusinessException("La reparación ya fue finalizada anteriormente.");

            var previousState = repair.State;
            repair.State = RepairStatus.Completed;
            repair.CompletionDate = DateTime.UtcNow;

            await _repairRepository.UpdateAsync(repair);

            // Registramos la finalización en el historial
            await _historyRepository.AddAsync(new RepairStatusHistory
            {
                RepairId = repair.Id,
                PreviousState = previousState,
                NewState = RepairStatus.Completed,
                ChangeDate = DateTime.UtcNow,
                Observation = "Reparación finalizada."
            });
        }

        public async Task DeleteAsync(int repairId)
        {
            var repair = await _repairRepository.GetByIdAsync(repairId);
            if (repair == null)
                throw new NotFoundException($"No se encontró la reparación con ID {repairId}.");

            await _repairRepository.DeleteAsync(repair);
        }

        // Métodos auxiliares para armar el RepairResponse con el nombre del Equipo y del Técnico
        // Convierte una lista de reparaciones (Repair) en una lista de respuestas (RepairResponse).
        // En vez de consultar la base de datos adentro de un bucle por cada reparación,
        // traemos los equipos y usuarios una sola vez al principio y los guardamos en memoria.
        private async Task<IEnumerable<RepairResponse>> MapManyToResponseAsync(IEnumerable<Repair> repairs)
        {
            // 1. Traemos todos los equipos y usuarios una sola vez desde la base de datos.
            // Con .ToDictionary(e => e.Id) los organizamos en un Diccionario (Clave -> Valor)
            // donde la Clave de búsqueda es el Id (ej: Clave 1 -> Objeto EquipmentHardware con Id 1).
            // Esto nos permite buscar cualquier equipo o técnico por su Id de forma instantánea en memoria.
            var equipments = (await _equipmentRepository.GetAllAsync()).ToDictionary(e => e.Id);
            var users = (await _userRepository.GetAllAsync()).ToDictionary(u => u.Id);

            // 2. Recorremos cada reparación 'r' de la lista con .Select() para transformarla en un RepairResponse.
            return repairs.Select(r =>
            {
                // Buscamos el EquipmentID de la reparación dentro del diccionario 'equipments'.
                // Si lo encuentra, TryGetValue guarda el equipo encontrado dentro de la variable 'eq' (si no existe, queda null).
                equipments.TryGetValue(r.EquipmentID, out var eq);

                // Buscamos el TechnicianId de la reparación dentro del diccionario 'users'.
                // Si lo encuentra, TryGetValue guarda el técnico encontrado dentro de la variable 'tech'.
                users.TryGetValue(r.TechnicianId, out var tech);

                // Le pasamos la reparación (r), su equipo (eq) y su técnico (tech) al método de abajo para armar el DTO.
                return MapToResponse(r, eq, tech);
            });
        }

        // Convierte UNA reparación junto con su equipo y su técnico en el DTO de salida RepairResponse.
        private static RepairResponse MapToResponse(Repair repair, EquipmentHardware? equipment, User? technician)
        {
            // Operador ternario (? :): funciona como un if / else abreviado.
            // Si 'equipment' no es null (?), armamos el texto con Tipo + Marca + Modelo (ej: "Notebook Lenovo ThinkPad").
            // Si fuera null (:), mostramos por defecto el número de ID (ej: "Equipo ID #1").
            string equipmentName = equipment != null
                ? $"{equipment.Type} {equipment.Brand} {equipment.Model}"
                : $"Equipo ID #{repair.EquipmentID}";

            // Hacemos lo mismo con el técnico: si 'technician' no es null (?), unimos Nombre y Apellido.
            // Si fuera null (:), mostramos "Técnico ID #4".
            string technicianName = technician != null
                ? $"{technician.Name} {technician.LastName}"
                : $"Técnico ID #{repair.TechnicianId}";

            // Finalmente creamos y devolvemos el RepairResponse con los textos ya armados.
            return new RepairResponse(
                repair.Id,
                repair.DateJoined,
                repair.State.ToString(),
                repair.ProblemDescription,
                equipmentName,
                technicianName
            );
        }
    }
}