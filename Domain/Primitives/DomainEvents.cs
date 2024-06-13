using MediatR;

namespace Domain.Primitives;

/// <summary>
/// Clase que representa los eventos de dominio
/// </summary>
public record DomainEvents(Guid Id) : INotification;